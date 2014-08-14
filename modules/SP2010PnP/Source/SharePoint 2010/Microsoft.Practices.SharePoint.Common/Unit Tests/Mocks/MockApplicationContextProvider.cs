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
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.UserCode;
using Microsoft.SharePoint.Utilities;

namespace Microsoft.Practices.SharePoint.Common.Tests.Mocks
{
    public class MockApplicationContextProvider : IApplicationContextProvider
    {
        public static string AppDomainFriendlyName { get; set; }
        public static SPFarm SPFarmLocal { get; set; }
        public static SPWeb SPContextCurrentWeb { get; set; }
        public static Exception  ExecuteRegisteredProxyOperationException { get; set; }
        public static object ExecuteRegisteredProxyOperationRetVal { get; set; }
        public static bool IsProxyCheckerInstalledRetVal { get; set; }
        public static bool IsProxyInstalledRetVal { get; set; }
        public static void Reset()
        {
            AppDomainFriendlyName = null;
            SPFarmLocal = null;
            SPContextCurrentWeb = null;
            ExecuteRegisteredProxyOperationException = null;
            ExecuteRegisteredProxyOperationRetVal = null;
            IsProxyCheckerInstalledRetVal = true;
            IsProxyInstalledRetVal = true;
        }

        public string GetCurrentAppDomainFriendlyName()
        {
            return AppDomainFriendlyName;
        }

        public SPWeb GetSPContextCurrentWeb()
        {
            return SPContextCurrentWeb;
        }

        public SPFarm GetSPFarmLocal()
        {
            return SPFarmLocal;
        }

        public object ExecuteRegisteredProxyOperation(string assemblyName, string typeName, SPProxyOperationArgs args)
        {
            if (ExecuteRegisteredProxyOperationException != null)
                throw ExecuteRegisteredProxyOperationException;

            return ExecuteRegisteredProxyOperationRetVal;
        }


        public bool IsProxyCheckerInstalled()
        {
            return IsProxyCheckerInstalledRetVal;
        }

        public bool IsProxyInstalled(string assemblyName, string typeForProxy)
        {
            return IsProxyInstalledRetVal;
        }
    }
}
