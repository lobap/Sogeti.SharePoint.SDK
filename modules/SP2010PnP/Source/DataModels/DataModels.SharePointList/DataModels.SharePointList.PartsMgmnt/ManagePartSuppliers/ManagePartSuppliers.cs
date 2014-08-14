//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace DataModels.SharePointList.PartsMgmnt.ManagePartSuppliers
{
    [ToolboxItemAttribute(false)]
    public class ManagePartSuppliers : WebPart
    {
        private const string _ascxPath1 = @"~/_CONTROLTEMPLATES/PartsMgmntControls/ManagePartSuppliers.ascx";
        private Control hostControl = new Control();

        protected override void CreateChildControls()
        {
            hostControl = Page.LoadControl(_ascxPath1);
            Controls.Add(hostControl);
        }
    }
}
