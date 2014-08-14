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
using System.Globalization;
using System.Runtime.InteropServices;
using Contoso.Common.ExceptionHandling;
using Contoso.Common.Repositories;
using Microsoft.SharePoint;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using VSeWSS;
using Contoso.PartnerPortal.Collaboration.Incident.Properties;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Contoso.PartnerPortal.Collaboration.Incident.ContentTypes.IncidentTask
{
    [TargetContentType("0x0108009bbba7168ed64455b16ecdbd00ba7997")]
    [Guid("9EC9CC50-80D2-4d15-8135-AFC67BCBA036")]
    public class IncidentTaskReceiver : SPItemEventReceiver
    {
        #region Private Constants

        private const string TaskCreatedMessage = "Task created: {0}";
        private const string TitleField = "Title";

        #endregion

        private IIncidentManagementRepository incidentManagementRepository;

        public IncidentTaskReceiver()
        {
            //Need to get the repository in the contructor because the SPContext is available. 
            //The SPContext is NOT available in the ItemAdding event receiver.
            incidentManagementRepository = SharePointServiceLocator.Current.GetInstance<IIncidentManagementRepository>();
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel=true)]
        public override void ItemAdding(SPItemEventProperties properties)
        {
            try
            {
                incidentManagementRepository.WriteToHistory(string.Format(CultureInfo.CurrentCulture, TaskCreatedMessage, properties.AfterProperties[TitleField]));
            }
            catch (Exception ex)
            {
                ListExceptionHandler handler = new ListExceptionHandler();
                handler.HandleListItemEventException(ex, properties, Resources.HandleListItemEventExceptionValue);
            }
        }
    }
}
