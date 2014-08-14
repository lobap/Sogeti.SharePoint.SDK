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
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using VSeWSS;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

using Contoso.TrainingManagement.Common.Constants;
using Contoso.TrainingManagement.Repository;
using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement
{
    /// <summary>
    /// The TrainingCourseItemEventReceiver inherits from the SPItemEventReceiver
    /// base class and provides validation code during the ItemAdding and ItemUpdating
    /// events for items based on TrainingCourse Content Type.
    /// </summary>
    [TargetContentType("0x01000cee433cd7484dc5adbec636a5dd0c07")]
    [Guid("813bfe9b-29e9-4dc0-bb29-82bf3e53bb19")]
    public class TrainingCourseItemEventReceiver : SPItemEventReceiver
    {
        #region Methods

        /// <summary>
        /// Synchronous before event that occurs when a new item is added to its containing object.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPItemEventProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemAdding(SPItemEventProperties properties)
        {
            bool isValid = true;
            StringBuilder errorMessage = new StringBuilder();

            string title = string.Empty;
            string code = string.Empty;
            DateTime enrollmentDate = DateTime.MinValue;
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            float cost = 0;

            //A web is required to retrieve Field names for iterating through ListItem collection
            //in the SPItemEventProperties as well as retrieveing existing items in the TrainingCourses list.
            using (SPWeb web = properties.OpenWeb())
            {
                ITrainingCourseRepository repository = ServiceLocator.GetInstance().Get<ITrainingCourseRepository>();
                Initalize(properties.AfterProperties, repository, web, out title, out code, out enrollmentDate, out startDate, out endDate, out cost);

                if (ValidateCourseCodeExists(repository, code, errorMessage, web))
                {
                    isValid = false;
                }
            }

            isValid = isValid & ValidateCourseCode(code, errorMessage);
            isValid = isValid & ValidateEnrollmentDate(enrollmentDate, errorMessage);
            isValid = isValid & ValidateStartDate(enrollmentDate, startDate, errorMessage);
            isValid = isValid & ValidateEndDate(startDate, endDate, errorMessage);
            isValid = isValid & ValidateCourseCost(cost, errorMessage);

            //if any of the rules fail, set an error message and set the
            //Cancel property to true to instruct SharePoint to redirect to 
            //standard error page.
            if (!isValid)
            {
                properties.ErrorMessage = errorMessage.ToString();
                properties.Cancel = true;
            }
        }     

        /// <summary>
        /// Synchronous before event that occurs when an existing item is changed, for example, when the user changes data in one or more fields.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPItemEventProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            bool isValid = true;
            StringBuilder errorMessage = new StringBuilder();

            string title = string.Empty;
            string code = string.Empty;
            DateTime enrollmentDate = DateTime.MinValue;
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            float cost = 0;

            using (SPWeb web = properties.OpenWeb())
            {
                ITrainingCourseRepository repository = ServiceLocator.GetInstance().Get<ITrainingCourseRepository>();
                Initalize(properties.AfterProperties, repository, web, out title, out code, out enrollmentDate, out startDate, out endDate, out cost);

                if (properties.ListItem[repository.GetFieldName(new Guid(Fields.TrainingCourseCode), web)].ToString() != code)
                {
                    if (ValidateCourseCodeExists(repository, code, errorMessage, web))
                    {
                        isValid = false;
                    }
                }
            }

            isValid = isValid & ValidateCourseCode(code, errorMessage);
            isValid = isValid & ValidateStartDate(enrollmentDate, startDate, errorMessage);
            isValid = isValid & ValidateEndDate(startDate, endDate, errorMessage);
            isValid = isValid & ValidateCourseCost(cost, errorMessage);

            //if any of the rules fail, set an error message and set the
            //Cancel property to true to instruct SharePoint to redirect to 
            //standard error page.
            if (!isValid)
            {
                properties.ErrorMessage = errorMessage.ToString();
                properties.Cancel = true;
            }
        }

        #endregion

        #region Private Methods

        private static void Initalize(SPItemEventDataCollection afterProperties, ITrainingCourseRepository repository, SPWeb web, out string title, out string code, out DateTime enrollmentDate, out DateTime startDate, out DateTime endDate, out float cost)
        {
            title = ((string)afterProperties[repository.GetFieldName(new Guid(Fields.Title), web)]).Trim();
            code = ((string)afterProperties[repository.GetFieldName(new Guid(Fields.TrainingCourseCode), web)]).Trim();
            enrollmentDate = Convert.ToDateTime(afterProperties[repository.GetFieldName(new Guid(Fields.TrainingCourseEnrollmentDate), web)]).ToUniversalTime(); ;
            startDate = Convert.ToDateTime(afterProperties[repository.GetFieldName(new Guid(Fields.TrainingCourseStartDate), web)]).ToUniversalTime(); ;
            endDate = Convert.ToDateTime(afterProperties[repository.GetFieldName(new Guid(Fields.TrainingCourseEndDate), web)]).ToUniversalTime();
            cost = Convert.ToSingle(afterProperties[repository.GetFieldName(new Guid(Fields.TrainingCourseCost), web)]);

            afterProperties[repository.GetFieldName(new Guid(Fields.Title), web)] = title;
            afterProperties[repository.GetFieldName(new Guid(Fields.TrainingCourseCode), web)] = code;
        }

        private static bool ValidateCourseCodeExists(ITrainingCourseRepository repository, string courseCode, StringBuilder errorMessage, SPWeb web)
        {
            bool courseExists = false;

            TrainingCourse course = new TrainingCourse();
            
            course = repository.Get(courseCode, web);

            if ( course != null )
            {
                courseExists = true;
                errorMessage.AppendLine("The Course Code is already in use.");
            }

            return courseExists;
        }

        private static bool ValidateCourseCode(string courseCode, StringBuilder errorMessage)
        {
            bool isValid = true;

            if (courseCode.Length != 8)
            {
                isValid = false;
                errorMessage.AppendLine("The Course Code must be 8 characters long.");
            }

            return isValid;
        }

        private static bool ValidateEnrollmentDate(DateTime enrollmentDate, StringBuilder errorMessage)
        {
            bool isValid = true;

            if (enrollmentDate < DateTime.Today)
            {
                isValid = false;
                errorMessage.AppendLine("The Enrollment Deadline Date can not be before today's date.");
            }

            return isValid;
        }

        private static bool ValidateStartDate(DateTime enrollmentDate, DateTime startDate, StringBuilder errorMessage)
        {
            bool isValid = true;

            if (startDate < enrollmentDate)
            {
                isValid = false;
                errorMessage.AppendLine("The Start Date can not be before the Enrollment Deadline Date.");
            }

            return isValid;
        }

        private static bool ValidateEndDate(DateTime startDate, DateTime endDate, StringBuilder errorMessage)
        {
            bool isValid = true;

            if (endDate < startDate)
            {
                isValid = false;
                errorMessage.AppendLine("The End Date can not be before the Start Date.");
            }

            return isValid;
        }

        private static bool ValidateCourseCost(float cost, StringBuilder errorMessage)
        {
            bool isValid = true;

            if (cost < 0)
            {
                isValid = false;
                errorMessage.AppendLine("Negative values are not allowed for Cost.");
            }

            return isValid;
        }

        #endregion
    }
}
