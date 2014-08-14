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
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.SharePoint.Common.ServiceLocation
{
    /// <summary>
    /// Class that represents a type mapping for the <see cref="IServiceLocator"/>.
    /// 
    /// A type mapping links an interface to an implementation. 
    /// </summary>
    public class TypeMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMapping"/> class.
        /// </summary>
        public TypeMapping()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMapping"/> class and populate it's values. 
        /// </summary>
        /// <param name="typeFrom">The type that's used to index the type mapping</param>
        /// <param name="typeTo">The type that the typeFrom is mapped to. </param>
        /// <param name="key">The key used to index the type mapping.</param>
        public TypeMapping(Type typeFrom, Type typeTo, string key)
        {
            Validation.ArgumentNotNull(typeFrom, "typeFrom");
            Validation.ArgumentNotNull(typeTo, "typeTo");

            if (typeTo.IsAbstract)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.NonAbstractType, typeTo.Name));
            }
            else if (!typeTo.IsSubclassOf(typeFrom) && typeTo.GetInterface(typeFrom.Name) == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.ImplicitConversionNotDefined, typeTo.Name, typeFrom.Name));
            }

            FromAssembly = typeFrom.Assembly.FullName;
            FromType = typeFrom.AssemblyQualifiedName;
            ToAssembly = typeTo.Assembly.FullName;
            ToType = typeTo.AssemblyQualifiedName;
            Key = key;
        }

        /// <summary>
        /// The name of the assembly that the 'from' type is located in. 
        /// </summary>
        public string FromAssembly { get; set; }

        /// <summary>
        /// The assembly qualified typename of the 'from' type. 
        /// </summary>
        public string FromType { get; set; }

        /// <summary>
        /// The assembly qualified typename of the 'to' type. 
        /// </summary>
        public string ToType { get; set; }

        /// <summary>
        /// The name of the Assembly that the 'to' type is located in. 
        /// </summary>
        public string ToAssembly { get; set; }

        /// <summary>
        /// A key that can differentiate several type mappings for the same fromtype. If you don't specify
        /// a key, null will be used. 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Returns a key that is assured to be non-null.  Returns an empty string if the 
        /// key is currently null.
        /// </summary>
        /// <returns>The key value, an empty string if the key is null</returns>
        public string GetNonNullKey()
        {
            if (Key == null)
                return string.Empty;
            return Key;
        }


        /// <summary>
        /// Determines how the objects for this type mapping should be instantiated. As a singleton or a new
        /// instance each time. 
        /// </summary>
        public InstantiationType InstantiationType { get; set; }

        /// <summary>
        /// Helper method to create type mapping objects more easily. Creates a type mapping with
        /// key (null) and instantiation type NewInstanceForEachRequest. 
        /// </summary>
        /// <typeparam name="TFrom">the from type</typeparam>
        /// <typeparam name="TTo">the target type</typeparam>
        /// <returns>the created type mapping</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static TypeMapping Create<TFrom, TTo>()
            where TTo : TFrom
        {
            return Create<TFrom, TTo>(null, InstantiationType.NewInstanceForEachRequest);
        }

        /// <summary>
        /// Helper methods to create type mapping objects more easily. Creates type mapping with
        /// specified key and instantiation type NewInstanceForEachRequest.
        /// </summary>
        /// <typeparam name="TFrom">the from type. </typeparam>
        /// <typeparam name="TTo">The target type</typeparam>
        /// <param name="key">The key to use. </param>
        /// <returns>the created type mapping</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static TypeMapping Create<TFrom, TTo>(string key)
            where TTo : TFrom
        {
            return Create<TFrom, TTo>(key, InstantiationType.NewInstanceForEachRequest);
        }

        /// <summary>
        /// Helper method to create type mapping objects more easily. Creates a type mapping with
        /// key (null) and specified instantiation type. 
        /// </summary>
        /// <typeparam name="TFrom">the from type</typeparam>
        /// <typeparam name="TTo">the target type</typeparam>
        /// <param name="instantiate">How to instantiate the types from this type mapping.</param>
        /// <returns>the created type mapping</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static TypeMapping Create<TFrom, TTo>(InstantiationType instantiate)
            where TTo : TFrom
        {
            return Create<TFrom, TTo>(null, instantiate);
        }

        /// <summary>
        /// Helper method to create type mapping objects more easily. Creates a type mapping with
        /// specified key and specified instantiation type. 
        /// </summary>
        /// <typeparam name="TFrom">the from type</typeparam>
        /// <typeparam name="TTo">the target type</typeparam>
        /// <param name="instantiate">How to instantiate the types from this type mapping.</param>
        /// <param name="key">The key to use. </param>
        /// <returns>the created type mapping</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static TypeMapping Create<TFrom, TTo>(string key, InstantiationType instantiate)
            where TTo : TFrom
        {
            return new TypeMapping(typeof(TFrom), typeof(TTo), key) { InstantiationType = instantiate };
        }

        /// <summary>
        /// Validates if the content of a type mapping is complete.
        /// </summary>
        /// <param name="mapping">The mapping to validate</param>
        public static void ValidateMapping(TypeMapping mapping)
        {
            Validation.ArgumentNotNull(mapping, "mapping");
            Validation.ArgumentNotNull(mapping.ToType, "mapping.ToType");
            Validation.ArgumentNotNull(mapping.FromType, "mapping.FromType");
            Validation.ArgumentNotNull(mapping.FromAssembly, "mapping.FromAssembly");
        }

        /// <summary>
        /// Overrides the equals for comparing a type mapping.
        /// </summary>
        /// <param name="o">The object to compare to this type mapping</param>
        /// <returns>True of the object provided is equlvalent to the type mapping, false otherwise</returns>
        public override bool Equals(object o)
        {
            var t = o as TypeMapping;

            if(t == null)
                return false;

            if (t.FromAssembly == this.FromAssembly &&
               t.FromType == this.FromType &&
               t.ToAssembly == this.ToAssembly &&
               t.ToType == this.ToType &&
               t.Key == this.Key)
                return true;

            return false;
        }

        /// <summary>
        /// Generates a hash code for the type mapping (overridden as a best practice when overriding equals).
        /// </summary>
        /// <returns>The hash code for the type mapping</returns>
        public override int GetHashCode()
        {
            string key = this.Key ?? string.Empty;
            string toAssem = this.ToAssembly ?? string.Empty;
            string toType = this.ToType ?? string.Empty;

            return (toAssem.GetHashCode() ^ toType.GetHashCode() ^ key.GetHashCode());
        }
    }
}
