//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Data;
using ExecutionModels.Sandboxed.Proxy.AggregateView;

namespace ExecutionModels.Sandboxed.Proxy.Tests.Mocks
{
    public class MockAggregateView : IAggregateView
    {
        public DataTable Data { get; set; }
        public DataTable SetSiteData
        {
            set { Data = value; }
        }
    }
}


