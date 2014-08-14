//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// The levels at which configuration information can be stored. These levels are used to determine if a specific config value
    /// can be stored at a specific level.
    /// </summary>
    public enum ConfigLevel
    {
        /// <summary>
        /// Store config information in the SPFarm property bag
        /// </summary>
        CurrentSPFarm = 3,

        /// <summary>
        /// Store config information in the SPWebApplication property bag
        /// </summary>
        CurrentSPWebApplication = 2,

        /// <summary>
        /// Store config information in the SPSite property bag
        /// </summary>
        CurrentSPSite = 1,

        /// <summary>
        /// Store config information in the SPWeb property bag
        /// </summary>
        CurrentSPWeb = 0
    }
}