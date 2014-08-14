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
using Microsoft.SharePoint;
using System.IO;
using System.Collections;
using Microsoft.SharePoint.Workflow;

namespace PublishData
{
    class Program
    {
        static void Main(string[] args)
        {
            string _siteName;
            string _webName;
            string _hostname;

            string _projectName;
            string _fileName;
            string _contentType;
            string _title;
            string _vendorId;
            string _vendorName;
            string _projectsLookup;
            string _sowStatus;
            string _estimateValue;
            string _paramSiteUrl;
            string _paramCtId;
            string _paramWfTemplateName;
            string _paramAssocName;
            string _paramTaskListName;
            string _paramHistoryListName;



            _siteName = "http://localhost/sites/Manufacturing";
            _webName = "Maintenance";

            # region Debug Adding Project
            // args = new string[3];

            //args[0] = "/Projects";
            //args[1] = "http://"+ System.Environment.MachineName  + "/sites/Manufacturing";
            //args[2] = "test project";
            # endregion
            # region Debug Adding Estimates
            //args = new string[11];
            //args[0] = "/Estimates";
            //args[1] = "http://" + System.Environment.MachineName + "/sites/Manufacturing";
            //args[2] = "Construction";
            //args[3] = "test.txt";
            //args[4] = "test";
            //args[5] = "SOW";
            //args[6] = "1";
            //args[7] = "clname1";
            //args[8] = "12";
            //args[9] = "Approved";
            //args[10] = "1";
            # endregion
            #region Debug WFAssociation
            //args = new string[7];
            //args[0] = "/WFAssociation";
            ////paramSiteUrl 
            //args[1] = "http://spgv3-01/sites/ManufacturingWF";
            ////paramCtId 
            //args[2] = "0x010100B022E34803AB43AEA87C937368261B49";
            ////paramWfTemplateName = 
            //args[3] = "Create Project Site";
            ////paramAssocName = 
            //args[4] = "WFRITEST1";
            ////paramTaskListName = 
            //args[5] = "Tasks";
            ////paramHistoryListName = 
            //args[6] = "Workflow History";
            #endregion


            if (args[0] == "/WFAssociation")
            {
                #region WFAssociation
                _paramSiteUrl = args[1];
                _paramCtId = args[2];
                _paramWfTemplateName = args[3];
                _paramAssocName = args[4];
                _paramTaskListName = args[5];
                _paramHistoryListName = args[6];

                CreateWFAssociation(_paramSiteUrl, _paramCtId, _paramWfTemplateName, _paramAssocName, _paramTaskListName, _paramHistoryListName);


                #endregion

            }
            else if (args[0] == "/Projects")
            {
                # region Projects


                try
                {

                    if (args[2].Trim().Length > 0)
                    {

                        _siteName = args[1];
                        _projectName = args[2];

                        Console.WriteLine("Updating Projects List in {0}", args[1]);
                        AddProject(_siteName, "Projects", _projectName);
                    }
                    else
                    {
                        //Console.WriteLine("Cannot Add blank Project.");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                # endregion


            }
            else if (args[0] == "/Estimates")
            {
                #region Estimates
                //TODO:Content type is not used while adding..

                try
                {
                    if (args[2].Trim().Length > 0)
                    {

                        _siteName = args[1];            //site collection name where below web exists
                        _webName = args[2];            //web name where Estimates available
                        _fileName = args[3];          //document name
                        _title = args[4];            //Title for the given document/file
                        _contentType = args[5];     //This is the content type to be used
                        _vendorId = args[6];       //vendor id
                        _vendorName = args[7];    //vendor name
                        _estimateValue = args[8]; //Estimate value:eg:- $20
                        _sowStatus = args[9];     //SOW Status: eg:-Draft/submitted/Approved
                        _projectsLookup = args[10]; //Projects Lookup
                        Console.WriteLine("Updating Estimates in {0}/{1} for  {2}", args[1], args[2], args[4]);


                        AddSOWToEstimates(_siteName, _webName, _fileName, _title, _contentType, _vendorId, _vendorName, _estimateValue, _sowStatus, _projectsLookup);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
                #endregion

            }
            else if (args[0] == "/UpdateHost")
            {
                #region UpdateHost
                try
                {
                    _fileName = args[1];
                    _hostname = args[2];
                  //  Console.WriteLine("1");
                    ReplaceServiceHostComputerNamer(_fileName, _hostname);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                #endregion
            }


        }
        /// <summary>
        /// Adding data by passing arguements as listed:
        /// </summary>
        /// <param name="SiteName"></param>
        /// <param name="WebName"></param>
        /// <param name="FileName"></param>
        /// <param name="Title"></param>
        /// <param name="ContentType"></param>
        /// <param name="ClientId"></param>
        /// <param name="ClientName"></param>
        /// <param name="EstimateValue"></param>
        /// <param name="SOWStatus"></param>
        /// <param name="ProjectsLookup"></param>
        static void AddSOWToEstimates(string SiteName, string WebName, string FileName, string Title, string ContentType, string VendorId, string VendorName,
            string EstimateValue, string SOWStatus, string ProjectsLookup)
        {
            // String fileToUpload = @"C:\2.txt";
            String fileToUpload = CreateFile(FileName); ///creating file dynamically in an intended path
            String documentLibraryName = "Estimates";

            try
            {
                using (SPSite oSite = new SPSite(SiteName))
                {
                    using (SPWeb oWeb = oSite.OpenWeb(WebName))
                    {
                        if (System.IO.File.Exists(fileToUpload))
                        {
                            SPFolder myLibrary = oWeb.GetFolder("Lists").SubFolders[documentLibraryName];

                            // Prepare to upload 
                            Boolean replaceExistingFiles = true;
                            String fileName = System.IO.Path.GetFileName(fileToUpload);
                            using (FileStream fileStream = File.OpenRead(fileToUpload))
                            {


                                Hashtable documentProperties = new Hashtable();
                                // documentProperties.Add("vti_title", fileName);
                                documentProperties.Add("vti_title", Title);
                                // documentProperties.Add("ClientId", "1");

                                documentProperties.Add("VendorId", VendorId);
                                //documentProperties.Add("ClientName", "1 Name");
                                documentProperties.Add("VendorName", VendorName);
                                //documentProperties.Add("EstimateValue", "1234");
                                documentProperties.Add("EstimateValue", EstimateValue);
                                //documentProperties.Add("SOWStatus", "Submitted");
                                documentProperties.Add("SOWStatus", SOWStatus);
                                //documentProperties.Add("ProjectsLookup", "2");
                                documentProperties.Add("ProjectsLookup", ProjectsLookup);
                                documentProperties.Add("ContentType", ContentType);

                                // documentProperties.Add("Title", "test2");

                                // Upload document 
                                SPFile spfile = myLibrary.Files.Add(fileName, fileStream, documentProperties,
                                                                    replaceExistingFiles);

                                // Commit  
                                myLibrary.Update();
                            }
                        }
                        else
                        {
                            throw new FileNotFoundException("File not found.", fileToUpload);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void AddProject(string SiteName, string ListName, string Value)
        {
            try
            {
                using (SPSite oSite = new SPSite(SiteName))
                {
                    using (SPWeb oWeb = oSite.OpenWeb())
                    {

                        Console.WriteLine("Adding Project :" + Value);
                        SPList oProjects = oWeb.Lists[ListName];
                        //create new item
                        SPListItem newProject = oProjects.Items.Add();
                        newProject["Title"] = Value;   // Assigning value
                        newProject.Update();          //updating the item in the Projects list

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);


            }

        }

        static string CreateFile(string FileName)
        {
            string _fileName;

            if (!Directory.Exists(System.Environment.CurrentDirectory + "\\Estimates"))
            {
                Directory.CreateDirectory(System.Environment.CurrentDirectory + "\\Estimates");

            }
            _fileName = System.Environment.CurrentDirectory + "\\Estimates\\" + FileName; /// declaring a local variable to specify the file to be created in a specified path
            FileStream file = null;
            try
            {
                file = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(file);
                sw.Write("Welcome To SharePoint Guidance."); ///Including matter in the file generated
                sw.Close();
            }
            finally
            {
                if (file != null)
                    file.Dispose();
            }

            return _fileName;

        }

        static void CreateWFAssociation(string SiteUrl, string CTId, string WfTemplateName, string AssocName, string TaskListName, string HistoryListName)
        {


            try
            {

                using (SPSite site = new SPSite(SiteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        if (!web.Exists) { Console.WriteLine("Web site not found"); return; }
                        SPContentTypeId spCtId = new SPContentTypeId(CTId);
                        SPContentType ct = web.AvailableContentTypes[spCtId];
                        if (ct == null) { Console.WriteLine("Content Type not found"); return; }
                        SPWorkflowTemplate template = GetWorkflowTemplateByName(web, WfTemplateName);
                        if (template == null) { Console.WriteLine("Workflow Template Not found"); return; }
                        SPWorkflowAssociation wFAssoc = SPWorkflowAssociation.CreateWebContentTypeAssociation(template, AssocName, TaskListName, HistoryListName);
                        wFAssoc.AutoStartCreate = true;
                        wFAssoc.AutoStartChange = true;
                        ct.WorkflowAssociations.Add(wFAssoc);
                        ct.UpdateWorkflowAssociationsOnChildren();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:{0}", e.Message);
            }
        }

        private static SPWorkflowTemplate GetWorkflowTemplateByName(SPWeb web, string paramWfTemplateName)
        {
            foreach (SPWorkflowTemplate template in web.WorkflowTemplates)
            {
                if (string.Compare(template.Name, paramWfTemplateName, true) == 0) return template;
            }
            return null;
        }
        static void ReplaceServiceHostComputerNamer(string FILE_NAME, string ComputerName)
        {

            try
            {
                StreamReader reader = new StreamReader(FILE_NAME);
                string content = reader.ReadToEnd();
                reader.Close();
                Console.WriteLine("Replacing ServiceHostComputerName in {0}. with {1}", FILE_NAME, ComputerName);
                //find existing Name
                int startPoint = content.IndexOf("http://");
               // Console.WriteLine(startPoint);
                int endPoint = content.IndexOf(":81", startPoint);
               // Console.WriteLine(endPoint);
                string hostName = content.Substring(startPoint + 7, endPoint - startPoint - 7);
               // Console.WriteLine(hostName);
               // string hostName = "COMPUTERNAME";

                content = System.Text.RegularExpressions.Regex.Replace(content, hostName, ComputerName);

                StreamWriter writer = new StreamWriter(FILE_NAME);
                writer.Write(content);
                writer.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While Replacing ServiceHostComputerName in {0}. Error :{1}", FILE_NAME, ex.Message.ToString());

            }
        }
    }

}
