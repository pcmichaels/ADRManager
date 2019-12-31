using AdrManager.MvvmUtilities;
using ADR.VisualStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ADR.Models;

namespace ADR.ViewModels
{
    public class AdrControlViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string fieldName = null) =>        
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(fieldName));

        private void UpdateField<T>(ref T field, T value, [CallerMemberName]string fieldName = null)
        {
            field = value;
            OnPropertyChanged(fieldName);
        }
        #endregion

        private SolutionData _solutionData;

        public AdrControlViewModel()
        {
            Scan = new RelayCommandAsync<object>(ScanCommand);
        }

        private string _summary;
        public string Summary 
        { 
            get => _summary; 
            set => UpdateField(ref _summary, value); 
        }

        public RelayCommandAsync<object> Scan { get; set; }

        public List<string> MarkdownFiles { get; set; }

        private async Task ScanCommand(object arg)
        {
            var solutionAnalyser = new SolutionAnalyser();
            var scanResult = await solutionAnalyser.ScanSolution();
            if (scanResult.IsSuccess)
            {
                _solutionData = scanResult.Data;
                Summary = "Successfully analysed project";
                MarkdownFiles = scanResult.Data.ProjectData
                    .SelectMany(a => a.Items)
                    .Select(a => a.Name)
                    .ToList();
                OnPropertyChanged(nameof(MarkdownFiles));
            }
            else
            {
                Summary = scanResult.ErrorMessage;
            }
        }
    }
}
