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
    /// The RegistrationRepository is responsible for managing the
    /// CRUD operations on the Registration list.
    /// </summary>
    public class RegistrationRepository : BaseEntityRepository<Registration>, IRegistrationRepository
    {
        #region Properties

        protected override string ListName
        {
            get { return Lists.Registrations; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Persists a new Registration.
        /// </summary>
        /// <param name="registration">The Registration to persist.</param>
        /// <param name="web">The SPWeb for the location of the Registration</param>
        /// <returns>The ID of the new Registration.</returns>
        public int Add(Registration registration, SPWeb web)
        {
            return AddListItem(registration, web);
        }

        /// <summary>
        /// Returns a Registration based on its ID.
        /// </summary>
        /// <param name="id">The ID of the Registration to return.</param>
        /// <param name="web">The SPWeb for the location of the Registration</param>
        /// <returns>Registration entity.</returns>
        public Registration Get(int id, SPWeb web)
        {
            StringBuilder queryBuilder = new StringBuilder("<Where>");
            queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Eq><FieldRef Name='ID'/>"));
            queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Value Type='Integer'>{0}</Value></Eq>", id));
            queryBuilder.Append("</Where>");

            return GetListItem(queryBuilder.ToString(), web);
        }

        /// <summary>
        /// Returns a Registration based on its Course ID and User ID
        /// </summary>
        /// <param name="courseId">The Course ID of the Registration to return.</param>
        /// <param name="userId">The User ID of the Registration to return.</param>
        /// <param name="web">The SPWeb for the location of the Registration</param>
        /// <returns>Registration entity.</returns>
        public Registration Get(int courseId, int userId, SPWeb web)
        {
            StringBuilder queryBuilder = new StringBuilder("<Where>");
            queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<And><Eq><FieldRef ID='{0}'/>", Fields.CourseId));
            queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Value Type='Integer'>{0}</Value></Eq>", courseId));
            queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Eq><FieldRef ID='{0}'/>", Fields.UserId));
            queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Value Type='Integer'>{0}</Value></Eq></And>", userId));
            queryBuilder.Append("</Where>");

            return GetListItem(queryBuilder.ToString(), web);
        }

        /// <summary>
        /// Persists a fully populated Registration with updates.
        /// </summary>
        /// <param name="registration">The fully populated Registration to update.</param>
        /// <param name="web">The SPWeb for the location of the Registration</param>
        public void Update(Registration registration, SPWeb web)
        {
            UpdateListItem(registration, web);
        }

        /// <summary>
        /// Deletes a Registration.
        /// </summary>
        /// <param name="id">The ID of the Registration to delete.</param>
        /// <param name="web">The SPWeb for the location of the Registration</param>
        public void Delete(int id, SPWeb web)
        {
            DeleteListItem(id, web);
        }

        protected override Dictionary<Guid, object> GatherParameters(Registration entity, SPWeb web)
        {
            Dictionary<Guid, object> fields = new Dictionary<Guid, object>();
            fields.Add(new Guid(Fields.Title), entity.Title);
            fields.Add(new Guid(Fields.CourseId), entity.CourseId);
            fields.Add(new Guid(Fields.UserId), entity.UserId);
            fields.Add(new Guid(Fields.User), web.SiteUsers.GetByID(entity.UserId));
            fields.Add(new Guid(Fields.RegistrationStatus), entity.RegistrationStatus);

            return fields;
        }

        protected override Registration PopulateEntity(SPListItem item)
        {
            Registration registration = new Registration();
            registration.Id = (int)item[new Guid(Fields.Id)];
            registration.Title = (string)item[new Guid(Fields.Title)];
            registration.CourseId = (int)item[new Guid(Fields.CourseId)];
            registration.UserId = (int)item[new Guid(Fields.UserId)];
            registration.RegistrationStatus = (string)item[new Guid(Fields.RegistrationStatus)];

            return registration;
        }

        #endregion
    }
}
