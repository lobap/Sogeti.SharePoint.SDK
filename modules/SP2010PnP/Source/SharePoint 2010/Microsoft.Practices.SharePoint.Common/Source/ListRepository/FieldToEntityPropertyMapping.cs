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

namespace Microsoft.Practices.SharePoint.Common.ListRepository
{
    /// <summary>
    /// Mapping between a field in an SPListItem and a property in an entity class. 
    /// </summary>
    public class FieldToEntityPropertyMapping
    {
        /// <summary>
        /// The guid that corresponds to the Id of the field.
        /// </summary>
        public Guid ListFieldId { get; set; }

        /// <summary>
        /// The name of the property on the entity. 
        /// </summary>
        public string EntityPropertyName { get; set; }
    }
}
