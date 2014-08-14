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
using Microsoft.Office.Server;
using Microsoft.SharePoint;
using Microsoft.Office.Server.UserProfiles;

namespace ContosoSetup
{
    static class PCSitestestdata
    {
        public static void AddPartnersURLs(string siteURL, string ContosoWebAppURL, string ContosoExtWebAppURL)
        {

            using (SPSite oSiteCollection = new SPSite(siteURL))
            {
                using (SPWeb oWebsiteRoot = oSiteCollection.OpenWeb("PartnerDirectory"))
                {
                    Guid partnerFieldId = new Guid("5AA31951-0DBE-4097-AF4F-60EB8D6B3E8F");
                    //Create a column "Partner" in "Sites" List
                    SPList oList = oWebsiteRoot.Lists["Sites"];
                    oList.EnableModeration = false; //so there is no approval required. 
                    oList.Update();

                    SPListItem partner1 = oList.Items.Add();
                    partner1["Title"] = "Partner1";
                    partner1[partnerFieldId] = "ContosoPartner1";
                    partner1["URL"] = "/sites/partner1";
                    partner1["Approval Status"] = "0";//set to "Approved"
                    partner1.Update();
                    Console.WriteLine("Partner1 URL : " + ContosoExtWebAppURL + "/sites/partner1" + " added.");

                    SPListItem partner2 = oList.Items.Add();
                    partner2["Title"] = "Partner2";
                    partner2[partnerFieldId] = "ContosoPartner2";
                    partner2["URL"] = "/sites/partner2";
                    partner2.Update();
                    Console.WriteLine("Partner2 URL : " + ContosoExtWebAppURL + "/sites/partner2" + " added.");

                    SPListItem winusers = oList.Items.Add();
                    winusers["Title"] = "winusers";
                    winusers[partnerFieldId] = @"Builtin\Users";
                    //winusers["URL"] = ContosoWebAppURL + "/sites/partner1";
                    winusers["URL"] = "/sites/partner1";
                    winusers.Update();
                    Console.WriteLine("winusers URL : " + ContosoWebAppURL + "/sites/partner1" + " added.");
                }

                using (SPWeb oWebsiteRoot = oSiteCollection.OpenWeb("SPGsubsite"))
                {

                    //Business Event Site Template Configuration
                    SPList oBESTC = oWebsiteRoot.Lists["Business Event Type Configuration"];
                    SPListItem estc1 = oBESTC.Items.Add();
                    estc1["Title"] = "Incident";
                    estc1["BusinessEvent"] = "Incident";
                    estc1["SiteTemplate"] = "SPGSubsiteTemplate";
                    estc1["BusinessEventIdKey"] = "IncidentId";
                    estc1["TopLevelSiteRelativeUrl"] = "Incidents";
                    estc1.Update();
                    Console.WriteLine("BusinessEventSiteTemplateConfig List Added Incident Item. ");
                    SPListItem estc2 = oBESTC.Items.Add();
                    estc2["Title"] = "Order Exception";
                    estc2["BusinessEvent"] = "OrderException";
                    estc2["SiteTemplate"] = "ORDEREXCEPTION#0";
                    estc2["BusinessEventIdKey"] = "OrderExceptionId";
                    estc2["TopLevelSiteRelativeUrl"] = "OrderExceptions";
                    estc2.Update();
                    Console.WriteLine("BusinessEventSiteTemplateConfig List Added Order Exception Item. ");



                }
            }

        }

        public static void AddUserProfiles(string siteURL)
        {
            using (SPSite spSite = new SPSite(siteURL))
            {
                
                ServerContext serverContext = ServerContext.GetContext(spSite);
                UserProfileManager userProfileManager = new UserProfileManager(serverContext);

                //Add PartnerId profile property
                try
                {
                    //userProfileManager.Properties.GetPropertyByName()

                    Property property = userProfileManager.Properties.Create(false);
                    property.Name = "PartnerId";
                    property.DisplayName = "Partner Id";
                    property.Type = PropertyDataType.String;
                    property.Length = 50;

                    userProfileManager.Properties.Add(property);
                    
                }
                catch(DuplicateEntryException ex)
                {
                    //swallow exception if PartnerId profile property already created.
                }

                //Create profile for logged in user
                CreateUserProfileAndPartnerId(userProfileManager, System.Environment.UserName, @"Builtin\Users");
                
                //Create profiles for partner users with windows accounts
                CreateUserProfileAndPartnerId(userProfileManager, "ContosoWinAdmin", "ContosoPartner1");
                CreateUserProfileAndPartnerId(userProfileManager, "ContosoPartner1User6", "ContosoPartner1");
                CreateUserProfileAndPartnerId(userProfileManager, "ContosoPartner1User7", "ContosoPartner1");
                CreateUserProfileAndPartnerId(userProfileManager, "ContosoPartner1User8", "ContosoPartner1");
                CreateUserProfileAndPartnerId(userProfileManager, "ContosoPartner1User9", "ContosoPartner1");
                CreateUserProfileAndPartnerId(userProfileManager, "ContosoPartner2User6", "ContosoPartner2");
                CreateUserProfileAndPartnerId(userProfileManager, "ContosoPartner2User7", "ContosoPartner2");
                CreateUserProfileAndPartnerId(userProfileManager, "ContosoPartner2User8", "ContosoPartner2");
                CreateUserProfileAndPartnerId(userProfileManager, "ContosoPartner2User9", "ContosoPartner2");

                //Create profiles for FBA partner users
                CreateUserProfileAndPartnerId(userProfileManager, "Partners:ContosoFbaAdmin", "ContosoPartner1");
                CreateUserProfileAndPartnerId(userProfileManager, "Partners:ContosoPartner1User1", "ContosoPartner1");
                CreateUserProfileAndPartnerId(userProfileManager, "Partners:ContosoPartner1User2", "ContosoPartner1");
                CreateUserProfileAndPartnerId(userProfileManager, "Partners:ContosoPartner1User3", "ContosoPartner1");
                CreateUserProfileAndPartnerId(userProfileManager, "Partners:ContosoPartner1User4", "ContosoPartner1");
                CreateUserProfileAndPartnerId(userProfileManager, "Partners:ContosoPartner1User5", "ContosoPartner1");
                CreateUserProfileAndPartnerId(userProfileManager, "Partners:ContosoPartner2User1", "ContosoPartner2");
                CreateUserProfileAndPartnerId(userProfileManager, "Partners:ContosoPartner2User2", "ContosoPartner2");
                CreateUserProfileAndPartnerId(userProfileManager, "Partners:ContosoPartner2User3", "ContosoPartner2");
                CreateUserProfileAndPartnerId(userProfileManager, "Partners:ContosoPartner2User4", "ContosoPartner2");
                CreateUserProfileAndPartnerId(userProfileManager, "Partners:ContosoPartner2User5", "ContosoPartner2");
            }
            
        }

        private static void CreateUserProfileAndPartnerId(UserProfileManager userProfileManager, string accountName, string partnerId)
        {
            UserProfile userProfile = userProfileManager.CreateUserProfile(accountName);
            userProfile["PartnerId"].Value = partnerId;
            userProfile.Commit();
            Console.WriteLine("User profile created for: " + accountName + " - " + partnerId);
        }
    }
}