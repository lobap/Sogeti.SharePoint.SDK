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
using System.Web.UI;

namespace Microsoft.Practices.SPG.AJAXSupport.Controls
{
    /// <summary>
    /// Control that you can put on your page or user control to ensure the ScriptManager is present. 
    /// If the ScriptManager is present, it will get a reference to it in the <see cref="SafeScriptManager.ScriptManager"/> Member. 
    /// If not, it will add it to the controls collection during the Init phase(). 
    /// </summary>
    public class SafeScriptManager : Control
    {
        /// <summary>
        /// Determines if support for ASP.NET update panels should be enabled. This will make some changes in the way
        /// postbacks are handled by the page. 
        /// </summary>
        public bool EnableUpdatePanelSupport { get; set; }

        /// <summary>
        /// Make sure the ScriptManager will be added to the ChildControls collection as soon as possible. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            EnsureChildControls();
            base.OnInit(e);
        }

        private ScriptManager scriptManager;

        /// <summary>
        /// Reference to the ScriptManager. 
        /// </summary>
        public ScriptManager ScriptManager
        {
            get
            {
                EnsureChildControls();
                return scriptManager;
            }
        }

        /// <summary>
        /// Ensure the ScriptManager is present
        /// </summary>
        protected override void CreateChildControls()
        {
            this.scriptManager = ScriptManager.GetCurrent(this.Page);
            if (this.ScriptManager == null)
            {
                this.scriptManager = CreateScriptManager();
                this.Controls.Add(this.scriptManager);
            }

            base.CreateChildControls();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// This method is overridden to add support for postbacks. 
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (this.EnableUpdatePanelSupport &&  this.Page.Form != null)
            {
                string formOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                if (formOnSubmitAtt == "return_spFormOnSubmitWrapper();")
                {
                    this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";

                    ScriptManager.RegisterStartupScript(this, typeof(SafeScriptManager), "UpdatePanelFixup", "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper=true;", true);
                }
            }
        }

        /// <summary>
        /// Create the ScriptManager
        /// </summary>
        /// <returns></returns>
        protected virtual ScriptManager CreateScriptManager()
        {
            return new ScriptManager();
        }
    }
}