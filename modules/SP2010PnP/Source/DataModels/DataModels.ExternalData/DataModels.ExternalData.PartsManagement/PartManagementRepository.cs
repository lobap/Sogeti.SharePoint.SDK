//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace DataModels.ExternalData.PartsManagement
{
    using System.Collections.Generic;
    using ContactsSystem;
    using Microsoft.BusinessData.MetadataModel;
    using Microsoft.BusinessData.Runtime;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.BusinessData.Runtime;
    using Microsoft.SharePoint.BusinessData.SharedService;
    using ViewModels;

    public class PartManagementRepository
    {
        BdcService bdcService;
        IMetadataCatalog catalog;
        ILobSystemInstance partsManagementLobSystemInstance;
        ILobSystemInstance contactsLobSystemInstance;

        public PartManagementRepository()
        {
            bdcService = SPFarm.Local.Services.GetValue<BdcService>();
            catalog = bdcService.GetDatabaseBackedMetadataCatalog(SPServiceContext.Current);
            partsManagementLobSystemInstance = catalog.GetLobSystem(Constants.LobSystemName).GetLobSystemInstances()[Constants.LobSystemName];
            contactsLobSystemInstance = catalog.GetLobSystem(Constants.ContactsLobSystemName).GetLobSystemInstances()[Constants.ContactsLobSystemName];
        }

        //this method uses an Association from the model to return Items associated with the selected machine
        public List<BdcPart> GetPartsByMachineId(int machineId)
        {
            //Get Destination entity and Association
            IEntity entity = catalog.GetEntity(Constants.BdcEntityNameSpace, "Parts");
            IAssociation association = (IAssociation)entity.GetMethodInstance("GetPartsByMachineID", MethodInstanceType.AssociationNavigator);

            //Get Source entity and add to collection (required for method call)
            IEntityInstance machineInstance = GetBdcEntityInstance(machineId, "Machines");
            EntityInstanceCollection collection = new EntityInstanceCollection();
            collection.Add(machineInstance);

            // Navigate the association.
            IEntityInstanceEnumerator associatedInstances = entity.FindAssociated(collection, association, partsManagementLobSystemInstance, OperationMode.Online);

            //Return a collection of View Models from the associated Data Table
            return new DataMapper<BdcPart>(entity.Catalog.Helper.CreateDataTable(associatedInstances)).Collection;
        }

        //This method uses a Wildcard Filter on the Read All Operation to return partial matches
        public List<BdcMachine> GetMachinesByModelNumber(string modelNumber)
        {
            //Get the BDC entity
            IEntity entity = catalog.GetEntity(Constants.BdcEntityNameSpace, "Machines");

            //Get all Filters for the entity
            IFilterCollection filters = entity.GetDefaultFinderFilters();

            //Set Filter value
            if (!string.IsNullOrEmpty(modelNumber))
            {
                WildcardFilter filter = (WildcardFilter)filters[0];
                filter.Value = modelNumber;
            }

            //Get Filtered ITems
            IEntityInstanceEnumerator enumerator = entity.FindFiltered(filters, partsManagementLobSystemInstance);
            return new DataMapper<BdcMachine>(entity.Catalog.Helper.CreateDataTable(enumerator)).Collection;
        }

        //This method returns a strongly Typed entity based on an identifier
        public BdcMachine GetMachine(int identifier)
        {
            IEntityInstance instance = GetBdcEntityInstance(identifier, "Machines");
            return new DataMapper<BdcMachine>(instance.EntityAsDataTable).Instance;
        }

        //This method executes the SpecificFinder method and returns a generic BDC Entity
        private IEntityInstance GetBdcEntityInstance(int identifier, string entityName)
        {
            Identity MyId = new Identity(identifier);
            IEntity entity = catalog.GetEntity(Constants.BdcEntityNameSpace, entityName);
            IEntityInstance instance = entity.FindSpecific(MyId, partsManagementLobSystemInstance);

            return instance;
        }

        //This method uses a Wildcard Filter on the Read All Operation to return partial matches
        public List<BdcSupplier> GetSuppliersByName(string supplierName)
        {
            //Get the BDC entity
            IEntity entity = catalog.GetEntity(Constants.BdcContactsEntityNameSpace, "BdcSupplier");

            //Get all Filters for the entity
            IFilterCollection filters = entity.GetMethodInstance("ReadList2", MethodInstanceType.Finder).GetFilters();

            //Set Filter value)
            if (!string.IsNullOrEmpty(supplierName))
            {
                WildcardFilter filter = (WildcardFilter)filters[0];
                filter.Value = supplierName;
            }

            //Get Filtered ITems
            IEntityInstanceEnumerator enumerator = entity.FindFiltered(filters, "ReadList2", contactsLobSystemInstance);
            return new DataMapper<BdcSupplier>(entity.Catalog.Helper.CreateDataTable(enumerator)).Collection;
        }


    }
}
