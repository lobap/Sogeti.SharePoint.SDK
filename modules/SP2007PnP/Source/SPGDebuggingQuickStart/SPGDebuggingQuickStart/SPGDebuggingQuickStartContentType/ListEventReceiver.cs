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
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using VSeWSS;

namespace SPGDebuggingQuickStart
{
    [CLSCompliant(false)]
    [TargetContentType("0x010046cc7715a39948c7bf4a3682cc8a02c3")]
    [Guid("ac281709-8b7f-4572-8784-ae3d50b48069")]
    public class SPGDebuggingQuickStartContentTypeListEventReceiver : SPListEventReceiver
    {
        /// <summary>
        /// Initializes a new instance of the Microsoft.SharePoint.SPListEventReceiver class.
        /// </summary>
        public SPGDebuggingQuickStartContentTypeListEventReceiver()
        {
        }

        /// <summary>
        /// Occurs after a field link is added.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPListEventProperties object that represents properties of the event handler.
        /// </param>
        //public override void FieldAdded(SPListEventProperties properties)
        //{
        //}

        /// <summary>
        /// Occurs when a fieldlink is being added to a content type.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPListEventProperties object that represents properties of the event handler.
        /// </param>
        //public override void FieldAdding(SPListEventProperties properties)
        //{
        //}

        /// <summary>
        /// Occurs after a field has been removed from the list.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPListEventProperties object that represents properties of the event handler.
        /// </param>
        //public override void FieldDeleted(SPListEventProperties properties)
        //{
        //}

        /// <summary>
        /// Occurs when a field is in process of being removed from the list.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPListEventProperties object that represents properties of the event handler.
        /// </param>
        //public override void FieldDeleting(SPListEventProperties properties)
        //{
        //}

        /// <summary>
        /// Occurs after a field link has been updated
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPListEventProperties object that represents properties of the event handler.
        /// </param>
        //public override void FieldUpdated(SPListEventProperties properties)
        //{
        //}

        /// <summary>
        /// Occurs when a fieldlink is being updated
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPListEventProperties object that represents properties of the event handler.
        /// </param>
        //public override void FieldUpdating(SPListEventProperties properties)
        //{
        //}
    }
}
