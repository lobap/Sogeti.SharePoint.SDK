<%@ Page Language="C#" Inherits="Contoso.TrainingManagement.Forms.RegistrationApproval, Contoso.TrainingManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5" MasterPageFile="~/_layouts/application.master" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
<SharePoint:EncodedLiteral ID="ListFormPageTitle" runat="server" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
<SharePoint:EncodedLiteral ID="LinkTitle" runat="server" EncodeMethod='HtmlEncode'/>: <SharePoint:EncodedLiteral ID="ItemProperty" runat="server" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Content4" contentplaceholderid="PlaceHolderPageImage" runat="server"/>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderPageDescriptionRowAttr" runat="server">
style="display:none;"
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderId="PlaceHolderPageDescriptionRowAttr2" runat="server">
style="display:none;"
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">
	<asp:Label id="LabelPageDescription" runat="server"/>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderId="PlaceHolderMain" runat="server">
	<style type="text/css">
	table.ms-propertysheet {
	height: 100%;
	}
	</style>
	<table cellpadding=0 cellspacing=0 id="onetIDListForm">
 <tr>
  <td>
      <asp:Label ID="ContentMessage" runat="server"></asp:Label>
      <asp:Panel ID="Confirmation" Visible="false" runat="server"> 
      <br />
          <asp:DropDownList ID="Status" runat="server">
          </asp:DropDownList> <asp:RequiredFieldValidator ID="StatusValidator" ControlToValidate="Status" Display="Dynamic" runat="server" ErrorMessage="(*) Required" />
      <br />     
      <br />
      <asp:Button ID="Submit" OnClick="Submit_Click" Text="Submit" runat="server" />
      <asp:Button ID="Cancel" UseSubmitBehavior="false" OnClientClick="history.back(); return false;" Text="Cancel" runat="server" />
      </asp:Panel>
  </td>
 </tr>
</table>
</asp:Content>