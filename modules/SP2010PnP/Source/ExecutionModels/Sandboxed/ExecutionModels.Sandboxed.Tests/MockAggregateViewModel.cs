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
using ExecutionModels.Sandboxed.AggregateView;
using Microsoft.SharePoint;
using System.Data;

namespace ExecutionModels.Sandboxed.Tests
{
    class MockEstimatesQueryService : IEstimatesService
    {
        private Exception _throwOnGetSiteData;

        public MockEstimatesQueryService() : this(null) { }

        public MockEstimatesQueryService(Exception throwOnGetSiteData)
        {
            this._throwOnGetSiteData = throwOnGetSiteData;
        }

        public DataTable Data { get; internal set; }
        public SPSiteDataQuery Query { get; internal set; }

        public DataTable GetSiteData()
        {
            if (this._throwOnGetSiteData != null)
            {
                throw this._throwOnGetSiteData;
            }
            this.Data = new DataTable();
            return this.Data;
        }
    }
}
