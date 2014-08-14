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
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace ExecutionModels
{
    public static class Constants
    {
        public static readonly Guid projectLookupFieldId = new Guid("{F52FAC8A-7028-4BE1-B5C7-2A316AB1B88E}");
        public static readonly Guid estimateStatusFieldId = new Guid("{91EBB5B9-D8C5-43C5-98A2-BCB1400438B7}");
        public static readonly Guid estimateValueFieldId = new Guid("{41B33F77-5931-40A2-88DB-09723AA89098}");
        public static readonly Guid VendorNameFieldId = new Guid("{2769CEEC-F157-4770-A83D-AEF610F60E3D}");
        public static readonly Guid VendorIdFieldId = new Guid("{DF4C7989-DD3B-450F-B957-B387DAE16CC3}");

        public static readonly SPContentTypeId sowContentTypeId = new SPContentTypeId("0x010100FF4AAD791DCC425D85F03F3F1DE61402");
        public static readonly SPContentTypeId estimateContentTypeId = new SPContentTypeId("0x010100B022E34803AB43AEA87C937368261B49");
        public static readonly SPContentTypeId documentContentTypeId = new SPContentTypeId("0x0101");
        public static readonly string documentContentTypeName = "Document";

        public static readonly Guid emptyGuid = Guid.Empty;

        public static string[] GetSystemColumns()
        {
            return new string[] { "ListId", "WebId","BdcIdentity" };
        }

        public static readonly string sowContentTypeName = "SOW";
        public static readonly string estimateContentTypeName = "Estimate";
        public static readonly string projectsListLocation = "/Lists/Projects";
        public static readonly string estimatesListLocation = "/Lists/Estimates";
        public static readonly string approvedEstimatesListLocation = "/Lists/ApprovedEstimates";
        public static readonly string sowTemplatePath = "/_cts/SOW/SOW.dotx";
        public static readonly string estimateTemplatePath = "/_cts/Estimate/estimate.xltx";
        public static readonly string LookupColumnsPath = "/SiteColumns/LookupColumns.xml";

        //Constansts that Define Property's Used to Configure the Full Trust Timer Job
        public static readonly string timerJobDestinationSiteAttribute = "TimerJobDestinationSite";
        public static readonly string timerJobSiteNameAttribute = "TimerJobSiteNames";
        public static readonly string timerJobListNameAttribute = "TimerJobListName";
        public static readonly string jobTitle = "Approved Estimates Job";
        public static readonly string timerJobSiteRootUrl = "TimerJobSiteUrl";

        //Sandbox - ECT Constants
        public static readonly string ectVendorListName = "Vendors";
        public static readonly string ectVendorTransactionListName = "VendorTransactions";
        
     }
}


