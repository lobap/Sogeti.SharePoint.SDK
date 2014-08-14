//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Vendor.Services.Contract;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Security.Permissions;

namespace Vendor.Services.Implementation
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class VendorServices : IVendorServices
    {
        public const int MaxAccountPayablePennies = 100000000;

        public double GetAccountsPayable(string vendorName)
        {
            int seed = Environment.TickCount;
            Random rnd = new Random(seed);

            int responsePennies = rnd.Next(MaxAccountPayablePennies);
            return ((double)responsePennies / 100);
        }

        public string WhoAmI()
        {
            string whoisthis = HttpContext.Current.User.Identity.Name.ToString();
            return whoisthis;
        }

    }
}
