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
    public interface ITrainingCourseRepository
    {
        /// <summary>
        /// Designed to add a new TrainingCourse to the list
        /// </summary> 
        int Add(TrainingCourse trainingCourse, SPWeb web);

        /// <summary>
        /// Designed to get a TrainingCourse based on its Id
        /// </summary> 
        TrainingCourse Get(int id, SPWeb web);

        /// <summary>
        /// Designed to get a TrainingCourse based on its courseCode
        /// </summary> 
        TrainingCourse Get(string courseCode, SPWeb web);

        /// <summary>
        /// Designed to get all TrainingCourse entities from the list
        /// </summary> 
        IList<TrainingCourse> Get(SPWeb web);

        /// <summary>
        /// Designed to update a TrainingCourse in the list
        /// </summary> 
        void Update(TrainingCourse trainingCourse, SPWeb web);

        /// <summary>
        /// Designed to delete TrainingCourse from the list
        /// </summary> 
        void Delete(int id, SPWeb web);

        /// <summary>
        /// Designed to get a internal field name from the list based on its Id key
        /// </summary> 
        string GetFieldName(Guid key, SPWeb web);
    }
}
