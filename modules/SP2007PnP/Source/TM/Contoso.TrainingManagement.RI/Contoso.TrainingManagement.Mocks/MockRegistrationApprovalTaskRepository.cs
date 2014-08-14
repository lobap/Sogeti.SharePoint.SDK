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
using Contoso.TrainingManagement.Repository;
using Contoso.TrainingManagement.Repository.BusinessEntities;
using Microsoft.SharePoint;

namespace Contoso.TrainingManagement.Mocks
{
    public class MockRegistrationApprovalTaskRepository : IRegistrationApprovalTaskRepository
    {
        #region Properties

        public static RegistrationApprovalTask RegistrationApprovalTaskReturnedByGet
        {
            get;
            set;
        }

        #endregion

        #region Public Methods

        public static void Clear()
        {
            RegistrationApprovalTaskReturnedByGet = null;
        }

        #endregion

        #region IRegistrationApprovalTaskRepository Members

        public int Add(RegistrationApprovalTask registrationApprovalTask, SPWeb web)
        {
            return 1;
        }

        public RegistrationApprovalTask Get(int id, SPWeb web)
        {
            return RegistrationApprovalTaskReturnedByGet;
        }

        public IList<RegistrationApprovalTask> Get(SPWeb web)
        {
            throw new NotImplementedException();
        }

        public void Update(RegistrationApprovalTask registrationApprovalTask, SPWeb web)
        {
            return;
        }

        public void Delete(int id, SPWeb web)
        {
            return;
        }

        public string GetFieldName(Guid key, SPWeb web)
        {
            switch ( key.ToString().ToUpper() )
            {
                case "FA564E0F-0C70-4AB9-B863-0177E6DDD247":
                    return "Title";
                case "8E234C69-02B0-42D9-8046-D5F49BF0174F":
                    return "WorkflowItemId";
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
