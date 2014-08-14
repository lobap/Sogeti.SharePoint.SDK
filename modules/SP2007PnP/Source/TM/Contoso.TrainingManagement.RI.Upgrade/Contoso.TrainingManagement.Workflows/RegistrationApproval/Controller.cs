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
using Contoso.AccountingManagement.BusinessEntities;
using Contoso.AccountingManagement.Services;
using Contoso.HRManagement.Services;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Contoso.TrainingManagement.Common.Constants;
using Contoso.TrainingManagement.Repository;
using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement.Workflows.RegistrationApproval
{
    /// <summary>
    /// The Controller class contains
    /// the business logic for the custom activities in the 
    /// Registration Approval Workflow.
    /// </summary>
    public class Controller
    {
        #region Private Fields

        private readonly ServiceLocator serviceLocator;
        private const string approvedString = "Approved";
        private const string rejectedString = "Rejected";
        private const string trainingBucketString = "Training";

        #endregion

        #region Contructor

        public Controller()
        {
            serviceLocator = ServiceLocator.GetInstance();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Populates the SPWorkflowTaskProperties with Title and AssignedTo data.
        /// </summary>
        /// <param name="taskProperties">SPWorkflowTaskProperties to populate.</param>
        /// <param name="web">The SPWeb in the current workflow context.</param>
        /// <param name="workflowItem">The SPListItem that the workflow instance is associated with.</param>
        public void PopulateManagerApprovalTaskProperties(SPWorkflowTaskProperties taskProperties, SPWeb web, SPListItem workflowItem)
        {
            IHRManager hrManager = serviceLocator.Get<IHRManager>();
            IRegistrationRepository registrationRepository = serviceLocator.Get<IRegistrationRepository>();
            ITrainingCourseRepository trainingCourseRepository = serviceLocator.Get<ITrainingCourseRepository>();

            //get registration and training course related to this task.
            Registration registration = registrationRepository.Get(workflowItem.ID, web);
            TrainingCourse trainingCourse = trainingCourseRepository.Get(registration.CourseId, web);

            //get the user and manager related to this registration.
            SPUser user = GetSPUser(web, registration.UserId);
            SPUser manager = GetSPUser(web, hrManager.GetManager(user.LoginName));

            taskProperties.AssignedTo = manager.LoginName;
            taskProperties.Title = String.Format("Approve new registration request from {0} for {1}.", user.Name, trainingCourse.Code);
        }

        /// <summary>
        /// Returns a flag indicating if the approval task is complete.
        /// </summary>
        /// <param name="workflowItem">The SPListItem that the workflow instance is associated with.</param>
        /// <returns>If the approval task is complete.</returns>
        public static bool IsManagerApprovalTaskComplete(SPListItem workflowItem)
        {
            bool isComplete = false;

            string status = workflowItem[new Guid(Fields.RegistrationStatus)].ToString();

            switch ( status )
            {
                case approvedString:
                    isComplete = true;
                    break;
                case rejectedString:
                    isComplete = true;
                    break;
                default:
                    isComplete = false;
                    break;
            }

            return isComplete;
        }

        /// <summary>
        /// Returns a flag indicating if the approval task is approved or rejected.
        /// </summary>
        /// <param name="workflowItem">The SPListItem that the workflow instance is associated with.</param>
        /// <returns>If the approval task is approved.</returns>
        public static bool IsManagerApprovalTaskApproved(SPListItem workflowItem)
        {
            bool isApproved = false;

            string status = workflowItem[new Guid(Fields.RegistrationStatus)].ToString();

            switch ( status )
            {
                case approvedString:
                    isApproved = true;
                    break;
                default:
                    isApproved = false;
                    break;
            }

            return isApproved;
        }

        /// <summary>
        /// Charges the Accounting service for the cost of the course registration.
        /// </summary>
        /// <param name="web">The SPWeb in the current workflow context.</param>
        /// <param name="workflowItem">The SPListItem that the workflow instance is associated with.</param>
        public void ChargeAccounting(SPWeb web, SPListItem workflowItem)
        {
            IHRManager hrManager = serviceLocator.Get<IHRManager>();
            IAccountingManager accountingManager = serviceLocator.Get<IAccountingManager>();
            IRegistrationRepository registrationRepository = serviceLocator.Get<IRegistrationRepository>();
            ITrainingCourseRepository trainingCourseRepository = serviceLocator.Get<ITrainingCourseRepository>();

            //get registration and training course related to this task.
            Registration registration = registrationRepository.Get(workflowItem.ID, web);
            TrainingCourse trainingCourse = trainingCourseRepository.Get(registration.CourseId, web);

            //get the user related to this registration
            SPUser user = GetSPUser(web, registration.UserId);

            //construct the transaction related to this approved registration
            Transaction tran = new Transaction();
            tran.Amount = trainingCourse.Cost;
            tran.CostCenter = hrManager.GetCostCenter(user.LoginName);
            tran.Bucket = trainingBucketString;
            tran.Description = String.Format("{0} training course registration by {1}.", trainingCourse.Title, user.Name);

            accountingManager.SaveTransaction(tran);
        }

        #endregion

        #region Private Methods

        private static SPUser GetSPUser(SPWeb web, int userID)
        {
            return web.SiteUsers.GetByID(userID);
        }

        private static SPUser GetSPUser(SPWeb web, string loginName)
        {
            SPUser user = null;

            try
            {
                user = web.SiteUsers[loginName];
            }
            catch ( Exception ex )
            {
                throw new ArgumentException(String.Format("Request for user: {0}. {1}", loginName, ex.Message));
            }

            return user;
        }

        #endregion
    }
}
