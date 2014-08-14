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
using DataModels.SharePointList.PartsMgmnt.ViewModels;
using DataModels.SharePointList.Model;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    public partial class DepartmentResults : UserControl
    {
        public event EventHandler<GenericEventArgs<IEnumerable<MachineDepartment>>> OnRelatedMachineDepartmentsFound;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            DepartmentResultsGridView.RowDataBound += DepartmentResultsGridView_RowDataBound;
            DepartmentResultsGridView.SelectedIndexChanged += DepartmentResultsGridView_SelectedIndexChanged;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public IEnumerable<Department> Data
        {
            set
            {
                var departmentResultsViewModels = value.Select(department => new DepartmentResultsViewModel
                {
                    DepartmentId = department.Id.HasValue ? department.Id.Value : 0,
                    DepartmentName = department.Title,
                }).ToList();

                DepartmentResultsGridView.DataSource = departmentResultsViewModels;
                DepartmentResultsGridView.DataBind(); ;
            }
        }

        public void ClearControls()
        {
            DepartmentResultsGridView.DataSource = null;
            DepartmentResultsGridView.DataBind();
            Update();
        }

        public void Update()
        {
            DepartmentResultUpdatePanel.Update();
        }

        protected void DepartmentResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink DepartmentNameLink = (HyperLink)e.Row.FindControl("DepartmentNameHyperLink");
                string DepartmentId = DepartmentResultsGridView.DataKeys[e.Row.RowIndex].Value.ToString();
                DepartmentNameLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Departments, "/EditForm.aspx?ID=" + DepartmentId + "&IsDlg=1');");
            }
        }

        protected void DepartmentResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedDepartmentId = int.Parse(DepartmentResultsGridView.DataKeys[DepartmentResultsGridView.SelectedRow.RowIndex].Value.ToString());
            using (var partManagementRepository = new PartManagementRepository())
            {
                RelatedMachineDepartmentsFound(new GenericEventArgs<IEnumerable<MachineDepartment>>
                                                   {
                                                       PayLoad =
                                                           partManagementRepository.GetMachineDepartmentsByDepartment(
                                                               selectedDepartmentId)
                                                   });
            }
        }

        protected virtual void RelatedMachineDepartmentsFound(GenericEventArgs<IEnumerable<MachineDepartment>> e)
        {
            OnRelatedMachineDepartmentsFound(this, e);
        }
    }
}

