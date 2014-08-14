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
    public class MockTrainingCourseRepository : ITrainingCourseRepository
    {
        #region Properties

        public static TrainingCourse TrainingCourseReturnedByGet { get; set; }
        public static IList<TrainingCourse> TrainingCourseListReturnedByGet
        {
            get;
            set;
        }

        #endregion

        #region Public Methods

        public static void Clear()
        {
            TrainingCourseReturnedByGet = null;
            TrainingCourseListReturnedByGet = null;
        }

        #endregion

        #region ITrainingCourseRepository Members

        public TrainingCourse Get(int id, SPWeb web)
        {
            return TrainingCourseReturnedByGet;
        }

        public TrainingCourse Get(string courseCode, SPWeb web)
        {
            return TrainingCourseReturnedByGet;
        }

        public IList<TrainingCourse> Get(SPWeb web)
        {
            return TrainingCourseListReturnedByGet;
        }

        public int Add(TrainingCourse trainingCourse, SPWeb web)
        {
            return 1;
        }

        public void Delete(int id, SPWeb web)
        {
            return;
        }

        public void Update(TrainingCourse trainingCourse, SPWeb web)
        {
            return;
        }

        public string GetFieldName(Guid key, SPWeb web)
        {
            switch (key.ToString().ToUpper())
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
