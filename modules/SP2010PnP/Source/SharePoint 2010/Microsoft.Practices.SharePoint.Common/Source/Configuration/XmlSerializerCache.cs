//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Xml.Serialization;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// A cache for xml serializers.  Improves performance by caching serialization classes.
    /// </summary>
    internal static class XmlSerializerCache
    {
        /// <summary>
        /// Caches the XmlSerializer instance for a specific type.
        /// </summary>
        private static XmlSerializerFactory serializerFactory = new XmlSerializerFactory();

        internal static XmlSerializer GetSerializer(Type type)
        {
            if (SharePointEnvironment.InSandbox)
                return new XmlSerializer(type);
            else
                return serializerFactory.CreateSerializer(type) ?? new XmlSerializer(type);
        }
    }
}
