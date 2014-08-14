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

namespace Contoso.TrainingManagement.Workflows.RegistrationApproval
{
	partial class Workflow
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
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.Runtime.CorrelationToken correlationtoken2 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            this.logToHistoryListErrorActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListRejectedActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListApprovedActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.codeChargeAccountingActivity = new System.Workflow.Activities.CodeActivity();
            this.faultHandlerActivity = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.ifElseRejectedBranchActivity = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseApprovedBranchActivity = new System.Workflow.Activities.IfElseBranchActivity();
            this.onRegistrationChanged = new Microsoft.SharePoint.WorkflowActions.OnWorkflowItemChanged();
            this.faultHandlersActivity = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.ifElseApprovalActivity = new System.Workflow.Activities.IfElseActivity();
            this.deleteManagerApprovalTask = new Microsoft.SharePoint.WorkflowActions.DeleteTask();
            this.completeManagerApprovalTask = new Microsoft.SharePoint.WorkflowActions.CompleteTask();
            this.whileActivity = new System.Workflow.Activities.WhileActivity();
            this.createManagerApprovalTask = new Microsoft.SharePoint.WorkflowActions.CreateTask();
            this.onWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // logToHistoryListErrorActivity
            // 
            this.logToHistoryListErrorActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListErrorActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowError;
            activitybind1.Name = "faultHandlerActivity";
            activitybind1.Path = "Fault.Message";
            this.logToHistoryListErrorActivity.HistoryOutcome = "";
            this.logToHistoryListErrorActivity.Name = "logToHistoryListErrorActivity";
            this.logToHistoryListErrorActivity.OtherData = "";
            this.logToHistoryListErrorActivity.UserId = -1;
            this.logToHistoryListErrorActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind1 ) ));
            // 
            // logToHistoryListRejectedActivity
            // 
            this.logToHistoryListRejectedActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListRejectedActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListRejectedActivity.HistoryDescription = "The registration was rejected.";
            this.logToHistoryListRejectedActivity.HistoryOutcome = "";
            this.logToHistoryListRejectedActivity.Name = "logToHistoryListRejectedActivity";
            this.logToHistoryListRejectedActivity.OtherData = "";
            this.logToHistoryListRejectedActivity.UserId = -1;
            // 
            // logToHistoryListApprovedActivity
            // 
            this.logToHistoryListApprovedActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListApprovedActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListApprovedActivity.HistoryDescription = "The registration was approved and Accounting was charged.";
            this.logToHistoryListApprovedActivity.HistoryOutcome = "";
            this.logToHistoryListApprovedActivity.Name = "logToHistoryListApprovedActivity";
            this.logToHistoryListApprovedActivity.OtherData = "";
            this.logToHistoryListApprovedActivity.UserId = -1;
            // 
            // codeChargeAccountingActivity
            // 
            this.codeChargeAccountingActivity.Name = "codeChargeAccountingActivity";
            this.codeChargeAccountingActivity.ExecuteCode += new System.EventHandler(this.CodeChargeAccountingActivity_ExecuteCode);
            // 
            // faultHandlerActivity
            // 
            this.faultHandlerActivity.Activities.Add(this.logToHistoryListErrorActivity);
            this.faultHandlerActivity.FaultType = typeof(System.Exception);
            this.faultHandlerActivity.Name = "faultHandlerActivity";
            // 
            // ifElseRejectedBranchActivity
            // 
            this.ifElseRejectedBranchActivity.Activities.Add(this.logToHistoryListRejectedActivity);
            this.ifElseRejectedBranchActivity.Name = "ifElseRejectedBranchActivity";
            // 
            // ifElseApprovedBranchActivity
            // 
            this.ifElseApprovedBranchActivity.Activities.Add(this.codeChargeAccountingActivity);
            this.ifElseApprovedBranchActivity.Activities.Add(this.logToHistoryListApprovedActivity);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ManagerApprovedCondition);
            this.ifElseApprovedBranchActivity.Condition = codecondition1;
            this.ifElseApprovedBranchActivity.Name = "ifElseApprovedBranchActivity";
            // 
            // onRegistrationChanged
            // 
            this.onRegistrationChanged.AfterProperties = null;
            this.onRegistrationChanged.BeforeProperties = null;
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "Workflow";
            this.onRegistrationChanged.CorrelationToken = correlationtoken1;
            this.onRegistrationChanged.Name = "onRegistrationChanged";
            this.onRegistrationChanged.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.OnRegistrationChanged_Invoked);
            // 
            // faultHandlersActivity
            // 
            this.faultHandlersActivity.Activities.Add(this.faultHandlerActivity);
            this.faultHandlersActivity.Name = "faultHandlersActivity";
            // 
            // ifElseApprovalActivity
            // 
            this.ifElseApprovalActivity.Activities.Add(this.ifElseApprovedBranchActivity);
            this.ifElseApprovalActivity.Activities.Add(this.ifElseRejectedBranchActivity);
            this.ifElseApprovalActivity.Name = "ifElseApprovalActivity";
            // 
            // deleteManagerApprovalTask
            // 
            correlationtoken2.Name = "taskToken";
            correlationtoken2.OwnerActivityName = "Workflow";
            this.deleteManagerApprovalTask.CorrelationToken = correlationtoken2;
            this.deleteManagerApprovalTask.Name = "deleteManagerApprovalTask";
            activitybind2.Name = "Workflow";
            activitybind2.Path = "taskId";
            this.deleteManagerApprovalTask.SetBinding(Microsoft.SharePoint.WorkflowActions.DeleteTask.TaskIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind2 ) ));
            // 
            // completeManagerApprovalTask
            // 
            this.completeManagerApprovalTask.CorrelationToken = correlationtoken2;
            this.completeManagerApprovalTask.Name = "completeManagerApprovalTask";
            activitybind3.Name = "Workflow";
            activitybind3.Path = "taskId";
            this.completeManagerApprovalTask.TaskOutcome = null;
            this.completeManagerApprovalTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CompleteTask.TaskIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind3 ) ));
            // 
            // whileActivity
            // 
            this.whileActivity.Activities.Add(this.onRegistrationChanged);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ManagerApprovalTaskCompleteCondition);
            this.whileActivity.Condition = codecondition2;
            this.whileActivity.Name = "whileActivity";
            // 
            // createManagerApprovalTask
            // 
            this.createManagerApprovalTask.CorrelationToken = correlationtoken2;
            this.createManagerApprovalTask.ListItemId = -1;
            this.createManagerApprovalTask.Name = "createManagerApprovalTask";
            this.createManagerApprovalTask.SpecialPermissions = null;
            activitybind4.Name = "Workflow";
            activitybind4.Path = "taskId";
            activitybind5.Name = "Workflow";
            activitybind5.Path = "taskProperties";
            this.createManagerApprovalTask.MethodInvoking += new System.EventHandler(this.CreateManagerApprovalTask_MethodInvoking);
            this.createManagerApprovalTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTask.TaskIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind4 ) ));
            this.createManagerApprovalTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTask.TaskPropertiesProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind5 ) ));
            // 
            // onWorkflowActivated
            // 
            this.onWorkflowActivated.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated.Name = "onWorkflowActivated";
            activitybind6.Name = "Workflow";
            activitybind6.Path = "workflowProperties";
            this.onWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind6 ) ));
            // 
            // Workflow
            // 
            this.Activities.Add(this.onWorkflowActivated);
            this.Activities.Add(this.createManagerApprovalTask);
            this.Activities.Add(this.whileActivity);
            this.Activities.Add(this.completeManagerApprovalTask);
            this.Activities.Add(this.deleteManagerApprovalTask);
            this.Activities.Add(this.ifElseApprovalActivity);
            this.Activities.Add(this.faultHandlersActivity);
            this.Name = "Workflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private IfElseBranchActivity ifElseRejectedBranchActivity;
        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListRejectedActivity;
        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListApprovedActivity;
        private Microsoft.SharePoint.WorkflowActions.OnWorkflowItemChanged onRegistrationChanged;
        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListErrorActivity;
        private Microsoft.SharePoint.WorkflowActions.DeleteTask deleteManagerApprovalTask;
        private CodeActivity codeChargeAccountingActivity;
        private FaultHandlerActivity faultHandlerActivity;
        private IfElseBranchActivity ifElseApprovedBranchActivity;
        private IfElseActivity ifElseApprovalActivity;
        private FaultHandlersActivity faultHandlersActivity;
        private WhileActivity whileActivity;
        private Microsoft.SharePoint.WorkflowActions.CompleteTask completeManagerApprovalTask;
        private Microsoft.SharePoint.WorkflowActions.CreateTask createManagerApprovalTask;
        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated;











    }
}
