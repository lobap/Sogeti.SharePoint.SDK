<%@ Control Language="C#" AutoEventWireup="true" Inherits="Contoso.PartnerPortal.ProductCatalog.Controls.RelatedPartsControl, Contoso.PartnerPortal.ProductCatalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0dcd9137292eac97"
    CodeBehind="RelatedPartsControl.ascx.cs" %>
<%@ Assembly Name="Contoso.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8a83c3e0a2fe6ae9" %>
<%@ Register Assembly="Microsoft.Practices.SPG.AjaxSupport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8768ccae1c3c9eb2"
    TagPrefix="spg" Namespace="Microsoft.Practices.SPG.AJAXSupport.Controls" %>
<spg:SafeScriptManager ID="SafeScriptManager" runat="server" EnableUpdatePanelSupport="True" />
<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
        <asp:Repeater ID="PartsRepeater" runat="server">
            <HeaderTemplate>
                <table>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#((Contoso.Common.BusinessEntities.Part)Container.DataItem).PartId%>
                    </td>
                    <td>
                        <%#((Contoso.Common.BusinessEntities.Part)Container.DataItem).Name%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <Div><%# ErrorMessage %></Div>
        <asp:Button ID="LoadPartsButton" runat="server" OnClick="LoadPartsButton_Click" Text="Load parts" />
    </ContentTemplate>
</asp:UpdatePanel>
