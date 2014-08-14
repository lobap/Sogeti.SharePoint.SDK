//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// A property bag to store settings for the web using the SPWeb property bag. 
    /// </summary>
    public class SPWebPropertyBag : IPropertyBag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SPWebPropertyBag"/> class.
        /// </summary>
        /// <param name="web">The SPWeb associated with this property bag.</param>
        public SPWebPropertyBag(SPWeb web)
        {
            Validation.ArgumentNotNull(web, "web");
            this.web = web;
        }

        /// <summary>
        /// The SPWeb that's wrapped by this property bag. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Sp")]
        private SPWeb web { get; set; }

        /// <summary>
        /// The config level this PropertyBag represents.
        /// </summary>
        /// <value></value>
        public virtual ConfigLevel Level
        {
            get { return ConfigLevel.CurrentSPWeb; }
        }

        /// <summary>
        /// Does a specific key exist in the PropertyBag.  
        /// </summary>
        /// <param name="key">the key to check.</param>
        /// <returns>true if the key exists, else false.</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool Contains(string key)
        {
            string fullKey = BuildKey(key);
            bool containsKey = false;
            if (this.web.AllProperties.Contains(fullKey))
            {
                containsKey = this.web.AllProperties[fullKey] != null;
            }
            return containsKey;
       }

        /// <summary>
        /// Gets or sets a value based on the key. 
        /// </summary>
        /// <returns>The config value defined in the property bag. </returns>
      
        public string this[string key]
        {
        
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                return this.web.AllProperties[BuildKey(key)] as string;
            }
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            set
            {
                if (this.Contains(key))
                {
                    this.web.SetProperty(BuildKey(key), value);
                }
                else
                {
                    this.web.AddProperty(BuildKey(key), value);
                }
                Update();
            }
        }

        /// <summary>
        /// Save any changes made to this PropertyBag.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Update()
        {
            this.web.Update();
        }

        /// <summary>
        /// Remove a particular config setting from this property bag.
        /// </summary>
        /// <param name="key">The key to remove</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void Remove(string key)
        {
            string fullKey = BuildKey(key);

            if (this.Contains(key))
            {
                this.web.DeleteProperty(fullKey);
                Update();
            }
        }

        /// <summary>
        /// Allow derived classes to change the key that's used
        /// </summary>
        /// <param name="key">The key to build</param>
        /// <returns>the key provided</returns>
        protected virtual string BuildKey(string key)
        {
            return key;
        }
    }
}
