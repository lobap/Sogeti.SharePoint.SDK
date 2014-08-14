//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections;
using System.IO;
using Microsoft.SharePoint;
using Microsoft.SharePoint.UserCode;
using System.Collections.Generic;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace ExecutionModels.Workflow.Sandboxed.Activities
{
    public class CopyLibrary
    {
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public Hashtable CopyLibraryActivity(SPUserCodeWorkflowContext context, string libraryName, string targetSiteUrl)
        {
            Hashtable results = null;
            try
            {
                results = Copylibrary(context.SiteUrl, context.WebUrl, libraryName, targetSiteUrl);
                results["Exception"] = string.Empty;
            }
            catch (Exception e)
            {
                results = new Hashtable();
                results["Exception"] = e.ToString();
            } 

            return (results);
        }
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public Hashtable Copylibrary(string siteUrl, string sourceWebUrl, string libraryName, string targetSiteUrl)
        {
            Hashtable results = new Hashtable();
            using (SPSite sourceSite = new SPSite(siteUrl))
            {
                using (SPWeb sourceWeb = sourceSite.OpenWeb())
                {
                    SPList sourceLib = sourceWeb.Lists.TryGetList(libraryName);
                    if (sourceLib != null)
                    {
                        // We have a source list, is it a library?
                        if ((sourceLib as SPDocumentLibrary) == null)
                        {
                            throw new InvalidOperationException("Source list is not a document library");
                        }

                        // library
                        using (SPSite targetSite = new SPSite(targetSiteUrl))
                        {
                            using (SPWeb targetWeb = targetSite.OpenWeb())
                            {
                                SPList targetLib = targetWeb.Lists.TryGetList(libraryName);
                                if (targetLib != null)
                                {
                                    throw new InvalidOperationException("Target library already exists");
                                }
                                else
                                {
                                    Guid targetListGuid = targetWeb.Lists.Add(libraryName, sourceLib.Description, sourceLib.BaseTemplate);
                                    targetLib = targetWeb.Lists.GetList(targetListGuid, true);

                                    // Copy Content Types
                                    foreach (SPContentType sourceCt in sourceLib.ContentTypes)
                                    {
                                        SPContentType sourceWebCt = sourceCt.Parent;
                                        SPContentType targetCt = targetLib.ContentTypes[targetLib.ContentTypes.BestMatch(sourceWebCt.Id)];
                                        if (!targetCt.Id.IsChildOf(sourceWebCt.Id))
                                        {
                                            // Target list doesn't contain content type, check target web
                                            SPContentType targetWebCt = targetWeb.AvailableContentTypes[targetWeb.AvailableContentTypes.BestMatch(sourceWebCt.Id)];
                                            if (targetWebCt == null || targetWebCt.Id != sourceWebCt.Id)
                                            {
                                                // Add to target web
                                                targetWeb.ContentTypes.Add(sourceCt.Parent);
                                                targetWeb.Update();
                                            }
                                            targetLib.ContentTypes.Add(sourceCt);
                                            targetLib.Update();
                                        }
                                    }

                                    // Copy Fields
                                    foreach (SPField field in sourceLib.Fields)
                                    {
                                        if (targetLib.Fields[field.Id] == null)
                                        {
                                            targetLib.Fields.Add(field);
                                        }
                                    }

                                    int copiedFiles = CopyFolder(sourceLib.RootFolder, targetLib.RootFolder, true);
                                    results["status"] = "success";
                                    results["copiedFiles"] = copiedFiles;
                                    return results;
                                }
                            }
                        }
                    }
                }
            }
            results["status"] = "failure";
            results["copiedFiles"] = 0;
            return (results);
        }

        /// <summary>
        /// Copies all the files in a folder and optionally all sub-folders
        /// </summary>
        /// <param name="source">Source folder to copy from</param>
        /// <param name="target">Target folder to copy to</param>
        /// <param name="includeSubFolders">indicates whether sub-folders will be copied</param>
        /// <returns>number of items copied</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public int CopyFolder(SPFolder source, SPFolder target, bool includeSubFolders)
        {
            int filesCopied = 0;
            //if (includeSubFolders)
            //{
            //    foreach (SPFolder sourceSub in source.SubFolders)
            //    {
            //        SPFolder targetFolder = null;

            //        for (int i = 0; i < target.SubFolders.Count; i++)
            //        {
            //            if (target.SubFolders[i].Url == sourceSub.Url)
            //            {
            //                targetFolder = target.SubFolders[i];
            //                break;
            //            }
            //        }

            //        if (targetFolder == null)
            //            targetFolder = target.SubFolders.Add(sourceSub.Url);

            //        filesCopied += CopyFolder(sourceSub, targetFolder, includeSubFolders);
            //    }
            //}

            // Copy files
            List<string> targetFiles = new List<string>();
            foreach (SPFile file in target.Files)
            {
                targetFiles.Add(file.Name);
            }

            foreach (SPFile sourceFile in source.Files)
            {
                if (targetFiles.Contains(sourceFile.Name)) continue; // Skip existing files
                byte[] data = sourceFile.OpenBinary();
                SPFile targetFile = target.Files.Add(sourceFile.Name, data);
                SPListItem targetItem = targetFile.Item;
                SPListItem sourceItem = sourceFile.Item;
                foreach (SPField field in sourceItem.Fields)
                {
                    if (!IsSystemOrReadOnlyField(field))
                    {
                        targetItem[field.Id] = sourceItem[field.Id];
                    }
                }

                targetItem.Update();
                filesCopied++;
            }
            return (filesCopied);
        }
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]

        public static bool IsSystemOrReadOnlyField(SPField field)
        {
            if (!string.IsNullOrEmpty(field.AggregationFunction) ||
                field.ReadOnlyField ||
                field.Type == SPFieldType.Attachments ||
                field.Type == SPFieldType.Calculated ||
                field.Type == SPFieldType.Computed ||
                field.Type == SPFieldType.Counter ||
                field.Type == SPFieldType.CrossProjectLink ||
                field.Type == SPFieldType.Error ||
                field.Type == SPFieldType.File ||
                field.Type == SPFieldType.Invalid ||
                field.Type == SPFieldType.MaxItems ||
                field.Type == SPFieldType.ModStat ||
                field.Type == SPFieldType.PageSeparator ||
                field.InternalName.IndexOf("_") == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}


