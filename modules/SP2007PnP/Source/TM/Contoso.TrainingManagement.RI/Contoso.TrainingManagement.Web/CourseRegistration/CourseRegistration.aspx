<%@ Page language="C#" MasterPageFile="~masterurl/default.master" Inherits="Contoso.TrainingManagement.Forms.CourseRegistration, Contoso.TrainingManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
    <asp:Literal ID="ListFormPageTitle" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
    <asp:Label ID="LinkTitle" runat="server" />: <asp:Label ID="ItemProperty" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageImage" runat="server">
	<IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt="">
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderLeftNavBar" runat="server"/>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
<table cellpadding=0 cellspacing=0 id="onetIDListForm">
 <tr>
  <td>
      <asp:Label ID="MainContent" runat="server" />
      <br />
      <asp:Panel ID="CourseSelect" Visible="false" runat="server">
      <asp:DropDownList ID="CourseList" runat="server">
      </asp:DropDownList>
      </asp:Panel>
      <asp:Panel ID="Confirmation" Visible="false" runat="server"> 
      <br />     
      <asp:Button ID="Submit" OnClick="Submit_Click" Text="Register for Course" runat="server" />
      <asp:Button ID="Cancel" UseSubmitBehavior="false" OnClientClick="history.back(); return false;" Text="Cancel" runat="server" />
      </asp:Panel>
      <br />
      <asp:HyperLink ID="GoBackToSiteLink" Text="Go back to site" CssClass="ms-descriptiontext" runat="server"/>
  </td>
 </tr>
</table>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleLeftBorder" runat="server">
<table cellpadding=0 height=100% width=100% cellspacing=0>
 <tr><td class="ms-areaseparatorleft"><IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt=""></td></tr>
</table>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
<script id="onetidPageTitleAreaFrameScript">
	document.getElementById("onetidPageTitleAreaFrame").className="ms-areaseparator";
</script>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderBodyAreaClass" runat="server">
<style type="text/css">
.ms-bodyareaframe {
	padding: 8px;
	border: none;
}
</style>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderBodyLeftBorder" runat="server">
<div class='ms-areaseparatorleft'><IMG SRC="/_layouts/images/blank.gif" width=8 height=100% alt=""></div>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleRightMargin" runat="server">
<div class='ms-areaseparatorright'><IMG SRC="/_layouts/images/blank.gif" width=8 height=100% alt=""></div>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderBodyRightMargin" runat="server">
<div class='ms-areaseparatorright'><IMG SRC="/_layouts/images/blank.gif" width=8 height=100% alt=""></div>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleAreaSeparator" runat="server"/>
