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
namespace Contoso.TrainingManagement.Repository.BusinessEntities
{
    /// <summary>
    /// The Registration class is a business entity wrapper
    /// for items based on the Registration Content Type
    /// </summary>
    public class Registration : BaseEntity
    {
        #region Public Properties

        /// <summary>
        /// Represents the Id of the Training Course list item
        /// </summary>
        public int CourseId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the Id of the registered user
        /// </summary>
        public int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the status of the registration.
        /// The valid statuses are Pending, Approved or Rejected.
        /// </summary>
        public string RegistrationStatus
        {
            get;
            set;
        }

        #endregion
    }
}
