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
namespace Contoso.TrainingManagement
{
    /// <summary>
    /// TypeMeta contains information about a service's type information
    /// </summary>
    public class TypeMetadata
    {
        #region Properties

        /// <summary>
        /// The name of the assembly containing the service contract
        /// </summary>
        public string ContractAssemblyName
        {
            get; 
            set;
        }

        /// <summary>
        /// The name of the assembly cotaining the concrete service
        /// </summary>
        public string ConcreteAssemblyName
        {
            get; 
            set;
        }

        /// <summary>
        /// The name of the type for the service contract
        /// </summary>
        public string ContractTypeName
        {
            get; 
            set;
        }

        /// <summary>
        /// The name of the type for concrete service
        /// </summary>
        public string ConcreteTypeName
        {
            get; 
            set;
        }

        #endregion
    }
}
