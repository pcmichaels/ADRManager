using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADR.Models
{
    public class ProjectData
    {
        public ProjectData()
        {
            Items = new List<ProjectItem>();
        }

        public List<ProjectItem> Items { get; set; }
        public string Name { get; internal set; }
    }
}
