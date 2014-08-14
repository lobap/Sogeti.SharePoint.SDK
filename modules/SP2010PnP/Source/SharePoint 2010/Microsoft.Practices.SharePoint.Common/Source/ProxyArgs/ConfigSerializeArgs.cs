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
using Microsoft.SharePoint.UserCode;

namespace Microsoft.Practices.SharePoint.Common.ProxyArgs
{
    [Serializable]
    public class ConfigSerializeArgs: SPProxyOperationArgs
    {

        public Type TypeToSerialize { get; set; }

        public object ValueToSerialize { get; set; }


        public ConfigSerializeArgs(Type typeToSerialize, object valueToSerialize)
        {
            this.TypeToSerialize = typeToSerialize;
            this.ValueToSerialize = valueToSerialize;
        }

        public ConfigSerializeArgs()
        {
        }

        public static string OperationAssemblyName
        {
            get { return ProxyOperationTypes.ConfigurationProxyAssemblyName; }
        }

        public static string OperationTypeName
        {
            get { return ProxyOperationTypes.SerializeOpTypeName; }
        }
    }
}


