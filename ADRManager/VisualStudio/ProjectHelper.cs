using EnvDTE;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ADR.VisualStudio
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

        public static async Task<string> GetDocumentText(this ProjectItem projectItem)
        {            
            if (projectItem == null) return string.Empty;
            await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            string path = projectItem.Properties?.Item("FullPath")?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            return File.ReadAllText(path);
        }
    }
}
