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
using System.ServiceModel;
using Contoso.LOB.Services.BusinessEntities;
using System.Collections;

namespace Contoso.LOB.Services.Contracts
{
    [ServiceContract(Namespace = "http://Contoso.LOB.Services/2009/01")]
    public interface IIncidentManagement
    {
        [OperationContract]
        Incident GetIncident(string incidentId);

        [OperationContract]
        void WriteToHistory(string message);

        [OperationContract]
        IEnumerable<Incident> GetAllIncidents();
    }
}
