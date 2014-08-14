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
using System.Linq;
using System.Security.Permissions;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

using Contoso.TrainingManagement.Repository;
using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement.Forms
{
    /// <summary>
    /// The CourseRegistrationPresenter class handles the 
    /// presentation logic for the Course Registration view.
    /// </summary>
    public class CourseRegistrationPresenter
    {
        #region Private Fields

        private readonly ICourseRegistrationView _view;

        #endregion

        #region Constructor

        public CourseRegistrationPresenter(ICourseRegistrationView view)
        {
            this._view = view;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Renders the view with content.
        /// </summary>
        /// <param name="web">The SPWeb in the current context.</param>
        /// <param name="loginName">The user in the current context.</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void RenderCourseRegistrationView(SPWeb web, string loginName)
        {
            if ( this._view.QueryString.AllKeys.Contains("ID") )
            {
                int courseId; 

                TrainingCourse course = null;
                Registration registration = null;
                bool success = false;
                SPUser user = null;

                Int32.TryParse(this._view.QueryString["ID"], out courseId);

                this._view.HeaderTitle = "Course Registration";
                this._view.SiteLink = web.Url;

                success = GetCourseUserRegistration(web, courseId, out course, out registration, out user, loginName);

                this._view.ShowConfirmationControls = success;

                if ( course != null )
                {
                    this._view.PageTitle = string.Format("Course Registration - {0}", course.Code);
                    this._view.HeaderSubtitle = course.Code;
                }

                if ( success )
                {
                    this._view.ContentMessage = string.Format("Would you like to register for course: {0}?", course.Code);
                }
            }
            else
            {
                this._view.HeaderTitle = "Course Registration";
                this._view.PageTitle = "Course Registration - New";
                this._view.HeaderSubtitle = "New";
                this._view.ContentMessage = "Which course would you like to register for?";
                this._view.SiteLink = web.Url;

                ITrainingCourseRepository trainingCourseRepository = ServiceLocator.GetInstance().Get<ITrainingCourseRepository>();
                this._view.Courses = trainingCourseRepository.Get(web);;

                this._view.ShowCourseSelectionControls = true;
                this._view.ShowConfirmationControls = true;
            }
        }

        /// <summary>
        /// Performs course registration.
        /// </summary>
        /// <param name="web">The SPWeb in the current context.</param>
        /// <param name="loginName">The user in the current context.</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Register(SPWeb web, string loginName)
        {
            int courseId;            

            TrainingCourse course = null;
            Registration registration = null;
            bool success = false;
            SPUser user = null;

            if ( this._view.QueryString.AllKeys.Contains("ID") )
            {
                success = Int32.TryParse(this._view.QueryString["ID"], out courseId);   
            }
            else
            {
                success = Int32.TryParse(this._view.SelectedCourse, out courseId);
            }

            this._view.HeaderTitle = "Course Registration";
            this._view.SiteLink = web.Url;

            if (!success) return;

            success = GetCourseUserRegistration(web, courseId, out course, out registration, out user, loginName);

            if ( success )
            {
                PerformRegistration(web, course, user);

                this._view.PageTitle = string.Format("Course Registration - {0}", course.Code);
                this._view.HeaderSubtitle = course.Code;
                this._view.ContentMessage = string.Format("Your registration request for {0} has been submitted.", course.Code);
                this._view.ShowConfirmationControls = false;
                this._view.ShowCourseSelectionControls = false;
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private bool GetCourseUserRegistration(SPWeb web, int courseId, out TrainingCourse course, out Registration registration, out SPUser user, string loginName)
        {
            user = null;
            registration = null;

            bool success;

            success = GetCourse(web, this._view, courseId, out course);
            success = success && GetUserRegistration(web, course, loginName, out user, out registration);

            if (!success)
            {
                this._view.PageTitle = "Course Registration - Failure";
                this._view.HeaderSubtitle = "Failure";
                this._view.ShowConfirmationControls = true;
            }
            return success;
        }
        
        /// <summary>
        /// Ensures the course related to the request is valid.
        /// </summary>
        /// <param name="web">The SPWeb in the current context.</param>
        /// <param name="view">The view to render content.</param>
        /// <param name="courseId">The ID of the training course to register for.</param>
        /// <param name="course">The course to populate.</param>
        /// <returns>Success flag.</returns>
        private static bool GetCourse(SPWeb web, ICourseRegistrationView view, int courseId, out TrainingCourse course)
        {
            bool success = false;

            ITrainingCourseRepository trainingCourseRepository = ServiceLocator.GetInstance().Get<ITrainingCourseRepository>();
            course = trainingCourseRepository.Get(courseId, web);

            if (course != null)
            {
                success = true;
            }
            else
            {
                view.ContentMessage = "The Course selected was not a valid.";
            }

            return success;
        }

        /// <summary>
        /// Ensures the registration request does not already exist. 
        /// </summary>
        /// <param name="web">The SPWeb in the current context.</param>
        /// <param name="course">The course related to the registration request.</param>
        /// <param name="loginName">The user requesting the registration.</param>
        /// <param name="user">The user to populate.</param>
        /// <param name="registration">The registration to populate if one already exists.</param>
        /// <returns>Success flag.</returns>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private bool GetUserRegistration(SPWeb web, TrainingCourse course, string loginName, out SPUser user, out Registration registration)
        {
            bool success = false;

            user = web.SiteUsers[loginName];

            IRegistrationRepository registrationRepository = ServiceLocator.GetInstance().Get<IRegistrationRepository>();
            registration = registrationRepository.Get(course.Id, user.ID, web);

            if (registration == null)
            {
                success = true;
            }
            else
            {
                this._view.ContentMessage = string.Format("A registration request for this Course with the status of '{0}' has already been submitted by you.", registration.RegistrationStatus);
            }

            return success;
        }

        /// <summary>
        /// Persits the registration request.
        /// </summary>
        /// <param name="web">The SPWeb in the current context.</param>
        /// <param name="course">The course related to the registration request.</param>
        /// <param name="user">The user related to the registration request.</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private static void PerformRegistration(SPWeb web, TrainingCourse course, SPUser user)
        {
            Registration registration = new Registration();
            registration.Title = String.Format("{0} - {1}", course.Code, user.Name);
            registration.CourseId = course.Id;
            registration.UserId = user.ID;
            registration.RegistrationStatus = "Pending";

            IRegistrationRepository registrationRepository = ServiceLocator.GetInstance().Get<IRegistrationRepository>();
            registrationRepository.Add(registration, web);
        }

        #endregion
    }
}
