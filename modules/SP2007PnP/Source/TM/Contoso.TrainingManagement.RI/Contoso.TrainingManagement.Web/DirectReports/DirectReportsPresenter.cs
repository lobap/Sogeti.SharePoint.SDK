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

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

using Contoso.HRManagement.Services;

namespace Contoso.TrainingManagement.ControlTemplates
{
    /// <summary>
    /// The DirectReportsPresenter class handles the
    /// presentation logic for the Direct Reports Web Part.
    /// </summary>
    public class DirectReportsPresenter
    {
        #region Private Fields

        private readonly IDirectReportsView view;
        private readonly IHRManager hrManager;

        #endregion

        #region Contructor

        public DirectReportsPresenter(IDirectReportsView view)
        {
            ServiceLocator serviceLocator = ServiceLocator.GetInstance();

            this.view = view;
            this.hrManager = serviceLocator.Get<IHRManager>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The SetDirectReportsSource method rertrieves direct reports
        /// for the current user and displays them.
        /// </summary>
        /// <param name="sourceUrl">The url of the current page to attach as the source url for the User Disp page cancel button.</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void SetDirectReportsSource(string sourceUrl)
        {
            bool isEmpty = true;

            if (this.view.Web != null && !String.IsNullOrEmpty(this.view.LoginName))
            {
                IList<string> directReports = hrManager.GetDirectReports(this.view.LoginName);

                //does the current user have direct reports
                if ( directReports.Count > 0 )
                {
                    Dictionary<int, string> usersInfo = new Dictionary<int, string>();

                    foreach ( string directReport in directReports )
                    {
                        SPUser user = GetSPUser(this.view.Web, directReport);
                        if ( user != null )
                        {
                            //show the login name for the direct report
                            if ( this.view.ShowLogin )
                            {
                                usersInfo.Add(user.ID, string.Format("{0} ({1})", user.Name, user.LoginName));
                            }
                            else
                            {
                                usersInfo.Add(user.ID, user.Name);
                            }
                        }
                    }

                    if ( usersInfo.Count > 0 )
                    {
                        view.DirectReports = usersInfo;
                        view.UserDisplayUrl = "/_layouts/userdisp.aspx?ID=";
                        view.SourceUrl = string.Format("&Source={0}", sourceUrl);
                        view.DirectReportsMessage = string.Format("You have {0} direct report(s).", usersInfo.Count);
                        isEmpty = false;
                    }
                }

                if ( isEmpty )
                {
                    view.DirectReportsMessage = "No direct reports were found.";
                }
            }
            else
            {
                view.DirectReportsMessage = "An unexpected error occurred.";
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private static SPUser GetSPUser(SPWeb web, string login)
        {
            string[] userQuery = { login };
            SPUserCollection usersFound = web.SiteUsers.GetCollection(userQuery);
            if ( ( usersFound != null ) && ( usersFound.Count == 1 ) && ( usersFound[0].LoginName == login ) )
            {
                return usersFound[0];
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
