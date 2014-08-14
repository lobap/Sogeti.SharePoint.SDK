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
using System.Collections.Generic;

using Microsoft.SharePoint;

namespace Contoso.TrainingManagement.ControlTemplates
{
    public interface IDirectReportsView
    {
        Dictionary<int, string> DirectReports
        {
            set;
        }

        string UserDisplayUrl
        {
            get;
            set;
        }

        string SourceUrl
        {
            get;
            set;
        }

        string DirectReportsMessage
        {
            set;
        }

        SPWeb Web
        {
            get;
            set;
        }

        string LoginName
        {
            get;
            set;
        }

        bool ShowLogin
        {
            get;
            set;
        }
    }
}
