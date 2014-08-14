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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel.Design;
using Microsoft.Practices.SPG.SubSiteCreation.Properties;

namespace Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities
{
    /// <summary>
    /// The designer class for the SynchronizeStatuActivity class.
    /// </summary>
	public class SynchronizeStatusActivityDesigner : ActivityDesigner
	{
        /// <summary>
        /// The Initialize method will set the image used for CreateSubSiteActivity class.
        /// </summary>
        /// <param name="activity"></param>
        protected override void Initialize(System.Workflow.ComponentModel.Activity activity)
        {
            base.Initialize(activity);
            this.Image = Resources.SychronizeList;
        }
	}
}
