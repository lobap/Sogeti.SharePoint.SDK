//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration.Moles;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint.Administration.Behaviors;

namespace Microsoft.Practices.SharePoint.Common.Tests.Behaviors
{
    public class BSPConfiguredWebApp: BSPWebApplication
    {
        WebAppSettingStore wss;

        public BSPConfiguredWebApp()
        {
            BSPWebApplication webApp = new BSPWebApplication();
            wss = new WebAppSettingStore();
        }

        public WebAppSettingStore SettingStore
        {
            get { return wss; }
        }
    }
}
