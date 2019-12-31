using System;

namespace AdrManager.VisualStudioExtensionHelpers
{
    public static class ProjectItemsHelper
    {
        public static bool IsKind(this Project project, params string[] kindGuids)
        {
            foreach (var guid in kindGuids)
            {
                if (project.Kind.Equals(guid, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

    }
}
