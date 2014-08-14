<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagePartLocations.ascx.cs" Inherits="DataModels.SharePointList.Sandbox.ManagePartLocations.ManagePartLocations" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<table>
    <tr>
        <td>
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
                <asp:BoundField DataField="PartSku" HeaderText="Part SKU"></asp:BoundField>
                <asp:BoundField DataField="LocationBin" HeaderText="Bin #"></asp:BoundField>
                <asp:BoundField DataField="InventoryQuantity" HeaderText="Quantity">
                </asp:BoundField>
            </Columns>
        </asp:gridview>
        </td>
    </tr>
    <tr>
        <td>
        <hr />
        <table>
    <tr>
      <td>
         <asp:label id="BinLabel" runat="server" text="Bin"></asp:label>
        </td>
        
        <td>
           <asp:textbox id="BinTextBox" runat="server"></asp:textbox>
        </td>
    </tr>
    <tr>
        <td>            
            <asp:label id="QuantityLabel" runat="server" text="Quantity"></asp:label>
        </td>
        <td>
        <asp:textbox id="QuantityTextBox" runat="server"></asp:textbox>
        </td>
    </tr>
    <tr>
        <td>
        </td>        
        <td><asp:button id="SaveButton" runat="server" text="Add Location" />
        </td>
    </tr>
</table>
        </td>
    </tr>
</table>
