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
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint;
using System.Collections;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.UserCode;

namespace DataModels.ListOf
{
    public class CreateLink
    {
        public CreateLink() { }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public Hashtable GetRootWebUrlAction(SPUserCodeWorkflowContext context)
        {
            Hashtable results = new Hashtable();
            results["rootWebUrl"] = context.SiteUrl;
            return (results);
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public Hashtable CreateLinkForItemAction(SPUserCodeWorkflowContext context, string targetListUrl, string linkTargetUrl, string linkDescription)
        {

            Hashtable results = new Hashtable();
            try
            {
                targetListUrl = FixUpRelativeUrl(targetListUrl, context.SiteUrl);
                CreateLinkForItem(targetListUrl, linkDescription, linkTargetUrl);
                results["success"] = true;
                results["exception"] = string.Empty;
            }
            catch (Exception e)
            {
                results = new Hashtable();
                results["exception"] = e.ToString();
                results["Success"] = false;
            }

            return (results);
        }

        private string FixUpRelativeUrl(string targetListUrl, string siteUrl)
        {
            Uri targetUri = null;
            if (Uri.TryCreate(targetListUrl, UriKind.RelativeOrAbsolute, out targetUri))
            {
                if (targetUri.IsAbsoluteUri == true) return targetListUrl;
                else
                {
                    if (Uri.TryCreate(new Uri(siteUrl), targetListUrl, out targetUri))
                    {
                        return (targetUri.AbsoluteUri);
                    }
                }
            }
            throw new ArgumentException("Invalid Url");   
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private static void CreateLinkForItem(string listUrl, string description, string linkUrl)
        {
            SPFieldUrlValue val = new SPFieldUrlValue();
            val.Description = description;
            val.Url = linkUrl;
            string fieldResult = val.ToString();
            Dictionary<Guid, object> values = new Dictionary<Guid, object>();
            values.Add(new Guid("{c29e077d-f466-4d8e-8bbe-72b66c5f205c}"), fieldResult); // GUID is the URL field GUID it's defined by WSS
            CreateItemInList(listUrl, values);
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private static void CreateItemInList(string listUrl, Dictionary<Guid, object> fields)
        {
            using (SPSite site = new SPSite(listUrl))
            {
                // SharePoint will open the web provide in the SPSite Constructor.
                // OpenWeb() provides overload with string parameter but may cause additional lookups / DB activity
                using (SPWeb web = site.OpenWeb())
                {
                    Uri listUri = new Uri(listUrl);

                    SPList list = web.GetList(listUri.AbsolutePath); // throws exception if not found

                    SPListItem item = list.Items.Add();
                    foreach (KeyValuePair<Guid, object> kvp in fields)
                    {
                        item[kvp.Key] = kvp.Value;
                    }
                    item.Update();
                }
            }
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static void CreateItemInList(string listUrl, Dictionary<string, object> fields)
        {
            using (SPSite site = new SPSite(listUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    Uri listUri = new Uri(listUrl);
                    string serverRelativeListUrl = listUri.AbsolutePath.Substring(web.ServerRelativeUrl.Length);
                    SPList list = web.GetList(serverRelativeListUrl); // throws exception if not found

                    SPListItem item = list.Items.Add();
                    foreach (KeyValuePair<string, object> kvp in fields)
                    {
                        Guid fieldGuid = list.Fields.GetFieldByInternalName(kvp.Key).Id;
                        item[fieldGuid] = kvp.Value;
                    }
                    item.Update();
                }
            }
        }
    }
}
