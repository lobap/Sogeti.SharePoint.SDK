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
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;

namespace Contoso.TrainingManagement.FieldTypes
{
    /// <summary>
    /// The CourseDifficultyLevelFieldControl class provides custom
    /// rendering logic for the CourseDifficultyLevel Field Type.
    /// This custom control displays a series of star images
    /// that can be used to set the value of the CourseDifficultyLevel field type.
    /// </summary>
    [Guid("f0c913a5-dcdc-49d0-a4ac-840a239615bc")]
    public class CourseDifficultyLevelFieldControl : BaseFieldControl
    {
        #region Private Fields

        private readonly Panel imagePanel = new Panel();
        private const string rollOverUrl = "~/_layouts/images/trainingmanagement/level{0}.gif";
        private const string blankIconUrl = "~/_layouts/images/trainingmanagement/level0.gif";

        #endregion

        #region Method

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
        protected override void CreateChildControls()
        {
            if (this.ControlMode == SPControlMode.Edit || this.ControlMode == SPControlMode.New)
            {
                for (int i = 1; i < 6; i++)
                {
                    ImageButton button = new ImageButton();
                    if (i <= Convert.ToInt32(this.ItemFieldValue))
                    {
                        button.ImageUrl = Page.ResolveUrl(string.Format(rollOverUrl, i));
                        button.Attributes.Add("onmouseover", "this.src='" + Page.ResolveUrl(string.Format(rollOverUrl, i)) + "'");
                        button.Attributes.Add("onmouseout", "this.src='" + Page.ResolveUrl(string.Format(rollOverUrl, i)) + "'");
                    }
                    else
                    {
                        button.ImageUrl = Page.ResolveUrl(blankIconUrl);
                        button.Attributes.Add("onmouseover", "this.src='" + Page.ResolveUrl(string.Format(rollOverUrl, i)) + "'");
                        button.Attributes.Add("onmouseout", "this.src='" + Page.ResolveUrl(blankIconUrl) + "'");
                    }

                    button.Attributes.Add("DiffcultyLevel", i.ToString());
                    button.AlternateText = string.Format("Difficulty Level {0}", i);
                    button.CausesValidation = false;
                    button.Click += button_Click;
                    this.imagePanel.Controls.Add(button);
                }

                this.Controls.Add(imagePanel);
            }
        }

        void button_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton diffcultyLevelButton = (ImageButton)sender;
            int difficultyLevel = Int32.Parse(diffcultyLevelButton.Attributes["DiffcultyLevel"]);
            diffcultyLevelButton.ImageUrl = Page.ResolveUrl(string.Format(rollOverUrl, difficultyLevel));
            ViewState["DiffcultyLevel"] = difficultyLevel;

            for (int i = 1; i < 6; i++)
            {
                ImageButton button = (ImageButton)this.imagePanel.Controls[i - 1];
                if (i <= difficultyLevel)
                {
                    button.ImageUrl = Page.ResolveUrl(string.Format(rollOverUrl, i));
                    button.Attributes.Add("onmouseover", "this.src='" + Page.ResolveUrl(string.Format(rollOverUrl, i)) + "'");
                    button.Attributes.Add("onmouseout", "this.src='" + Page.ResolveUrl(string.Format(rollOverUrl, i)) + "'");
                }
                else
                {
                    button.ImageUrl = Page.ResolveUrl(blankIconUrl);
                    button.Attributes.Add("onmouseover", "this.src='" + Page.ResolveUrl(string.Format(rollOverUrl, i)) + "'");
                    button.Attributes.Add("onmouseout", "this.src='" + Page.ResolveUrl(blankIconUrl) + "'");
                }
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
        public override void UpdateFieldValueInItem()
        {
            this.EnsureChildControls();

            if (ViewState["DiffcultyLevel"] != null)
            {
                this.Value = (int)ViewState["DiffcultyLevel"];
                this.ItemFieldValue = (int)ViewState["DiffcultyLevel"];
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
        protected override void RenderFieldForDisplay(System.Web.UI.HtmlTextWriter output)
        {
            //store and decode the html of the base render pattern
            using ( TextWriter textWriter = new StringWriter() )
            {
                using ( HtmlTextWriter htmlTextWriter = new HtmlTextWriter(textWriter) )
                {
                    base.RenderFieldForDisplay(htmlTextWriter);

                    string baseFieldHtml = HttpUtility.HtmlDecode(textWriter.ToString());

                    //replace the alignment style and write out the updated html string
                    string updatedFieldHtml = baseFieldHtml.Replace("<div style='text-align:right;'>", "<div style='text-align:left;'>");
                    output.Write(updatedFieldHtml);
                }
            }
        }

        #endregion
    }
}
