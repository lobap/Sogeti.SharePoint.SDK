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
using Microsoft.Practices.SPG.Common.Configuration;

namespace Microsoft.Practices.SPG.Common.Tests.ServiceLocation
{
    public class MockPropertyBag : IPropertyBag
    {
        private Dictionary<string, string> values = new Dictionary<string, string>();

        public bool Contains(string key)
        {
            return values.ContainsKey(key);
        }

        public string this[string key]
        {
            get
            {
                if (!values.ContainsKey(key))
                    return null;

                return values[key];
                    
            }
            set
            {
                values[key] = value;
            }
        }


        public void Update()
        {
        }

        public ConfigLevel Level
        {
            get { return ConfigLevel.CurrentSPWeb; }
        }

        public void Remove(string key)
        {
            this.values.Remove(key);
        }

        public IPropertyBag GetParent()
        {
            return null;
        }

        public IEnumerable<IPropertyBag> GetChildren()
        {
            yield break;
        }
    }
}