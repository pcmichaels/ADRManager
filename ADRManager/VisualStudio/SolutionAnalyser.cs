using EnvDTE;
using ADR.Models;
using System.Threading.Tasks;
using System;
using ADR.Helpers;

namespace ADR.VisualStudio
{
    public class SolutionAnalyser
    {
        public async Task<DataResult<SolutionData>> ScanSolution()
        {
            var solutionData = new SolutionData();

            try
            {
                await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var dte = (DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE));

                var sln = Microsoft.Build.Construction.SolutionFile.Parse(dte.Solution.FullName);
                string summaryText = $"{sln.ProjectsInOrder.Count.ToString()} projects";

                foreach (Project p in dte.Solution.Projects)
                {
                    var projectData = new ProjectData()
                    {
                        Name = p.Name                        
                    };

                    await ScanProjectItems(p.ProjectItems, projectData);

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

        private async Task ScanProjectItems(ProjectItems projectItems, ProjectData projectData)
        {
            await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            foreach (EnvDTE.ProjectItem pi in projectItems)
            {
                if (pi.IsKind(ProjectItemTypes.SOLUTION_FOLDER, 
                              ProjectItemTypes.PROJECT_FOLDER,
                              ProjectItemTypes.SOLUTION_ITEM)
                    && pi.ProjectItems != null)
                {                    
                    await ScanProjectItems(pi.ProjectItems, projectData);
                    return;
                }

                string text = await pi.GetDocumentText();
                if (string.IsNullOrWhiteSpace(text)) continue;

                projectData.Items.Add(new Models.ProjectItem()
                {
                    Name = pi.Name,
                    Data = text
                });
            }
        }

    }
}
