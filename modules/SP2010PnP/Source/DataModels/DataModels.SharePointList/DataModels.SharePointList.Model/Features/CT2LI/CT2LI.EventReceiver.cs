//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace DataModels.SharePointList.Model.Features.DataModels.InternalData.Model.CT2LI
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("99d757a7-10a5-4fd5-9ad6-c9fc8668895d")]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class DataModelsInternalDataModelEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSite thisSite = properties.Feature.Parent as SPSite;
                SPWeb rootWeb = thisSite.RootWeb;
                SPContentTypeId itemCtId = new SPContentTypeId("0x01");
                RemoveCTDirectDescendantFromList(rootWeb, Constants.ListUrls.Categories, itemCtId);
                RemoveCTDirectDescendantFromList(rootWeb, Constants.ListUrls.Departments, itemCtId);
                RemoveCTDirectDescendantFromList(rootWeb, Constants.ListUrls.InventoryLocations, itemCtId);
                RemoveCTDirectDescendantFromList(rootWeb, Constants.ListUrls.MachineDepartments, itemCtId);
                RemoveCTDirectDescendantFromList(rootWeb, Constants.ListUrls.MachineParts, itemCtId);
                RemoveCTDirectDescendantFromList(rootWeb, Constants.ListUrls.Machines, itemCtId);
                RemoveCTDirectDescendantFromList(rootWeb, Constants.ListUrls.Manufacturers, itemCtId);
                RemoveCTDirectDescendantFromList(rootWeb, Constants.ListUrls.Parts, itemCtId);
                RemoveCTDirectDescendantFromList(rootWeb, Constants.ListUrls.PartSuppliers, itemCtId);
                RemoveCTDirectDescendantFromList(rootWeb, Constants.ListUrls.Suppliers, itemCtId);

                //Restrict deletion of list items that would create a broken lookup
                RestrictDeleteOnLookupField(rootWeb, Constants.ListUrls.InventoryLocations, Constants.Fields.Guids.Part);
                RestrictDeleteOnLookupField(rootWeb, Constants.ListUrls.Machines, Constants.Fields.Guids.Manufacturer);
                RestrictDeleteOnLookupField(rootWeb, Constants.ListUrls.Machines, Constants.Fields.Guids.Category);
                RestrictDeleteOnLookupField(rootWeb, Constants.ListUrls.MachineDepartments, Constants.Fields.Guids.Department);
                RestrictDeleteOnLookupField(rootWeb, Constants.ListUrls.MachineDepartments, Constants.Fields.Guids.Machine);
                RestrictDeleteOnLookupField(rootWeb, Constants.ListUrls.MachineParts, Constants.Fields.Guids.Machine);
                RestrictDeleteOnLookupField(rootWeb, Constants.ListUrls.MachineParts, Constants.Fields.Guids.Part);
                RestrictDeleteOnLookupField(rootWeb, Constants.ListUrls.PartSuppliers, Constants.Fields.Guids.Part);
                RestrictDeleteOnLookupField(rootWeb, Constants.ListUrls.PartSuppliers, Constants.Fields.Guids.Supplier);

                // Add views
                AddAllViewFilesToLists(properties);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.ToString()); }

        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void AddAllViewFilesToLists(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            SPWeb web = site.RootWeb;

            XmlNode featureNode = properties.Definition.GetXmlDefinition(System.Threading.Thread.CurrentThread.CurrentCulture);

            XmlNodeList elementNodes = featureNode.SelectNodes("/*[local-name()='Feature']/*[local-name()='ElementManifests']/*[local-name()='ElementFile']");

            if (elementNodes != null)
            {
                foreach (XmlNode node in elementNodes)
                {
                    string viewRelativePath = node.Attributes["Location"].Value;
                    string listRelativeUrl = GetListRelativeUrlFromPath(viewRelativePath);
                    string viewXmlLocation = properties.Definition.RootDirectory + '\\' + viewRelativePath;
                    string viewString = File.ReadAllText(viewXmlLocation);
                    AddViewToList(web, listRelativeUrl, viewString);
                }
            }
        }

        private string GetListRelativeUrlFromPath(string relativePath)
        {
            int firstSlash = relativePath.IndexOf('\\');
            int lastSlash = relativePath.LastIndexOf('\\');
            string retVal = relativePath.Substring(firstSlash + 1, lastSlash - firstSlash - 1);
            return retVal;
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void AddViewToList(SPWeb web, string listUrl, string viewString)
        {
            SPList list = web.GetList(GetListUrl(web.ServerRelativeUrl, listUrl));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(viewString);
            SPView loadedView = new SPView(list, doc);

            SPView newView = list.Views.Add(loadedView.Title, loadedView.ViewFields.ToStringCollection(), loadedView.Query,
                                            loadedView.RowLimit, loadedView.Paged, loadedView.DefaultView);
            string viewXmlString = doc.DocumentElement.OuterXml;
            newView.SetViewXml(viewXmlString);

            newView.Update();
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void RestrictDeleteOnLookupField(SPWeb web, string listUrl, Guid fieldGuid)
        {
            SPList list = web.GetList(GetListUrl(web.ServerRelativeUrl, listUrl));
            SPField field = list.Fields[fieldGuid];
            SPFieldLookup fieldLookup = (SPFieldLookup)field;
            fieldLookup.Indexed = true;
            fieldLookup.RelationshipDeleteBehavior = SPRelationshipDeleteBehavior.Restrict;
            fieldLookup.Update();
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void RemoveCTDirectDescendantFromList(SPWeb web, string listUrl, SPContentTypeId ctId)
        {
            SPList list = web.GetList(GetListUrl(web.ServerRelativeUrl, listUrl));

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

        private string GetListUrl(string webRelativeUrl, string listUrl)
        {
            if (webRelativeUrl[webRelativeUrl.Length - 1] != '/') return (webRelativeUrl + '/' + listUrl);
            else return (webRelativeUrl + listUrl); // Root web case
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
