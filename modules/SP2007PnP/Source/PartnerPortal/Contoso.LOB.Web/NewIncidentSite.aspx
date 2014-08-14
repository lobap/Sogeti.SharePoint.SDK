<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewIncidentSite.aspx.cs" Inherits="Contoso.LOB.Web.NewIncidentSite" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Incident Management System: Create Incident Management Site</h1>
        </div>
        
        <table width="100%">
            <tr>
                <td align="center" colspan="2">
                    <asp:DropDownList runat="server" ID="incidentsDropDownList" DataValueField="Id" DataTextField="Text" />                
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="createSite" Text="Create Site" runat="server" onclick="CreateSite_Click" />
                </td>
                <td>
                    <asp:Button ID="closeIncident" Text="Close Incident" runat="server" onclick="CloseIncident_Click"/>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
