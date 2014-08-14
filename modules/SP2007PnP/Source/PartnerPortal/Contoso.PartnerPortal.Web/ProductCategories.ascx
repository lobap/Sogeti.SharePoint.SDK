<%@ Control Language="C#" AutoEventWireup="true" Inherits="Contoso.PartnerPortal.Portal.ProductCategories, Contoso.PartnerPortal.Portal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0561205e6cefaed4" %>
<asp:TreeView ID="TreeView1" runat="server" NodeStyle-CssClass="ms-navitem">
    <Nodes>
        <asp:TreeNode Value="Child1" Expanded="True" Text="Group Category 1">
            <asp:TreeNode Value="Grandchild1" Text="Category 1" />
            <asp:TreeNode Value="Grandchild2" Text="Category 2" />
        </asp:TreeNode>
        <asp:TreeNode Value="Child2" Text="Group Category 2" />
        <asp:TreeNode Value="Child3" Expanded="True" Text="Group Category 3">
            <asp:TreeNode Value="Grandchild1" Text="Category 3" />
        </asp:TreeNode>
    </Nodes>
</asp:TreeView>