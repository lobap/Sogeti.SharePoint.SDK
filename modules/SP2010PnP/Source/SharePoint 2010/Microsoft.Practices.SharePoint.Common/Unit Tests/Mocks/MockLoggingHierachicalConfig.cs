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
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using Microsoft.SharePoint.Administration.Moles;

namespace Microsoft.Practices.SharePoint.Common.Tests.Logging
{
    public class BIConfigManager : SIConfigManager
    {
        public int SaveCount { get; private set; }
        public int LoadedCount { get; private set; }
        private int failureCount = -1;
        private int failures = 0;
        private DiagnosticsAreaCollection Areas = null;

        public BIConfigManager()
        {
            this.SetInPropertyBagStringObjectSPFarm = this.SetInPropertyBagImpl;
            this.ContainsKeyInPropertyBagStringSPFarm = this.ContainsKeyInPropertyBagImpl;
            this.GetFromPropertyBagStringSPFarm<DiagnosticsAreaCollection>((key, propertyBag) =>
                {
                        LoadedCount++;
                        return Areas;
                }); 
        }

        public void SetFailureCount(int count)
        {
            this.failureCount = count;
        }

        void SetInPropertyBagImpl(string key, object value, SPFarm farm)
        {
            if (key == Constants.AreasConfigKey)
            {
                if (this.failureCount != -1)
                {
                    this.failures++;

                    if (this.failures == this.failureCount)
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
        }

        bool ContainsKeyInPropertyBagImpl(string key, SPFarm propertyBag)
        {
            if (key == Constants.AreasConfigKey)
            {
                if (Areas == null)
                    return false;
                else
                    return true;
            }
            return false;
        }

    }



    public class MockLoggingHierarchicalConfig : IConfigManager, IHierarchicalConfig
    {
        public int LoadedCount = 0;
        public int SaveCount = 0;

        public DiagnosticsAreaCollection Areas;
        public static string Area1Name = Guid.NewGuid().ToString();
        public static string Area2Name = Guid.NewGuid().ToString();
        public static string Area1Category1Name = Guid.NewGuid().ToString();
        public static string Area1Category2Name = Guid.NewGuid().ToString();
        public static string Area2Category1Name = Guid.NewGuid().ToString();
        public static string Area2Category2Name = Guid.NewGuid().ToString();


        public MockLoggingHierarchicalConfig()
        {
            this.Areas = new DiagnosticsAreaCollection();
            DiagnosticsArea area1 = new DiagnosticsArea(Area1Name);
            DiagnosticsArea area2 = new DiagnosticsArea(Area2Name);
            area1.DiagnosticsCategories.Add(new DiagnosticsCategory(Area1Category1Name, EventSeverity.ErrorCritical, TraceSeverity.Medium));
            area1.DiagnosticsCategories.Add(new DiagnosticsCategory(Area1Category2Name, EventSeverity.ErrorCritical, TraceSeverity.Medium));
            area2.DiagnosticsCategories.Add(new DiagnosticsCategory(Area2Category1Name, EventSeverity.ErrorCritical, TraceSeverity.Medium));
            area2.DiagnosticsCategories.Add(new DiagnosticsCategory(Area2Category2Name, EventSeverity.ErrorCritical, TraceSeverity.Medium));
            this.Areas.Add(area1);
            this.Areas.Add(area2);
        }

        public TValue GetByKey<TValue>(string key)
        {
            throw new NotImplementedException();
        }

        public TValue GetByKey<TValue>(string key, ConfigLevel level)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public IPropertyBag GetRootConfig()
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key, ConfigLevel level)
        {
            if (level != ConfigLevel.CurrentSPFarm)
                throw new InvalidOperationException();

            if (key == Constants.AreasConfigKey)
            {
                return true;
            }

            return false;
        }

        public void RemoveKeyFromPropertyBag(string key, SPFarm propertyBag)
        {
            throw new NotImplementedException();
        }

        public void RemoveKeyFromPropertyBag(string key, Microsoft.SharePoint.SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public void RemoveKeyFromPropertyBag(string key, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public void RemoveKeyFromPropertyBag(string key, Microsoft.SharePoint.SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyInPropertyBag(string key, SPFarm propertyBag)
        {
            if (key == Constants.AreasConfigKey && Areas != null)
                return true;
            return false;
        }

        public bool ContainsKeyInPropertyBag(string key, Microsoft.SharePoint.SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyInPropertyBag(string key, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyInPropertyBag(string key, Microsoft.SharePoint.SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, Microsoft.SharePoint.SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, Microsoft.SharePoint.SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, SPFarm propertyBag)
        {
            if(key == Constants.AreasConfigKey)
                SaveCount++;
        }

        public TValue GetFromPropertyBag<TValue>(string key, Microsoft.SharePoint.SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetFromPropertyBag<TValue>(string key, Microsoft.SharePoint.SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPFarm propertyBag)
        {
            if (typeof(TValue) == typeof(DiagnosticsAreaCollection))
            {
                LoadedCount++;
                return (TValue)(object)Areas;
            }
            return default(TValue);
        }

        internal void Clear()
        {
            Areas = null;
        }

        public void RemoveKeyFromPropertyBag(string key, IPropertyBag propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyInPropertyBag(string key, IPropertyBag propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, IPropertyBag propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetFromPropertyBag<TValue>(string key, IPropertyBag propertyBag)
        {
            throw new NotImplementedException();
        }


        public object GetFromPropertyBag(Type type, string key, ConfigLevel level)
        {
            throw new NotImplementedException();
        }

        public IPropertyBag GetPropertyBag(ConfigLevel level)
        {
            throw new NotImplementedException();
        }


        public bool CanAccessFarmConfig
        {
            get { return true; }
        }
    }
}
