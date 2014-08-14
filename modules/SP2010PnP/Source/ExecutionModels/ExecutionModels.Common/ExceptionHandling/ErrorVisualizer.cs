//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Web.UI;
using ExecutionModels.Common.ExceptionHandling;
using Microsoft.Practices.SharePoint.Common;

namespace ExecutionModels.ExceptionHandling
{
    /// <summary>
    /// Control that can show error messages for views, in such a way that it will completely replace
    /// the views with the error message. It implements the <see cref="IErrorVisualizer"/> iterface
    /// so the <see cref="ViewExceptionHandler"/> can interact with it.
    /// If an errormessage needs to be displayed, it will suppress the rendering of any of it's
    /// child controls and only display the error message.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualiser", Justification = "Visualiser is the most common spelling for this word.")]
    public class ErrorVisualizer : Control, IErrorVisualizer
    {
        public const string DefaultErrorMessage = "A problem with this view is preventing it from being displayed.";
  
        private string ErrorMessage;

        /// <summary>
        /// Creates an error Visualizer
        /// </summary>
        public ErrorVisualizer()
        {
        }

        /// <summary>
        /// A constructor that makes it easier to add the error visualzer to it. 
        /// </summary>
        /// <param name="hostControl">
        /// The control that hosts the errorvisualizer. Typically this is the webpart. 
        /// 
        /// The ErrorVisualizer will be added as a childcontrol to the host. 
        /// </param>
        /// <param name="childControls">Any child controls that should be added to the error visualizers Controls collection.</param>
        public ErrorVisualizer(Control hostControl, params Control[] childControls)
        {
            Validation.ArgumentNotNull(hostControl, "hostControl");

            hostControl.Controls.Add(this);

            if (childControls == null)
                return;

            foreach (Control child in childControls)
                this.Controls.Add(child);
        }


        /// <summary>
        /// Show a default error message. This should be a friendly, non technical error message, that tells the end user something is wrong.
        /// </summary>
        public virtual void ShowDefaultErrorMessage()
        {
            ShowErrorMessage(DefaultErrorMessage);
        }

        /// <summary>
        /// Show a cusotm error message. This should be a friendly, non technical error message, that tells the end user something is wrong.
        /// </summary>
        /// <param name="errorMessage">The error message to display. This should be a friendly, non technical error message, that tells the end user something is wrong.</param>
        public virtual void ShowErrorMessage(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Renders the error message.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="errorMessage">The error message.</param>
        protected virtual void RenderErrorMessage(HtmlTextWriter writer, string errorMessage)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(errorMessage);
            writer.RenderEndTag();
        }


        /// <summary>
        /// Override the render method to render an error message if there is one and to suppress the rendering
        /// of any of it's children. If there is no error message (if <see cref="ErrorMessage"/> is null or empty) then
        /// the child controls will render normally. 
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                base.Render(writer);
            }
            else
            {
                RenderErrorMessage(writer, this.ErrorMessage);
            }
        }
    }
}