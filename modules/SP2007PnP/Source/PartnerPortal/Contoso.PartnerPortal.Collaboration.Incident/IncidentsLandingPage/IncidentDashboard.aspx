﻿<%@ Page language="C#" MasterPageFile="~masterurl/default.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="<%$Resources:wss,multipages_homelink_text%>" EncodeMethod="HtmlEncode"/> - <SharePoint:ProjectProperty ID="ProjectProperty1" Property="Title" runat="server"/> - Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageImage" runat="server"><IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt=""></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderSiteName" runat="server">
        <h1 class="ms-sitetitle">
		   <SharePoint:SPLinkButton runat="server" NavigateUrl="~site/" id="onetidProjectPropertyTitle">
			 <SharePoint:ProjectProperty ID="Title" Property="Title" runat="server" />
		   </SharePoint:SPLinkButton>
		  </h1>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
		 <label class="ms-hidden"><SharePoint:ProjectProperty ID="ProjectProperty2" Property="Title" runat="server"/></label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderTitleBreadcrumb" runat="server"/>
<asp:Content ID="Content6" ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
<style type="text/css">
TD.ms-titleareaframe, .ms-pagetitleareaframe {
	height: 10px;
}
Div.ms-titleareaframe {
	height: 100%;
}
.ms-pagetitleareaframe table {
	background: none;
	height: 10px;
}
</style>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	<META Name="CollaborationServer" Content="SharePoint Team Web Site">
	<script type="text/javascript">
	    var navBarHelpOverrideKey = "wssmain";
	</script>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderId="PlaceHolderSearchArea" runat="server">
	<SharePoint:DelegateControl ID="DelegateControl1" runat="server"
		ControlId="SmallSearchInputBox" />
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderId="PlaceHolderLeftActions" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderId="PlaceHolderPageDescription" runat="server"/>
<asp:Content ID="Content11" ContentPlaceHolderId="PlaceHolderBodyAreaClass" runat="server">
<style type="text/css">
.ms-bodyareaframe {
	padding: 0px;
}
</style>
</asp:Content>
<asp:Content ID="Content13" ContentPlaceHolderID="PlaceHolderLeftNavBarTop" runat="server">
</asp:Content>
<asp:Content ID="Content12" ContentPlaceHolderId="PlaceHolderMain" runat="server">
    <table cellspacing="0" border="0" width="100%">
	  <tr>
	   <td class="ms-pagebreadcrumb">
		<asp:SiteMapPath SiteMapProvider="SPContentMapProvider" id="ContentMap" SkipLinkText="" NodeStyle-CssClass="ms-sitemapdirectional" runat="server"/>
	   </td>
	  </tr>
	  <tr>
	   <td class="ms-webpartpagedescription"><SharePoint:ProjectProperty ID="ProjectProperty3" Property="Description" runat="server"/></td>
	  </tr>
	  <tr>
		<td>
		 <table width="100%" cellpadding=0 cellspacing=0 style="padding: 5px 10px 10px 10px;">
		  <tr>
		   <td valign="top" width="100%">
			   <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="Left" Title="loc:Left" />
			   &nbsp;
		   </td>
		   <td>&nbsp;</td>
		  </tr>
		 </table>
		</td>
	  </tr>
	</table>
</asp:Content>
