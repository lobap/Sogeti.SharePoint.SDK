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

namespace Microsoft.Practices.SPG.Tests.Config
{
    public static class CustConfig
    {
        //Server Name
        //Host should be publishing Site only
        private static string _hostURL = "http://" + System.Environment.MachineName.ToString() + ":9001";
        
        
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
