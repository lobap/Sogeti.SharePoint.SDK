//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;

namespace ListBasedConfig.ListBackedConfigurationTests
{
    public partial class ListBackedConfigurationTestsUserControl : UserControl
    {
        private string testSetting = "List Backed Config Test Setting";
        private string key = "TestKey";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TestSaveValues();
            TestReadValues();
            TestReadHiearchyValues();
            CleanValues();
            CheckContains();
            TestFarmGet();
        }

        private void TestSaveValues()
        {
            IConfigManager mgr = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>();

            var bag = mgr.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            mgr.SetInPropertyBag(key, testSetting + ".web", bag);

            bag = mgr.GetPropertyBag(ConfigLevel.CurrentSPSite);
            mgr.SetInPropertyBag(key, testSetting + ".site", bag);


            bag = mgr.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            mgr.SetInPropertyBag(key, testSetting + ".webapp", bag);

            Literal1.Text = Literal1.Text + "<br/> Saved Completed";
        }

        private void TestReadValues()
        {
            IConfigManager mgr = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>();

            var bag = mgr.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            string webValue = mgr.GetFromPropertyBag<string>(key, bag);

            bag = mgr.GetPropertyBag(ConfigLevel.CurrentSPSite);
            string siteValue = mgr.GetFromPropertyBag<string>(key, bag);


            bag = mgr.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            string webApp = mgr.GetFromPropertyBag<string>(key, bag);

            Literal1.Text = Literal1.Text + "<br/> Read Completed with config mgr";
            Literal1.Text += string.Format("<br/>Web Value: {0}", webValue);
            Literal1.Text += string.Format("<br/>Site Value: {0}", siteValue);
            Literal1.Text += string.Format("<br/>Web App Value: {0}", webApp);
        }


        private void TestReadHiearchyValues()
        {
            var cfg = SharePointServiceLocator.GetCurrent().GetInstance<IHierarchicalConfig>();

            string webValue = cfg.GetByKey<string>(key);
            string siteValue = cfg.GetByKey<string>(key, ConfigLevel.CurrentSPSite);
            string webApp = cfg.GetByKey<string>(key, ConfigLevel.CurrentSPWebApplication);

            Literal1.Text = Literal1.Text + "<br/><br/> Read Completed With Hierarchy Manager";
            Literal1.Text += string.Format("<br/>Web Value: {0}", webValue);
            Literal1.Text += string.Format("<br/>Site Value: {0}", siteValue);
            Literal1.Text += string.Format("<br/>Web App Value: {0}", webApp);
        }

        private void CleanValues()
        {
            IConfigManager mgr = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>();

            var bag = mgr.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            mgr.RemoveKeyFromPropertyBag(key, bag);

            bag = mgr.GetPropertyBag(ConfigLevel.CurrentSPSite);
            mgr.RemoveKeyFromPropertyBag(key, bag);

            bag = mgr.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            mgr.RemoveKeyFromPropertyBag(key, bag);

            Literal1.Text = Literal1.Text + "<br/><br/> Remove Completed";

        }


        private void CheckContains()
        {
            var cfg = SharePointServiceLocator.GetCurrent().GetInstance<IHierarchicalConfig>();

            bool contains = cfg.ContainsKey(key);

            Literal1.Text = Literal1.Text + "<br/><br/> Contains Key check: " + contains.ToString();

        }

        private void TestFarmGet()
        {
            var mgr = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>();

            var bag = mgr.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            mgr.SetInPropertyBag(key, testSetting + ".farm", bag);

            var cfg = SharePointServiceLocator.GetCurrent().GetInstance<IHierarchicalConfig>();

            string val = cfg.GetByKey<string>(key);

            Literal1.Text = Literal1.Text + "<br/> <br/>  TestFarmGet, value retrieved: " + val.ToString();
        
        }
    }
}
