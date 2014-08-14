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

namespace Microsoft.Practices.SPG.Common.Configuration
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
        CurrentSPFarm,

        /// <summary>
        /// Store config information in the SPWebApplication property bag
        /// </summary>
        CurrentSPWebApplication,

        /// <summary>
        /// Store config information in the SPSite property bag
        /// </summary>
        CurrentSPSite,

        /// <summary>
        /// Store config information in the SPWeb property bag
        /// </summary>
        CurrentSPWeb
    }
}