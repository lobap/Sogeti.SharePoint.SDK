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
using System.Security.Permissions;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// Class that helps in creating Web.Config modifications. 
    /// </summary>
    public static class SPWebConfigModificationHelper
    {
        /// <summary>
        /// Create and add an SPWebConfigModification to a list of modifications.
        /// </summary>
        /// <param name="modifications">The list to add the created webconfigmodification to.</param>
        /// <param name="name">The name of the modification.</param>
        /// <param name="xpath">The XPath expression to apply the config change to. </param>
        /// <param name="type">The type of modification to make.</param>
        /// <param name="value">The value of the modification. This is the Xml fragment to change or update</param>
        /// <param name="owner">
        /// The owner of the modification. This value is used by <see cref="CleanUpWebConfigModifications"/> to remove the modifications again. Typically
        /// this field is the name of the assembly that is making the modifications. 
        ///  </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "xpath")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static void CreateAndAddWebConfigModification(IList<SPWebConfigModification> modifications, string name, string xpath, SPWebConfigModification.SPWebConfigModificationType type, string value, string owner)
        {
            Validation.ArgumentNotNull(modifications, "modifications");

            SPWebConfigModification modification = new SPWebConfigModification(name, xpath);
            modification.Owner = owner;
            modification.Type = type;
            modification.Value = value;
            modifications.Add(modification);
        }

        ///<summary>
        /// Remove web.config modifications from the Web application, based on the owner field.
        ///</summary>
        ///<param name="webApp">The Web Application that holds the SPWebConfigModifications to remove.</param>
        ///<param name="owner">The owner of the webconfigmodifications. </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public static void CleanUpWebConfigModifications(SPWebApplication webApp, string owner)
        {
            Validation.ArgumentNotNull(webApp, "webApp");
            List<SPWebConfigModification> toBeDeleted = new List<SPWebConfigModification>();
            foreach (SPWebConfigModification spWebConfigModification in webApp.WebConfigModifications)
            {
                if (spWebConfigModification.Owner == owner)
                {
                    toBeDeleted.Add(spWebConfigModification);
                }
            }
            foreach (SPWebConfigModification configModification in toBeDeleted)
            {
                webApp.WebConfigModifications.Remove(configModification);
            }
        }
    }
}