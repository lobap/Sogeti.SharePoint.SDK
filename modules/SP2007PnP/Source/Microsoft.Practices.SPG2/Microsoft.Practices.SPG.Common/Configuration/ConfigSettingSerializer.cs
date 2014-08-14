//===============================================================================
// Microsoft patterns & practices
// SharePoint Guidance version 2.0
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Microsoft.Practices.SPG.Common.Configuration
{
    /// <summary>
    /// The Serializer that help with serializing the values of config settings to and from strings. 
    /// </summary>
    public class ConfigSettingSerializer : IConfigSettingSerializer
    {
        /// <summary>
        /// Deserialize a value that was serialized by the <see cref="Serialize"/> method.
        /// </summary>
        /// <param name="type">The type of object that was serialized.</param>
        /// <param name="value">The serialized value that should be deserialized.</param>
        /// <returns>The object that was serialized in XML.</returns>
        public object Deserialize(Type type, string value)
        {
            if (value == null)
                return null;

            if (typeof(string) == type)
                return value;

            if (typeof(Type) == type)
                return Type.GetType(value);

            XmlSerializer serializer = new XmlSerializer(type);
            return serializer.Deserialize(new StringReader(value));
        }

        /// <summary>
        /// Serialize a value to a string in XML format.
        /// </summary>
        /// <param name="type">The type of value to serialize</param>
        /// <param name="value">The value to serialize</param>
        /// <returns>the value, serialized as xml</returns>
        public string Serialize(Type type, object value)
        {
            if (value == null)
                return null;

            if (typeof(string) == type)
            {
                return (string)value;
            }

            if (typeof(Type) == type)
            {
                return ConvertToAssemblyQualifiedName(value as Type);
            }

            XmlSerializer serializer = new XmlSerializer(type);

            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            
            serializer.Serialize(writer, value);
            return writer.ToString();

        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private string ConvertToAssemblyQualifiedName(Type type)
        {
            return type.AssemblyQualifiedName;
        }
    }
}
