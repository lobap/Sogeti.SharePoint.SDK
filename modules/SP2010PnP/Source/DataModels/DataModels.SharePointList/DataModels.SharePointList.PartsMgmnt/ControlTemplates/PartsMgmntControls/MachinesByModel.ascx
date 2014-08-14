<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MachinesByModel.ascx.cs"
    Inherits="DataModels.SharePointList.PartsMgmnt.PartsMgmntControls.MachinesByModel" %>
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
