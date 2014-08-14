//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Globalization;
using Microsoft.Practices.SharePoint.Common.Properties;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    class ExceptionHelper
    {
        internal static void ThrowSandboxConfigurationException(Exception exception, ConfigLevel configLevel)
        {
            var ex = new ConfigurationException(string.Format(CultureInfo.CurrentCulture,
            Resources.UnexpectedExceptionFromSandbox, configLevel.ToString()), exception);
            throw ex;
        }

    }
}
