//===============================================================================
// Microsoft patterns & practices
// SharePoint Guidance version 2.0
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contoso.Common.ExceptionHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contoso.Common.Tests.ExceptionHandling
{
    [TestClass]
    public class ErrorVisualiserFixture
    {
        [TestMethod]
        public void CanRenderErrorMessage()
        {
            ErrorVisualizer target = new ErrorVisualizer();
            target.Controls.Add(new LiteralControl("Flawless"));
            target.ShowErrorMessage("Error");

            Assert.IsTrue(target.RenderToString().Contains("Error"));
            Assert.IsFalse(target.RenderToString().Contains("Flawless"));

        }

        [TestMethod]
        public void RendersNormalIfNoError()
        {
            ErrorVisualizer target = new ErrorVisualizer();
            target.Controls.Add(new LiteralControl("Flawless"));

            Assert.IsTrue(target.RenderToString().Contains("Flawless"));
        }

        [TestMethod]
        public void CanAddChildControlsInConstructor()
        {
            Control hostControl = new Control();

            Control control1 = new Control();
            TextBox control2 = new TextBox();

            ErrorVisualizer target = new ErrorVisualizer(hostControl, control1, control2);

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