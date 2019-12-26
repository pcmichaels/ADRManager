using AdrManager.MvvmUtilities;
using MarkDownViewer.VisualStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownViewer.ViewModels
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

        private async Task ScanCommand(object arg)
        {
            var solutionAnalyser = new SolutionAnalyser();
            Summary = await solutionAnalyser.ScanSolution();            
        }
    }
}
