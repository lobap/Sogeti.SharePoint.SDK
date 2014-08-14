/// <reference path="http://ajax.microsoft.com/ajax/jQuery/jquery-1.4.1-vsdoc.js"/>

var inventoryLocation;

$(function AssociateButtonClickWithJSONCall() {

    $('#Button1').click(function OnButtonClick() {
        $('#ContentDiv').html("");
        $('#divSupplierResults').html("");
        var sku = $('#skuTextBox').val();
        $.getJSON(
            "/sites/sharepointlist/_vti_bin/listdata.svc/Parts()?$filter=startswith(SKU,'" + sku + "')&$select=Title,SKU,Id,Description",
            {},
            function ClearDivsAndMerge(data, status) {
                var parts = data.d.results;
                mergePartsWithInventoryLocations(sku, parts);
            }
        );
    });

    $('#buttonSave').click(function OnSaveButtonClick() {
        savePartLocation();
    });

    $('#buttonNew').click(function OnSaveButtonClick() {
        showLocation('0','0');
    });

});

var mergePartsWithInventoryLocations = function MergePartsWithInventoryLocations(sku, parts) {
    $.getJSON(
            "/sites/sharepointlist/_vti_bin/listdata.svc/InventoryLocations()?$filter=startswith(Part/SKU,'" + sku + "')&$orderby=Part/SKU&$expand=Part&$select=Id,BinNumber,Quantity,Part/Title,Part/SKU,Part/Id",
            {},
            function mergePartsAndInventory(data) {
                var inventoryLocations = data.d.results;

                var bindingViewsModels = new Array();
                var inventoryPartResults = new Array();
                var noInventoryPartResults = new Array();

                $.each(inventoryLocations, function bindViewModel(index, inventoryLocation) {
                    var bindingViewModel =
                        {
                            Id: inventoryLocation.Part.Id,
                            SKU: inventoryLocation.Part.SKU,
                            Title: inventoryLocation.Part.Title,
                            InventoryLocationId: inventoryLocation.Id,
                            LocationBin: inventoryLocation.BinNumber,
                            InventoryQuantity: inventoryLocation.Quantity
                        };

                    bindingViewsModels.push(bindingViewModel);
                    inventoryPartResults.push(inventoryLocation.Part.Id);
                });

                //Determine parts with no inventory location
                $.each(parts, function addIfNoInventory(index, part) {
                    if (arrayContainsValue(inventoryPartResults, part.Id) != true) {
                        noInventoryPartResults.push(part);
                    };
                });

                $.each(noInventoryPartResults, function bindNoInventory(index, partWithNoInventoryLocation) {
                    var bindingViewModel =
                        {
                            Id: partWithNoInventoryLocation.Id,
                            SKU: partWithNoInventoryLocation.SKU,
                            Title: partWithNoInventoryLocation.Title,
                            LocationBin: "unassigned",
                            InventoryQuantity: ""
                        };

                    bindingViewsModels.push(bindingViewModel);
                });

                buildTable(bindingViewsModels);

            });

}

var supplierSearch = function (partId) {
    var suppliersList = $('#divSupplierResults');
    suppliersList.html("");
    $.getJSON(
        "/sites/sharepointlist/_vti_bin/listdata.svc/PartSuppliers()?$filter=PartId eq " + partId + "&$expand=Supplier&$select=Supplier/DUNS,Supplier/Title,Supplier/Rating",
        {},
        function (data) {
            var partSuppliers = data.d.results;
            var supplierMarkup = '<table style=\"border: solid 1px black\;width:100%"><tr style=\"font-weight:bold;font-style:underline\"><td>Name</td><td>DUNS</td><td>Rating</td></tr>';
            $.each(partSuppliers, function (index, partSupplier) {
                supplierMarkup = supplierMarkup + '<tr><td>' + partSupplier.Supplier.Title + '</td><td>' + partSupplier.Supplier.DUNS + '</td><td>' + partSupplier.Supplier.Rating + '</td></tr>';
            });
            supplierMarkup = supplierMarkup + '</table>';
            suppliersList.html(supplierMarkup);
        }
    );
}

var loadPartLocation = function (locationId) {
    $.getJSON(
        "/sites/sharepointlist/_vti_bin/listdata.svc/InventoryLocations()?$filter=Id eq " + locationId + "&$select=BinNumber,Quantity",
        {},
        function (data) {
            inventoryLocation = data.d.results[0];

            $('#binText').val(inventoryLocation.BinNumber);
            $('#quantityText').val(inventoryLocation.Quantity);
        }
    );

}

var savePartLocation = function () {

    var locationId = $('#hidLocationId').val();
    var url = '/sites/sharepointlist/_vti_bin/listdata.svc/InventoryLocations';
    var beforeSendFunction;
    var inventoryLocationModifications = {};

    if (locationId == '0') {
        //Insert a new Part Location
        inventoryLocationModifications.PartId = $('#hidPartId').val();
        beforeSendFunction = function () { };
    }
    else {
        //Update Existing Part Location
        url = url + "(" + locationId + ")";
        beforeSendFunction = function (xhr) {
            xhr.setRequestHeader("If-Match", inventoryLocation.__metadata.etag);
            //Using MERGE so that the entire entity doesn't need to be sent over the wire. 
            xhr.setRequestHeader("X-HTTP-Method", 'MERGE');
        }
    }

    inventoryLocationModifications.BinNumber = $('#binText').val();
    inventoryLocationModifications.Quantity = $('#quantityText').val();

    var body = Sys.Serialization.JavaScriptSerializer.serialize(inventoryLocationModifications);

    $.ajax({
        type: 'POST',
        url: url,
        contentType: 'application/json',
        processData: false,
        beforeSend: beforeSendFunction,
        data: body,
        success: function () {
            alert('Inventory Location Saved.');
        }
    });


    hideLocationDialogue();
}