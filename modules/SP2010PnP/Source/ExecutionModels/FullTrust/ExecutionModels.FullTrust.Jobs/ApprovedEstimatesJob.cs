//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Linq;
using ExecutionModels.Common.ExceptionHandling;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using System.Diagnostics;
using System.Data;
using System.IO;

namespace ExecutionModels.FullTrust.Jobs
{
    /// <summary>
    /// A timer job which pulls data from the Estimates list instances 
    /// in the departmental sub sites and populates the Project Data list with it.
    /// </summary>
    public class ApprovedEstimatesJob : SPJobDefinition
    {
        public const string JobName = "ApprovedEstimatesJob";

        private string TimerDestinationSiteName = string.Empty;
        private string TimerSiteNames = string.Empty;
        private string[] TimerSiteNamesArray = null;

        public ApprovedEstimatesJob() : base() { }

        public ApprovedEstimatesJob(SPWebApplication webApplication) :
            base(JobName, webApplication, null, SPJobLockType.Job)
        {
            Title = Constants.jobTitle;
        }



        /// <summary>
        /// Method will populate the private members with data from the PropertyBag of the Timer Job Definition 
        /// </summary>
        private void GetConfigurationValues()
        {
            if (Properties[Constants.timerJobDestinationSiteAttribute] != null && !string.IsNullOrEmpty(Properties[Constants.timerJobDestinationSiteAttribute].ToString()))
            {
                TimerDestinationSiteName = Properties[Constants.timerJobDestinationSiteAttribute].ToString();
            }
            else
            {
                var ex = new Exception(Constants.timerJobDestinationSiteAttribute + " Property has not been configured for the Timer Job:" + Constants.jobTitle);
                var handler = new TimerJobExceptionHandler();
                handler.HandleTimerJobException(ex);
            }

            if (Properties[Constants.timerJobSiteNameAttribute] != null && !string.IsNullOrEmpty(Properties[Constants.timerJobSiteNameAttribute].ToString()))
            {
                TimerSiteNames = Properties[Constants.timerJobSiteNameAttribute].ToString();
                TimerSiteNamesArray = TimerSiteNames.Split(";".ToArray());
            }
            else
            {
                var ex = new Exception(Constants.timerJobSiteNameAttribute + " Property has not been configured for the Timer Job:" + Constants.jobTitle);
                var handler = new TimerJobExceptionHandler();
                handler.HandleTimerJobException(ex);
            }

            if (Properties[Constants.timerJobListNameAttribute] != null && !string.IsNullOrEmpty(Properties[Constants.timerJobListNameAttribute].ToString()))
            {
                Properties[Constants.timerJobListNameAttribute].ToString();
            }
            else
            {
                var ex = new Exception(Constants.timerJobListNameAttribute + " Property has not been configured for the Timer Job:" + Constants.jobTitle);
                var handler = new TimerJobExceptionHandler();
                handler.HandleTimerJobException(ex);
            }

        }

        public override void Execute(Guid targetInstanceId)
        {
            //Populate local variable from PropertyBag of Entity
            GetConfigurationValues();

            //Pull the Url and Format it
            var webApplication = this.Parent as SPWebApplication;
            string rootUrl = webApplication.GetResponseUri(SPUrlZone.Default).AbsoluteUri;
            rootUrl = rootUrl.TrimEnd("/".ToCharArray());
            
            try
            {
                using (var destSite = new SPSite( rootUrl + TimerDestinationSiteName))
                {
                    var targetLibrary = destSite.RootWeb.GetList(destSite.Url + Constants.approvedEstimatesListLocation);
                    var targetFolder = targetLibrary.RootFolder;

                    foreach (var timerSiteName in TimerSiteNamesArray)
                    {
                        try
                        {
                            using (var site = new SPSite(rootUrl + timerSiteName))
                            {
                                using (var data = site.RootWeb.GetSiteData(GetAllApprovedEstimatesFromSiteCollection()))
                                {
                                    // Add the new approved item
                                    foreach (var sourceFile in from DataRow row in data.Rows
                                                               select site.MakeFullUrl(ParseFileRef(row["FileRef"].ToString()))
                                                               into url select site.RootWeb.GetFile(url))
                                    {
                                        using (Stream sourceFileStream = sourceFile.OpenBinaryStream())
                                        {
                                            targetFolder.Files.Add(sourceFile.Name, sourceFileStream, sourceFile.Properties, true);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var handler = new TimerJobExceptionHandler();
                            handler.HandleTimerJobException(ex);
                        }
                    }
                }
                return;
            }
            catch (Exception e)
            {
                var handler = new TimerJobExceptionHandler();
                handler.HandleTimerJobException(e);
            }
            finally
            {
            }
        }


        private static string ParseFileRef(string fileRef)
        {
            return "/" + fileRef.Substring(fileRef.IndexOf("#") + 1);
        }

        private static SPSiteDataQuery GetAllApprovedEstimatesFromSiteCollection()
        {
            var query = new SPSiteDataQuery();

            query.Lists = "<Lists BaseType='1' />";

            query.ViewFields = "<FieldRef Name='Title' Nullable='TRUE' />" +
                               "<FieldRef Name='FileRef' Nullable='TRUE' />";

            query.Query = "<Where>" +
                                "<Eq>" +
                                    "<FieldRef Name='SOWStatus' />" +
                                    "<Value Type='Choice'>Approved</Value>" +
                                "</Eq>" +
                           "</Where>";
            query.Webs = "<Webs Scope='SiteCollection' />";
            return query;
        }
    }
}