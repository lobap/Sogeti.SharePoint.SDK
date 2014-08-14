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
using Microsoft.Practices.SharePoint.Common.ServiceLocation;

namespace Microsoft.Practices.SharePoint.Common.Tests.Mocks
{
    public class MockServiceLocatorConfig : IServiceLocatorConfig
    {
        public event EventHandler GotTypeMappingsFromConfig;
        public int SiteCachingTimeoutInSecondsRetVal { get; set; }
        public List<TypeMapping> GetTypeMappingsRetVal { get; set; }
        public List<TypeMapping> GetTypeMappings()
        {
            if (GotTypeMappingsFromConfig != null)
                GotTypeMappingsFromConfig.Invoke(this, null);

            return GetTypeMappingsRetVal;
        }

        public DateTime? LastUpdateRetVal { get; set; }
        public DateTime? LastUpdate
        {
            get { return LastUpdateRetVal; }
        }

        public void RegisterTypeMapping<TFrom, TTo>() where TTo : TFrom, new()
        {
            throw new NotImplementedException();
        }

        public void RegisterTypeMapping<TFrom, TTo>(string key) where TTo : TFrom, new()
        {
            throw new NotImplementedException();
        }

        public void RemoveTypeMappings<T>()
        {
            throw new NotImplementedException();
        }

        public void RemoveTypeMapping<T>(string key)
        {
            throw new NotImplementedException();
        }



        public Microsoft.SharePoint.SPSite Site
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public int GetSiteCacheInterval()
        {
            return SiteCachingTimeoutInSecondsRetVal;
        }

        public void SetSiteCacheInterval(int interval)
        {
            throw new NotImplementedException();
        }
    }
}
