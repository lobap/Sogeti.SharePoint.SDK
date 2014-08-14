<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagePartMachines.ascx.cs"
    Inherits="DataModels.SharePointList.PartsMgmnt.ControlTemplates.PartsMgmntControls.ManagePartMachines" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<table>
    <tr>
        <td>
            <asp:updatepanel runat="server" id="PartMachineResultsUpdatePanel" childrenastriggers="false"
                updatemode="Conditional">
 <contenttemplate>
 <table>
 <tr>
    <td>            
        <asp:gridview runat="server" id="PartMachineResultsGridView" 
    AutoGenerateColumns="False" DataKeyNames="Id"
    EnableModelValidation="True" >
    <Columns>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:LinkButton ID="DeleteLinkButton" runat="server" CausesValidation="false" 
                    CommandName="Delete" Text="Delete"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="MachineName" HeaderText="Machine Name"></asp:BoundField>
        <asp:BoundField DataField="Manufacturer" HeaderText="Manufacturer"></asp:BoundField>
        <asp:BoundField DataField="Model" HeaderText="Model No"></asp:BoundField>       
    </Columns>
            <HeaderStyle Wrap="False" />
            <RowStyle Wrap="False" />
</asp:gridview></td></tr></table>
 <tr></tr>

            </contenttemplate>
            </asp:updatepanel>
        </td>
    </tr>
    <tr>
        <td>
            <hr />
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
        </td>
    </tr>
    <tr><td>
    <asp:updatepanel runat="server" id="MachineResultsUpdatePanel" childrenastriggers="false"
        updatemode="Conditional">
 <contenttemplate>
 <table>
 <tr>
    <td>            
        <asp:gridview runat="server" id="MachineResultsGridView" 
    AutoGenerateColumns="False" DataKeyNames="Id"
    EnableModelValidation="True">
    <Columns>
        <asp:BoundField DataField="Id" Visible="False"></asp:BoundField>
 
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:LinkButton ID="SelectLinkButton" runat="server" CausesValidation="false" 
                    CommandName="Select" Text="Select"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="MachineName" HeaderText="Machine Name"></asp:BoundField>
        <asp:BoundField DataField="Manufacturer" HeaderText="Manufacturer"></asp:BoundField>
        <asp:BoundField DataField="Model" HeaderText="Model No"></asp:BoundField>
         
    </Columns>
            <HeaderStyle Wrap="False" />
            <RowStyle Wrap="False" />
</asp:gridview>
     </td></tr>
</table>
            </contenttemplate>
            </asp:updatepanel>
    </td></tr>
</table>
