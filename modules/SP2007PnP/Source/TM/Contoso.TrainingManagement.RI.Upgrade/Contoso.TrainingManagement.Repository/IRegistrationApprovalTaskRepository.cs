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

using Microsoft.SharePoint;

using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement.Repository
{
    public interface IRegistrationApprovalTaskRepository
    {
        /// <summary>
        /// Designed to add a new RegistrationApprovalTask to the list
        /// </summary> 
        int Add(RegistrationApprovalTask registrationApprovalTask, SPWeb web);

        /// <summary>
        /// Designed to get a RegistrationApprovalTask based on its Id
        /// </summary>
        RegistrationApprovalTask Get(int id, SPWeb web);

        /// <summary>
        /// Designed to get all RegistrationApprovalTask entities from the list
        /// </summary> 
        IList<RegistrationApprovalTask> Get(SPWeb web);

        /// <summary>
        /// Designed to update a RegistrationApprovalTask in the list
        /// </summary>
        void Update(RegistrationApprovalTask registrationApprovalTask, SPWeb web);

        /// <summary>
        /// Designed to delete RegistrationApprovalTask from the list
        /// </summary>
        void Delete(int id, SPWeb web);

        /// <summary>
        /// Designed to get a internal field name from the list based on its Id key
        /// </summary>
        string GetFieldName(Guid key, SPWeb web);
    }
}
