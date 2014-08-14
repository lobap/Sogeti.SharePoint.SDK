<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilterByArea.ascx.cs"
    Inherits="DataModels.SharePointList.PartsMgmnt.PartsMgmntControls.FilterByArea" %>
<table width="100%">
    <tr>
        <td colspan="2">
            <asp:label runat="server" id="AreaLabel" text="Find By Area"></asp:label>
        </td>
    </tr>
    <tr>
        <td style="width:125px" valign="top" >

            <asp:radiobuttonlist id="AreaRadioButton"
                runat="server" AutoPostBack="True">
                <asp:ListItem>Category</asp:ListItem>
                <asp:ListItem>Department</asp:ListItem>
                <asp:ListItem>Manufacturer</asp:ListItem>
            </asp:radiobuttonlist>
        </td>
        <td>
            <asp:gridview runat="server" id="AreaResultsGridView" 
                AutoGenerateColumns="False" BorderStyle="None" DataKeyNames="Id" 
                EnableModelValidation="True" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Id" Visible="False"></asp:BoundField>
                    <asp:ButtonField DataTextField="Title" Text="Button"></asp:ButtonField>
                </Columns>
            </asp:gridview>
        </td>
    </tr>
</table>
