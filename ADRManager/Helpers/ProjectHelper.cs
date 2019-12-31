using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADR.Helpers
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
            //var path = YourProjectItem.Properties.Item("FullPath").Value.ToString()
            if (projectItem == null) return string.Empty;
            await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                TextDocument textDocument;
                if (!projectItem.IsOpen)
                {
                    var doc = projectItem.Open(EnvDTE.Constants.vsViewKindCode);
                    textDocument = (TextDocument)doc.Document.Object("TextDocument");
                }
                else
                {
                    textDocument = (TextDocument)projectItem.Document.Object("TextDocument");
                }
                EditPoint editPoint = textDocument.StartPoint.CreateEditPoint();
                return editPoint.GetText(textDocument.EndPoint);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
