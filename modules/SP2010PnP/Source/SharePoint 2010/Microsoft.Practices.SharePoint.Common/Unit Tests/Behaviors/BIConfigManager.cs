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
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using Microsoft.SharePoint.Administration.Moles;

namespace Microsoft.Practices.SharePoint.Common.Tests.Behaviors
{
    public class BIConfigManager: SIConfigManager
    {
        static public BIConfigStack Stack = new BIConfigStack();
        public int FailureCount { get; set; }
        public int SaveCount { get; private set; }
        private int failures;

        public void SetFarmStack()
        {
            Stack = new BIConfigStack();
            Stack.Bags.Add(GetPropertyBag(ConfigLevel.CurrentSPWeb));
            Stack.Bags.Add(GetPropertyBag(ConfigLevel.CurrentSPSite));
            Stack.Bags.Add(GetPropertyBag(ConfigLevel.CurrentSPWebApplication));
            Stack.Bags.Add(GetPropertyBag(ConfigLevel.CurrentSPFarm));
        }


        public BIConfigManager()
        {
            Reset();
        }


        private BIPropertyBag GetPropertyBag(ConfigLevel level)
        {
            var propBag = new BIPropertyBag();
            propBag.Values.Count = 0;
            propBag.Level = level;
            return propBag;
        }

        public void Reset()
        {
            Stack = new BIConfigStack();
            this.FailureCount = -1;
            this.SetInPropertyBagStringObjectIPropertyBag = this.SetInPropertyBagImpl;
            this.ContainsKeyInPropertyBagStringIPropertyBag = this.ContainsKeyInPropertyBagImpl;
            this.GetFromPropertyBagStringIPropertyBag<string>((key, propBag) => GetFromPropertyBagStrImpl(key, propBag));
            this.GetPropertyBagConfigLevel = GetPropertyBagLevelImpl;
            SaveCount = 0;
            failures = 0;
        }

        string GetFromPropertyBagStrImpl(string key, IPropertyBag bag)
        {
            return bag[key];
        }

        void SetInPropertyBagImpl(string key, object value, IPropertyBag bag)
        {
            if (FailureCount != -1)
            {
                this.failures++;

                if (this.failures == this.FailureCount)
                    this.SaveCount++;
                else
                {
                    var ex = new MSPUpdatedConcurrencyException();
                    throw ex.Instance;
                }
            }
            else
                this.SaveCount++;
        }

        bool ContainsKeyInPropertyBagImpl(string key, IPropertyBag propertyBag)
        {
            return propertyBag.Contains(key);
        }

        IPropertyBag GetPropertyBagLevelImpl(ConfigLevel level)
        {
            return Stack.GetPropertyBagForLevel(level);
        }

        
    }
}
