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

namespace PartnerPortalBVT.Settings
{
     public static class CustConfig
    {
         //Server Name
         //Host should be publishing Site only
         private static string _hostURL = "http://" + System.Environment.MachineName.ToString() + ":9001";
         private static string _fbaURL = "http://" + System.Environment.MachineName.ToString() + ":9002";
                  
         private static string _servicesPort = "8585";
         private static string _spServicesPort = "8787";
         private static string _LOBservicesVdirName = "Contoso.LOB.Services";
         private static string _PSSservicesVdirName = "Contoso.PartnerPortal.Services";
         private static string _SPGservicesVdirName = "Microsoft.Practices.SPG.SiteCreation.Service";
         private static string _LobWebVdirName = "Contoso.LOB.Web";
         private static string _partner1fbaUserName = "ContosoPartner1User1";
         private static string _partner2fbaUserName = "ContosoPartner2User1";
         private static string _fbaPassWord = "P2ssw0rd$";
         private static string _partner1WinUserName = "ContosoPartner1User6";
         private static string _partner2WinUserName = "ContosoPartner2User6";
         private static string _winPassWord = "P2ssw0rd$";

         /// <summary>
         /// return fba username
         /// </summary>
         public static string Partner1FBAUserName
         {
             get { return _partner1fbaUserName; }
         }

         /// <summary>
         /// return fba username
         /// </summary>
         public static string Partner2FBAUserName
         {
             get { return _partner2fbaUserName; }
         }
         /// <summary>
         /// return fba username
         /// </summary>
         public static string FBAUserPassword
         {
             get { return _fbaPassWord; }
         }


         /// <summary>
         /// return windows username
         /// </summary>
         public static string Partner1WinUserName
         {
             get { return System.Environment.MachineName + "\\"+ _partner1WinUserName; }
         }

         /// <summary>
         /// return windows username
         /// </summary>
         public static string Partner2WinUserName
         {
             get { return System.Environment.MachineName + "\\" + _partner2WinUserName; }
         }
         /// <summary>
         /// return windows user password
         /// </summary>
         public static string WINUserPassword
         {
             get { return _winPassWord; }
         }

         public static string GetHostURL 
         
         {
             get
             {
                 string hostURL;
                 //validate Hostname last char as / before add port
                 if (_hostURL.Substring(_hostURL.Length - 1, 1).Contains("/"))
                 {
                     hostURL = _hostURL.Substring(0, _hostURL.Length - 1);
                 }
                 else
                 {
                     hostURL = _hostURL;
                 }
                 
                 return hostURL;
             }
         }
         public static string GetFBAURL
         {
             get
             {
                 string fbaURL;
                 //validate Hostname last char as / before add port
                 if (_fbaURL.Substring(_fbaURL.Length - 1, 1).Contains("/"))
                 {
                     fbaURL = _fbaURL.Substring(0, _fbaURL.Length - 1);
                 }
                 else
                 {
                     fbaURL = _fbaURL;
                 }

                 return fbaURL;
             }
         }
         /// <summary>
         /// Returns LobService URL
         /// </summary>
         public static string GetLOBServicesURL
         {
             get
             {
                 string servURL;

                 servURL = GetRootURL();
                 servURL = servURL +":" + _servicesPort + "/"+ _LOBservicesVdirName;

                 return servURL;
             }

         }
         /// <summary>
         /// Returns LOBWeb  URL
         /// </summary>
         public static string GetLOBWebURL
         {
             get
             {
                 string servURL;

                 servURL = GetRootURL();
                 servURL = servURL + ":" + _servicesPort + "/" + _LobWebVdirName;

                 return servURL;
             }

         }
         /// <summary>
         /// Returns  PSS Service URL
         /// </summary>
         public static string GetPSSServicesURL
         {
             get
             {
                 string servURL = GetRootURL();
                   
                 servURL = servURL + ":" + _spServicesPort + "/" + _PSSservicesVdirName;

                 return servURL;
             }

         }
         /// <summary>
         /// Returns  PSS Service URL
         /// </summary>
         public static string GetSPGServicesURL
         {
             get
             {
                 string servURL = GetRootURL();

                 servURL = servURL + ":" + _servicesPort + "/" + _SPGservicesVdirName;

                 return servURL;
             }

         }
         private static string GetRootURL()
         {
             string servURL;
             //validate Hostname last char as / before add port
             if (_hostURL.Substring(_hostURL.Length - 1, 1).Contains("/"))
             {
                 servURL = _hostURL.Substring(0, _hostURL.Length - 1);
             }
             else
             {
                 servURL = _hostURL;
             }
             //check for host has any port
             if (servURL.IndexOf(":", 6) > 0)
             {
                 //Remove Port part 
                 servURL = servURL.Substring(0, servURL.IndexOf(":", 5));
             }
             return servURL;
         }
         
         

    
    }
}
