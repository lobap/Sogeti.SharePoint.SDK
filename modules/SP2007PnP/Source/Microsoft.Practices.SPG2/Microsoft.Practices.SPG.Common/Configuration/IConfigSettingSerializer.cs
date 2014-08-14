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

namespace Microsoft.Practices.SPG.Common.Configuration
{
    /// <summary>
    /// Class that helps with serializing configuration settings. 
    /// </summary>
    public interface IConfigSettingSerializer
    {
        /// <summary>
        /// Serialize a value to a string in XML format. 
        /// </summary>
        /// <param name="type">The type of value to serialize</param>
        /// <param name="value">The value to serialize</param>
        /// <returns>the value, serialized as XML</returns>
        string Serialize(Type type, object value);

        /// <summary>
        /// Deserialize a value that was serialized by the <see cref="Serialize"/> method. 
        /// </summary>
        /// <param name="type">The type of object that was serialized.</param>
        /// <param name="value">The serialized value that should be deserialized.</param>
        /// <returns>The object that was serialized in XML.</returns>
        object Deserialize(Type type, string value);
        
    }
}