using EnvDTE;
using ADR.Models;
using System.Threading.Tasks;
using ADR.Rules;

namespace ADR.VisualStudio
{
    public class SolutionAnalyser : ISolutionAnalyser
    {
        private readonly IRulesAnalyser _rulesAnalyser;

        public SolutionAnalyser(IRulesAnalyser rulesAnalyser)
        {
            _rulesAnalyser = rulesAnalyser;
        }

        public async Task<DataResult<SolutionData>> ScanSolution()
        {
            var solutionData = new SolutionData();

            try
            {
                await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var dte = (DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE));
                
                string solutionDirectory = System.IO.Path.GetDirectoryName(dte.Solution.FullName);

                var sln = Microsoft.Build.Construction.SolutionFile.Parse(dte.Solution.FullName);
                string summaryText = $"{sln.ProjectsInOrder.Count.ToString()} projects";

                foreach (Project p in dte.Solution.Projects)
                {
                    System.Diagnostics.Debug.WriteLine($"Project {p.Name}");
                    var projectData = new ProjectData()
                    {
                        Name = p.Name                        
                    };

                    await ScanProjectItems(p.ProjectItems, projectData, solutionDirectory);

                    if (projectData?.Items?.Count > 0)
                    {
                        solutionData.ProjectData.Add(projectData);
                    }
                }
                return DataResult<SolutionData>.Success(solutionData);
            }
            catch
            {
                return DataResult<SolutionData>.Fail("Solution is not ready yet.");
            }            
        }

        private async Task ScanProjectItems(
            ProjectItems projectItems, ProjectData projectData, string solutionDirectory)
        {
            await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            foreach (EnvDTE.ProjectItem pi in projectItems)
            {
                if (pi.IsKind(ProjectItemTypes.SOLUTION_FOLDER, 
                              ProjectItemTypes.PROJECT_FOLDER,
                              ProjectItemTypes.SOLUTION_ITEM))
                {
                    if (pi.ProjectItems != null)
                    {
                        await ScanProjectItems(pi.ProjectItems, projectData, solutionDirectory);
                        continue;
                    }
                    else if (pi.SubProject != null)
                    {
                        await ScanProjectItems(pi.SubProject.ProjectItems, projectData, solutionDirectory);
                        continue;
                    }                    
                }

                if (!_rulesAnalyser.IsProjectItemNameValid(pi.Name))
                {
                    continue;
                }

                var docResult = await pi.GetDocumentText(solutionDirectory);
                if (string.IsNullOrWhiteSpace(docResult.text)) continue;

                projectData.Items.Add(new Models.ProjectItem()
                {
                    Name = pi.Name,
                    Data = docResult.text,
                    Path = docResult.path,
                    OriginalProjectItem = pi
                });
            }
        }
    }
}
