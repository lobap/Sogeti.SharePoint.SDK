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

namespace Contoso.TrainingManagement.Repository.BusinessEntities
{
    /// <summary>
    /// The TrainingCourse class is a business entity wrapper
    /// for items based on the Training Course Content Type
    /// </summary>
    public class TrainingCourse : BaseEntity
    {
        #region Public Properties

        /// <summary>
        /// Unique 8 character code of a training course
        /// </summary>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// Detailed description of the training course
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// The deadline date for when a user can register
        /// for the training course
        /// </summary>
        public DateTime EnrollmentDate
        {
            get;
            set;
        }

        /// <summary>
        /// The starting date of the training course
        /// </summary>
        public DateTime StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// The ending date of the training course
        /// </summary>
        public DateTime EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// The cost of the training course
        /// </summary>
        public float Cost
        {
            get;
            set;
        }

        #endregion
    }
}
