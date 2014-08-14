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
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.Practices.SharePoint.Common.Configuration;

namespace Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy
{
    public class DeserializeOperation : SPProxyOperation
    {
        public override object Execute(SPProxyOperationArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            try
            {
                var proxyArgs = args as ConfigDeserializeArgs;
                var configSerializer = new ConfigSettingSerializer();
                object deserializedValue = configSerializer.Deserialize(proxyArgs.TypeToDeserialize, proxyArgs.DeserializedData);
                return deserializedValue;
            }
            catch (Exception excpt)
            {
                return excpt;
            }
        }
    }
}
