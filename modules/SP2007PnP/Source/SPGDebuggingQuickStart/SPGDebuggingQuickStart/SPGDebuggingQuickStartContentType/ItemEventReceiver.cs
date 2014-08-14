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
    [Guid("fb154e84-a558-4542-a8c5-988acec4d930")]
    public class SPGDebuggingQuickStartContentTypeItemEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// Microsoft.SharePoint.SPItemEventReceiver class.
        /// </summary>
        public SPGDebuggingQuickStartContentTypeItemEventReceiver()
        {
        }

        /// <summary>
        /// Synchronous before event that occurs when 
        /// a new item is added to its containing object.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPItemEventProperties 
        /// object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemAdding(SPItemEventProperties properties)
        {
            #region Incorrect Code
            //TODO: The following line of code will throw an 
            //"unspecified cast exception" and should be removed.
            DateTime date1 = (DateTime)properties.AfterProperties["MyDateTimeField1"];
            properties.AfterProperties["MyDateTimeField2"] = date1.AddDays(1);
            #endregion

            #region Correct Code
            //TODO: uncomment the following lines of code after debugging
            //DateTime date1 = new DateTime();
            //if (DateTime.TryParse((string)properties.AfterProperties["MyDateTimeField1"], out date1))
            //{
            //    properties.AfterProperties["MyDateTimeField2"] = date1.ToUniversalTime().AddDays(1).ToString("yyyy-MM-dd");
            //}
            //else
            //{
            //    properties.Cancel = true;
            //    properties.ErrorMessage = "An error occured.";
            //}
            #endregion
        }
    }
}
