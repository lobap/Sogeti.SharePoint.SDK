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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Contoso.TrainingManagement
{
    /// <summary>
    /// The ConfigurationService maintains configuration
    /// information for managing type mappings for
    /// services used by the Training Management application.
    /// </summary>
    public class ConfigurationService
    {
        #region Private Fields

        private readonly ICollection<TypeMetadata> typeMetadataList = new Collection<TypeMetadata>();
        private static readonly ConfigurationService instance = new ConfigurationService();

        #endregion

        #region Constructor
        
        static ConfigurationService()
        {
            LoadTypeMetadata();
        }

        private ConfigurationService()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the instance of the Configuration Service singleton
        /// </summary>
        /// <returns>Instance of the ConfigurationService singleton</returns>
        public static ConfigurationService GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// Returns the collection of type mappings for services
        /// </summary>
        /// <returns></returns>
        public ICollection<TypeMetadata> GetTypeMappings()
        {
            return typeMetadataList;
        }

        /// <summary>
        /// Clears the collection of type mappings
        /// </summary>
        public void Clear()
        {
            typeMetadataList.Clear();
        }

        public static void LoadTypeMetadata()
        {
            instance.typeMetadataList.Add(new TypeMetadata()
            {
                ContractAssemblyName =
                    "Contoso.HRManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ConcreteAssemblyName =
                      "Contoso.HRManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ContractTypeName = "Contoso.HRManagement.Services.IHRManager",
                ConcreteTypeName = "Contoso.HRManagement.Services.HRManager"
            });

            instance.typeMetadataList.Add(new TypeMetadata()
            {
                ContractAssemblyName =
                    "Contoso.AccountingManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ConcreteAssemblyName =
                    "Contoso.AccountingManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ContractTypeName = "Contoso.AccountingManagement.Services.IAccountingManager",
                ConcreteTypeName = "Contoso.AccountingManagement.Services.AccountingManager"
            });

            instance.typeMetadataList.Add(new TypeMetadata()
            {
                ContractAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ConcreteAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ContractTypeName = "Contoso.TrainingManagement.Repository.IListItemRepository",
                ConcreteTypeName = "Contoso.TrainingManagement.Repository.ListItemRepository"
            });

            instance.typeMetadataList.Add(new TypeMetadata()
            {
                ContractAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ConcreteAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ContractTypeName = "Contoso.TrainingManagement.Repository.ITrainingCourseRepository",
                ConcreteTypeName = "Contoso.TrainingManagement.Repository.TrainingCourseRepository"
            });

            instance.typeMetadataList.Add(new TypeMetadata()
            {
                ContractAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ConcreteAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ContractTypeName = "Contoso.TrainingManagement.Repository.IRegistrationRepository",
                ConcreteTypeName = "Contoso.TrainingManagement.Repository.RegistrationRepository"
            });

            instance.typeMetadataList.Add(new TypeMetadata()
            {
                ContractAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ConcreteAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ContractTypeName = "Contoso.TrainingManagement.Repository.IRegistrationApprovalTaskRepository",
                ConcreteTypeName = "Contoso.TrainingManagement.Repository.RegistrationApprovalTaskRepository"
            });

        }

        #endregion
    }
}
