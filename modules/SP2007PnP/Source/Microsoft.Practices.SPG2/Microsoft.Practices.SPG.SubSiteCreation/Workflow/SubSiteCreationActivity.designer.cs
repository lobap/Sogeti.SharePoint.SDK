using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Microsoft.Practices.SPG.SubSiteCreation.Workflow
{
    /// <summary>
    /// Workflow activity that creates subsites in the workflow. 
    /// </summary>
    public sealed partial class SubSiteCreationActivity
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.subSiteCreationFaultHandler = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.SynchronizeStatus = new Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.SynchronizeStatusActivity();
            this.logToHistoryListActivity3 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.CreateSubSite = new Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.CreateSubSiteActivity();
            this.logToHistoryListActivity2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ResolveSiteTemplate = new Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.ResolveSiteTemplateActivity();
            this.LogResolveSiteTemplateStart = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // logToHistoryListActivity1
            // 
            this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowError;
            activitybind1.Name = "subSiteCreationFaultHandler";
            activitybind1.Path = "Fault.Message";
            this.logToHistoryListActivity1.HistoryOutcome = "";
            this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
            this.logToHistoryListActivity1.OtherData = "";
            this.logToHistoryListActivity1.UserId = -1;
            this.logToHistoryListActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // subSiteCreationFaultHandler
            // 
            this.subSiteCreationFaultHandler.Activities.Add(this.logToHistoryListActivity1);
            this.subSiteCreationFaultHandler.FaultType = typeof(Microsoft.Practices.SPG.SubSiteCreation.SubSiteCreationException);
            this.subSiteCreationFaultHandler.Name = "subSiteCreationFaultHandler";
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Activities.Add(this.subSiteCreationFaultHandler);
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // SynchronizeStatus
            // 
            this.SynchronizeStatus.Name = "SynchronizeStatus";
            this.SynchronizeStatus.Status = "\"Active\"";
            activitybind2.Name = "SubSiteCreationActivity";
            activitybind2.Path = "SubSiteUrl";
            this.SynchronizeStatus.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.SynchronizeStatusActivity.TargetWebUrlProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // logToHistoryListActivity3
            // 
            this.logToHistoryListActivity3.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity3.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity3.HistoryDescription = "synchronizing status";
            this.logToHistoryListActivity3.HistoryOutcome = "";
            this.logToHistoryListActivity3.Name = "logToHistoryListActivity3";
            this.logToHistoryListActivity3.OtherData = "";
            this.logToHistoryListActivity3.UserId = -1;
            activitybind8.Name = "SubSiteCreationActivity";
            activitybind8.Path = "SubSiteUrl";
            activitybind9.Name = "SubSiteCreationActivity";
            activitybind9.Path = "BusinessEventIdKey";
            // 
            // CreateSubSite
            // 
            activitybind3.Name = "SubSiteCreationActivity";
            activitybind3.Path = "BusinessEvent";
            activitybind4.Name = "SubSiteCreationActivity";
            activitybind4.Path = "EventId";
            this.CreateSubSite.Name = "CreateSubSite";
            activitybind5.Name = "SubSiteCreationActivity";
            activitybind5.Path = "SiteCollectionUrl";
            activitybind6.Name = "SubSiteCreationActivity";
            activitybind6.Path = "SiteTemplateName";
            activitybind7.Name = "SubSiteCreationActivity";
            activitybind7.Path = "TopLevelSiteRelativeUrl";
            this.CreateSubSite.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.CreateSubSiteActivity.BusinessEventProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.CreateSubSite.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.CreateSubSiteActivity.BusinessEventIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.CreateSubSite.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.CreateSubSiteActivity.SiteCollectionUrlProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.CreateSubSite.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.CreateSubSiteActivity.SiteTemplateNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.CreateSubSite.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.CreateSubSiteActivity.SubSiteUrlProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.CreateSubSite.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.CreateSubSiteActivity.TopLevelSiteRelativeUrlProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.CreateSubSite.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.CreateSubSiteActivity.BusinessEventIdKeyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // logToHistoryListActivity2
            // 
            this.logToHistoryListActivity2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity2.HistoryDescription = "creating subsite";
            this.logToHistoryListActivity2.HistoryOutcome = "";
            this.logToHistoryListActivity2.Name = "logToHistoryListActivity2";
            this.logToHistoryListActivity2.OtherData = "";
            this.logToHistoryListActivity2.UserId = -1;
            // 
            // ResolveSiteTemplate
            // 
            activitybind10.Name = "SubSiteCreationActivity";
            activitybind10.Path = "BusinessEventIdKey";
            activitybind11.Name = "SubSiteCreationActivity";
            activitybind11.Path = "BusinessEvent";
            this.ResolveSiteTemplate.Name = "ResolveSiteTemplate";
            activitybind12.Name = "SubSiteCreationActivity";
            activitybind12.Path = "SiteTemplateName";
            activitybind13.Name = "SubSiteCreationActivity";
            activitybind13.Path = "TopLevelSiteRelativeUrl";
            this.ResolveSiteTemplate.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.ResolveSiteTemplateActivity.BusinessEventNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.ResolveSiteTemplate.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.ResolveSiteTemplateActivity.SiteTemplateNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            this.ResolveSiteTemplate.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.ResolveSiteTemplateActivity.TopLevelSiteRelativeUrlProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.ResolveSiteTemplate.SetBinding(Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.ResolveSiteTemplateActivity.BusinessEventIdKeyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // LogResolveSiteTemplateStart
            // 
            this.LogResolveSiteTemplateStart.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.LogResolveSiteTemplateStart.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.LogResolveSiteTemplateStart.HistoryDescription = "resolving site template";
            this.LogResolveSiteTemplateStart.HistoryOutcome = "";
            this.LogResolveSiteTemplateStart.Name = "LogResolveSiteTemplateStart";
            this.LogResolveSiteTemplateStart.OtherData = "";
            this.LogResolveSiteTemplateStart.UserId = -1;
            activitybind15.Name = "SubSiteCreationActivity";
            activitybind15.Path = "WorkflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "SubSiteCreationActivity";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind14.Name = "SubSiteCreationActivity";
            activitybind14.Path = "WorkflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            // 
            // SubSiteCreationActivity
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.LogResolveSiteTemplateStart);
            this.Activities.Add(this.ResolveSiteTemplate);
            this.Activities.Add(this.logToHistoryListActivity2);
            this.Activities.Add(this.CreateSubSite);
            this.Activities.Add(this.logToHistoryListActivity3);
            this.Activities.Add(this.SynchronizeStatus);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "SubSiteCreationActivity";
            this.CanModifyActivities = false;

        }

        #endregion

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity LogResolveSiteTemplateStart;
        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity3;
        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity2;
        private FaultHandlersActivity faultHandlersActivity1;
        private FaultHandlerActivity subSiteCreationFaultHandler;
        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;
        private Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.SynchronizeStatusActivity SynchronizeStatus;
        private Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.CreateSubSiteActivity CreateSubSite;
        private Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities.ResolveSiteTemplateActivity ResolveSiteTemplate;
        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;



























































    }
}
