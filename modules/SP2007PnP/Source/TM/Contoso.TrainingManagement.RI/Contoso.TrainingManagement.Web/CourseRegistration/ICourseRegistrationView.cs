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
using System.Collections.Generic;
using System.Collections.Specialized;
using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement.Forms
{
    /// <summary>
    /// The ICourseRegistrationView provides the interface
    /// between the Course Registration view and presenter.
    /// </summary>
    public interface ICourseRegistrationView
    {
        string PageTitle
        {
            set;
        }

        string HeaderTitle
        {
            set;
        }

        string HeaderSubtitle
        {
            set;
        }

        string ContentMessage
        {
            set;
        }

        bool ShowConfirmationControls
        {
            set;
        }

        IList<TrainingCourse> Courses
        {
            set;
        }

        bool ShowCourseSelectionControls
        {
            set;
        }

        NameValueCollection QueryString
        {
            get;
        }

        string SelectedCourse
        {
            get;
        }

        string SiteLink
        {
            set;
        }
    }
}
