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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DataModels.SharePointList.PartsMgmnt.PartsMgmntControls;

namespace DataModels.SharePointList.PartsMgmnt.ManageSuppliers
{
    [ToolboxItemAttribute(false)]
    public class ManageSuppliers : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath1 = @"~/_CONTROLTEMPLATES/PartsMgmntControls/ManageSuppliers.ascx";
        private Control hostControl = new Control();

        protected override void CreateChildControls()
        {
            hostControl = Page.LoadControl(_ascxPath1);
            Controls.Add(hostControl);
        }

    }
}
