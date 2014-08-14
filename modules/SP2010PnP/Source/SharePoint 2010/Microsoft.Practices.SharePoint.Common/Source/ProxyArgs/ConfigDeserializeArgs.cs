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
    public class ConfigDeserializeArgs : SPProxyOperationArgs
    {

        public Type TypeToDeserialize { get; set; }


        public string DeserializedData { get; set; }


        public ConfigDeserializeArgs(Type typeToSerialize, string deserializeData)
        {
            this.TypeToDeserialize = typeToSerialize;
            this.DeserializedData = deserializeData;
        }

        public ConfigDeserializeArgs()
        {
        }

        public static string OperationAssemblyName
        {
            get { return ProxyOperationTypes.ConfigurationProxyAssemblyName; }
        }

        public static string OperationTypeName
        {
            get { return ProxyOperationTypes.DeserializeOpTypeName; }
        }
    }
}


