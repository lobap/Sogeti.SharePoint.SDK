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
namespace Contoso.LOB.Services.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;    
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Contoso.LOB.Services.BusinessEntities;    
    using Contoso.LOB.Services.Contracts;
    using System.Configuration;

    public partial class PricingDataSet
    {
        #region private fields

        private static PricingDataSet _instance = new PricingDataSet();

        #endregion

        #region constructors

        static PricingDataSet()
        {
            _instance.LoadXmlData();
        }

        #endregion

        #region Properties

        public static PricingDataSet Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region private methods

        private void LoadXmlData()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["pricingSampleDataFile"]))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("Contoso.LOB.Services.App_Data.PricingDataSet.xml"))
                {
                    this.ReadXml(stream);
                }
            }
            else
            {
                this.ReadXml(ConfigurationManager.AppSettings["pricingSampleDataFile"]);
            }
        }

        #endregion
    }
}
