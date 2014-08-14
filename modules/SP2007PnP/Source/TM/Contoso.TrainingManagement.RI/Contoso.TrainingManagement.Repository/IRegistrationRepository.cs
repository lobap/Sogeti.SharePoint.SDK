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

using Microsoft.SharePoint;

using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement.Repository
{
    public interface IRegistrationRepository
    {
        /// <summary>
        /// Designed to add a new Registration to the list
        /// </summary> 
        int Add(Registration registration, SPWeb web);

        /// <summary>
        /// Designed to get a Registration based on its Id
        /// </summary>
        Registration Get(int id, SPWeb web);

        /// <summary>
        /// Designed to get a Registration based on its courseId and userId
        /// </summary>
        Registration Get(int courseId, int userId, SPWeb web);

        /// <summary>
        /// Designed to update a Registration in the list
        /// </summary>
        void Update(Registration registration, SPWeb web);

        /// <summary>
        /// Designed to delete Registration from the list
        /// </summary>
        void Delete(int id, SPWeb web);

        /// <summary>
        /// Designed to get a internal field name from the list based on its Id key
        /// </summary>
        string GetFieldName(Guid key, SPWeb web);
    }
}
