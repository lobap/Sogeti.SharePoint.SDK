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
using System.IO;
using System.Linq;
using System.Text;
using DataModels.SharePointList.Model;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.Sandbox
{
    using System.Security.Permissions;
    using DTOs;
    using Microsoft.SharePoint.Security;

    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class PartManagementRepository : IPartManagementRepository
    {
        private PartsSiteDataContext dataContext { get; set; }


        public PartManagementRepository()
        {
            dataContext = new PartsSiteDataContext(SPContext.Current.Web.Url);
        }

        public IEnumerable<Part> GetPartsByPartialSku(string sku)
        {
            return from part in dataContext.Parts
                   where part.SKU==sku
                   select part;
        }

        public IEnumerable<InventoryLocation> GetInventoryLocations(Part part)
        {
            return part.InventoryLocation;
        }

        /// <summary>
        /// Helper class for use in merging results.
        /// </summary>
        private class PartResult
        {
            public int? PartId;
            public string Title;
            public string SKU;
        }

        /// <summary>
        /// Helper class for use in merging results.
        /// </summary>
        private class InvResult
        {
            public int? PartId;
            public int? LocationId;
            public string BinNumber;
            public double? Quantity;
        }

        public IEnumerable<PartInventoryDTO> GetPartsInventoryView(string sku)
        {
            //Get all matching parts that have inventory.
            var invResults = (from location in dataContext.InventoryLocations
                                    where location.Part.SKU==sku
                                    select new InvResult
                                    {
                                        PartId = location.Part.Id,
                                        LocationId = location.Id,
                                        BinNumber = location.BinNumber,
                                        Quantity = location.Quantity
                                    });
            var inventoryResults = invResults.ToArray();

            //get all matching parts.
            var partResults = (from part in dataContext.Parts
                               where part.SKU==sku
                               select new PartResult { PartId = part.Id, Title = part.Title, SKU = part.SKU }).ToArray();

            //Merge inventor and parts together into PartInventoryDTO instances.
            return MergePartInventory(partResults, inventoryResults);
        }

        IEnumerable<PartInventoryDTO> MergePartInventory(IEnumerable<PartResult> partResults, IEnumerable<InvResult> inventoryResults)
        {
            // do a left outer join between the two result sets 
            // This associates the parts with inventory info and includes the parts that have no inventory info (inv == null).
            var results = from part in partResults
                          join inv in inventoryResults on part.PartId equals inv.PartId into gj
                          from subInv in gj.DefaultIfEmpty()
                          select new PartInventoryDTO
                                         {
                                             PartId = part.PartId.HasValue ? part.PartId.Value : 0,
                                             PartSku = part.SKU,
                                             PartName = part.Title,
                                             InventoryLocationId = (subInv != null && subInv.LocationId.HasValue ? subInv.LocationId.Value : 0),
                                             InventoryQuantity = (subInv != null && subInv.Quantity.HasValue ? subInv.Quantity.Value : 0),
                                             LocationBin = (subInv != null ? subInv.BinNumber : "")
                                         };
            return results.ToArray();
        }

        public IEnumerable<PartInventoryDTO> GetPartsByMachineId(int machineId)
        {
            //get all matching parts.
            var partResults = (from machinePart in dataContext.MachineParts
                               where machinePart.Machine.Id == machineId
                               select new PartResult
                               {
                                   PartId = machinePart.Part.Id,
                                   Title =
                                       machinePart.Part.Title,
                                   SKU = machinePart.Part.SKU
                               });

            IEnumerable<int?> partIds = (from part in partResults where part.PartId != null select part.PartId);

            //Get all matching parts that have inventory.
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            dataContext.Log = writer;

            List<InvResult> inventoryResults = GetInventoryListForParts(partIds);
            return MergePartInventory(partResults, inventoryResults);

        }

        List<InvResult> GetInventoryListForParts(IEnumerable<int?> partIds)
        {
            List<InvResult> inventoryResults = new List<InvResult>();
            foreach (int? partId in partIds)
            {
                if (partId != null)
                {
                    var locations = from location in dataContext.InventoryLocations
                                    where location.Part.Id == partId
                                    select new InvResult
                                    {
                                        PartId = location.Part.Id,
                                        LocationId = location.Id,
                                        BinNumber = location.BinNumber,
                                        Quantity = location.Quantity
                                    };
                    foreach (var loc in locations)
                    {
                        inventoryResults.Add(loc);
                    }
                }
            }
            return inventoryResults;
        }

        public IEnumerable<PartInventoryDTO> GetPartsBySupplierId(int supplierId)
        {
            var partResults = (from partSupplier in dataContext.PartSuppliers
                               where partSupplier.Supplier.Id == supplierId
                               select new PartResult { PartId = partSupplier.Part.Id, Title = partSupplier.Part.Title, SKU = partSupplier.Part.SKU });

            IEnumerable<int?> partIds = from part in partResults where part.PartId != null select part.PartId;

            var inventoryResults = GetInventoryListForParts(partIds);
            return MergePartInventory(partResults, inventoryResults);
        }


        public IEnumerable<Machine> GetMachinesByPartialModelNumber(string modelNumber)
        {
            return from machine in dataContext.Machines
                   where machine.ModelNumber==modelNumber
                   select machine;
        }

        public Machine GetMachine(int MachineId)
        {
            return dataContext.Machines.FirstOrDefault(p => p.Id == MachineId);
        }

        public MachineDepartment GetMachineDepartment(int machineDepartmentId)
        {
            return dataContext.MachineDepartments.FirstOrDefault(p => p.Id == machineDepartmentId);
        }

        public Part GetPart(string partSku)
        {
            return dataContext.Parts.FirstOrDefault(p => p.SKU == partSku);
        }

        public MachinePart GetMachinePart(int machinePartId)
        {
            return dataContext.MachineParts.FirstOrDefault(p => p.Id == machinePartId);
        }

        public PartSupplier GetPartSupplier(int partSupplierId)
        {
            return dataContext.PartSuppliers.FirstOrDefault(p => p.Id == partSupplierId);
        }

        public Supplier GetSupplier(int supplierId)
        {
            return dataContext.Suppliers.FirstOrDefault(p => p.Id == supplierId);
        }

        public IEnumerable<MachineDTO> GetMachinesByCategory(int categoryId)
        {
            return from machine in dataContext.Machines
                   where machine.Category.Id.Value == categoryId
                   select new MachineDTO
                              {
                                  Id = machine.Id.HasValue ? machine.Id.Value : 0,
                                  MachineName = machine.Title,
                                  Manufacturer = machine.Manufacturer.Title,
                                  Model = machine.ModelNumber
                              };
        }

        public IEnumerable<MachineDTO> GetMachinesByManufacturer(int manufacturerId)
        {
            return from machine in dataContext.Machines
                   where machine.Manufacturer.Id.Value == manufacturerId
                   select new MachineDTO
                   {
                       Id = machine.Id.HasValue ? machine.Id.Value : 0,
                       MachineName = machine.Title,
                       Manufacturer = machine.Manufacturer.Title,
                       Model = machine.ModelNumber
                   };
        }

        public IEnumerable<MachineDTO> GetMachinesByDepartment(int departmentId)
        {
            return from machineDepartment in dataContext.MachineDepartments
                   where machineDepartment.Department.Id.Value == departmentId
                   select new MachineDTO
                   {
                       Id = machineDepartment.Machine.Id.HasValue ? machineDepartment.Machine.Id.Value : 0,
                       MachineName = machineDepartment.Machine.Title,
                       Manufacturer = machineDepartment.Machine.Manufacturer.Title,
                       Model = machineDepartment.Machine.ModelNumber
                   };
        }

        public IEnumerable<MachinePart> GetMachinePartsByPart(string partSku)
        {
            return from machinePart in dataContext.MachineParts
                   where machinePart.Part.SKU == partSku
                   select machinePart;
        }

        public IEnumerable<MachineDepartment> GetMachineDepartmentsByDepartment(int departmentId)
        {
            return from machineDepartment in dataContext.MachineDepartments
                   where machineDepartment.Department.Id.Value == departmentId
                   select machineDepartment;
        }

        public IEnumerable<Category> GetCategories()
        {
            return dataContext.Categories.OrderBy(p => p.Title);
        }

        public IEnumerable<Category> GetCategoriesByPartialName(string categoryName)
        {
            return from category in dataContext.Categories
                   where category.Title==categoryName
                   select category;
        }

        public IEnumerable<Department> GetDepartmentsByPartialName(string departmentName)
        {
            return from department in dataContext.Departments
                   where department.Title==departmentName
                   select department;
        }

        public IEnumerable<Department> GetDepartments()
        {
            return dataContext.Departments.OrderBy(p => p.Title);
        }

        public IEnumerable<Manufacturer> GetManufacturers()
        {
            return dataContext.Manufacturers.OrderBy(p => p.Title);
        }

        public IEnumerable<Manufacturer> GetManufacturersByPartialName(string manufacturerName)
        {
            return from manufacturer in dataContext.Manufacturers
                   where manufacturer.Title==manufacturerName
                   select manufacturer;
        }

        public IEnumerable<PartSupplier> GetPartSuppliers(Part part)
        {
            return from partSupplier in dataContext.PartSuppliers
                   where partSupplier.Part.Id == part.Id
                   select partSupplier;
        }

        public IEnumerable<PartSupplier> GetPartSuppliers(string partSku)
        {
            return from partSupplier in dataContext.PartSuppliers
                   where partSupplier.Part.SKU == partSku
                   select partSupplier;
        }

        public IEnumerable<Supplier> GetSuppliersByPartialName(string supplierName)
        {
            return from supplier in dataContext.Suppliers
                   where supplier.Title==supplierName
                   select supplier;
        }

        public bool AddNewInventoryLocationToPart(InventoryLocation inventoryLocation, Part part)
        {
            inventoryLocation.Part = part;
            dataContext.InventoryLocations.InsertOnSubmit(inventoryLocation);
            dataContext.SubmitChanges();
            return (dataContext.ChangeConflicts != null && dataContext.ChangeConflicts.Count == 0);
        }

        public bool AddNewInventoryLocationToPart(string partSku, double quantity, string bin)
        {
            Part part = GetPart(partSku);
            InventoryLocation location = new InventoryLocation();
            location.BinNumber = bin;
            location.Quantity = quantity;
            location.Part = part;

            dataContext.InventoryLocations.InsertOnSubmit(location);
            dataContext.SubmitChanges();

            return (dataContext.ChangeConflicts != null && dataContext.ChangeConflicts.Count == 0);
        }

        public bool AddNewMachinePart(int machineId, string partSku)
        {
            Part part = GetPart(partSku);
            Machine machine = GetMachine(machineId);
            var machinePart = new MachinePart
                                 {
                                     Machine = machine,
                                     Part = part
                                 };
            dataContext.MachineParts.InsertOnSubmit(machinePart);
            dataContext.SubmitChanges();
            return (dataContext.ChangeConflicts != null && dataContext.ChangeConflicts.Count == 0);
        }

        public bool AddNewPartSupplier(int supplierId, string partSku)
        {
            Part part = GetPart(partSku);
            Supplier supplier = GetSupplier(supplierId);
            var partSupplier = new PartSupplier
                                   {
                                       Part = part,
                                       Supplier = supplier
                                   };
            dataContext.PartSuppliers.InsertOnSubmit(partSupplier);
            dataContext.SubmitChanges();
            return (dataContext.ChangeConflicts != null && dataContext.ChangeConflicts.Count == 0);
        }

        public bool DeletePartSupplier(int partSupplierId)
        {
            PartSupplier partSupplier = GetPartSupplier(partSupplierId);

            dataContext.PartSuppliers.DeleteOnSubmit(partSupplier);
            dataContext.SubmitChanges();
            return (dataContext.ChangeConflicts != null && dataContext.ChangeConflicts.Count == 0);
        }

        public bool DeleteMachinePart(int machinePartId)
        {
            MachinePart machinePart = GetMachinePart(machinePartId);

            dataContext.MachineParts.DeleteOnSubmit(machinePart);
            dataContext.SubmitChanges();
            return (dataContext.ChangeConflicts != null && dataContext.ChangeConflicts.Count == 0);
        }

        public bool DeleteMachineDepartment(int machineDepartmentId)
        {
            MachineDepartment machineDepartment = GetMachineDepartment(machineDepartmentId);

            dataContext.MachineDepartments.DeleteOnSubmit(machineDepartment);
            dataContext.SubmitChanges();

            return (dataContext.ChangeConflicts != null && dataContext.ChangeConflicts.Count == 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (dataContext != null)
                {
                    dataContext.Dispose();
                    dataContext = null;
                }
            }
        }
    }
}

