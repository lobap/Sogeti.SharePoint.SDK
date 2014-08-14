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
using System.Security.Permissions;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ExecutionModels.FullTrust.CentralAdminForms
{

    public partial class TimerJobConfig : LayoutsPageBase
    {
        SPFarm farm = SPFarm.Local;
        private const string ListName = "Approved Estimates";
        private const string SiteName = "/sites/MaintenanceBB;/sites/ConstructionBB;/sites/MaintenanceNB;/sites/ConstructionNB";
        private const string DestinationSiteName = "/sites/HeadQuarters";

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ApplyButton.Click += new EventHandler(ApplyButton_Click);
            RunButton.Click += new EventHandler(RunButton_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Load the Job we are looking for and feedback to user
                List<SPJobDefinition> allJobs = GetTimerJobsByName(Constants.jobTitle);
                SPJobDefinition selectedJob = allJobs.FirstOrDefault();
                if (selectedJob != null)
                {
                    DisplayJobProperties(selectedJob);
                }
                else
                {
                    PropertyInfoLabel.Text = string.Format("No Timer Job with the Display Name '{0}' was found.",
                                                           Constants.jobTitle);
                }

            }          
        }

        protected void RunButton_Click(object sender, EventArgs e)
        {
            //Get All Items that Match the Given Criteria
            List<SPJobDefinition> allJobs = GetTimerJobsByName(Constants.jobTitle);

            foreach (SPJobDefinition selectedJob in allJobs)
            {
                //Execute the job
                selectedJob.RunNow();
            }
            
            //Feedback to User 
            PropertyInfoLabel.Text = @"Timer Job(s) Started !";      
        }

        protected void ApplyButton_Click(object sender, EventArgs e)
        {
            IEnumerable<SPJobDefinition> allJobs = GetTimerJobsByName(Constants.jobTitle);

            foreach (SPJobDefinition selectedJob in allJobs)
            {
                if (!string.IsNullOrEmpty(ListNameTextBox.Text))
                {
                    selectedJob.Properties[Constants.timerJobListNameAttribute] = ListNameTextBox.Text;
                }
                if (!string.IsNullOrEmpty(SiteNamesTextBox.Text))
                {
                    selectedJob.Properties[Constants.timerJobSiteNameAttribute] = SiteNamesTextBox.Text;
                }
                if (!string.IsNullOrEmpty(DestinationSiteTextBox.Text))
                {
                    selectedJob.Properties[Constants.timerJobDestinationSiteAttribute] = DestinationSiteTextBox.Text;
                }

                selectedJob.Update();
            }

           
            ClearControls();

            //Display Property Info to the User based on the first item returned
            if (allJobs.Count() > 0)
            {
                DisplayJobProperties(allJobs.FirstOrDefault());  
            }
        }

        private void ClearControls()
        {
            ListNameTextBox.Text = string.Empty;
            SiteNamesTextBox.Text = string.Empty;
            DestinationSiteTextBox.Text = string.Empty;
            PropertyInfoLabel.Text = string.Empty;
            PropertyInfoDataList.DataSource = null;
            PropertyInfoDataList.DataBind();
        }

        private void DisplayJobProperties(SPJobDefinition selectedJob)
        {
            if (selectedJob.Properties.Values.Count > 0)
            {
                //Display Summary info
                PropertyInfoDataList.DataSource = selectedJob.Properties;
                PropertyInfoDataList.DataBind();

                //Set inputs to the existing values
                if (!string.IsNullOrEmpty(selectedJob.Properties[Constants.timerJobListNameAttribute].ToString()))
                {
                    ListNameTextBox.Text = selectedJob.Properties[Constants.timerJobListNameAttribute].ToString(); 
                }
                if (!string.IsNullOrEmpty(selectedJob.Properties[Constants.timerJobSiteNameAttribute].ToString()))
                {
                    SiteNamesTextBox.Text = selectedJob.Properties[Constants.timerJobSiteNameAttribute].ToString();
                }
                if (!string.IsNullOrEmpty(selectedJob.Properties[Constants.timerJobDestinationSiteAttribute].ToString()))
                {
                    DestinationSiteTextBox.Text = selectedJob.Properties[Constants.timerJobDestinationSiteAttribute].ToString();
                }
            }
            else
            {
                //Set the inputs Default Values
                ListNameTextBox.Text = ListName;
                SiteNamesTextBox.Text = SiteName;
                DestinationSiteTextBox.Text = DestinationSiteName;
            }
          ;
        }
        
        private List<SPJobDefinition> GetTimerJobsByName(string displayName)
        {
            List<SPJobDefinition> AllJobs = new List<SPJobDefinition>();

             //For all Servers in the Farm (could be different)
             foreach (SPServer server in farm.Servers)
             {
                 //Each Service Instance on the Server
                 foreach (SPServiceInstance svc in server.ServiceInstances)
                 {
                     if (svc.Service.JobDefinitions.Count > 0)
                     {
                         //If its a Web Service, must get the WebApp from the Web Service Entity
                         if (svc.Service is SPWebService)
                         {
                             SPWebService websvc = (SPWebService) svc.Service;
                             AllJobs.AddRange(from webapp in websvc.WebApplications
                                              from def in webapp.JobDefinitions
                                              where def.DisplayName.ToLower() == displayName.ToLower()
                                              select def);
                         }
                         else
                         {
                             //Otherwise Get it directly from the Service
                             AllJobs.AddRange(svc.Service.JobDefinitions.Where(def => def.DisplayName.ToLower() == displayName.ToLower()));
                         }
                     }
                 }
             }

            return AllJobs;
        }

        
    }
}
