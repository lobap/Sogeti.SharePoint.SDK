<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageDepartments.ascx.cs" Inherits="DataModels.SharePointList.Sandbox.ManageDepartments.ManageDepartments" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<table width="100%">
    <tr>
        <td valign="top"  style="width:200px">
            <table width="100%">
    <tr>
        <td colspan="2">
            <asp:label runat="server" id="DepartmentSearchLabel" text="Search By Department Name"></asp:label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:textbox id="DepartmentSearchTextBox" runat="server"></asp:textbox>
        </td>
        <td>
            <asp:button id="GoButton" runat="server" text="Go" />
        </td>
    </tr>
          <tr>
            <td colspan="2" align="right">
                <asp:hyperlink runat="server" id="AddNewHyperlink">Add New Department</asp:hyperlink>
            </td>
        </tr>
</table>
        </td>
        <td valign="top">
 
        <asp:gridview runat="server" ID="DepartmentResultsGridView" 
            AutoGenerateColumns="False" EnableModelValidation="True" 
            DataKeyNames="DepartmentId">
            <Columns>
            <asp:BoundField DataField="DepartmentId" Visible="False"></asp:BoundField>
                <asp:TemplateField HeaderText="Department Name">
                    <ItemTemplate>
                        <asp:HyperLink ID="DepartmentNameHyperLink" runat="server" NavigateUrl="" 
                            Text='<%# Eval("DepartmentName") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField CommandName="Select" HeaderText="Machines" Text="Machines">
                </asp:ButtonField>
            </Columns>
             <HeaderStyle Wrap="False" />
            <RowStyle Wrap="False" />
        </asp:gridview>

        </td>
    </tr>
    <tr>
    <td></td>
        <td valign="top">

 <table>
 <tr>
    <td>            
        <asp:gridview runat="server" id="MachineResultsGridView" 
    AutoGenerateColumns="False" DataKeyNames="Id"
    EnableModelValidation="True" >
    <Columns>
        <asp:ButtonField CommandName="Delete" Text="Delete"></asp:ButtonField>          
        <asp:BoundField DataField="MachineName" HeaderText="Machine Name"></asp:BoundField>
        <asp:BoundField DataField="LocationDescription" HeaderText="Location"></asp:BoundField>
        <asp:BoundField DataField="Model" HeaderText="Model No"></asp:BoundField>
    </Columns>
            <HeaderStyle Wrap="False" />
            <RowStyle Wrap="False" />
</asp:gridview></td></tr></table>
 <tr></tr>

        </td>
    </tr>
</table>