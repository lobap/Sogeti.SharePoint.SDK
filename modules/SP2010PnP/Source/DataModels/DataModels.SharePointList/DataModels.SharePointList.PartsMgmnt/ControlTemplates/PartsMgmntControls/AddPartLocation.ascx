<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddPartLocation.ascx.cs"
    Inherits="DataModels.SharePointList.PartsMgmnt.ControlTemplates.PartsMgmntControls.AddPartLocation" %>
<asp:updatepanel runat="server" updatemode="Conditional" childrenastriggers="false" id="UpdatePanel1">
        <contenttemplate>
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

        </contenttemplate>
        </asp:updatepanel>
