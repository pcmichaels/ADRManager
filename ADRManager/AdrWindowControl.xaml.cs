namespace ADR
{
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for AdrWindowControl.
    /// </summary>
    public partial class AdrWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdrWindowControl"/> class.
        /// </summary>
        public AdrWindowControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = (DTE)Package.GetGlobalService(typeof(DTE));            

            var sln = Microsoft.Build.Construction.SolutionFile.Parse(dte.Solution.FullName);
            projectsText.Text = $"{sln.ProjectsInOrder.Count.ToString()} projects";

            foreach (Project p in dte.Solution.Projects)
            {
                projectsText.Text += $"{Environment.NewLine} {p.Name} {p.ProjectItems.Count}";
            }
        }


    }
}