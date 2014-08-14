//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;

namespace ExecutionModels.FullTrust.Jobs.Features.Approved_Estimates_Job
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("e5264e48-93c7-4b34-9ab2-98ea7d9f1911")]
    public class Approved_Estimates_JobEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApplication = properties.Feature.Parent as SPWebApplication;            
            
            DeleteJob(webApplication.JobDefinitions);

            ApprovedEstimatesJob estimatesJob = new ApprovedEstimatesJob(webApplication);

            SPMinuteSchedule schedule = new SPMinuteSchedule();
            schedule.BeginSecond = 0;
            schedule.EndSecond = 59;
            schedule.Interval = 5;

            estimatesJob.Schedule = schedule;
            estimatesJob.Update();
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApplication = properties.Feature.Parent as SPWebApplication;
            DeleteJob(webApplication.JobDefinitions);
        }

        private void DeleteJob(SPJobDefinitionCollection jobs)
        {
            foreach (SPJobDefinition job in jobs)
            {
                if (job.Name.Equals(ApprovedEstimatesJob.JobName,
                    StringComparison.OrdinalIgnoreCase))
                {
                    job.Delete();
                }
            }
        }
    }
}
