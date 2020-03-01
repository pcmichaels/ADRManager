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
using ADR.Rules;
using Unity;
using AdrManager.VisualStudioSDKHelper;

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
        private string _summary;
        private readonly IRulesAnalyser _rulesAnalyser;
        private readonly ISolutionAnalyser _solutionAnalyser;

        public AdrControlViewModel() 
            : this(AdrPackage.UnityContainer.Value.Resolve<IRulesAnalyser>(),
                  AdrPackage.UnityContainer.Value.Resolve<ISolutionAnalyser>())
        {}

        public AdrControlViewModel(IRulesAnalyser rulesAnalyser, ISolutionAnalyser solutionAnalyser)
        {            
            _rulesAnalyser = rulesAnalyser;
            _solutionAnalyser = solutionAnalyser;

            ScanCommand = new RelayCommandAsync<object>(Scan);
            OpenFileCommand = new RelayCommandAsync<string>(OpenFile);
        }

        private async Task OpenFile(string path)
        {
            var file = MarkdownFiles.FirstOrDefault(a => a.Path == path);
            await file.OriginalProjectItem.OpenDocumentForProjectItem();
        }

        public string Summary 
        { 
            get => _summary; 
            set => UpdateField(ref _summary, value); 
        }

        public RelayCommandAsync<object> ScanCommand { get; set; }

        public List<ProjectItem> MarkdownFiles { get; set; }

        public RelayCommandAsync<string> OpenFileCommand { get; set; }

        private async Task Scan(object arg)
        {
            var scanResult = await _solutionAnalyser.ScanSolution();
            if (scanResult.IsSuccess)
            {
                _solutionData = scanResult.Data;
                Summary = "Successfully analysed project";
                MarkdownFiles = scanResult.Data.ProjectData
                    .SelectMany(a => a.Items)                    
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
