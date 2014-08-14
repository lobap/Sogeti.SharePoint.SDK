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

namespace VSeWSS
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class TargetListAttribute : Attribute
    {
        public TargetListAttribute(string id)
        {
            this.m_id = id;
        }

        private string m_id = String.Empty;
        public string Id
        {
            get { return this.m_id; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class TargetContentTypeAttribute : Attribute
    {
        public TargetContentTypeAttribute(string id)
        {
            this.m_id = id;
        }

        private string m_id = String.Empty;
        public string Id
        {
            get { return this.m_id; }
        }
    }
}
