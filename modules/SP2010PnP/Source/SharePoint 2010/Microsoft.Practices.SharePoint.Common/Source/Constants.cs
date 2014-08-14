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
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint;

namespace Microsoft.Practices.SharePoint.Common
{
    /// <summary>
    /// Class that holds the constants for the SharePoint.Common project. 
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Constant that specifies the default category name.
        /// </summary>
        public const string DefaultCategoryName = "SharePoint Guidance";

        /// <summary>
        /// Constant that defines the default area name.
        /// </summary>
        public const string DefaultAreaName = "Patterns and Practices";

        /// <summary>
        /// Constant that specifies the name of the configuration key for searching the areas and categories section.
        /// </summary>
        public const string AreasConfigKey = "Microsoft.Practices.SharePoint.DiagnosticAreas";


        /// <summary>
        /// Constant that specifies the name of the event log in which the event sources will be created for logging.
        /// </summary>
        public const string EventLogName = "SharePoint Event Log";

        /// <summary>
        /// Constant that defines the path separator for categories (Area/Category).
        /// </summary>
        public const char CategoryPathSeparator = '/';

        /// <summary>
        /// The name of the Item content type.
        /// </summary>
        public static readonly string ItemContentTypeName = "Item";
    }
}
