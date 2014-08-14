var returnTable = null;
var copyOfForm = null;

function buildTable(viewModels) {
    returnTable = '<table style=\"border: solid 1px black\"><tr style=\"font-weight:bold;font-style:underline\"><td>ID</td><td>Part Name</td><td>Part SKU</td><td>Bin #</td><td>Quantity</td><td>Inventory</td><td>Suppliers</td></tr>';
    for (var i = 0; i < viewModels.length; i++) {
        var item = viewModels[i];
        buildRow(item);
    }

    returnTable = returnTable + '</table>';
    $('#ContentDiv').html(returnTable);
}

function buildRow(item) {
    var sku = item["SKU"];
    var partTitle = item["Title"];
    var partId = item["Id"];
    var bin = item["LocationBin"];
    var quantity = item["InventoryQuantity"];
    //id needs to be 0 if it doesn't exist
    var id = '0';
    if (item["InventoryLocationId"] !== undefined) {
        id = item["InventoryLocationId"]
    }

    returnTable = returnTable + '<tr><td>' + id + '</td><td>' + partTitle + '</td><td>' + sku + '</td><td>' + bin + '</td><td style=\"text-align:center\">' + quantity + '</td><td><a href=\"javascript:showLocation(\'' + id + '\',\'' + partId + '\');\">|&nbsp;Edit Inventory&nbsp;|</a></td><td><a href=\"javascript:showSuppliers(\'' + partId + '\');\">&nbsp;Suppliers&nbsp;|</a></td></tr>';
}


function showSuppliers(partId) {

    supplierSearch(partId);

    var divSuppliers = document.getElementById("divSuppliers");
    // showModalDialog removes the element passed in from the DOM
    // so we save a copy and add it back later    
    copyOfForm = divSuppliers.cloneNode(true);
    divSuppliers.style.display = "block";
    var options = { html: divSuppliers, title: 'Suppliers', width: 300, height: 350, dialogReturnValueCallback: ReAddClonedForm };
    modalDialog = SP.UI.ModalDialog.showModalDialog(options);
}

function hideSuppliers() {
    modalDialog.close();
}

function hideLocationDialogue() {
    modalDialog.close();
    $("#divLocationAdd").hide();
}

function ReAddClonedForm() {
    document.body.appendChild(copyOfForm);
}

function showLocation(locationId, partId) {

    var div = document.getElementById("divLocations");

    copyOfForm = div.cloneNode(true);
    div.style.display = "block";

    var options = { html: div, title: 'Inventory Locations', width: 300, height: 300, dialogReturnValueCallback: ReAddClonedForm };
    modalDialog = SP.UI.ModalDialog.showModalDialog(options);

    var divAdd = document.getElementById("divLocationAdd");
    divAdd.style.display = "block";

    $('#hidLocationId').val(locationId);
    $('#hidPartId').val(partId);

    if (locationId == '0') {
        $('#hidLocationId').val('0');
        $('#binText').val('');
        $('#quantityText').val('');
        $('#buttonNew').hide();
    }
    else {
        loadPartLocation(locationId);
    }

}

var arrayContainsValue = function (array, value) {
    for (var i = 0; i < array.length; i++) {
        if (array[i] == value) {
            return true;
        }
    };
    return false;
}
