//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.SharePoint;
using System.Globalization;
using Microsoft.Practices.SharePoint.Common.Properties;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// Manages creation of the configuration list, and getting/setting values to the configuration list.
    /// Used by list based property bags.
    /// </summary>
    public class ConfigurationList
    {
        // constants

        /// <summary>
        /// The name of the list to store configuration
        /// </summary>
        public static readonly string ConfigListName = "Microsoft_Practices_SharePoint_ConfigList";

        /// <summary>
        /// The title of the list to store configuration
        /// </summary>
        public static readonly string ConfigListTitle = "Microsoft patterns &amp; practices Configuration Data";

        /// <summary>
        /// The name of the field that holds the configuration key
        /// </summary>
        public static readonly string ConfigKeyField = "SettingKey";

        /// <summary>
        /// The name of the field that holds the configuration value
        /// </summary>
        public static readonly string ConfigValueField = "SettingValue";

        /// <summary>
        /// The name of the title field
        /// </summary>
        public static readonly string TitleField = "Title";

        /// <summary>
        /// The name of the field that contains the unique id associated with the web that owns the config
        /// </summary>
        public static readonly string WebIdField = "SettingWebId";

        /// <summary>
        /// The name of the content type group to add this content type to for the config list.
        /// </summary>
        public static readonly string PNPContentTypeGroup = "patterns &amp; practices Content Types";

        /// <summary>
        /// The name of the content type created for defining configuratio ndata.
        /// </summary>
        public static readonly string ConfigContentTypeName = "ConfigStoreContentType";

        /// <summary>
        /// The GUID ID for the field holding the configuration key
        /// </summary>
        public static readonly Guid SettingKeyFieldId = new Guid("{891B57CF-B826-4B0C-9EDF-8948C824D96F}");

        /// <summary>
        /// The GUID ID for the field holding the configuration value.
        /// </summary>
        public static readonly Guid SettingValueFieldId = new Guid("{A0BE1995-F8BE-4AEA-8C54-13EEB207D88E}");

        /// <summary>
        /// The ID for the field holding the unique ID for the web for the config item.
        /// </summary>
        public static readonly Guid SettingWebIdFieldId = new Guid("{30C0BFD9-16B3-4D56-AEB1-A1B3C7950EA7}");

        /// <summary>
        /// The unique ID for the content type for the configuration list.
        /// </summary>
        public static readonly SPContentTypeId ConfigurationContentTypeId = new SPContentTypeId("0x0100FF4AAD791DCC425D85F03F3F1DE61402");

        /// <summary>
        /// The CAML definition fo the web ID site column.
        /// </summary>
        public static readonly string SettingWebIdFieldDefXml = "<Field ID=\"{30C0BFD9-16B3-4D56-AEB1-A1B3C7950EA7}\"" +
                                                     " Name=\"SettingWebId\"" +
                                                     " StaticName=\"SettingWebId\"" +
                                                     " Type=\"Text\"" +
                                                     " DisplayName=\"Setting Web Id\"" +
                                                     " Group=\"p &amp; p Columns\"" +
                                                     " DisplaceOnUpgrade=\"TRUE\"" +
                                                     " ></Field>";

        /// <summary>
        /// The CAML for creating the setting key site column.
        /// </summary>
        public static readonly string SettingKeyFieldDefXml = "<Field ID=\"{891B57CF-B826-4B0C-9EDF-8948C824D96F}\"" +
                                                            " Name=\"SettingKey\"" +
                                                            " StaticName=\"SettingKey\"" +
                                                            " Type=\"Text\"" +
                                                            " DisplayName=\"Setting Key\"" +
                                                            " Group=\"p &amp; p Columns\"" +
                                                            " DisplaceOnUpgrade=\"TRUE\"" +
                                                            " ></Field>";

        /// <summary>
        /// The CAML for creating the setting value site column.
        /// </summary>
        
        public static readonly string SettingValueFieldDefXml = "<Field ID=\"{A0BE1995-F8BE-4AEA-8C54-13EEB207D88E}\"" +
                                                            " Name=\"SettingValue\"" +
                                                            " StaticName=\"SettingValue\"" +
                                                            " Type=\"Note\"" +
                                                            " DisplayName=\"Setting Value\"" +
                                                            " MaxLength =\"4096\"" +
                                                            " NumLines=\"10\"" +
                                                            " Group=\"p &amp; p Columns\"" +
                                                            " DisplaceOnUpgrade=\"TRUE\"" +
                                                            " ></Field>";

        SPList _listInstance = null;
        SPSite site;

        /// <summary>
        /// Gets the configuration list instance.
        /// </summary>
        protected SPList ListInstance
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
           [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                if (_listInstance == null)
                {
                    _listInstance = GetList(site);
                }
                return _listInstance;
            }
        }

        /// <summary>
        /// Gets the list for the configuration from the root web of a site.
        /// </summary>
        /// <param name="site">The site to get the config list for</param>
        /// <returns>The list instance for configuration</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]

        static protected SPList GetList(SPSite site)
        {
            try
            {
                SPWeb rootWeb = site.RootWeb;
                string listUrl = string.Format("/Lists/{0}", ConfigurationList.ConfigListName);
                string relativeListUrl = null;

                if (rootWeb.ServerRelativeUrl == "/")
                {
                    relativeListUrl = listUrl;
                }
                else
                {
                    relativeListUrl = string.Concat(rootWeb.ServerRelativeUrl, listUrl);
                }

                SPList configList = rootWeb.GetList(relativeListUrl);
                return configList;
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Change to take SPSite.
        /// </summary>
        /// <param name="site">The site to load the configuration list for</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public ConfigurationList(SPSite site)
        {
            this.site = site;
            if (ListInstance == null)
                throw new ConfigurationException("Configuration list was missing");
        }

        /// <summary>
        /// Ensures the configuration list is created for the site provided.  Checks if the list
        /// and all related content types and site columns are created, and creates them if they do
        /// not exist.
        /// </summary>
        /// <param name="site">The site to ensure the existance of the config list for</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static void EnsureConfigurationList(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;
            SPList list = GetList(site);
            bool listIsDirty = false;

            // Verify list exists
            if (list == null)
            {
                // Create list
                Guid listGuid = rootWeb.Lists.Add(ConfigListName, ConfigListTitle, SPListTemplateType.GenericList);
                list = rootWeb.Lists[listGuid];
                listIsDirty = true;
            }

            // Add/Remove Content Types
            SPContentType contentType = EnsureContentType(site);
            if (!list.ContentTypes.BestMatch(ConfigurationContentTypeId).IsChildOf(ConfigurationContentTypeId))
            {
                // Content Type isn't present
                list.ContentTypesEnabled = true;
                list.ContentTypes.Add(contentType);
                RemoveCTDirectDescendantFromList(rootWeb, list, new SPContentTypeId("0x01"));
                listIsDirty = true;
            }

            // Add Index
            SPField webIdField = list.Fields[SettingWebIdFieldId];

            bool indexFound = false;
            foreach (SPFieldIndex indx in list.FieldIndexes)
            {
                if (indx.FieldCount == 1 &&
                    indx.GetField(0) == SettingWebIdFieldId)
                {
                    indexFound = true;
                    break;
                }
            }

            if (!indexFound)
            {
                list.FieldIndexes.Add(webIdField);
                listIsDirty = true;
            }

            if (list.Hidden == false)
            {
                list.Hidden = true;
                listIsDirty = true;
            }

            if (listIsDirty) list.Update();
        }

        private static void RemoveCTDirectDescendantFromList(SPWeb web, SPList list, SPContentTypeId ctId)
        {
            SPContentTypeId bestMatch = list.ContentTypes.BestMatch(ctId);
            if (bestMatch.IsChildOf(ctId))
            {
                if (bestMatch.Parent.Equals(ctId))
                {
                    // Is direct descendant
                    list.ContentTypes.Delete(bestMatch);
                }
            }
        }

        private static SPContentType EnsureContentType(SPSite site)
        {
            SPWeb web = site.RootWeb;
            SPContentType configContentType = web.AvailableContentTypes[ConfigurationContentTypeId];

            if (configContentType == null)
            {
                try
                {
                    EnsureSiteColumns(web);
                    configContentType = new SPContentType(ConfigurationContentTypeId, web.ContentTypes, ConfigContentTypeName);

                    web.ContentTypes.Add(configContentType);
                    configContentType.Group = PNPContentTypeGroup;
                    configContentType.FieldLinks.Add(new SPFieldLink(web.AvailableFields[SettingWebIdFieldId]));
                    configContentType.FieldLinks.Add(new SPFieldLink(web.AvailableFields[SettingKeyFieldId]));
                    configContentType.FieldLinks.Add(new SPFieldLink(web.AvailableFields[SettingValueFieldId]));
                    configContentType.Update();
                }
                catch (Exception ex)
                {
                    //try to cleanup
                    configContentType = web.AvailableContentTypes[ConfigurationContentTypeId];

                    if (configContentType != null)
                        web.ContentTypes.Delete(ConfigurationContentTypeId);

                    ConfigurationException configEx = new ConfigurationException(Resources.CreateConfigContentTypeFailed, ex);
                    throw (configEx);
                }
            }

            return configContentType;
        }

        private static void EnsureSiteColumns(SPWeb web)
        {
            if (web.AvailableFields.Contains(SettingKeyFieldId) == false)
                web.Fields.AddFieldAsXml(SettingKeyFieldDefXml);

            if (web.AvailableFields.Contains(SettingValueFieldId) == false)
                web.Fields.AddFieldAsXml(SettingValueFieldDefXml);

            if (web.AvailableFields.Contains(SettingWebIdFieldId) == false)
                web.Fields.AddFieldAsXml(SettingWebIdFieldDefXml);
        }

        /// <summary>
        /// Checks if a key is in the list for the context ID provided.  
        /// </summary>
        /// <param name="key">The name of the key to check for</param>
        /// <param name="contextId">The ID that is unique for the configuration context, for example the ID of an SPWeb</param>
        /// <returns></returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool ContainsKey(string key, string contextId)
        {
            return Get(key, contextId) != null;
        }

        /// <summary>
        /// Saves they value for the key provided for the given context into the configuration list.
        /// </summary>
        /// <param name="key">The key to save</param>
        /// <param name="value">The value to save</param>
        /// <param name="contextId">The ID that is unique for the configuration context, for example the ID of a SPWeb</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Save(string key, string value, string contextId)
        {
            Validation.ArgumentNotNull(key, "key");
            Validation.ArgumentNotNull(value, "value");

            SPListItemCollection items = QueryListItems(key, contextId);

            if (items.Count == 0)
            {

                SPList list = ListInstance;
                SPListItem item = list.AddItem();
                item[SettingKeyFieldId] = key;
                item[SettingWebIdFieldId] = contextId;
                item[SettingValueFieldId] = value;
                item["Title"] = key;
                item.Update();
            }
            else
            {
                SPListItem item = items[0];
                item[ConfigValueField] = value;
                item.Update();
            }
        }

        /// <summary>
        /// Removes the setting for the key and context ID provided.
        /// </summary>
        /// <param name="key">The key of the configuration item to remove</param>
        /// <param name="contextId">The ID that is unique for the configuration context, for example the ID of a SPWeb</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Remove(string key, string contextId)
        {
            SPListItemCollection items = QueryListItems(key, contextId);

            if (items.Count > 0)
            {
                SPListItem item = items[0];
                item.Delete();
            }
        }

        /// <summary>
        /// Gets the value of a setting for the key and context provided.
        /// </summary>
        /// <param name="key">The key for the setting</param>
        /// <param name="contextId">The ID that is unique for the configuration context, for example the ID of a SPWeb</param>
        /// <returns>The item retrieved, null if no item existed</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public string Get(string key, string contextId)
        {
            SPListItemCollection items = QueryListItems(key, contextId);

            if (items.Count == 0)
                return null;

            return items[0][ConfigValueField] as string;
        }

        private SPListItemCollection QueryListItems(string key, string contextId)
        {
            const string camlQuery = @"<Where><And>
                                        <Eq><FieldRef Name='SettingWebId' /><Value Type='Text'>{0}</Value></Eq>
                                        <Eq><FieldRef Name='SettingKey' /><Value Type='Text'>{1}</Value></Eq>
                                        </And></Where>";

            string query = string.Format(CultureInfo.InvariantCulture, camlQuery, contextId, key);
            SPList list = ListInstance;
            SPQuery oQuery = new SPQuery();
            oQuery.Query = query;
            oQuery.ViewFields = "<FieldRef Name='SettingWebId'/><FieldRef Name='SettingKey'/><FieldRef Name='SettingValue'/>";
            return list.GetItems(oQuery);
        }
    }
}
