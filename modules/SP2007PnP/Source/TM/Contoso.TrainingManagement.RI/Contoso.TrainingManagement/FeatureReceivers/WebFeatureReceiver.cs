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
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Contoso.TrainingManagement.Common.Constants;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Navigation;

namespace Contoso.TrainingManagement
{
    /// <summary>
    /// The WebFeatureReceiver class implements methods to perform during
    /// the life of Contoso Training Management's web scoped feature.
    /// </summary>
    [Guid("8007101d-34cd-45dc-83ec-d0b6dc35edf9")]
    public class WebFeatureReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// The FeatureActivated method executes when the Contoso Training Management's
        /// web scoped feature is activated. It will provision security groups and permission levels,
        /// Update the quick launch and top link bar navigations.
        /// </summary>
        /// <param name="properties"></param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            ProvisionSecurity(web);

            UpdateQuickLaunch(web);

            UpdateTopLinkBar(web);
        }

        #region Unused

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }

        #endregion

        #region Private Helpers

        private static void UpdateTopLinkBar(SPWeb web)
        {
            if (!web.Navigation.UseShared)
            {
                bool nodeExists = false;
                foreach (SPNavigationNode node in web.Navigation.TopNavigationBar)
                {
                    if (node.Title == web.Title && node.Url == web.ServerRelativeUrl)
                    {
                        nodeExists = true;
                        break;
                    }
                }

                if (!nodeExists)
                {
                    SPNavigationNode newNode = new SPNavigationNode(web.Title, web.ServerRelativeUrl);
                    web.Navigation.TopNavigationBar.AddAsFirst(newNode);
                }
            }
        }

        private static void ProvisionSecurity(SPWeb web)
        {
            //The following is required so that custom Role Definitions can be applied to our site
            if (!web.IsRootWeb)
            {
                web.RoleDefinitions.BreakInheritance(true, false);
                web.Update();
            }
            //Create/Retrieve the Constoso Employees SharePoint Group
            SPGroup employeeGroup = EnsuredSPGroup(web, "Contoso Employees");
            web.Update();

            //Create/Retrieve the Contoso Managers SharePoint Group
            SPGroup managerGroup = EnsuredSPGroup(web, "Contoso Managers");
            web.Update();

            //Add the Contoso Employees and Contoso Managers Groups as Read on the new site
            SPRoleDefinition viewers = web.RoleDefinitions["Read"];
            SPRoleAssignment roleAssignment = new SPRoleAssignment(employeeGroup);
            roleAssignment.RoleDefinitionBindings.Add(viewers);
            web.RoleAssignments.Add(roleAssignment);

            roleAssignment = new SPRoleAssignment(managerGroup);
            roleAssignment.RoleDefinitionBindings.Add(viewers);
            web.RoleAssignments.Add(roleAssignment);

            //Create the Training - View Only permission level
            SPRoleDefinition viewOnly = EnsureRoleDefinition(web,
                                                             "Training - View Only",
                                                             "ReadOnly Permission for Training Management Site",
                                                             SPBasePermissions.ViewFormPages | 
                                                             SPBasePermissions.ViewListItems | 
                                                             SPBasePermissions.Open | 
                                                             SPBasePermissions.ViewPages);

            //Create the Training - Participant permission level
            SPRoleDefinition participant = EnsureRoleDefinition(web,
                                                                "Training - Participant",
                                                                "Read and Add Item Permissions for Training Management Site",
                                                                SPBasePermissions.ViewFormPages | 
                                                                SPBasePermissions.ViewListItems | 
                                                                SPBasePermissions.Open | 
                                                                SPBasePermissions.ViewPages | 
                                                                SPBasePermissions.AddListItems);

            //Create the Training - Manager permission level
            SPRoleDefinition manager = EnsureRoleDefinition(web,
                                                            "Training - Manager",
                                                            "Read, Add Item and Edit Item Permissions for Training Management Site",
                                                            SPBasePermissions.ViewListItems | 
                                                            SPBasePermissions.ViewFormPages | 
                                                            SPBasePermissions.Open | 
                                                            SPBasePermissions.ViewPages | 
                                                            SPBasePermissions.AddListItems | 
                                                            SPBasePermissions.EditListItems);

            //Assign the Contoso Employee group the Training - View Only permission level on the Training Courses List
            EnsureRoleAssignment(web, Lists.TrainingCourses, employeeGroup, viewOnly);

            //Assign the Contoso Manager group the Training - View Only permission level on the Training Courses List
            EnsureRoleAssignment(web, Lists.TrainingCourses, managerGroup, viewOnly);

            //Assign the Contoso Employee group the Training - Participant permission level on the Registrations List
            EnsureRoleAssignment(web, Lists.Registrations, employeeGroup, participant);

            //Assign the Contoso Manager group the Training - Manager permission level on the Registrations List
            EnsureRoleAssignment(web, Lists.Registrations, managerGroup, manager);

            //Assign the Contoso Manager group the Training - Manager permission level on the Registratin Approval Tasks List
            EnsureRoleAssignment(web, Lists.RegistrationApprovalTasks, managerGroup, viewOnly);
        }

        private static SPGroup EnsuredSPGroup(SPWeb web, string groupName)
        {
            SPGroup group = null;

            string[] groupQuery = { groupName };
            SPGroupCollection groupsFound = web.SiteGroups.GetCollection(groupQuery);
            if ((groupsFound != null) && (groupsFound.Count == 1) && (groupsFound[0].Name == groupName))
            {
                group = groupsFound[0];
            }
            else
            {
                web.SiteGroups.Add(groupName, web.Site.Owner, null, groupName);
                group = web.SiteGroups[groupName];
                web.Update();
            }

            return group;
        }

        private static SPRoleDefinition EnsureRoleDefinition(SPWeb web, string name, string description, SPBasePermissions permissions)
        {
            for (int i = 0; i < web.RoleDefinitions.Count; i++)
            {
                if (web.RoleDefinitions[i].Name == name)
                {
                    web.RoleDefinitions.Delete(i);
                }
            }

            SPRoleDefinition roleDefinition = new SPRoleDefinition();
            roleDefinition.Name = name;
            roleDefinition.Description = description;
            roleDefinition.BasePermissions = permissions;

            web.RoleDefinitions.Add(roleDefinition);
            web.Update();

            roleDefinition = web.RoleDefinitions[name];

            return roleDefinition;
        }

        private static void EnsureRoleAssignment(SPWeb web, string listName, SPGroup spGroup, SPRoleDefinition role)
        {
            SPList list = web.Lists[listName];
            list.BreakRoleInheritance(false);
            list.ParentWeb.Dispose();

            for (int i = 0; i < list.RoleAssignments.Count; i++)
            {
                if (list.RoleAssignments[i].Member.Name == spGroup.Name)
                {
                    list.RoleAssignments.Remove(i);
                }
            }

            SPRoleAssignment roleAssignment = new SPRoleAssignment(spGroup);
            roleAssignment.RoleDefinitionBindings.Add(role);
            list.RoleAssignments.Add(roleAssignment);
            list.Update();
        }

        private static void UpdateQuickLaunch(SPWeb web)
        {
            SPNavigationNode groupNode = null;

            foreach (SPNavigationNode navigationNode in web.Navigation.QuickLaunch)
            {
                if (navigationNode.Title == "Dashboards")
                {
                    groupNode = navigationNode;
                }
            }

            if (groupNode == null)
            {
                groupNode = new SPNavigationNode("Dashboards", String.Empty);
                web.Navigation.QuickLaunch.AddAsFirst(groupNode);
                web.Update();
            }

            SPNavigationNode dashboardNode = null;
            SPNavigationNode trainingNode = null;
            foreach (SPNavigationNode navigationNode in groupNode.Children)
            {
                if (navigationNode.Title == "Manager Dashboard")
                {
                    dashboardNode = navigationNode;
                }
                else if (navigationNode.Title == "My Training")
                {
                    trainingNode = navigationNode;
                }
            }

            if (trainingNode == null)
            {
                trainingNode = new SPNavigationNode("My Training", "trainingdashboard.aspx");
                groupNode.Children.AddAsFirst(trainingNode);
            }

            if (dashboardNode == null)
            {
                dashboardNode = new SPNavigationNode("Manager Dashboard", "managerdashboard.aspx");
                groupNode.Children.AddAsFirst(dashboardNode);
                web.Update();
            }
        }

        #endregion
    }
}
