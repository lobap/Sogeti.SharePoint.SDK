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
using Microsoft.Practices.ServiceLocation.Moles;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ServiceLocation.Moles;
using Microsoft.Moles.Framework.Behaviors;

namespace Microsoft.Practices.SharePoint.Common.Configuration.Moles
{
    class BIServiceLocatorFactory 
        : SIServiceLocatorFactory
    {
        static public int LoadCount { get; private set; }
        static public Type LocatorTypeToUse { get; set; }

        static BIServiceLocatorFactory()
        {
            LocatorTypeToUse = typeof (SIServiceLocator);
        }

        static public void Reset()
        {
            LocatorTypeToUse = typeof (SIServiceLocator);
            LoadCount = 0;
        }

        public BIServiceLocatorFactory()
        {
            this.Create = () =>
                              {
                                  LoadCount++;
                                  return
                                      (IServiceLocator) LocatorTypeToUse.GetConstructor(Type.EmptyTypes).Invoke((null));
                              };

            this.LoadTypeMappingsIServiceLocatorIEnumerableOfTypeMapping =
                (serviceLocator, typeMappings) => {};
        }
    }
}
