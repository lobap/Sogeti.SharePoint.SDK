//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace DataModels.SharePointList.PartsMgmnt.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using DTOs;
    using Microsoft.SharePoint.Linq.Behaviors;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;

    [TestClass]
    public class PartManagementRepositoryTests
    {
        [TestMethod]
        [HostType("Moles")]
        public void GetPartsByPartialSku_ReturnsPartsStartingWithMatchingSku()
        {
            // Arrange
            var dataContext = new BDataContext<PartsSiteDataContext>();
            dataContext.SetEmpty();

            BEntityList<Part> partsList = dataContext.SetOne("Parts",
                                                new Part { SKU = "sku0" },
                                                new Part { SKU = "1sku1" },
                                                new Part { SKU = "2sku" });

            var target = new PartManagementRepository(dataContext);
            var parts = new List<Part>(target.GetPartsByPartialSku("sku"));

            Assert.AreEqual(1, parts.Count);
            Assert.AreEqual(partsList.Entities[0], parts[0]);
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetInventoryLocations_ReturnsInventoryLocationsRelatedToPart()
        {
            Part[] parts = 
            { 
                new Part { SKU = "sku1", Id = 111},
                new Part { SKU = "sku2", Id = 222},
            };
            InventoryLocation[] inventoryLocations = 
            {
                new InventoryLocation { 
                    Part = parts[0], 
                    BinNumber = "Bin1", 
                    Quantity = 1 
                },
                new InventoryLocation { 
                    Part = parts[0], 
                    BinNumber = "Bin2", 
                    Quantity = 2
                },
                new InventoryLocation { 
                    Part = parts[1], 
                    BinNumber = "Bin3", 
                    Quantity = 3
                },
            };

            var dataContext = new BDataContext<PartsSiteDataContext>();
            dataContext.SetOne("Parts", parts);
            dataContext.SetNext("Inventory Locations", inventoryLocations);

            var target = new PartManagementRepository(dataContext);
            var results = target.GetInventoryLocations(parts[0]).OrderBy((il) => il.BinNumber).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("Bin1", results[0].BinNumber);
            Assert.AreEqual("Bin2", results[1].BinNumber);
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetPartsInventoryView_ReturnsPartsAndAnyRelatedInventoryLocations()
        {
            Part[] parts = 
            { 
                new Part { SKU = "sku1", Id = 1},
                new Part { SKU = "sku2", Id = 2},
            };
            InventoryLocation[] inventoryLocations = 
            {
                new InventoryLocation { 
                    Part = parts[0], 
                    BinNumber = "Bin1", 
                    Quantity = 1 
                },
            };

            var dataContext = new BDataContext<PartsSiteDataContext>();
            dataContext.SetOne("Parts", parts);
            dataContext.SetNext("Inventory Locations", inventoryLocations);

            var target = new PartManagementRepository(dataContext);
            var results = target.GetPartsInventoryView("sku").OrderBy((p) => p.Sku).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("sku1", results[0].Sku);
            Assert.AreEqual("Bin1", results[0].LocationBin);
            Assert.AreEqual(1, results[0].InventoryQuantity);
            Assert.AreEqual("sku2", results[1].Sku);
        }


        [TestMethod]
        [HostType("Moles")]
        public void GetPartsByMachineId_ReturnsRelatedParts()
        {
            Part[] parts = 
            { 
                new Part { Id = 1, SKU = "sku1"},
                new Part { Id = 2, SKU = "sku2"},
                new Part { Id = 3, SKU = "sku3"},
                new Part { Id = 4, SKU = "sku4"},
            };
            Machine[] machines = 
            {
                new Machine { Id = 1111, ModelNumber = "mn1"},
                new Machine { Id = 2222, ModelNumber = "mn2"},
            };
            MachinePart[] machineParts = 
            {
                new MachinePart {Machine = machines[0], Part = parts[0]},
                new MachinePart {Machine = machines[0], Part = parts[1]},
                new MachinePart {Machine = machines[1], Part = parts[2]},
                new MachinePart {Machine = machines[1], Part = parts[3]},
            };

            var dataContext = new BDataContext<PartsSiteDataContext>();
            dataContext.SetOne("Parts", parts);
            dataContext.SetNext("Machines", machines);
            dataContext.SetNext("Machine Parts", machineParts);
            dataContext.SetNext("Inventory Locations", new InventoryLocation[]{});

            var target = new PartManagementRepository(dataContext);
            var results = target.GetPartsByMachineId(1111).OrderBy((p) => p.Sku).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("sku1", results[0].Sku);
            Assert.AreEqual("sku2", results[1].Sku);
        }
    }
}
