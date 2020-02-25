using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADR.Models
{
    public class SolutionData
    {
        public List<ProjectData> ProjectData { get; internal set; } = new List<ProjectData>();
    }
}
