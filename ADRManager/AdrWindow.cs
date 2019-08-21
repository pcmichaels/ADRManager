namespace ADR
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("f1b9f6a1-9d88-428e-900b-6f67ad255eb7")]
    public class AdrWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdrWindow"/> class.
        /// </summary>
        public AdrWindow() : base(null)
        {
            this.Caption = "AdrWindow";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new AdrWindowControl();
        }
    }
}
