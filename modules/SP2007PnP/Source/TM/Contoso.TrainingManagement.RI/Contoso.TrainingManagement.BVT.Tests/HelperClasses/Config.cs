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

namespace TrainingManagementAcceptanceTest.HelperClasses
{
    static class Config
    {
        public const string siteURL = "http://localhost" + "/";
        public const string webURL = "training";
        //public const string adminUser =  "Administrator";
        //public const string adminPass = "P2ssw0rd";
        public const string spgEmpUser = "spgemployee";
        public const string spgEmpPass = "Password$1";
        public const string spgManagerUser = "spgmanager";
        public const string spgManagerPass = "Password$1";
        /// <summary>
        /// this function return unique course code.
        /// </summary>
        /// <returns></returns>
        public static string NewCourseCode()
        {
            Random rnd = new Random();
            return "C" + rnd.Next(999999,9999999).ToString();
        }

    }
}
