<%@ Control Language="C#" AutoEventWireup="true" Inherits="Contoso.TrainingManagement.ControlTemplates.DirectReports, Contoso.TrainingManagement, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5" %>
<br />
<asp:DataList ID="DirectReportsList" runat="server">
    <ItemTemplate>
        <a href="<%# this.UserDisplayUrl %><%# Eval("Key") %><%# this.SourceUrl %>"><%# Eval("Value") %></a>
    </ItemTemplate>
    <AlternatingItemStyle CssClass="ms-alternating" />
</asp:DataList>
<br />
<asp:Label ID="Message" runat="server"></asp:Label>