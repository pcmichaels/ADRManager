using ADR.Models;
using System.Threading.Tasks;

namespace ADR.VisualStudio
{
    public interface ISolutionAnalyser
    {
        Task<DataResult<SolutionData>> ScanSolution();        
    }
}
