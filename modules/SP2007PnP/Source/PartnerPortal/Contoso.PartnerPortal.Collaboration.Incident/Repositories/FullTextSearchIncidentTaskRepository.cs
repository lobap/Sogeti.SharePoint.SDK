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
using System.Data;
using System.Globalization;
using Contoso.Common.BusinessEntities;
using Contoso.Common.Repositories;
using Contoso.PartnerPortal.Collaboration.Incident.Properties;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;

namespace Contoso.PartnerPortal.Collaboration.Incident.Repositories
{
    /// <summary>
    /// Repository that uses Full Text Search to find all open incident tasks. 
    /// </summary>
    public class FullTextSearchIncidentTaskRepository : IIncidentTaskRepository
    {
        #region Private Constants

        private const string PathField = "Path";
        private const string TitleField = "Title";
        private const string AssignedToField = "AssignedTo";
        private const string StatusField = "Status";
        private const string PriorityField = "Priority";

        #endregion

        #region IIncidentTaskRepository Members

        public IEnumerable<IncidentTask> GetAllOpenIncidentTasks()
        {
            FullTextSqlQuery sqlQuery = new FullTextSqlQuery(ServerContext.Current);
            sqlQuery.QueryText = Resources.PartnerRollupFullTextQuery;
            sqlQuery.ResultTypes = ResultType.RelevantResults;

            // Write the query to the trace log, so developers can debug it more easily. 
            ILogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
            logger.TraceToDeveloper(
                string.Format(CultureInfo.CurrentCulture,
                              Resources.IncidentTasksFullTextSqlQuery,
                              sqlQuery.QueryText));

            ResultTableCollection queryResults = sqlQuery.Execute();
            DataTable resultsTable = new DataTable();
            resultsTable.Locale = CultureInfo.CurrentCulture;
            resultsTable.Load(queryResults[ResultType.RelevantResults]);

            List<IncidentTask> results = new List<IncidentTask>();
            foreach (DataRow datarow in resultsTable.Rows)
            {
                IncidentTask result = new IncidentTask();
                result.Path = datarow[PathField] == DBNull.Value ? string.Empty : (string) datarow[PathField];
                result.Title = datarow[TitleField] == DBNull.Value ? string.Empty : (string) datarow[TitleField];
                result.AssignedTo = datarow[AssignedToField] == DBNull.Value
                                        ? string.Empty
                                        : (string) datarow[AssignedToField];

                // For some reason, the the Status field is returned as an array of strings. The status can be accessed
                // using the index 0.
                result.Status = datarow[StatusField] == DBNull.Value ? string.Empty : ((string[]) datarow[StatusField])[0];
                result.Priority = datarow[PriorityField] == DBNull.Value ? string.Empty : (string) datarow[PriorityField];
                results.Add(result);
            }
            return results;
        }

        #endregion
    }
}