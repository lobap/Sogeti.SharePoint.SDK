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
using Microsoft.Practices.SPG.AJAXSupport.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.UI;

namespace Contoso.PartnerPortal.Portal.IntegrationTests
{
    [TestClass]
    public class SafeScriptManagerFixture
    {
        [TestMethod]
        public void AddingMultipleSafeScriptManagersOnPageResultInOneScriptManager()
        {
            MyPage myPage = new MyPage();
            myPage.CallEnsureChildControls();
            
            int numberOfScriptManagers = 0;
            foreach (var control in myPage.Controls)
            {
                if (control is ScriptManager)
                    numberOfScriptManagers++;
            }

            Assert.AreEqual(1, numberOfScriptManagers);
        }
    }

    class MyPage : Page
    {

        public void CallEnsureChildControls()
        {
            EnsureChildControls(); 
            OnInit(EventArgs.Empty);
        }

        protected override void CreateChildControls()
        {
            Controls.Add(new SafeScriptManager());
            Controls.Add(new SafeScriptManager());
            Controls.Add(new SafeScriptManager());
        }
    }
}
