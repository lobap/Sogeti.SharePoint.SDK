//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;

namespace Microsoft.Practices.SharePoint.Common.Configuration
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