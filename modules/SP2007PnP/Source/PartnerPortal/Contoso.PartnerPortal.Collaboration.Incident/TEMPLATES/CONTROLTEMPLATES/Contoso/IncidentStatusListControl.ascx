<%@ Control Language="C#" AutoEventWireup="true" Inherits="Contoso.PartnerPortal.Collaboration.Incident.Controls.IncidentStatusListControl, Contoso.PartnerPortal.Collaboration.Incident, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f46683bd8f0b79b4" %>
<asp:Repeater ID="IncidentStatusRepeater" runat="server">
    <HeaderTemplate>
        <table>
            <tr style="background-color:Silver">
                <td>Incident</td>
                <td style="width:60">Status</td>
                <td style="width:140">Created Date</td>
                <td style="width:140">Last Modified Date</td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>'><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
            </td>                
            <td style="width:80">
                <%# DataBinder.Eval(Container.DataItem, "Status") %>
            </td>
            <td style="width:160">
                <%# DataBinder.Eval(Container.DataItem, "CreatedDate") %>
            </td>
            <td style="width:160">
                <%# DataBinder.Eval(Container.DataItem, "LastModifiedDate") %>
            </td>
        </tr>        
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>