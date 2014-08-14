<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MachineResults.ascx.cs"
    Inherits="DataModels.SharePointList.PartsMgmnt.PartsMgmntControls.MachineResults" %>
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
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:LinkButton ID="SelectLinkButton" runat="server" CausesValidation="false" 
                    CommandName="Select" Text="Select"></asp:LinkButton>
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
