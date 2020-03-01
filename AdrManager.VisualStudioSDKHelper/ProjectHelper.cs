using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AdrManager.VisualStudioSDKHelper
{
    // https://github.com/madskristensen/MarkdownEditor/blob/master/src/Helpers/ProjectHelpers.cs
    public static class ProjectHelper
    {
        public static bool IsKind(this Project project, params string[] kindGuids)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            foreach (var guid in kindGuids)
            {
                if (project.Kind.Equals(guid, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public static bool IsKind(this ProjectItem projectItem, params string[] kindGuids)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            foreach (var guid in kindGuids)
            {
                if (projectItem.Kind.Equals(guid, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public static async Task<(string path, string text)> GetDocumentText(this ProjectItem projectItem, string solutionDirectory)
        {
            if (projectItem == null) return (string.Empty, string.Empty);

            await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            string path = await GetFullPath(projectItem.Properties);
            if (string.IsNullOrWhiteSpace(path))
            {
                path = await GetFullPath(projectItem.ContainingProject?.Properties);

                if (string.IsNullOrWhiteSpace(path))
                {
                    path = Path.Combine(solutionDirectory, projectItem.Name);
                }
                else
                {
                    path = Path.Combine(path, projectItem.Name);
                }
            }

            return (path, File.ReadAllText(path));
        }

        public static async System.Threading.Tasks.Task OpenDocumentForProjectItem(this ProjectItem originalProjectItem)
        {
            await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var window = originalProjectItem.Open();
            window.Visible = true;
        }

        private async static Task<string> GetFullPath(Properties properties)
        {
            try
            {
                await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                return properties?.Item("FullPath")?.Value?.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task<Project> GetProjectByName(this Solution solution, string name)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var sln = Microsoft.Build.Construction.SolutionFile.Parse(solution.FullName);

            foreach (Project p in solution.Projects)
            {
                if (p.Name == name)
                {
                    return p;
                }
            }

            return null;
        }

        public static async Task<ProjectItem> GetProjectItemByName(this ProjectItems projectItems, string name, CancellationToken cancellationToken)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            foreach (ProjectItem item in projectItems)
            {
                if (item.Name == name) return item;
            }

            return null;
        }

    }
}
