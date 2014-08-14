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
using System.Security.Permissions;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

using Contoso.TrainingManagement.Repository;
using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement.Forms
{
    /// <summary>
    /// RegistrationApprovalPresenter handles the presentation logic
    /// for the Registration Approval view.
    /// </summary>
    public class RegistrationApprovalPresenter
    {
        #region Private Fields

        private readonly IRegistrationApprovalView _view;

        #endregion

        #region Contructor

        public RegistrationApprovalPresenter(IRegistrationApprovalView view)
        {
            _view = view;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders the view with content.
        /// </summary>
        /// <param name="web">The SPWeb in the current context.</param>
        /// <param name="taskId">The ID of the approval task item.</param>
        /// <returns>Returns true if sucessful.</returns>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool RenderRegApprovalView(SPWeb web, string taskId)
        {
            Registration registration = null;
            TrainingCourse course = null;
            SPUser user = null;

            _view.HeaderTitle = "Registration Approval";

            bool success = GetTaskAndRegistration(web, _view, taskId, out registration);
            success = success && GetCourseAndUser(web, _view, registration, out course, out user);

            if (!success)
            {
                _view.PageTitle = "Registration Approval - Failure";
                _view.HeaderSubtitle = "Failure";
                _view.ShowConfirmationControls = false;
            }
            else
            {
                _view.PageTitle = "Registration Approval - " + user.Name;
                _view.HeaderSubtitle = user.Name;

                StringBuilder content = new StringBuilder();
                content.AppendLine(String.Format("Please approve or reject the registration request by {0} for course: {1} - {2}.",
                                  user.Name, course.Code, course.Title));
                content.AppendLine();
                content.AppendLine(String.Format("The {0} has been requested by {1}.", course.Title, user.Name));
                content.AppendLine(String.Format("The cost of this course is {0}.", course.Cost.ToString("C")));
                content.AppendLine(String.Format("{0} can be contacted at this alias: {1}.", user.Name, user.LoginName));

                _view.Message = content.ToString();
                _view.Status = new List<string>() { "Approved", "Rejected" };
                _view.ShowConfirmationControls = true;
            }

            return success;
        }

        /// <summary>
        /// Performs the registration approval process.
        /// </summary>
        /// <param name="web">The SPWeb of the current context</param>
        /// <param name="taskId">The ID of the approval task item.</param>
        /// <param name="registrationStatus">the Approved/Rejected status.</param>
        /// <returns>Returns true if successful.</returns>
        public bool ProcessApproval(SPWeb web, string taskId, string registrationStatus)
        {
            Registration registration = null;

            bool success = GetTaskAndRegistration(web, _view, taskId, out registration);

            if (success)
            {
                registration.RegistrationStatus = registrationStatus;
                IRegistrationRepository registrationRepository = ServiceLocator.GetInstance().Get<IRegistrationRepository>();
                registrationRepository.Update(registration, web);
            }

            return success;
        }

        #endregion

        #region Private Methods

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private static bool GetCourseAndUser(SPWeb web, IRegistrationApprovalView view, Registration registration, out TrainingCourse course, out SPUser user)
        {
            ITrainingCourseRepository trainingCourseRepository = ServiceLocator.GetInstance().Get<ITrainingCourseRepository>();
            course = trainingCourseRepository.Get(registration.CourseId, web);
            
            if (course == null)
            {
                view.Message = "The Course associated with the selected Approval Task is not valid.";
                user = null;
                return false;
            }
            user = GetSPUser(web, registration.UserId.ToString());
            if (user == null)
            {
                view.Message = "The Employee associated with the selected Approval Task is not valid.";
                return false;
            }

            return true;
        }

        private static bool GetTaskAndRegistration(SPWeb web, IRegistrationApprovalView view, string taskID, out Registration registration)
        {
            IRegistrationApprovalTaskRepository registrationApprovalTaskRepository = ServiceLocator.GetInstance().Get<IRegistrationApprovalTaskRepository>();
            RegistrationApprovalTask registrationApprovalTask = null;
           
            if (!String.IsNullOrEmpty(taskID))
            {
                int queryID;
                if (int.TryParse(taskID, out queryID))
                {
                    registrationApprovalTask = registrationApprovalTaskRepository.Get(queryID, web);
                }
            }

            if (registrationApprovalTask == null)
            {
                view.Message = "The Approval Task selected is not valid.";
                registration = null;
                return false;
            }

            IRegistrationRepository registrationRepository = ServiceLocator.GetInstance().Get<IRegistrationRepository>();
            int registrationID = registrationApprovalTask.WorkflowItemId;
            registration = registrationRepository.Get(registrationID, web);
            if (registration == null)
            {
                view.Message = "The Registration associated with the selected Approval Task is not valid.";
                return false;
            }

            return true;
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private static SPUser GetSPUser(SPWeb web, string userID)
        {
            return web.SiteUsers.GetByID(Convert.ToInt32(userID));
        }

        #endregion
    }
}
