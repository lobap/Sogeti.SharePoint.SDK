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
using System.Globalization;
using System.Text;

using Microsoft.SharePoint;

using Contoso.TrainingManagement.Common.Constants;
using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement.Repository
{
    /// <summary>
    /// The TrainingCourseRepository is responsible for managing the
    /// CRUD operations on Training Course List
    /// </summary>
    public class TrainingCourseRepository : BaseEntityRepository<TrainingCourse>, ITrainingCourseRepository
    {
        #region Properties

        protected override string ListName
        {
            get
            {
                return Lists.TrainingCourses;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Persists a new Training Course.
        /// </summary>
        /// <param name="trainingCourse">The Training Course to persist.</param>
        /// <param name="web">The SPWeb for the location of the Registration</param>
        /// <returns>The ID of the new Training Course.</returns>
        public int Add(TrainingCourse trainingCourse, SPWeb web)
        {
            return AddListItem(trainingCourse, web);
        }

        /// <summary>
        /// Returns a Training Course based on its ID.
        /// </summary>
        /// <param name="id">The ID of the Training Course to return.</param>
        /// <param name="web">The SPWeb for the location of the TrainingCourse</param>
        /// <returns>Training Course entity.</returns>
        public TrainingCourse Get(int id, SPWeb web)
        {
            StringBuilder queryBuilder = new StringBuilder("<Where>");
            queryBuilder.Append("<Eq><FieldRef Name='ID'/>");
            queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Value Type='Integer'>{0}</Value></Eq>", id));
            queryBuilder.Append("</Where>");

            return GetListItem(queryBuilder.ToString(), web);
        }

        /// <summary>
        /// Returns a Training Course based on its Course Code.
        /// </summary>
        /// <param name="courseCode">The Course Code of the Training Course to return.</param>
        /// <param name="web">The SPWeb for the location of the TrainingCourse</param>
        /// <returns>Training Course entity.</returns>
        public TrainingCourse Get(string courseCode, SPWeb web)
        {
            StringBuilder queryBuilder = new StringBuilder("<Where>");
            queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Eq><FieldRef ID='{0}'/>", Fields.TrainingCourseCode));
            queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Value Type='Text'>{0}</Value></Eq>", courseCode));
            queryBuilder.Append("</Where>");

            return GetListItem(queryBuilder.ToString(), web);
        }

        /// <summary>
        /// Returns a collection of all Training Courses.
        /// </summary>
        /// <param name="web">The SPWeb for the location of the TrainingCourse</param>
        /// <returns>Training Course collection.</returns>
        public IList<TrainingCourse> Get(SPWeb web)
        {
            return GetListItemList(web);
        }

        /// <summary>
        /// Persists a fully populated Training Course with updates.
        /// </summary>
        /// <param name="trainingCourse">The fully populated Training Course to update.</param>
        /// <param name="web">The SPWeb for the location of the TrainingCourse</param>
        public void Update(TrainingCourse trainingCourse, SPWeb web)
        {
            UpdateListItem(trainingCourse, web);
        }

        /// <summary>
        /// Deletes a Training Course.
        /// </summary>
        /// <param name="id">The ID of the Training Course to delete.</param>
        /// <param name="web">The SPWeb for the location of the TrainingCourse</param>
        public void Delete(int id, SPWeb web)
        {
            DeleteListItem(id, web);
        }

        protected override Dictionary<Guid, object> GatherParameters(TrainingCourse entity, SPWeb web)
        {
            Dictionary<Guid, object> fields = new Dictionary<Guid, object>();
            fields.Add(new Guid(Fields.Title), entity.Title);
            fields.Add(new Guid(Fields.TrainingCourseCode), entity.Code);
            fields.Add(new Guid(Fields.TrainingCourseDescription), entity.Description);
            fields.Add(new Guid(Fields.TrainingCourseEnrollmentDate), entity.EnrollmentDate);
            fields.Add(new Guid(Fields.TrainingCourseStartDate), entity.StartDate);
            fields.Add(new Guid(Fields.TrainingCourseEndDate), entity.EndDate);
            fields.Add(new Guid(Fields.TrainingCourseCost), entity.Cost);

            return fields;
        }

        protected override TrainingCourse PopulateEntity(SPListItem item)
        {
            TrainingCourse course = new TrainingCourse();
            course.Id = (int)item[new Guid(Fields.Id)];
            course.Title = (string)item[new Guid(Fields.Title)];
            course.Code = (string)item[new Guid(Fields.TrainingCourseCode)];
            course.Description = (string)item[new Guid(Fields.TrainingCourseDescription)];
            course.EnrollmentDate = (DateTime)item[new Guid(Fields.TrainingCourseEnrollmentDate)];
            course.StartDate = (DateTime)item[new Guid(Fields.TrainingCourseStartDate)];
            course.EndDate = (DateTime)item[new Guid(Fields.TrainingCourseEndDate)];
            course.Cost = Convert.ToSingle(item[new Guid(Fields.TrainingCourseCost)], CultureInfo.InvariantCulture);

            return course;
        }

        #endregion
    }
}