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
using Contoso.TrainingManagement.Repository;
using Contoso.TrainingManagement.Repository.BusinessEntities;
using Microsoft.SharePoint;

namespace Contoso.TrainingManagement.Mocks
{
    public class MockRegistrationRepository : IRegistrationRepository
    {
        #region Public Fields

        public static Registration RegistrationReturnedByGet
        {
            get;
            set;
        }
        public static Registration UpdateCalledWithRegistrationParam
        {
            get;
            set;
        }

        #endregion

        #region Public Methods

        public static void Clear()
        {
            RegistrationReturnedByGet = null;
            UpdateCalledWithRegistrationParam = null;
        }

        #endregion

        #region IRegistrationRepository Members

        public int Add(Registration registration, SPWeb web)
        {
            RegistrationReturnedByGet = registration;
            return 1;
        }

        public void Delete(int id, SPWeb web)
        {
            throw new System.NotImplementedException();
        }

        public Registration Get(int id, SPWeb web)
        {
            return RegistrationReturnedByGet;
        }

        public Registration Get(int courseId, int userId, SPWeb web)
        {
            return RegistrationReturnedByGet;
        }

        public void Update(Registration registration, SPWeb web)
        {
            UpdateCalledWithRegistrationParam = registration;
        }

        public string GetFieldName(Guid key, SPWeb web)
        {
            switch ( key.ToString().ToUpper() )
            {
                case "FA564E0F-0C70-4AB9-B863-0177E6DDD247":
                    return "Title";
                case "E5509750-CB71-4DE3-873D-171BA6448FA5":
                    return "TrainingCourseCode";
                case "7E4004FA-D0BE-4611-A817-65D17CF11A6A":
                    return "TrainingCourseCost";
                case "8E39DAD4-65FA-4395-BA0C-43BF52586B3E":
                    return "TrainingCourseDescription";
                case "43568365-8448-4130-831C-98C074B61E89":
                    return "TrainingCourseEnrollmentDate";
                case "AE2A0BBD-F22E-41DC-8753-451067122318":
                    return "TrainingCourseStartDate";
                case "F5E6F566-FA7C-4883-BF7F-006727760E22":
                    return "TrainingCourseEndDate";
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
