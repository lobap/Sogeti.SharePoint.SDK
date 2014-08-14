<%@ Control Language="C#" AutoEventWireup="true" Inherits="Contoso.PartnerPortal.Promotions.Controls.PartnerPromotionsControl, Contoso.PartnerPortal.Promotions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7dfe266eb414dba8" %>
<asp:Repeater ID="Repeater1" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "PromotionUrl") %>'
                    Text='<%# DataBinder.Eval(Container.DataItem, "PromotionName") %>' />
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
