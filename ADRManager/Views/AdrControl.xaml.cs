namespace ADR
{
    using EnvDTE;
    using MarkDownViewer.ViewModels;
    using MarkDownViewer.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for AdrWindowControl.
    /// </summary>
    public partial class AdrControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdrControl"/> class.
        /// </summary>
        public AdrControl()
        {
            this.InitializeComponent();
            DataContext = new AdrControlViewModel();
        }   

    }
}