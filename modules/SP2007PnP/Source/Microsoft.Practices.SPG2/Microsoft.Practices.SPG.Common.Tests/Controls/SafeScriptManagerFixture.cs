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
using System.Web.UI;
using Microsoft.Practices.SPG.AJAXSupport.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace Microsoft.Practices.SPG.Common.Tests.Controls
{
    [TestClass]
    public class SafeScriptManagerFixture
    {
        [TestInitialize]
        public void Init()
        {
            Isolate.CleanUp();
        }

        [TestMethod]
        public void WillAddScriptManagerToControlsCollection()
        {
            TestableSafeScriptManager target = new TestableSafeScriptManager();
            Page p = new Page();
            p.Controls.Add(target);
            
            target.CallOnInit();

            Assert.AreEqual(1, target.Controls.Count);
            Assert.IsInstanceOfType(target.Controls[0], typeof(ScriptManager));
            Assert.AreSame(target.Controls[0], target.ScriptManager);

        }

        [TestMethod]
        public void WillNotAddNewScriptManagerIfAlreadyExists()
        {
           
            Page p = new Page();
            ScriptManager expected = new ScriptManager();
            Isolate.Fake.StaticMethods<ScriptManager>(Members.CallOriginal);
            Isolate.WhenCalled(() => ScriptManager.GetCurrent(p)).WillReturn(expected);

            TestableSafeScriptManager target = new TestableSafeScriptManager();
            target.CallOnInit();

            Assert.AreEqual(0, target.Controls.Count);
            Assert.AreSame(expected, target.ScriptManager);

        }

        class TestableSafeScriptManager : SafeScriptManager

        {

            public void CallOnInit()
            {
                this.OnInit(EventArgs.Empty);
            }

            public void CallOnPreRender()
            {
                this.OnPreRender(EventArgs.Empty);
            }
        }
    }
}