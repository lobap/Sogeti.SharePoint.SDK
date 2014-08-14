using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities
{
    /// <summary>
    /// Workflow activity that writes the status of the subsite creation request in the PropertyBag
    /// of the SPWeb that's created. 
    /// </summary>
	public partial class SynchronizeStatusActivity
	{
		#region Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
		private void InitializeComponent()
		{
			this.Name = "SynchronizeStatus";
		}

		#endregion
	}
}
