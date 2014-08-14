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
using Contoso.Common.BusinessEntities;

namespace Contoso.Common.Repositories
{
    /// <summary>
    /// Repository for incident management related tasks. 
    /// </summary>
    public interface IIncidentManagementRepository
    {
        /// <summary>
        /// Write incidents to the incident history log. 
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        void WriteToHistory(string message);

        /// <summary>
        /// Get a particular incident. 
        /// </summary>
        /// <param name="incidentId">The ID of the incident to retrieve</param>
        /// <returns>The incident. </returns>
        Incident GetIncident(string incidentId);
    }
}