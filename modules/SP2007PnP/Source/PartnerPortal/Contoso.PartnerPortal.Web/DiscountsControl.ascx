<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscountsControl.ascx.cs"
    Inherits="Contoso.PartnerPortal.ProductCatalog.Controls.DiscountsControl, Contoso.PartnerPortal.ProductCatalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0dcd9137292eac97" %>
<%@ Import Namespace="Contoso.Common.BusinessEntities"%>
<%@ Assembly Name="Contoso.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8a83c3e0a2fe6ae9" %>
<asp:Repeater ID="DiscountsRepeater" runat="server">
    <HeaderTemplate>
        <table>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
            <%#((Discount)Container.DataItem).ProductSku %>
            </td>
            <td>
            <%#((Discount)Container.DataItem).Name %>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
