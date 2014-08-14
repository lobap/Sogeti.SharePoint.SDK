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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using Microsoft.Office.Server.ApplicationRegistry.Administration;
using Microsoft.Office.Server.ApplicationRegistry.Infrastructure;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint;
using WSSAdmin = Microsoft.SharePoint.Administration;
//using OSSAdmin = Microsoft.Office.Server.Administration;
using Administration = Microsoft.Office.Server.ApplicationRegistry.Administration;




namespace ContosoSetup
{
    class Program
    {

        # region variables
        static string SSPName = "SharedServices3";
        static string[] XMLFileLocation;
        static string VirDirPath;
        static string SQLServer;
        static string ContosoWebURL;
        # endregion

        static void Main(string[] args)
        {

            # region Debug Section
            //args = new string[3];
            //args[0] = "/testdata";
            //args[1] = "http://localhost:44354";
            //args[2] = "http://localhost";

            //args = new string[4];
            //args[0] = "/ssp";
            //args[1] = "Sharedservices1";
            //args[2] = "http://localhost";
            //args[3] = @"C:\TFS-New\Trunk\Source\PSS\ProductCatalogDefinition.xml";

            # endregion


            if ((args[0] == "/webconfig") || (args[0] == "/centralwebconfig"))
            {

                # region Update WebConfig For FBA
                //This Section will update Web.config with proper xml content for FBA setup
                //Required following params
                // ContosoSetup.exe /WebConfig SQLSErvername WEBConfigPath
                SQLServer = args[1];
                VirDirPath = args[2].Replace("\"", "");
                string WebConfig = VirDirPath + @"\web.config";
                Console.WriteLine("Updating Web.Config for  Web.");
                Console.WriteLine("Web.Config:" + WebConfig);
                try
                {
                    AddConnectionString(WebConfig, SQLServer);
                    AddMembership(WebConfig);
                    AddPeoplePickerWildcards(WebConfig);
                    if ((args[0] == "/webconfig"))
                    {
                        AddRoleManager(WebConfig, "PartnerGroups");
                    }
                    else
                    {
                        AddRoleManager(WebConfig, "AspNetWindowsTokenRoleProvider");
                    }
                }
                catch (Exception ex)
                { Console.WriteLine("Exception While updating Web.config: " + ex.Message.ToString()); }

                #endregion webconfig
            }
            else if (args[0] == "/testdata")
            {
                # region Add TestData into Subweb Under CentralAdmin

                //This Section update SPG subsite list with test data(subsite exists under Central Admin)
                //Params are
                //Usage : ContosoSEtup.exe /testdata CentralAdminURL ContosoWebURL
                //it will consider that partner1 and partner2 are created under ContosoWebURL/sites
                //Ex: if contosowebURL is http://localhost:9001 then partner1 url should http://localhost:9001/sites/partner1

                string siteURL = args[1]; //CentralAdmin URL
                string ContosoWebAppURL = args[2];  //ContosoWebURL
                string ContosoExtWebAppURL = args[3];  //ContosoWebURL

                try
                {
                    Console.WriteLine("Adding Partner URLs into List...");
                    PCSitestestdata.AddPartnersURLs(siteURL, ContosoWebAppURL, ContosoExtWebAppURL);

                    Console.WriteLine("Adding User Profiles into SharePoint User Profile store...");
                    PCSitestestdata.AddUserProfiles(siteURL);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception While Adding Partners: {0}", ex.ToString());
                }
                # endregion
            }
            else if (args[0] == "/wcfconfig")
            {
                # region This Section Update provided Webconfig with EndPoints information & update SiteMapProvider
                //It will add Service.Model with endpoints info into Provided Web.Config File
                //NOTE : It will not Check before Add

                VirDirPath = args[1].Replace("\"", "");
                string Root_Webconfig = VirDirPath + @"\web.config";
                Console.WriteLine("Updating Web.Config for Root Web.");
                Console.WriteLine("Web.Config:" + Root_Webconfig);
                try
                {
                    UpdateWCFTags(Root_Webconfig);
                }
                catch (Exception ex)
                { Console.WriteLine("Exception while updating webconfig with WCF info:" + ex.Message.ToString()); }

                // update web.config with sitemapprovider
                try
                {
                    UpdateSiteMapProvider(Root_Webconfig);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception while updating  SiteMapProvider info:" + ex.Message.ToString());
                }
                # endregion

            }
            else if (args[0] == "/bdcfbaaccess")
            {
                # region Grant BDC Access to FBA Users & Roles
                //This is connect SSP and add permissions for FBA Roles & users
                //usage : ContosoSetup.exe /bdcfbaaccess SSPName


                SSPName = args[1];
                Console.WriteLine("Connecting to SSP:" + SSPName);

                try
                {
                    SetupBDC();
                    Console.WriteLine("Adding Permissions for Partners:ContosofbaAdmin");
                    AddFBAAccess("Partners:ContosofbaAdmin");
                    Console.WriteLine("Adding Permissions for PartnerGroups:Contosofbapartner1");
                    AddFBAAccess("PartnerGroups:Contosofbapartner1");
                    Console.WriteLine("Adding Permissions for PartnerGroups:Contosofbapartner2");
                    AddFBAAccess("PartnerGroups:Contosofbapartner2");
                }
                catch (Exception ex)
                { Console.WriteLine("Exception While Adding FBA Users/Roles into BDC :" + ex.Message.ToString()); }
                # endregion

            }

            else if (args[0] == "/bdcwinaccess")
            {
                # region Grant BDC Access to Win Users & Roles
                //This is connect SSP and add permissions for FBA Roles & users
                //usage : ContosoSetup.exe /bdcfbaaccess SSPName


                SSPName = args[1];
                Console.WriteLine("Connecting to SSP:" + SSPName);

                string domainName = args[2];
                Console.WriteLine("domainName" + domainName);
                try
                {
                    SetupBDC();
                    Console.WriteLine("Adding Permissions for " + domainName + "\\ContosoWinPartner1");
                    AddFBAAccess(domainName + "\\ContosoWinPartner1");
                    Console.WriteLine("Adding Permissions for " + domainName + "\\ContosoWinPartner2");
                    AddFBAAccess(domainName + "\\ContosoWinPartner2");
                }
                catch (Exception ex)
                { Console.WriteLine("Exception While Adding WIN Users/Roles into BDC :" + ex.Message.ToString()); }
                # endregion

            }

            else if (args[0] == "/ssp")
            {
                # region Import Application Def into SSP
                //This will connect to provided SSP and import Application Def
                //If all ready exists then it will delete existing one and import new one
                //Add profile page with contosoweburl, where product.aspx exists
                //Usage : ContosoSetup.Exe /ssp SSPName ContosoWebURL AppDefFilePath1 AppDefFilePath2 

                SSPName = args[1];
                ContosoWebURL = args[2];

                XMLFileLocation = new string[args.Length - 2];
                //set fourth arg as BDCFile Path
                for (int i = 3; i < args.Length; i++)
                {
                    XMLFileLocation[i - 3] = args[i];
                }
                try
                {
                    ImportApplicationFile();
                }
                catch (Exception ex)
                { Console.WriteLine("Exception while Importing:" + ex.Message.ToString()); }
                # endregion

               // MapCrawledPropertyToStatusManagedMetadataProperty(ContosoWebURL);

            }
            if ((args[0] == "/lobwebconfig"))
            {

                # region Update WebConfig For ConotosoLobWebConfig
                //Required following params
                // ContosoSetup.exe /lobwebconfig MyComputerNamer c:\spgdrop\source\partnerportal\contoso.lob.web\
                string ComputerName = args[1];
                VirDirPath = args[2].Replace("\"", "");
                string WebConfig = VirDirPath + @"\web.config";
                Console.WriteLine("Replace ServiceHostComputerNamer in Contoso.LOB.");
                Console.WriteLine("Web.Config:" + WebConfig);
                try
                {
                    
                    ReplaceServiceHostComputerNamer(WebConfig, ComputerName);
                }
                catch (Exception ex)
                { Console.WriteLine("Exception While updating Web.config: " + ex.Message.ToString()); }

                #endregion webconfig
            }

        }

        private static void MapCrawledPropertyToStatusManagedMetadataProperty(string url)
        {
            SearchContext context;
            using (SPSite site = new SPSite(url))
            {
                context = SearchContext.GetContext(site);
            }
            Schema sspSchema = new Schema(context);
            ManagedPropertyCollection properties = sspSchema.AllManagedProperties;
            ManagedProperty managedProperty = properties["Status"];
            int vType = 31; //Text vType for Search Server 2008
            Mapping newMapping = new Mapping(new Guid("00130329-0000-0130-c000-000000131346"), "ows_Status", vType, managedProperty.PID);
            MappingCollection mappings = managedProperty.GetMappings();
            mappings.Add(newMapping);
            managedProperty.SetMappings(mappings);
        }

        # region Methods
        static void UpdateSiteMapProvider(string FILE_NAME)
        {
            //Check SiteMapProvider already Exists for Contoso


            try
            {
                XmlTextReader reader = new XmlTextReader(FILE_NAME);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                reader.Close();

                //Select the connectionstrings node for ContosoPartnerfbaSQL
                XmlNode nodeToFind;
                XmlElement root = doc.DocumentElement;

                //Select 
                nodeToFind = root.SelectSingleNode("/configuration/system.web/siteMap/providers/add[@name='CategorySiteMapProvider']");

                if (nodeToFind == null)
                {


                    nodeToFind = root.SelectSingleNode("/configuration/system.web/siteMap/providers");
                    XmlNode contosoSiteMapProvider = doc.CreateNode(XmlNodeType.Element, "add", "");
                    XmlAttribute nameatt = doc.CreateAttribute("name");
                    nameatt.Value = "CategorySiteMapProvider";
                    contosoSiteMapProvider.Attributes.Append(nameatt);
                    XmlAttribute desc = doc.CreateAttribute("description");
                    desc.Value = "Provider for category navigation using Business Data Catalog";
                    contosoSiteMapProvider.Attributes.Append(desc);
                    XmlAttribute type = doc.CreateAttribute("type");
                    type.Value = "Contoso.PartnerPortal.Portal.Navigation.BusinessDataCatalogSiteMapProvider, Contoso.PartnerPortal.Portal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0561205e6cefaed4";
                    contosoSiteMapProvider.Attributes.Append(type);

                    XmlAttribute culture = doc.CreateAttribute("Culture");
                    culture.Value = "neutral";
                    contosoSiteMapProvider.Attributes.Append(culture);

                    XmlAttribute PublicKey = doc.CreateAttribute("PublicKeyToken");
                    PublicKey.Value = "0561205e6cefaed4";
                    contosoSiteMapProvider.Attributes.Append(PublicKey);

                    nodeToFind.AppendChild(contosoSiteMapProvider);
                    //save the output to a file 
                    doc.Save(FILE_NAME);

                    Console.WriteLine("ContosoSiteMapProvider ConnectionString Added !!!!");
                }
                else
                {
                    Console.WriteLine("ContosoSiteMapProvider  Already Exists!!!!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While Adding SiteMapProvider String: {0}", ex.ToString());

            }
        }
        static void AddFBAAccess(string UserName)
        {

            LobSystemInstance mySysInstance = null;
            LobSystemInstanceCollection sysInsCollection = ApplicationRegistry.Instance.GetLobSystemInstancesLikeName("ContosoProductCatalogService");
            foreach (LobSystemInstance sysInstance in sysInsCollection)
            {
                if (sysInstance.Name == "ContosoProductCatalogService")
                {
                    mySysInstance = sysInstance;
                    break;
                }
            }
            LobSystem ls = mySysInstance.LobSystem;
            IAccessControlList acl = ls.GetAccessControlList();
            //replace the domain and user names here
            String currentIdentity = UserName;
            acl.Add(new IndividualAccessControlEntry(currentIdentity, BdcRights.SetPermissions | BdcRights.Execute | BdcRights.Edit | BdcRights.UseInBusinessDataInLists | BdcRights.SelectableInClients));
            try
            {
                ls.SetAccessControlList(acl);
                ls.CopyAclAcrossChildren();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While Adding Access for " + UserName + ". Ex:" + ex.Message);
            }
            Console.WriteLine("Done");


        }
        static void UpdateWCFTags(string FILE_NAME)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(FILE_NAME);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                reader.Close();

                XmlElement root = doc.DocumentElement;
                //Select the connectionstrings node for ContosoPartnerfbaSQL
                XmlNode nodeToFind;

                //Select 
                nodeToFind = root.SelectSingleNode("/configuration/system.serviceModel");

                if (nodeToFind == null)
                {
                    //Select the cd node with the matching title
                    XmlNode sysweb = root.SelectSingleNode("/configuration");

                    XmlNode Wildcards = doc.CreateNode(XmlNodeType.Element, "system.serviceModel", "");
                    Wildcards.InnerXml = @"<bindings>
      <wsHttpBinding>        
        <binding name=""WSHttpBinding_UserNameCredentials"" closeTimeout=""00:01:00""
            openTimeout=""00:01:00"" receiveTimeout=""00:10:00"" sendTimeout=""00:01:00""
            bypassProxyOnLocal=""false"" transactionFlow=""false"" hostNameComparisonMode=""StrongWildcard""
            maxBufferPoolSize=""524288"" maxReceivedMessageSize=""65536""
            messageEncoding=""Text"" textEncoding=""utf-8"" useDefaultWebProxy=""true""
            allowCookies=""false"">
          <readerQuotas maxDepth=""32"" maxStringContentLength=""8192"" maxArrayLength=""16384""
              maxBytesPerRead=""4096"" maxNameTableCharCount=""16384"" />
          <reliableSession ordered=""true"" inactivityTimeout=""00:10:00""
              enabled=""false"" />
          <security mode=""Message"">
            <transport clientCredentialType=""Windows"" proxyCredentialType=""None""
                realm="""" />
            <message clientCredentialType=""UserName"" negotiateServiceCredential=""true""
                algorithmSuite=""Default"" establishSecurityContext=""true"" />
          </security>
        </binding>
        <binding name=""WSHttpBinding_WindowsCredentials"" closeTimeout=""00:01:00""
            openTimeout=""00:01:00"" receiveTimeout=""00:10:00"" sendTimeout=""00:01:00""
            bypassProxyOnLocal=""false"" transactionFlow=""false"" hostNameComparisonMode=""StrongWildcard""
            maxBufferPoolSize=""524288"" maxReceivedMessageSize=""65536""
            messageEncoding=""Text"" textEncoding=""utf-8"" useDefaultWebProxy=""true""
            allowCookies=""false"">
          <readerQuotas maxDepth=""32"" maxStringContentLength=""8192"" maxArrayLength=""16384""
              maxBytesPerRead=""4096"" maxNameTableCharCount=""16384"" />
          <reliableSession ordered=""true"" inactivityTimeout=""00:10:00""
              enabled=""false"" />
          <security mode=""Message"">
            <transport clientCredentialType=""Windows"" proxyCredentialType=""None""
                realm="""" />
            <message clientCredentialType=""Windows"" negotiateServiceCredential=""true""
                algorithmSuite=""Default"" establishSecurityContext=""true"" />
          </security>
        </binding>
      </wsHttpBinding>
      <basicHttpBinding>
        <binding name=""BasicHttpBinding"" closeTimeout=""00:01:00""
            openTimeout=""00:01:00"" receiveTimeout=""00:10:00"" sendTimeout=""00:01:00""
            allowCookies=""false"" bypassProxyOnLocal=""false"" hostNameComparisonMode=""StrongWildcard""
            maxBufferSize=""65536"" maxBufferPoolSize=""524288"" maxReceivedMessageSize=""65536""
            messageEncoding=""Text"" textEncoding=""utf-8"" transferMode=""Buffered""
            useDefaultWebProxy=""true"">
          <readerQuotas maxDepth=""32"" maxStringContentLength=""8192"" maxArrayLength=""16384""
              maxBytesPerRead=""4096"" maxNameTableCharCount=""16384"" />
          <security mode=""None"">
            <transport clientCredentialType=""None"" proxyCredentialType=""None""
                realm="""" />
            <message clientCredentialType=""UserName"" algorithmSuite=""Default"" />
          </security>
        </binding>
      </basicHttpBinding>
    </bindings>
    <client>
      <endpoint address=""http://localhost:8585/Contoso.LOB.Services/Pricing.svc""
                binding=""wsHttpBinding"" bindingConfiguration=""WSHttpBinding_UserNameCredentials""
                contract=""Contoso.LOB.Services.IPricing"" name=""WSHttpBinding_IPricing"" 
                behaviorConfiguration=""ClientCertificateBehavior"">
        <identity>
          <dns value=""localhost"" />
        </identity>
      </endpoint>
      <endpoint address=""http://localhost:8585/Contoso.LOB.Services/ProductCatalog.svc""
                binding=""basicHttpBinding"" bindingConfiguration=""BasicHttpBinding""
                contract=""Contoso.LOB.Services.IProductCatalog"" name=""BasicHttpBinding_IProductCatalog"" />
      <endpoint address=""http://localhost:8585/Contoso.LOB.Services/IncidentManagement.svc""
                binding=""wsHttpBinding"" bindingConfiguration=""WSHttpBinding_UserNameCredentials""
                contract=""Contoso.LOB.Services.IIncidentManagement"" name=""WSHttpBinding_IIncidentManagement""
                behaviorConfiguration=""ClientCertificateBehavior""/>
      <endpoint address=""http://localhost:8585/Microsoft.Practices.SPG.SiteCreation.Service/SubSiteCreation.svc""
                binding=""wsHttpBinding"" bindingConfiguration=""WSHttpBinding_WindowsCredentials""
                contract=""Microsoft.Practices.SPG.SiteCreation.ISubSiteCreation""
                name=""WSHttpBinding_ISubSiteCreation"">
        <identity>
          <dns value=""localhost"" />
        </identity>
      </endpoint>
    </client>
    <behaviors>
      <endpointBehaviors>
        <behavior name=""ClientCertificateBehavior"">
          <clientCredentials>
            <serviceCertificate>
              <authentication certificateValidationMode=""None""/>
            </serviceCertificate>
            <clientCertificate findValue=""localhost"" storeLocation=""CurrentUser"" storeName=""Root"" x509FindType=""FindBySubjectName""/>
          </clientCredentials>
        </behavior>
      </endpointBehaviors>
    </behaviors>";
                    sysweb.AppendChild(Wildcards);

                    //save the output to a file 
                    doc.Save(FILE_NAME);
                    Console.WriteLine("system.serviceModel Updated!!");
                }
                else
                {
                    Console.WriteLine("system.serviceModel already exists!!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While updating System.servicemodel Nodes String: {0}", ex.ToString());

            }


        }
        static void ImportApplicationFile()
        {

            Console.WriteLine("Connecting to SSP:" + SSPName);
            SetupBDC();
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine("Importing Application Defination File from " + XMLFileLocation[i]);
                ImportLobSystemFromXML(XMLFileLocation[i]);
                Console.WriteLine("Imported Application Defination File from " + XMLFileLocation[i]);
                Console.WriteLine("Adding Profile Page Action... ");
                AddProfilePage();
                //Console.WriteLine("Added Profile Page Action... ");
            }
            // Console.WriteLine("Press any key to exit...");
            // Console.Read();
        }

        static void AddProfilePage()
        {
            uint productid = 0;
            uint categoryId = 0;
            LobSystemInstance mySysInstance = null;
            LobSystemInstanceCollection sysInsCollection = ApplicationRegistry.Instance.GetLobSystemInstancesLikeName("ContosoProductCatalogService");
            foreach (LobSystemInstance sysInstance in sysInsCollection)
            {
                if (sysInstance.Name == "ContosoProductCatalogService")
                {
                    mySysInstance = sysInstance;
                    break;
                }
            }
            IList<Entity> entityCollection = new List<Entity>(mySysInstance.LobSystem.Entities);
            foreach (Entity entity in entityCollection)
            {
                if (entity.Name == "Product")
                {
                    productid = entity.Id;
                }
                if (entity.Name == "Category")
                {
                    categoryId = entity.Id;
                }
            }
            Entity e = Entity.GetById(productid);
            Administration.Action pmaction = e.Actions.Create("View Profile", true, 1, false, "/sites/ProductCatalog/Product.aspx?sku={0}", "/_layouts/1033/images/viewprof.gif");
            pmaction.ActionParameters.Create("Sku", true, 0);
            e.DefaultAction = pmaction;

            e.Update();
            Console.WriteLine(" Product profile updated the entity successfully.");
            //update category profile page
            Entity cat = Entity.GetById(categoryId);
            Administration.Action catpmaction = cat.Actions.Create("View Profile", true, 1, false, "/sites/productcatalog/Category.aspx?CategoryId={0}", "/_layouts/1033/images/viewprof.gif");
            catpmaction.ActionParameters.Create("CategoryId", true, 0);
            cat.DefaultAction = catpmaction;
            cat.Update();
            Console.WriteLine(" Category profile updated the entity successfully.");
        }

        static void SetupBDC()
        {
            try
            {

                SqlSessionProvider.Instance().SetSharedResourceProviderToUse(SSPName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While Connecting SSP {0}", ex.ToString());
            }
        }
        static void ImportLobSystemFromXML(string XMLFileLocation)
        {
            string LOBSysName;
            try
            {
                if (File.Exists(XMLFileLocation))
                {
                    FileStream xmlStream = new FileStream(XMLFileLocation, FileMode.Open, FileAccess.Read);
                    ParseContext parseContext = new ParseContext();
                    LOBSysName = GetLOBSystemName(XMLFileLocation);
                    IAccessControlList acl = ApplicationRegistry.Instance.GetAccessControlList();
                    string currentIdentity = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    acl.Add(new IndividualAccessControlEntry(currentIdentity, BdcRights.Edit | BdcRights.Execute | BdcRights.SelectableInClients | BdcRights.SetPermissions | BdcRights.UseInBusinessDataInLists));
                    ApplicationRegistry.Instance.SetAccessControlList(acl);
                    ApplicationRegistry.Instance.CopyAclAcrossChildren();
                    DeleteApplicationFileFromSSP(LOBSysName);
                    ApplicationRegistry.Instance.ImportPackage(xmlStream, parseContext, PackageContents.Model | PackageContents.Properties);
                }
                else
                {
                    //throw new ArgumentException(string.Format("Specified path is invalid [{0}]", XMLFileLocation));
                    Console.WriteLine(string.Format("Specified path is invalid [{0}]", XMLFileLocation));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While Importing Application: {0}", ex.ToString());

            }
        }
        /// <summary>
        /// Delete the application file from SSP if exists
        /// </summary>
        /// <param name="LOBSystemName"></param>
        private static void DeleteApplicationFileFromSSP(string LOBSystemName)
        {
            try
            {
                if (Administration.ApplicationRegistry.Instance.LobSystems.Where(x => x.Name == LOBSystemName).LongCount() > 0)
                {


                    Administration.LobSystem lobsys = Administration.ApplicationRegistry.Instance.LobSystems.Where(x => x.Name == LOBSystemName).First();
                    //Console.WriteLine("Deleteing Existing Application with Name:" + LOBSystemName);
                    lobsys.Delete();
                    Console.WriteLine("Deleted Existing Application with Name:" + LOBSystemName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While deleting Application: {0}", ex.ToString());

            }
        }
        /// <summary>
        /// Returns the LOB system name from provided application definationFile
        /// </summary>
        /// <param name="FILE_NAME"></param>
        /// <returns></returns>
        private static string GetLOBSystemName(string FILE_NAME)
        {
            //findout the LOBSystem Name.
            string LOBSystemName;
            XmlTextReader reader = new XmlTextReader(FILE_NAME);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();

            //Select the connectionstrings node for ContosoPartnerfbaSQL

            XmlElement root = doc.DocumentElement;
            LOBSystemName = root.Attributes["Name"].Value.Trim();

            return LOBSystemName;


        }
        static void AddPeoplePickerWildcards(string FILE_NAME)
        {

            try
            {
                XmlTextReader reader = new XmlTextReader(FILE_NAME);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                reader.Close();

                XmlElement root = doc.DocumentElement;
                //Select the connectionstrings node for ContosoPartnerfbaSQL
                XmlNode nodeToFind;

                //Select 
                nodeToFind = root.SelectSingleNode("/configuration/SharePoint/PeoplePickerWildcards/add[@name='Partners']");

                if (nodeToFind == null)
                {
                    //Select the cd node with the matching title
                    XmlNode sysweb = root.SelectSingleNode("/configuration/SharePoint/PeoplePickerWildcards");

                    XmlNode Wildcards = doc.CreateNode(XmlNodeType.Element, "add", "");
                    XmlAttribute Wildcardsname = doc.CreateAttribute("name");
                    Wildcardsname.Value = "Partners";

                    XmlAttribute WildcardsValue = doc.CreateAttribute("value");
                    WildcardsValue.Value = "%";

                    Wildcards.Attributes.Append(Wildcardsname);
                    Wildcards.Attributes.Append(WildcardsValue);
                    Wildcards.InnerXml = "";
                    sysweb.AppendChild(Wildcards);

                    //save the output to a file 
                    doc.Save(FILE_NAME);
                    Console.WriteLine("PeoplePickerWildcards Updated!!");
                }
                else
                {
                    Console.WriteLine("PeoplePickerWildcards already exists!!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While Adding System.Web Nodes String: {0}", ex.ToString());

            }

        }
        static void AddConnectionString(string FILE_NAME, string sqlserver)
        {

            //Check Connectionstring already Exists for ContosoPartnerfbaSQL

            //Insert connection String into extended web.config 
            try
            {
                XmlTextReader reader = new XmlTextReader(FILE_NAME);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                reader.Close();

                //Select the connectionstrings node for ContosoPartnerfbaSQL
                XmlNode nodeToFind;
                XmlElement root = doc.DocumentElement;

                //Select 
                nodeToFind = root.SelectSingleNode("/configuration/connectionStrings/add[@name='ContosoPartnerfbaSQL']");

                if (nodeToFind == null)
                {

                    //check if connectionstrings node exists
                    nodeToFind = root.SelectSingleNode("/configuration/connectionStrings");
                    if (nodeToFind == null)
                    {

                        nodeToFind = doc.CreateNode(XmlNodeType.Element, "connectionStrings", "");
                        root.AppendChild(nodeToFind);

                    }


                    XmlNode ContosoPartnerfbaSQLconn = doc.CreateNode(XmlNodeType.Element, "add", "");
                    XmlAttribute nameatt = doc.CreateAttribute("name");
                    nameatt.Value = "ContosoPartnerfbaSQL";
                    ContosoPartnerfbaSQLconn.Attributes.Append(nameatt);
                    XmlAttribute connstratt = doc.CreateAttribute("connectionString");
                    connstratt.Value = "server=" + sqlserver + ";database=ContosoFBAdb;Trusted_Connection=true";
                    ContosoPartnerfbaSQLconn.Attributes.Append(connstratt);

                    nodeToFind.AppendChild(ContosoPartnerfbaSQLconn);
                    //save the output to a file 
                    doc.Save(FILE_NAME);

                    Console.WriteLine("ContosoPartnerfbaSQL ConnectionString Added !!!!");
                }
                else
                {
                    Console.WriteLine("ContosoPartnerfbaSQL ConnectionString Already Exists!!!!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While Adding Connection String: {0}", ex.ToString());

            }
        }
        static void AddMembership(string FILE_NAME)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(FILE_NAME);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                reader.Close();

                XmlElement root = doc.DocumentElement;

                //Check for exists

                //Select the connectionstrings node for ContosoPartnerfbaSQL
                XmlNode nodeToFind;



                //Select the cd node with the matching title
                XmlNode sysweb = root.SelectSingleNode("/configuration/system.web");

                XmlNode membershipnode = doc.CreateNode(XmlNodeType.Element, "membership", "");
                XmlAttribute defProv = doc.CreateAttribute("defaultProvider");
                defProv.Value = "Partners";
                membershipnode.Attributes.Append(defProv);

                XmlNode providernode = doc.CreateNode(XmlNodeType.Element, "providers", "");
                providernode.InnerXml = "";
                // membershipnode.AppendChild(providernode);


                XmlNode fbaMemNode = doc.CreateNode(XmlNodeType.Element, "add", "");
                XmlAttribute nameatt = doc.CreateAttribute("name");
                nameatt.Value = "Partners";
                fbaMemNode.Attributes.Append(nameatt);
                XmlAttribute connstratt = doc.CreateAttribute("connectionStringName");
                connstratt.Value = "ContosoPartnerfbaSQL";
                fbaMemNode.Attributes.Append(connstratt);
                XmlAttribute appnameatt = doc.CreateAttribute("applicationName");
                appnameatt.Value = "/";
                fbaMemNode.Attributes.Append(appnameatt);
                XmlAttribute typeatt = doc.CreateAttribute("type");
                typeatt.Value = "System.Web.Security.SqlMembershipProvider, System.Web, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
                fbaMemNode.Attributes.Append(typeatt);

                //Select 
                nodeToFind = root.SelectSingleNode("/configuration/system.web/membership/providers");

                if (nodeToFind == null)
                {

                    providernode.AppendChild(fbaMemNode);
                    membershipnode.AppendChild(providernode);
                    sysweb.AppendChild(membershipnode);
                    //save the output to a file 
                    doc.Save(FILE_NAME);
                    Console.WriteLine("Partners membership provider Added!!");
                }
                else
                {
                    XmlNode childnodeToFind = root.SelectSingleNode("/configuration/system.web/membership/providers/add[@name='Partners']");
                    if (childnodeToFind == null)
                    {
                        nodeToFind.AppendChild(fbaMemNode);
                        doc.Save(FILE_NAME);
                        Console.WriteLine("Partners Membership provider Updated!!");
                    }
                    else
                    {

                        Console.WriteLine("Partners Membership provider Already Exists!!");
                    }



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While Adding Membership String: {0}", ex.ToString());

            }


        }
        static void AddRoleManager(string FILE_NAME, string RoleProvider)
        {

            try
            {
                XmlTextReader reader = new XmlTextReader(FILE_NAME);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                reader.Close();

                XmlElement root = doc.DocumentElement;

                XmlNode sysweb = root.SelectSingleNode("/configuration/system.web");
                //Create RoleManager Node
                XmlNode roleManagerNode = doc.CreateNode(XmlNodeType.Element, "roleManager", "");
                XmlAttribute roleManEnable = doc.CreateAttribute("enabled");
                roleManEnable.Value = "true";

                XmlAttribute roleManProv = doc.CreateAttribute("defaultProvider");
                roleManProv.Value = RoleProvider;//"PartnerGroups";

                roleManagerNode.Attributes.Append(roleManEnable);
                roleManagerNode.Attributes.Append(roleManProv);

                //Create Providers Node
                XmlNode providerNode = doc.CreateNode(XmlNodeType.Element, "providers", "");
                providerNode.InnerXml = "";
                //Create ContosoPartnerfbaSQL Node
                XmlNode ContosoPartnerfbaSQLNode = doc.CreateNode(XmlNodeType.Element, "add", "");
                XmlAttribute connstratt = doc.CreateAttribute("connectionStringName");
                connstratt.Value = "ContosoPartnerfbaSQL";
                XmlAttribute appnameatt = doc.CreateAttribute("applicationName");
                appnameatt.Value = "/";
                XmlAttribute nameatt = doc.CreateAttribute("name");
                nameatt.Value = "PartnerGroups";
                XmlAttribute typeatt = doc.CreateAttribute("type");
                typeatt.Value = "System.Web.Security.SqlRoleProvider, System.Web, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
                ContosoPartnerfbaSQLNode.Attributes.Append(connstratt);
                ContosoPartnerfbaSQLNode.Attributes.Append(appnameatt);
                ContosoPartnerfbaSQLNode.Attributes.Append(nameatt);
                ContosoPartnerfbaSQLNode.Attributes.Append(typeatt);

                //Select 
                XmlNode nodeToFind = root.SelectSingleNode("/configuration/system.web/roleManager/providers");

                if (nodeToFind == null)
                {

                    providerNode.AppendChild(ContosoPartnerfbaSQLNode);
                    roleManagerNode.AppendChild(providerNode);
                    sysweb.AppendChild(roleManagerNode);
                    //save the output to a file 
                    doc.Save(FILE_NAME);
                    Console.WriteLine("Partners membership provider Added!!");
                }
                else
                {
                    XmlNode fbanodeToFind = root.SelectSingleNode("/configuration/system.web/roleManager/providers/add[@connectionStringName='ContosoPartnerfbaSQL']");
                    if (fbanodeToFind == null)
                    {
                        nodeToFind.AppendChild(ContosoPartnerfbaSQLNode);
                        doc.Save(FILE_NAME);
                        Console.WriteLine("Partners Membership provider Updated!!");
                    }
                    else
                    {

                        Console.WriteLine("ContosoPartnerfbaSQL rolemanager provider Already Exists!!");
                    }



                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While Adding System.Web Nodes String: {0}", ex.ToString());

            }

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
                int startPoint = content.IndexOf("https://");
                int endPoint = content.IndexOf(":8686/Contoso.LOB.Services", startPoint);
                string hostName = content.Substring(startPoint + 8, endPoint - startPoint - 8);

                content = System.Text.RegularExpressions.Regex.Replace(content, hostName, ComputerName);

                StreamWriter writer = new StreamWriter(FILE_NAME);
                writer.Write(content);
                writer.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception While Replacing ServiceHostComputerName in {0}. Error :{1}", FILE_NAME ,ex.Message.ToString());

            }
        }

        # endregion
    }
}
