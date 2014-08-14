//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Collections.Generic;
using DataModels.SharePointList.Model;
using System;

namespace DataModels.SharePointList.PartsMgmnt
{
    using DTOs;

    public interface IPartManagementRepository : IDisposable
    {
        Machine GetMachine(int machineId);
        MachineDepartment GetMachineDepartment(int machineDepartmentId);
        Part GetPart(string sku);
        MachinePart GetMachinePart(int machinePartId);
        PartSupplier GetPartSupplier(int partSupplierId);
        Supplier GetSupplier(int supplierId);
        IEnumerable<Part> GetPartsByPartialSku(string sku);
        IEnumerable<InventoryLocation> GetInventoryLocations(Part part);
        IEnumerable<Machine> GetMachinesByPartialModelNumber(string modelNumber);
        IEnumerable<MachineDTO> GetMachinesByCategory(int categoryId);
        IEnumerable<MachineDTO> GetMachinesByManufacturer(int manufacturerId);
        IEnumerable<MachineDTO> GetMachinesByDepartment(int departmentId);
        IEnumerable<MachinePart> GetMachinePartsByPart(string sku);
        IEnumerable<MachineDepartment> GetMachineDepartmentsByDepartment(int departmentId);
        IEnumerable<Category> GetCategories();
        IEnumerable<Category> GetCategoriesByPartialName(string categoryName);
        IEnumerable<Department> GetDepartmentsByPartialName(string departmentName);
        IEnumerable<Department> GetDepartments();
        IEnumerable<Manufacturer> GetManufacturers();
        IEnumerable<Manufacturer> GetManufacturersByPartialName(string manufacturerName);
        IEnumerable<PartSupplier> GetPartSuppliers(Part part);
        IEnumerable<PartSupplier> GetPartSuppliers(string sku);
        IEnumerable<Supplier> GetSuppliersByPartialName(string supplierName);
        bool AddNewInventoryLocationToPart(InventoryLocation inventoryLocation, Part part);
        bool AddNewInventoryLocationToPart(string sku, double quantity, string bin);
        bool AddNewMachinePart(int machineId, string sku);
        bool AddNewPartSupplier(int supplierId, string sku);
        bool DeletePartSupplier(int partSupplierId);
        bool DeleteMachinePart(int machinePartId);
        bool DeleteMachineDepartment(int machineDepartmentId);
        IEnumerable<PartInventoryDTO> GetPartsInventoryView(string sku);
        IEnumerable<PartInventoryDTO> GetPartsByMachineId(int machineId);
        IEnumerable<PartInventoryDTO> GetPartsBySupplierId(int supplierId);
    }
}
