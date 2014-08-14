<%@ Page Language="C#" Inherits="Contoso.PartnerPortal.PartnerRedirectPage, Contoso.PartnerPortal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0561205e6cefaed4" MasterPageFile="~masterurl/default.master" %> 
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
        <h1>The system could not find your partner collaboration site.</h1>
      </td>
    </tr>
</table>
</asp:Content>