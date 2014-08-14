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
using Microsoft.Practices.SharePoint.Common.Tests.Mocks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.SharePoint;

namespace Microsoft.Practices.SharePoint.Common.Tests.ServiceLocation
{
    public class TestableSharePointServiceLocator: SharePointServiceLocator
    {
        public List<TypeMapping> FarmTypeMappings { get; set; }
        public List<TypeMapping> SiteTypeMappings { get; set; }
        
        public DateTime? FarmLastUpdatedRetVal { get; set; }
        public DateTime? SiteLastUpdatedRetVal { get; set; }

        public bool FarmGotTypeMappingsFromConfig { get; set; }
        public bool SiteGotTypeMappingsFromConfig { get; set; }
        public int SiteCachingTimeoutInSecondsRetVal { get; set; }
 
        new public IServiceLocator GetCurrent()
        {
            return base.DoGetCurrent();
        }

        new public IServiceLocator GetCurrent(SPSite site)
        {
            return base.DoGetCurrent(site);
        }

        protected override IServiceLocatorConfig GetServiceLocatorConfig()
        {
                var farmConfig = new MockServiceLocatorConfig();
                farmConfig.GetTypeMappingsRetVal = FarmTypeMappings;
                farmConfig.LastUpdateRetVal = FarmLastUpdatedRetVal;
                farmConfig.SiteCachingTimeoutInSecondsRetVal = SiteCachingTimeoutInSecondsRetVal;
                farmConfig.GotTypeMappingsFromConfig += delegate
                                                            {
                                                                FarmGotTypeMappingsFromConfig = true;
                                                            };

            return farmConfig;
        }

        protected override IServiceLocatorConfig GetServiceLocatorConfig(SPSite site)
        {
                var siteConfig = new MockServiceLocatorConfig
                                     {
                                         GetTypeMappingsRetVal = SiteTypeMappings,
                                         LastUpdateRetVal = SiteLastUpdatedRetVal,
                                         SiteCachingTimeoutInSecondsRetVal = SiteCachingTimeoutInSecondsRetVal
                                     };
            siteConfig.GotTypeMappingsFromConfig += delegate
                                                            {
                                                                SiteGotTypeMappingsFromConfig = true;
                                                            };
            return siteConfig;
        }

    }
}
