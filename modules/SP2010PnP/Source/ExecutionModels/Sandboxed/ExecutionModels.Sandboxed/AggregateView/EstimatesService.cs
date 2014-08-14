//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.Practices.SharePoint.Common;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Security;

namespace ExecutionModels.Sandboxed.AggregateView
{
    public class EstimatesService : IEstimatesService
    {
        private SPSiteDataQuery query;

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public EstimatesService()
        {
            query = new SPSiteDataQuery();
            query.Lists = "<Lists BaseType='1' />";
            query.ViewFields = "<FieldRef Name='SOWStatus' />" +
                               "<FieldRef Name='EstimateValue' />";
            query.Query = "<OrderBy>" +
                              "<FieldRef Name='EstimateValue' />" +
                          "</OrderBy>";
            query.Webs = "<Webs Scope='SiteCollection' />";
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public System.Data.DataTable GetSiteData()
        {
            SPWeb web = SPContext.Current.Web;
            return web.GetSiteData(query);
        }       
    }
}
