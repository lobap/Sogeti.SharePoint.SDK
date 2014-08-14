var clientContext = null;
var web = null;
var returnTable = null;

function SkuSearch() {
    //Accessing list data from a different site collection is not currently supported.
    clientContext = new SP.ClientContext("/sites/sharepointlist");
    web = clientContext.get_web();
    var partList = web.get_lists().getByTitle("Parts");
    var inventoryList = web.get_lists().getByTitle("Inventory Locations");
    var camlQueryPartsList = new SP.CamlQuery();
    var camlPartsList = '<View><Query><Where><BeginsWith><FieldRef Name=\"SKU\" /><Value Type=\"Text"\>' + $('#skuTextBox').val() + '</Value></BeginsWith></Where></Query></View>';
    camlQueryPartsList.set_viewXml(camlPartsList);

    var camlQueryInventoryList = new SP.CamlQuery();
    var camlInventoryList = '<View><Query><Where><BeginsWith><FieldRef Name=\"PartLookupSKU\" /><Value Type=\"Lookup\">' + $('#skuTextBox').val() + '</Value></BeginsWith></Where><OrderBy Override=\"TRUE\"><FieldRef Name=\"PartLookupSKU\" /></OrderBy></Query><ViewFields><FieldRef Name=\"PartLookup\" LookupId=\"TRUE\" /><FieldRef Name=\"PartLookupSKU\" /><FieldRef Name=\"PartLookupTitle\" /><FieldRef Name=\"BinNumber\" /><FieldRef Name=\"Quantity\" /></ViewFields><ProjectedFields><Field Name=\"PartLookupSKU\" Type=\"Lookup\" List=\"PartLookup\" ShowField=\"SKU\" /><Field Name=\"PartLookupTitle\" Type=\"Lookup\" List=\"PartLookup\" ShowField=\"Title\" /></ProjectedFields><Joins><Join Type=\"LEFT\" ListAlias=\"PartLookup\"><Eq><FieldRef Name=\"PartLookup\" RefType=\"ID\" /><FieldRef List=\"PartLookup\" Name=\"ID\" /></Eq></Join></Joins></View>';
    camlQueryInventoryList.set_viewXml(camlInventoryList);

    this.partListItems = partList.getItems(camlQueryPartsList);
    this.inventoryListItems = inventoryList.getItems(camlQueryInventoryList);

    clientContext.load(this.partListItems, 'Include(DisplayName, ID, SKU)');
    clientContext.load(this.inventoryListItems, 'Include(BinNumber, Quantity, ID, PartLookup, PartLookupSKU, PartLookupTitle)');

    clientContext.executeQueryAsync(Function.createDelegate(this, this.onListItemsLoadSuccess),
    Function.createDelegate(this, this.onQueryFailed));
}

function onListItemsLoadSuccess(sender, args) {
    var inventoryPartResults = new Array();
    var bindingViewsModels = new Array();
    var noInventoryPartResults = new Array();

    var inventoryListEnumerator = this.inventoryListItems.getEnumerator();
    while (inventoryListEnumerator.moveNext()) {
        var currentItem = inventoryListEnumerator.get_current();

        var partTitle = simpleHTMLEncode(currentItem.get_item('PartLookupTitle').get_lookupValue());
        var partSku = simpleHTMLEncode(currentItem.get_item('PartLookupSKU').get_lookupValue());
        var partId = simpleHTMLEncode(currentItem.get_item('PartLookup').get_lookupId());
        var bin = simpleHTMLEncode(currentItem.get_item('BinNumber'));
        var quantity = simpleHTMLEncode(currentItem.get_item('Quantity'));
        var Id = simpleHTMLEncode(currentItem.get_item('ID'));

        var bindingViewModel =
                        {
                            Id: partId,
                            SKU: partSku,
                            Title: partTitle,
                            InventoryLocationId: Id,
                            LocationBin: bin,
                            InventoryQuantity: quantity
                        };

        bindingViewsModels.push(bindingViewModel);
        inventoryPartResults.push(partId);
    };

    //Determine parts with no inventory location
    var partListEnumerator = this.partListItems.getEnumerator();
    //Determine parts with no inventory location
    while (partListEnumerator.moveNext()) {
        var part = partListEnumerator.get_current();
        var currentPartId = part.get_item('ID');
        var inArray = arrayContainsValue(inventoryPartResults, currentPartId);
        if (inArray != true) {
            noInventoryPartResults.push(part);
        };
    }

    if (noInventoryPartResults.length > 0) {
        for (var i = 0; i < noInventoryPartResults.length; i++) {
            var partWithNoInventoryLocation = noInventoryPartResults[i];
            var title = simpleHTMLEncode(partWithNoInventoryLocation.get_displayName());
            var id = simpleHTMLEncode(partWithNoInventoryLocation.get_item('ID'));
            var sku = simpleHTMLEncode(partWithNoInventoryLocation.get_item('SKU'));
            var bindingViewModel =
                        {
                            Id: id,
                            SKU: sku,
                            Title: title,
                            LocationBin: "unassigned",
                            InventoryQuantity: ""

                        };

            bindingViewsModels.push(bindingViewModel);

        }
    }

    buildTable(bindingViewsModels);
}


function onQueryFailed(sender, args) {
    alert('request failed ' + args.get_message() + '\n' + args.get_stackTrace());
}

function supplierSearch(partId) {
    clientContext = new SP.ClientContext("/sites/sharepointlist");
    web = clientContext.get_web();
    var list2 = web.get_lists().getByTitle("Part Suppliers");
    var camlQuery2 = new SP.CamlQuery();
    var q2 = '<View><Query><Where><Eq><FieldRef Name=\"PartLookup\" LookupId=\"TRUE\" /><Value Type=\"Lookup\">' + partId + '</Value></Eq></Where></Query><ViewFields><FieldRef Name=\"SupplierLookupTitle\" /><FieldRef Name=\"SupplierLookupDUNS\" /><FieldRef Name=\"SupplierLookupRating\" /></ViewFields><ProjectedFields><Field Name=\"SupplierLookupTitle\" Type=\"Lookup\" List=\"SupplierLookup\" ShowField=\"Title\" /><Field Name=\"SupplierLookupDUNS\" Type=\"Lookup\" List=\"SupplierLookup\" ShowField=\"DUNS\" /><Field Name=\"SupplierLookupRating\" Type=\"Lookup\" List=\"SupplierLookup\" ShowField=\"Rating\" /></ProjectedFields><Joins><Join Type=\"LEFT\" ListAlias=\"SupplierLookup\"><Eq><FieldRef Name=\"SupplierLookup\" RefType=\"ID\" /><FieldRef List=\"SupplierLookup\" Name=\"ID\" /></Eq></Join></Joins></View>';
    camlQuery2.set_viewXml(q2);
    this.listItems = list2.getItems(camlQuery2);
    clientContext.load(this.listItems, 'Include(SupplierLookupTitle, SupplierLookupDUNS, SupplierLookupRating)');
    clientContext.executeQueryAsync(Function.createDelegate(this, this.onPartSupplierLoadSuccess),
        Function.createDelegate(this, this.onQueryFailed));
}

function onPartSupplierLoadSuccess(sender, args) {

    var partSupplierEnumerator = this.listItems.getEnumerator();
    var returnSupplierTable = '<table style=\"border: solid 1px black\;width:100%"><tr style=\"font-weight:bold;font-style:underline\"><td>Name</td><td>DUNS</td><td>Rating</td></tr>';
    while (partSupplierEnumerator.moveNext()) {
        var supplierItem = partSupplierEnumerator.get_current();

        var name = simpleHTMLEncode(supplierItem.get_item('SupplierLookupTitle').get_lookupValue());
        var duns = simpleHTMLEncode(supplierItem.get_item('SupplierLookupDUNS').get_lookupValue());
        var rating = simpleHTMLEncode(supplierItem.get_item('SupplierLookupRating').get_lookupValue());

        returnSupplierTable = returnSupplierTable + '<tr><td>' + name + '</td><td>' + duns + '</td><td>' + rating + '</td></tr>';
    }

    returnSupplierTable = returnSupplierTable + '</table>'
    $('#divSupplierResults').html(returnSupplierTable);
}

function savePartLocation() {
    clientContext = new SP.ClientContext("/sites/sharepointlist");
    web = clientContext.get_web();
    this.list = web.get_lists().getByTitle('Inventory Locations');
    var locationId = $('#hidLocationId').val()

    if (locationId == '0') {
        var itemCreateInfo = new SP.ListItemCreationInformation();
        this.oListItem = this.list.addItem(itemCreateInfo);
        var partId = $('#hidPartId').val();
        var partLookupField = new SP.FieldLookupValue();
        partLookupField.set_lookupId(partId);
        oListItem.set_item('PartLookup', partLookupField);
    }
    else {
        this.oListItem = list.getItemById(locationId);
    }

    oListItem.set_item('BinNumber', $('#binText').val());
    oListItem.set_item('Quantity', $('#quantityText').val());
    oListItem.update();
    clientContext.executeQueryAsync(Function.createDelegate(this, this.onUpdatePartLocationSuccess), Function.createDelegate(this, this.onQueryFailed));
}

function onUpdatePartLocationSuccess(sender, args) {
    SkuSearch();
    alert("Inventory Location Saved Successfully");
    var divAdd = document.getElementById("divLocationAdd");
    divAdd.style.display = "none";
    $('#buttonNew').show();

    hideLocationDialogue();
}

function loadPartLocation(locationId) {
    $('#hidLocationId').val(locationId);

    clientContext = new SP.ClientContext("/sites/sharepointlist");
    web = clientContext.get_web();
    this.list = web.get_lists().getByTitle('Inventory Locations');
    this.listItem = this.list.getItemById(locationId);
    clientContext.load(this.listItem);
    clientContext.executeQueryAsync(Function.createDelegate(this, this.onPartItemLoadSuccess), Function.createDelegate(this, this.onQueryFailed));
}

function onPartItemLoadSuccess(sender, args) {
    $('#binText').val(listItem.get_item('BinNumber'));
    $('#quantityText').val(listItem.get_item('Quantity'));
}

function simpleHTMLEncode(text) {
    encodedHtml = new String(text);
    encodedHtml = encodedHtml.replace(/</g, "&lt;");
    encodedHtml = encodedHtml.replace(/>/g, "&gt;");
    return encodedHtml;
} 