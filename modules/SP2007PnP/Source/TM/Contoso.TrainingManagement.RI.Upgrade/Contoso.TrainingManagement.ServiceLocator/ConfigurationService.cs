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

        private readonly List<TypeMetadata> typeMetaDataList = new List<TypeMetadata>();
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
        /// Returns collection of type mappings
        /// </summary>
        /// <returns></returns>
        public IList<TypeMetadata> GetTypeMappings()
        {
            return typeMetaDataList;
        }

        /// <summary>
        /// Clears the collection of type mappings
        /// </summary>
        public void Clear()
        {
            typeMetaDataList.Clear();
        }

        private static void LoadTypeMetadata()
        {
            instance.typeMetaDataList.Add(new TypeMetadata()
            {
                ContractAssemblyName =
                    "Contoso.HRManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ConcreteAssemblyName =
                      "Contoso.HRManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ContractTypeName = "Contoso.HRManagement.Services.IHRManager",
                ConcreteTypeName = "Contoso.HRManagement.Services.HRManager"
            });

            instance.typeMetaDataList.Add(new TypeMetadata()
            {
                ContractAssemblyName =
                    "Contoso.AccountingManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ConcreteAssemblyName =
                    "Contoso.AccountingManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ContractTypeName = "Contoso.AccountingManagement.Services.IAccountingManager",
                ConcreteTypeName = "Contoso.AccountingManagement.Services.AccountingManager"
            });

            instance.typeMetaDataList.Add(new TypeMetadata()
            {
                ContractAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ConcreteAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ContractTypeName = "Contoso.TrainingManagement.Repository.IListItemRepository",
                ConcreteTypeName = "Contoso.TrainingManagement.Repository.ListItemRepository"
            });

            instance.typeMetaDataList.Add(new TypeMetadata()
            {
                ContractAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ConcreteAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ContractTypeName = "Contoso.TrainingManagement.Repository.ITrainingCourseRepository",
                ConcreteTypeName = "Contoso.TrainingManagement.Repository.TrainingCourseRepository"
            });

            instance.typeMetaDataList.Add(new TypeMetadata()
            {
                ContractAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ConcreteAssemblyName =
                    "Contoso.TrainingManagement.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5",
                ContractTypeName = "Contoso.TrainingManagement.Repository.IRegistrationRepository",
                ConcreteTypeName = "Contoso.TrainingManagement.Repository.RegistrationRepository"
            });

            instance.typeMetaDataList.Add(new TypeMetadata()
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
