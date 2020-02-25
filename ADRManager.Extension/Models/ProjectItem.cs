using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADR.Models
{
    public class ProjectItem
    {
        public string Name { get; set; }
        public string Data { get; set; }
        public EnvDTE.ProjectItem OriginalProjectItem { get; internal set; }
        public string Path { get; internal set; }
    }
}
