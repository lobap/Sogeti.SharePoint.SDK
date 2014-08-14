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
    public class RegistrationApprovalTaskRepository : BaseEntityRepository<RegistrationApprovalTask>, IRegistrationApprovalTaskRepository
    {
        #region Public Properties

        protected override string ListName
        {
            get
            {
                return Lists.RegistrationApprovalTasks;
            }
        }

        #endregion

        #region Methods

        #region IRegistrationApprovalTaskRepository Members

        /// <summary>
        /// Persists a new RegistrationApprovalTask.
        /// </summary>
        /// <param name="registrationApprovalTask">The RegistrationApprovalTask to persist.</param>
        /// <param name="web">The SPWeb for the location of the RegistrationApprovalTask</param>
        /// <returns>The ID of the new RegistrationApprovalTask.</returns>
        public int Add(RegistrationApprovalTask registrationApprovalTask, SPWeb web)
        {
            return AddListItem(registrationApprovalTask, web);
        }

        /// <summary>
        /// Returns a RegistrationApprovalTask based on its ID.
        /// </summary>
        /// <param name="id">The ID of the RegistrationApprovalTask to return.</param>
        /// <param name="web">The SPWeb for the location of the RegistrationApprovalTask</param>
        /// <returns>RegistrationApprovalTask entity.</returns>
        public RegistrationApprovalTask Get(int id, SPWeb web)
        {
            StringBuilder queryBuilder = new StringBuilder("<Where>");
            queryBuilder.Append("<Eq><FieldRef Name='ID'/>");
            queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Value Type='Integer'>{0}</Value></Eq>", id));
            queryBuilder.Append("</Where>");

            return GetListItem(queryBuilder.ToString(), web);
        }

        /// <summary>
        /// Returns a collection of all RegistrationApprovalTask.
        /// </summary>
        /// <param name="web">The SPWeb for the location of the RegistrationApprovalTask</param>
        /// <returns>RegistrationApprovalTask collection.</returns>
        public IList<RegistrationApprovalTask> Get(SPWeb web)
        {
            return GetListItemList(web);
        }

        /// <summary>
        /// Persists a fully populated RegistrationApprovalTask with updates.
        /// </summary>
        /// <param name="registrationApprovalTask">The fully populated RegistrationApprovalTask to update.</param>
        /// <param name="web">The SPWeb for the location of the RegistrationApprovalTask</param>
        public void Update(RegistrationApprovalTask registrationApprovalTask, SPWeb web)
        {
            UpdateListItem(registrationApprovalTask, web);
        }

        /// <summary>
        /// Deletes a RegistrationApprovalTask.
        /// </summary>
        /// <param name="id">The ID of the RegistrationApprovalTask to delete.</param>
        /// <param name="web">The SPWeb for the location of the RegistrationApprovalTask</param>
        public void Delete(int id, SPWeb web)
        {
            DeleteListItem(id, web);
        }

        #endregion

        protected override Dictionary<Guid, object> GatherParameters(RegistrationApprovalTask entity, SPWeb web)
        {
            Dictionary<Guid, object> fields = new Dictionary<Guid, object>();
            fields.Add(new Guid(Fields.Title), entity.Title);
            fields.Add(new Guid(Fields.WorkflowItemId), entity.WorkflowItemId);

            return fields;
        }

        protected override RegistrationApprovalTask PopulateEntity(SPListItem item)
        {
            RegistrationApprovalTask registrationApprovalTask = new RegistrationApprovalTask();
            registrationApprovalTask.Id = (int)item[new Guid(Fields.Id)];
            registrationApprovalTask.Title = (string)item[new Guid(Fields.Title)];
            registrationApprovalTask.WorkflowItemId = (int)item[new Guid(Fields.WorkflowItemId)];

            return registrationApprovalTask;
        }

        #endregion
    }
}
