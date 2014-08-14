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
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Globalization;
using TrainingManagementAcceptanceTest.HelperClasses;
using Microsoft.SharePoint.Workflow;

namespace TrainingManagementAcceptanceTest.HelperClasses
{  
    /// <summary>
    /// This Class contains all sharepoint calls to findount courseid, registrationid 
    /// </summary>
    static public class SharePointCalls
    {
        #region Methods
        /// <summary>
        /// The get method will get RegApprovalId from workflow attached to given registration
        /// </summary>
        /// <param name="regItem">Registratoin List item with workflow</param>
        /// <returns>RegApprovalId</returns>
        static public int GetRegApprovalID(SPListItem regItem)
        {
            int regApprovalID = 0;
            if (regItem != null)
                 {
                        SPWorkflowCollection workflows = regItem.Workflows;
                        foreach (SPWorkflow workflow in workflows)
                        {
                            if ( workflow.InternalState == SPWorkflowState.Running )
                                regApprovalID = workflow.Tasks[0].ID;
                        }

                 }
            
            return regApprovalID;
        }
        /// <summary>
        /// This Method findout the courseid for given CourseCode in a particular site & web
        /// </summary>
        /// <param name="CourseCode">CourseCode</param>
        /// <param name="siteURL">Site URL</param>
        /// <param name="webURL">Sub site URL(web URL)</param>
        /// <returns>CourseId. Returns 0 if not exists.</returns>
        static public int GetCourseID(string CourseCode, string siteURL, string webURL)
        {
            int CourseId =0;
            SPListItem course = null;


            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web = site.OpenWeb(webURL))
                {

                    StringBuilder queryBuilder = new StringBuilder("<Where>");
                    queryBuilder.Append("<Eq><FieldRef Name='TrainingCourseCode'/>");
                    queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Value Type='Text'>{0}</Value></Eq>", CourseCode));
                    queryBuilder.Append("</Where>");

                    SPQuery query = new SPQuery();
                    query.Query = queryBuilder.ToString();
                     course = GetListItem(web, Lists.TrainingCourses, query);


                }

            }
                
            
            if (course != null)
                {
                    CourseId = (int)course[new Guid(Fields.Id)];

                }


            return CourseId;
        }
        /// <summary>
        /// This Get returns UserId in provided sharepoint site for given user loginby
        /// </summary>
        /// <param name="login">User Login</param>
        /// <param name="siteURL">Site collection URL. http://localhost/</param>
        /// <param name="webURL">Web URL (subsite url).ex : training</param>
        /// <returns>Return UserID,0 if user not exists.</returns>
        static public int GetUserId(string login, string siteURL,string webURL)
        {
            SPUser user = null;

            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web = site.OpenWeb(webURL))
                {
                    user = web.SiteUsers[String.Format(@"{0}\{1}", Environment.MachineName, login)];

                }
            }
            
            if (user != null)
                return user.ID;
            else
                return 0;
            
        }
        /// <summary>
        /// This Get returns Registratoin List Item for givin Coursecode and login.
        /// </summary>
        /// <param name="CourseCode">CourseCode</param>
        /// <param name="login">User Login</param>
        /// <param name="siteURL">Site Collection URL. ex: http://localhost</param>
        /// <param name="webURL">Web URL. ex : training</param>
        /// <returns>Return Registraton list item,null if not exists</returns>
        static public SPListItem GetRegistration(string CourseCode,string login, string siteURL, string webURL)
        {
            int regiId = 0;
            int courseId = 0;
            int userId = 0;
            SPListItem regItem = null;

            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web = site.OpenWeb(webURL))
                {

                    courseId = GetCourseID(CourseCode, siteURL, webURL);
                    userId = GetUserId(login, siteURL, webURL);

                    StringBuilder queryBuilder = new StringBuilder("<Where>");
                    queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<And><Eq><FieldRef ID='{0}'/>", Fields.CourseId));
                    queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Value Type='Integer'>{0}</Value></Eq>", courseId));
                    queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Eq><FieldRef ID='{0}'/>", Fields.UserId));
                    queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Value Type='Integer'>{0}</Value></Eq></And>", userId));
                    queryBuilder.Append("</Where>");

                    SPQuery query = new SPQuery();
                    query.Query = queryBuilder.ToString();
                     regItem = GetListItem(web, Lists.Registrations, query);
                }
            }



            return regItem;
        }
        /// <summary>
        /// This Get returns Registratoin List Item for givin CourseId and loginId.
        /// </summary>
        /// <param name="CourseId">CourseId</param>
        /// <param name="userid">User Login ID</param>
        /// <param name="siteURL">Site Collection URL. ex: http://localhost</param>
        /// <param name="webURL">Web URL. ex : training</param>
        /// <returns>Return Registraton list item,null if not exists</returns>
        static public SPListItem GetRegistration(int courseId, int userId, string siteURL, string webURL)
        {
            int regiId = 0;
           
            SPListItem regItem = null;

            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web = site.OpenWeb(webURL))
                {

                    StringBuilder queryBuilder = new StringBuilder("<Where>");
                    queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<And><Eq><FieldRef ID='{0}'/>", Fields.CourseId));
                    queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Value Type='Integer'>{0}</Value></Eq>", courseId));
                    queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Eq><FieldRef ID='{0}'/>", Fields.UserId));
                    queryBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Value Type='Integer'>{0}</Value></Eq></And>", userId));
                    queryBuilder.Append("</Where>");

                    SPQuery query = new SPQuery();
                    query.Query = queryBuilder.ToString();
                    regItem = GetListItem(web, Lists.Registrations, query);
                }
            }



            return regItem;
        }
        #endregion
        /// <summary>
        /// The Get method will get an SPListItem from the specified list based on a specified query.
        /// </summary>
        /// <param name="web">The SPWeb of the SPList</param>
        /// <param name="listName">The name of the SPList</param>
        /// <param name="query">The SPQuery to find the item</param>
        /// <returns>The SPListItem in the SPList</returns>
        static SPListItem GetListItem(SPWeb web, string listName, SPQuery query)
        {
            SPListItem item = null;
            SPListItemCollection collection = null;

            collection = web.Lists[listName].GetItems(query);

            if (collection != null && collection.Count > 0)
            {
                item = collection[0];
            }

            return item;
        }
    }
}
