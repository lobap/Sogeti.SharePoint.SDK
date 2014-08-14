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
using DataModels.SharePointList.PartsMgmnt.Presenters;
using DataModels.SharePointList.PartsMgmnt.ViewModels;
using DataModels.SharePointList.Model;

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    public enum AreaDisplayTypes
    {
        Category,
        Manufacturer,
        Department
    }

    public partial class FilterByArea : UserControl
    {
        public event EventHandler<GenericEventArgs<IEnumerable<Machine>>> OnRelatedMachinesFound;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AreaResultsGridView.RowCommand += areaResultsGridView_RowCommand;
            AreaRadioButton.SelectedIndexChanged += AreaRadioButton_SelectedIndexChanged;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public FilterByArea()
        {
            OnRelatedMachinesFound += delegate { };
        }

        private string MakePlural(AreaDisplayTypes areaDisplayType)
        {
            switch (areaDisplayType)
            {
                case AreaDisplayTypes.Category:
                    return "Categories";
                case AreaDisplayTypes.Manufacturer:
                    return "Manufacturers";
                case AreaDisplayTypes.Department:
                    return "Departments";
                default:
                    throw new ArgumentOutOfRangeException("areaDisplayType");
            }
        }


        private IEnumerable<AreaResultsViewModel> GetAreaViewModels(IEnumerable<Category> categories)
        {
            return categories.Select(category => new AreaResultsViewModel
            {
                Title = category.Title,
                Id = category.Id.HasValue ? category.Id.Value : 0
            });
        }

        protected void areaResultsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AreaDisplayTypes displayType = (AreaDisplayTypes)System.Enum.Parse(typeof(AreaDisplayTypes), AreaRadioButton.SelectedValue);
            int selectedAreaId = int.Parse(AreaResultsGridView.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString());

            if (OnRelatedMachinesFound != null)
            {
                RelatedMachinesFound(new GenericEventArgs<IEnumerable<Machine>>
                {
                    PayLoad = GetMachinesByArea(displayType, selectedAreaId)
                });   
            }
            
        }

        private IEnumerable<AreaResultsViewModel> GetAreaViewModels(IEnumerable<Department> departments)
        {
            return departments.Select(department => new AreaResultsViewModel
            {
                Title = department.Title,
                Id = department.Id.HasValue ? department.Id.Value : 0
            });
        }

        private IEnumerable<AreaResultsViewModel> GetAreaViewModels(IEnumerable<Manufacturer> manufacturers)
        {
            return manufacturers.Select(manufacturer => new AreaResultsViewModel
            {
                Title = manufacturer.Title,
                Id = manufacturer.Id.HasValue ? manufacturer.Id.Value : 0
            });
        }



        public IEnumerable<Machine> GetMachinesByArea(AreaDisplayTypes displayType, int selectedAreaId)
        {
            IEnumerable<Machine> machinesByArea = new List<Machine>();

            var presenter = new FindByAreaPresenter();
            machinesByArea = presenter.GetMachines(displayType, selectedAreaId);
            return machinesByArea;
        }

        protected virtual void RelatedMachinesFound(GenericEventArgs<IEnumerable<Machine>> e)
        {
            OnRelatedMachinesFound(this, e);
        }

        protected void AreaRadioButton_SelectedIndexChanged(object sender, EventArgs e)
        {

            AreaDisplayTypes displayType = (AreaDisplayTypes)System.Enum.Parse(typeof(AreaDisplayTypes), AreaRadioButton.SelectedValue);
            AreaResultsGridView.Columns[1].HeaderText = "All " + MakePlural(displayType);

            using (var partManagementRepository = new PartManagementRepository())
            {
                switch (displayType)
                {
                    case AreaDisplayTypes.Category:
                        IEnumerable<Category> categories = partManagementRepository.GetCategories();
                        AreaResultsGridView.DataSource = GetAreaViewModels(categories);
                        break;
                    case AreaDisplayTypes.Manufacturer:
                        IEnumerable<Manufacturer> manufacturers = partManagementRepository.GetManufacturers();
                        AreaResultsGridView.DataSource = GetAreaViewModels(manufacturers);
                        break;
                    case AreaDisplayTypes.Department:
                        IEnumerable<Department> departments = partManagementRepository.GetDepartments();
                        AreaResultsGridView.DataSource = GetAreaViewModels(departments);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Area Display Type Not Found");
                }
            }
            AreaResultsGridView.DataBind();

            //Reset the MAchine Results Grid by clearing out the results and firing the event
            RelatedMachinesFound(new GenericEventArgs<IEnumerable<Machine>>
                                     {
                                         PayLoad = new List<Machine>()
                                     });
        }
    }
}
