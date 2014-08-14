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
using System.Reflection;
using System.IO;
using System.Configuration;
namespace Contoso.LOB.Services.Data
{
    public partial class IncidentDataSet
    {
        private static IncidentDataSet _instance = new IncidentDataSet();

        static IncidentDataSet()
        {
            _instance.LoadXmlData();
        }

        public static IncidentDataSet Instance
        {
            get
            {
                return _instance;
            }
        }

        private void LoadXmlData()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["incidentSampleDataFile"]))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("Contoso.LOB.Services.App_Data.IncidentDataSet.xml"))
                {
                    this.ReadXml(stream);
                }
            }
            else
            {
                this.ReadXml(ConfigurationManager.AppSettings["incidentSampleDataFile"]);
            }
        }
    }
}
