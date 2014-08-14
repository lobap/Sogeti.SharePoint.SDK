<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchPage.ascx.cs"
    Inherits="DataModels.SharePointList.PartsMgmnt.PartsMgmntControls.SearchPage" %>
<table width="100%">
    <tr>
        <td width="250px" valign="top">
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <asp:label runat="server" id="MachineSearchLabel" text="Search By Machine Model No."></asp:label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:textbox id="MachineSearchTextBox" runat="server"></asp:textbox>
                    </td>
                    <td>
                        <asp:button id="GoButton" runat="server" text="Go" />
                    </td>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:hyperlink runat="server" id="AddNewHyperlink">Add New Machine</asp:hyperlink>
                        </td>
                    </tr>
                </tr>
            </table>
            <hr />
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <asp:label runat="server" id="PartSearchLabel" text="Search By Part SKU"></asp:label>
                        <br />
                        <asp:regularexpressionvalidator runat="server" errormessage="RegularExpressionValidator"
                            id="SkuRegExValidator" controltovalidate="PartSearchTextBox" validationexpression="^[a-zA-Z0-9]{3,255}$"
                            display="Dynamic" setfocusonerror="True">Part SKU must contain at least three (3) alpha-numeric characters !</asp:regularexpressionvalidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:textbox id="PartSearchTextBox" runat="server"></asp:textbox>
                    </td>
                    <td>
                        <asp:button id="PartGoButton" runat="server" text="Go" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:hyperlink runat="server" id="PartAddNewHyperlink">Add New Part</asp:hyperlink>
                    </td>
                </tr>
            </table>
            <hr />
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <asp:label runat="server" id="AreaLabel" text="Find By Area"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 125px" valign="top">
                        <asp:radiobuttonlist id="AreaRadioButton" runat="server" autopostback="True">
                <asp:ListItem>Category</asp:ListItem>
                <asp:ListItem>Department</asp:ListItem>
                <asp:ListItem>Manufacturer</asp:ListItem>
            </asp:radiobuttonlist>
                    </td>
                    <td>
                        <asp:gridview runat="server" id="AreaResultsGridView" autogeneratecolumns="False"
                            borderstyle="None" datakeynames="Id" enablemodelvalidation="True" gridlines="None">
                <Columns>
                    <asp:BoundField DataField="Id" Visible="False"></asp:BoundField>
                    <asp:ButtonField DataTextField="Title" Text="Button"></asp:ButtonField>
                </Columns>
            </asp:gridview>
                    </td>
                </tr>
            </table>
        </td>
        <td valign="top">
            <asp:updatepanel runat="server" id="MachineResultsUpdatePanel" childrenastriggers="false"
                updatemode="Conditional">
 <contenttemplate>
 <table>
 <tr>
    <td>            
        <asp:gridview runat="server" id="MachineResultsGridView" 
    AutoGenerateColumns="False" DataKeyNames="Id"
    EnableModelValidation="True" >
    <Columns>
        <asp:BoundField DataField="Id" Visible="False"></asp:BoundField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HyperLink ID="MachineNameHyperLink" runat="server" NavigateUrl="" Text="Edit/Delete"></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="MachineName" HeaderText="Machine Name"></asp:BoundField>
        <asp:BoundField DataField="Manufacturer" HeaderText="Manufacturer"></asp:BoundField>
        <asp:BoundField DataField="Model" HeaderText="Model No"></asp:BoundField>
        <asp:ButtonField CommandName="ViewParts" 
            Text="Show Parts" HeaderText="Parts"></asp:ButtonField>          
    </Columns>
            <HeaderStyle Wrap="False" />
            <RowStyle Wrap="False" />
</asp:gridview></td></tr></table>
 
            </contenttemplate>
            </asp:updatepanel>
            <br />
            <br />
            <asp:updatepanel runat="server" id="PartResultUpdatePanel" childrenastriggers="False"
                updatemode="Conditional">
    <contenttemplate>
        <asp:gridview runat="server" ID="PartResultsGridView" 
            AutoGenerateColumns="False" EnableModelValidation="True" 
            DataKeyNames="PartId,Sku" >
            <Columns>
            <asp:BoundField DataField="PartId" Visible="False"></asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:HyperLink ID="PartNameHyperLink" runat="server" NavigateUrl="" 
                            Text='Edit / Delete'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PartName" HeaderText="Part Name"></asp:BoundField>
                <asp:BoundField DataField="Sku" HeaderText="SKU"></asp:BoundField>
                <asp:BoundField DataField="LocationBin" HeaderText="Bin #"></asp:BoundField>
                <asp:BoundField DataField="InventoryQuantity" HeaderText="Quantity">
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:HyperLink ID="MachineHyperLink" runat="server" NavigateUrl="" 
                            Text='Machines'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:HyperLink ID="SupplierHyperLink" runat="server" NavigateUrl="" 
                            Text='Suppliers'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:HyperLink ID="LocationHyperLink" runat="server" NavigateUrl="" 
                            Text='Locations'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
             <HeaderStyle Wrap="False" />
            <RowStyle Wrap="False" />
        </asp:gridview>
    </contenttemplate>
    <triggers>
        <asp:AsyncPostBackTrigger ControlID="PartResultsGridView" 
            EventName="PageIndexChanging" />
    </triggers>
</asp:updatepanel>
        </td>
    </tr>
</table>
