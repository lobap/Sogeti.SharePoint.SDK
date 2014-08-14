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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DataModels.SharePointList.PartsMgmnt.ViewModels;
using DataModels.SharePointList.Model;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    public partial class CategoryResults : UserControl
    {
        public event EventHandler<GenericEventArgs<IEnumerable<Machine>>> OnRelatedMachinesFound;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CategoryResultsGridView.RowDataBound += CategoryResultsGridView_RowDataBound;
            CategoryResultsGridView.SelectedIndexChanged += CategoryResultsGridView_SelectedIndexChanged;
        }

        public CategoryResults()
        {
            OnRelatedMachinesFound += delegate { };
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public IEnumerable<Category> Data
        {
            set
            {
                var categoryResultsViewModels = value.Select(category => new CategoryResultsViewModel
                {
                    CategoryId = category.Id.HasValue ? category.Id.Value : 0,
                    CategoryName = category.Title,
                }).ToList();

                CategoryResultsGridView.DataSource = categoryResultsViewModels;
                CategoryResultsGridView.DataBind(); 
            }
        }

        public void ClearControls()
        {
            CategoryResultsGridView.DataSource = null;
            CategoryResultsGridView.DataBind();
            Update();
        }

        public void Update()
        {
            CategoryResultUpdatePanel.Update();
        }

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

        protected void CategoryResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedCategoryId = int.Parse(CategoryResultsGridView.DataKeys[CategoryResultsGridView.SelectedRow.RowIndex].Value.ToString());
            using (var partManagementRepository = new PartManagementRepository())
            {
                RelatedMachinesFound(new GenericEventArgs<IEnumerable<Machine>>
                                         {
                                             PayLoad = partManagementRepository.GetMachinesByCategory(selectedCategoryId)
                                         });
            }
        }

        protected virtual void RelatedMachinesFound(GenericEventArgs<IEnumerable<Machine>> e)
        {
            OnRelatedMachinesFound(this, e);
        }

    }
}
