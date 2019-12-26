using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownViewer.VisualStudio
{
    public class SolutionAnalyser
    {
        internal async Task<string> ScanSolution()
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var dte = (DTE)Package.GetGlobalService(typeof(DTE));

                var sln = Microsoft.Build.Construction.SolutionFile.Parse(dte.Solution.FullName);
                string summaryText = $"{sln.ProjectsInOrder.Count.ToString()} projects";

                foreach (Project p in dte.Solution.Projects)
                {
                    summaryText += $"{Environment.NewLine} {p.Name} {p.ProjectItems.Count}";
                }
                return summaryText;
            }
            catch
            {
                return "Solution is not ready yet.";
            }            
        }
    }
}
