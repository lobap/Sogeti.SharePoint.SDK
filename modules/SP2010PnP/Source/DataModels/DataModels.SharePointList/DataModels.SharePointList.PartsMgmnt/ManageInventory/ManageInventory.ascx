<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageInventory.ascx.cs" Inherits="DataModels.SharePointList.PartsMgmnt.ManageInventory.ManageInventoryUserControl" %>

<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<table width="100%">
    <tr>
        <td width="200px" valign="top">
           <table width="100%">
    <tr>
        <td colspan="2">
            <asp:label runat="server" id="PartSearchLabel" text="Search By Part SKU"></asp:label>
            <br />
            <asp:regularexpressionvalidator runat="server" errormessage="RegularExpressionValidator"
                id="SkuRegExValidator" controltovalidate="PartSearchTextBox" 
                validationexpression="^[a-zA-Z0-9]{3,255}$" Display="Dynamic" 
                SetFocusOnError="True">Part SKU must contain at least three (3) alpha-numeric characters !</asp:regularexpressionvalidator>
        </td>
    </tr>
    <tr>
        <td>
            <asp:textbox id="PartSearchTextBox" runat="server"></asp:textbox>
        </td>
        <td>
            <asp:button id="GoButton" runat="server" text="Go" />
        </td>
    </tr>
          <tr>
            <td colspan="2" align="right">
                <asp:hyperlink runat="server" id="AddNewHyperlink">Add New Part</asp:hyperlink>
            </td>
        </tr>
</table>
        </td>
        <td valign="top">
            <asp:updatepanel runat="server" id="PartResultUpdatePanel" childrenastriggers="false"
    updatemode="Conditional">
    <contenttemplate>
        <asp:gridview runat="server" ID="PartInventoryResultsGridView" 
            AutoGenerateColumns="False" EnableModelValidation="True" 
            DataKeyNames="InventoryLocationId" >
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="SelectHyperLink" runat="server" NavigateUrl="" 
                            Text='select'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PartName" HeaderText="Part Name"></asp:BoundField>
                <asp:BoundField DataField="Sku" HeaderText="SKU"></asp:BoundField>
                <asp:BoundField DataField="LocationBin" HeaderText="Bin #"></asp:BoundField>
                <asp:BoundField DataField="InventoryQuantity" HeaderText="Quantity">
                </asp:BoundField>
            </Columns>
        </asp:gridview>
    </contenttemplate>
</asp:updatepanel>
        </td>
    </tr>
</table>
