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
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DataModels.SharePointList.Model;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace DataModels.SharePointList.Sandbox.ManageCategories
{
    using System.Security.Permissions;
    using DTOs;
    using Microsoft.SharePoint.Security;

    [ToolboxItem(false)]
    public partial class ManageCategories : System.Web.UI.WebControls.WebParts.WebPart
    {
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            GoButton.Click += GoButton_Click;
            MachineResultsGridView.RowDataBound += MachineResultsGridView_RowDataBound;
            CategoryResultsGridView.RowDataBound += CategoryResultsGridView_RowDataBound;
            CategoryResultsGridView.SelectedIndexChanged += CategoryResultsGridView_SelectedIndexChanged;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Categories, "/NewForm.aspx?IsDlg=1')");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void GoButton_Click(object sender, EventArgs eventArgs)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<Category> categories = partManagementRepository.GetCategoriesByPartialName(CategorySearchTextBox.Text);
                var categoryDtos = categories.Select(category => new CategoryDTO
                {
                    CategoryId =
                        category.Id.HasValue
                            ? category.Id.Value
                            : 0,
                    CategoryName = category.Title,
                });

                ShowCategoryResults(categoryDtos);
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void MachineResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink machineNameLink = (HyperLink)e.Row.FindControl("MachineNameHyperLink");
                string machineId = MachineResultsGridView.DataKeys[e.Row.RowIndex].Value.ToString();
                machineNameLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                       "/", SharePointList.Model.Constants.ListUrls.Machines, "/EditForm.aspx?ID=" + machineId + "&IsDlg=1');");
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void CategoryResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink CategoryNameLink = (HyperLink)e.Row.FindControl("CategoryNameHyperLink");
                string categoryId = CategoryResultsGridView.DataKeys[e.Row.RowIndex].Value.ToString();
                CategoryNameLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Categories, "/EditForm.aspx?ID=" + categoryId + "&IsDlg=1');");
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void CategoryResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedCategoryId = int.Parse(CategoryResultsGridView.DataKeys[CategoryResultsGridView.SelectedRow.RowIndex].Value.ToString());
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<MachineDTO> machines = partManagementRepository.GetMachinesByCategory(selectedCategoryId);
                ShowMachines(machines);
            }

        }

        public void ShowCategoryResults(IEnumerable<CategoryDTO> categoryDtos)
        {
            CategoryResultsGridView.EmptyDataText = Constants.EmptyData.CategoryResults;
            CategoryResultsGridView.DataSource = categoryDtos;
            CategoryResultsGridView.DataBind();
            CategoryResultsGridView.EmptyDataText = string.Empty;
        }

        public void ShowMachines(IEnumerable<MachineDTO> machineDtos)
        {
            MachineResultsGridView.DataSource = machineDtos;
            MachineResultsGridView.EmptyDataText = Constants.EmptyData.MachineCategoryResults;
            MachineResultsGridView.DataBind();
            MachineResultsGridView.EmptyDataText = string.Empty;
        }
    }
}
