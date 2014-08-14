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
using System.IO;
using System.Xml.Serialization;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// The Serializer that help with serializing the values of config settings to and from strings. 
    /// </summary>
    public class ConfigSettingSerializer : IConfigSettingSerializer
    {
        /// <summary>
        /// Serialize a value to a string in XML format.
        /// </summary>
        /// <param name="type">The type of value to serialize</param>
        /// <param name="value">The value to serialize</param>
        /// <returns>the value, serialized as xml</returns>
        public string Serialize(Type type, object value)
        {
            return Serializer(type, value);
        }

        /// <summary>
        /// Deserialize a value that was serialized by the <see cref="Serialize"/> method.
        /// </summary>
        /// <param name="type">The type of object that was serialized.</param>
        /// <param name="value">The serialized value that should be deserialized.</param>
        /// <returns>The object that was serialized in XML.</returns>
        public object Deserialize(Type type, string value)
        {
            return Deserializer(type, value);
        }

        private string Serializer(Type type, object value)
        {
             if (value == null)
                return null;

            string retVal = null;

            //Try to simplify serialization for simple types first without using xmlserializer.
            if (typeof(string) == type)
                retVal = value as string;
            else if (typeof(Type) == type)
                retVal = ((Type)value).AssemblyQualifiedName;
            else if (type.IsEnum)
                retVal = value.ToString();
            else
                retVal = TrySerializePrimitive(type, value);

            if (retVal == null)
            {
                using (var writer = new StringWriter(CultureInfo.InvariantCulture))
                {
                    try
                    {
                        XmlSerializer xmlserializer = XmlSerializerCache.GetSerializer(type);
                        xmlserializer.Serialize(writer, value);
                        retVal = writer.ToString();
                    }
                    catch (Exception ex)
                    {
                        var configEx = new ConfigurationException("Error on serializing configuration data", ex);
                        throw configEx;
                    }
                }
            }
            return retVal;
        }

        private object Deserializer(Type type, string value)
        {
            object retVal = null;

            // First try to deserialize using simple types first, then use xml serializer
            if (value == null)
                return null;
            else if (typeof(string) == type)
                retVal = value;
            else if (typeof(Type) == type)
               retVal= Type.GetType(value);
            else if (type.IsEnum)
                retVal = Enum.Parse(type, value);

            if (retVal == null)
                retVal = TryParsePrimitive(type, value);

            if (retVal == null)
            {
                using (var reader = new StringReader(value))
                {
                    try
                    {
                        XmlSerializer xmlserializer = XmlSerializerCache.GetSerializer(type);
                        retVal = xmlserializer.Deserialize(reader);
                    }
                    catch (Exception ex)
                    {
                        var configEx = new ConfigurationException("Error on deserializing configuration data", ex);
                        throw configEx;
                    }
                }
            }

            return retVal;
        }

        static string TrySerializePrimitive(Type type, object value)
        {
            string retVal = null;
            if (type.IsPrimitive)
            {
                if (typeof(bool) == type || typeof(byte) == type || typeof(sbyte) == type || typeof(char) == type ||
                    typeof(double) == type || typeof(Single) == type || typeof(int) == type || typeof(uint) == type ||
                    typeof(float) == type)
                {
                    retVal = value.ToString();
                }
            }

            return retVal;
        }

        static object TryParsePrimitive(Type type, string value)
        {
            object retVal = null;
            if (type.IsPrimitive)
            {
                if (typeof(bool) == type)
                    retVal = bool.Parse(value);

                if (typeof(byte) == type)
                    retVal = byte.Parse(value);

                if (typeof(sbyte) == type)
                    retVal = sbyte.Parse(value);

                if (typeof(double) == type)
                    retVal = double.Parse(value);

                if (typeof(char) == type)
                    retVal = char.Parse(value);

                if (typeof(Single) == type)
                    retVal = Single.Parse(value);

                if (typeof(int) == type)
                    retVal = int.Parse(value);

                if (typeof(uint) == type)
                    retVal = uint.Parse(value);

                if (typeof(float) == type)
                    retVal = float.Parse(value);
            }

            return retVal;
        }
    }
}
