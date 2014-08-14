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
using System.Security.Permissions;
using System.Web;
using Microsoft.Practices.SPG.AJAXSupport.Controls;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.SilverlightControls;

namespace Contoso.PartnerPortal.Promotions.FieldControls
{
    public class WindowsMediaFieldControl : TextField
    {
        private MediaPlayer mediaPlayer;

        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            if (this.ControlMode == SPControlMode.Display)
            {
                this.Controls.Add(new SafeScriptManager());

                mediaPlayer = new MediaPlayer()
                                  {
                                      AutoLoad = true,
                                      AutoPlay = true,
                                      MediaSource = (string) ListItemFieldValue
                                  };
                this.Controls.Add(mediaPlayer);

            }
        }

        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
        protected override void Render(System.Web.UI.HtmlTextWriter output)
        {

            if (this.ControlMode == SPControlMode.Display)
            {
                mediaPlayer.RenderControl(output);

            }
            else
                base.Render(output);
        }
    }
}