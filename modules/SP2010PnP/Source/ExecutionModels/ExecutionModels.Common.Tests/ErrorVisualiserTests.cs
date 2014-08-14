//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace ExecutionModel.ExceptionHandling.Tests
{
    using System.IO;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using ExecutionModels.ExceptionHandling;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ErrorVisualiserFixture
    {
        [TestMethod]
        public void CanRenderErrorMessage()
        {
            // Arrange
            ErrorVisualizer target = new ErrorVisualizer();
            target.Controls.Add(new LiteralControl("Flawless"));
            
            // Act
            target.ShowErrorMessage("Error");

            // Assert
            Assert.IsTrue(target.RenderToString().Contains("Error"));
            Assert.IsFalse(target.RenderToString().Contains("Flawless"));

        }

        [TestMethod]
        public void RendersNormalIfNoError()
        {
            // Arrange
            ErrorVisualizer target = new ErrorVisualizer();
            target.Controls.Add(new LiteralControl("Flawless"));

            // Act
            // Assert
            Assert.IsTrue(target.RenderToString().Contains("Flawless"));
        }

        [TestMethod]
        public void CanAddChildControlsInConstructor()
        {
            // Arrange
            Control hostControl = new Control();
            Control control1 = new Control();
            TextBox control2 = new TextBox();

            // Act
            ErrorVisualizer target = new ErrorVisualizer(hostControl, control1, control2);

            // Assert
            // make sure target is child of hostcontrol
            Assert.AreSame(target, hostControl.Controls[0]);

            // make sure children are parents of host
            Assert.AreSame(control1, target.Controls[0]);
            Assert.AreSame(control2, target.Controls[1]);
        }
    }

    static class TestExtensions
    {
        public static string RenderToString(this Control control)
        {
            StringBuilder output = new StringBuilder();

            control.RenderControl(new HtmlTextWriter(new StringWriter(output)));

            return output.ToString();
        }
    }
}