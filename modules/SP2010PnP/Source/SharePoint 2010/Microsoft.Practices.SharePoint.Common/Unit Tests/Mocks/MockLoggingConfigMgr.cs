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
    public class MockLoggingConfigMgr : SIConfigManager
    {
        public int SaveCount { get; private set; }
        public int LoadedCount { get; private set; }
        private int failureCount = -1;
        private int failures = 0;
        private DiagnosticsAreaCollection Areas = null;


        public MockLoggingConfigMgr()
        {
            this.SetInPropertyBagStringObjectIPropertyBag = this.SetInPropertyBagImpl;
            this.ContainsKeyInPropertyBagStringIPropertyBag = this.ContainsKeyInPropertyBagImpl;
            this.GetFromPropertyBagStringIPropertyBag<DiagnosticsAreaCollection>((key, propertyBag) =>
                {
                        LoadedCount++;
                        return Areas;
                }); 
            this.GetPropertyBagConfigLevel = GetPropertyBagLevelImpl;
        }

        public void SetFailureCount(int count)
        {
            this.failureCount = count;
        }

        void SetInPropertyBagImpl(string key, object value, IPropertyBag bag)
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

        bool ContainsKeyInPropertyBagImpl(string key, IPropertyBag propertyBag)
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

        IPropertyBag GetPropertyBagLevelImpl(ConfigLevel level)
        {
            BIPropertyBag bag = new BIPropertyBag();
            bag.Level = level;
            bag.Values.Count = 0;
            return bag;
        }
    }

    public class MockHierarchicalConfig : IHierarchicalConfig
    {

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

        public bool ContainsKey(string key, ConfigLevel level)
        {
            throw new NotImplementedException();
        }


        public void SetWeb(Microsoft.SharePoint.SPWeb web)
        {
            throw new NotImplementedException();
        }
    }

    public class MockConfigManager : IConfigManager
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
        private BIPropertyBag bag = new BIPropertyBag() { Level = ConfigLevel.CurrentSPFarm };

        public MockConfigManager()
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

 
        internal void Clear()
        {
            Areas = null;
        }
         
        public bool CanAccessFarmConfig
        {
            get { return true; }
        }

        public void RemoveKeyFromPropertyBag(string key, IPropertyBag propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyInPropertyBag(string key, IPropertyBag propertyBag)
        {
            if (key == Constants.AreasConfigKey && Areas != null)
                return true;
            return false;
        }

        public void SetInPropertyBag(string key, object value, IPropertyBag propertyBag)
        {
            if (key == Constants.AreasConfigKey)
                SaveCount++;
        }

        public TValue GetFromPropertyBag<TValue>(string key, IPropertyBag propertyBag)
        {
            if (typeof(TValue) == typeof(DiagnosticsAreaCollection))
            {
                LoadedCount++;
                return (TValue)(object)Areas;
            }
            return default(TValue);
        }

        public IPropertyBag GetPropertyBag(ConfigLevel level)
        {
            return this.bag;
        }



        public IPropertyBag GetPropertyBag(ConfigLevel level, Microsoft.SharePoint.SPWeb web)
        {
            throw new NotImplementedException();
        }


        public void SetWeb(Microsoft.SharePoint.SPWeb web)
        {
            throw new NotImplementedException();
        }
    }
}
