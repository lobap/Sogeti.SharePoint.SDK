<%@ Page language="C#"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<HTML dir="<%$Resources:wss,multipages_direction_dir_value%>" runat="server">
<HEAD>
	<META Name="GENERATOR" Content="Microsoft SharePoint">
	<META Name="ProgId" Content="SharePoint.WebPartPage.Document">
	<META HTTP-EQUIV="Content-Type" CONTENT="text/html; charset=utf-8">
	<META HTTP-EQUIV="Expires" content="0">
	
	<Title ID=onetidTitle><SharePoint:ListProperty Property="Title" runat="server"/>: <SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss, slidshow_slideshowview%>" EncodeMethod='HtmlEncode'/></Title>
<SharePoint:CssLink runat="server"/>
	<!--mstheme--><SharePoint:Theme runat="server"/>
	<META Name="Microsoft Theme" Content="default">
	<META Name="Microsoft Border" Content="none">
<SharePoint:ScriptLink name="init.js" language="javascript" runat="server" />
<SharePoint:ScriptLink name="core.js" language="javascript" runat="server" />
<link type="text/xml" rel='alternate' href="_vti_bin/spdisco.aspx" />
</HEAD>
<base target="_blank">
<BODY marginwidth=0 marginheight=0 scroll="yes" OnLoad="javascript:ProcessPNGImages();">
	<TABLE class="ms-main" CELLPADDING=0 CELLSPACING=0 BORDER=0 WIDTH="100%" HEIGHT="100%">
	<form runat="server">
		<WebPartPages:SPWebPartManager ID="SPWebPartManager" runat="Server"/>
	<TR> <TD width=100%> <TABLE width=100% height="100%" cellspacing="0" cellpadding="0"> <tr> <td class="ms-titlearealeft" id="TitleAreaImageCell" valign="middle" nowrap> <div style="height:100%" class="ms-titleareaframe"><SharePoint:AlphaImage ID=onetIDPageIcon Src="/_layouts/images/plicon.png" Width=145 Height=54 Alt="" Runat="server" /></div> </td> <td width=100% valign=top id="onetidPageTitleAreaFrame" class='ms-pagetitleareaframe' nowrap> <table id="onetidPageTitleAreaTable" cellpadding=0 cellspacing=0 width=100% border="0"> <tr><td valign="top" class="ms-titlearea"><asp:SiteMapPath SiteMapProvider="SPContentMapProvider" id="ContentMap" SkipLinkText="" NodeStyle-CssClass="ms-sitemapdirectional" runat="server"/> &nbsp;</td></tr> <tr> <td height=100% valign=top ID=onetidPageTitle class="ms-pagetitle"> <h2 class="ms-pagetitle"><SharePoint:ListProperty Property="LinkTitle" runat="server"/></h2> </td> </tr> </table> </td> </tr> </table> </td> </tr>
	<TR height=100% id=STS_Content_Row style="display:none">
	<TD><IMG SRC="/_layouts/images/blank.gif" width=3 height=1 alt=""></TD> <TD width="100%" height="100%"><PlaceHolder id="MSO_ContentDiv" runat="server"> <table cellpadding=2 cellspacing=0><tr><td><IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt=""></td></tr></table><table cellpadding="4" cellspacing="0" border="0" class="ms-bodyareaframe" width="100%"><tr valign=top><td width="100%" style="padding-top:3px;padding-right:10px;">
	<WebPartPages:WebPartZone runat="server" FrameType="None" ID="Main" Title="loc:Main" />
	</td></tr></table></PlaceHolder></TD></TR>
	<TR height=100% id=SlideShow_Content_Row><TD colspan=2>
	<script language="Javascript">if (IsImgLibJssLoaded()) SlideShowContent();</script>
	</TD></TR>
	</form>
	</TABLE>
</BODY>
</HTML>
