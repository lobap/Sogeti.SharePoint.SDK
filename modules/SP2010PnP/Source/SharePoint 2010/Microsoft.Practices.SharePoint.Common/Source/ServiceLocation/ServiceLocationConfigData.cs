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

namespace Microsoft.Practices.SharePoint.Common.ServiceLocation
{
    /// <summary>
    /// Contains the configuration data for the sharepoint service locator that is serialized to 
    /// and stored in configuration.
    /// </summary>
    public class ServiceLocationConfigData : List<TypeMapping>
    {
        /// <summary>
        /// Default constructor for ServiceLocationConfigData.
        /// </summary>
        public ServiceLocationConfigData()
        {
        }

        /// <summary>
        /// Constructs the service location configuration data initializnig with the 
        /// type mapping list provided.
        /// </summary>
        /// <param name="list"></param>
        public ServiceLocationConfigData(IList<TypeMapping> list) :
            base(list)
        {
        }

        /// <summary>
        /// Returns the last time the configuration data was updated.
        /// </summary>
        public DateTime? LastUpdate { get; set; }
    }
}
