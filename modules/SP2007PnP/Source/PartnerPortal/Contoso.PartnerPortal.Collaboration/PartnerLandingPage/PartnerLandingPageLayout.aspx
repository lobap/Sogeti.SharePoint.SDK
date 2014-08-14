<%@ Page Language="C#"  Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register TagPrefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls"
    Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation"
    Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PortalWebControls" Namespace="Microsoft.SharePoint.Portal.WebControls" 
    Assembly="Microsoft.SharePoint.Portal, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<%@ Register TagPrefix="PartnerPromotions" Namespace="Contoso.PartnerPortal.Promotions.WebParts.PartnerPromotions"
    Assembly="Contoso.PartnerPortal.Promotions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7dfe266eb414dba8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <SharePointWebControls:FieldValue ID="PageTitle" FieldName="Title" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="imageSplash">
		<PublishingWebControls:RichImageField id="ImageField" FieldName="PublishingPageImage" runat="server"/>
	</div>
    <table>
        <table width="100%">
        <tr>
            <td style="width:80%">
            	<table width="100%">
            	    <tr>
            	        <td>
            	            <h3>Contoso Company News</h3>
            	        </td>
            	    </tr>
            		<tr>
            			<td>
            				<PublishingWebControls:RichHtmlField ID="RichHtmlField1" FieldName="PublishingPageContent" runat="server"/>
            			</td>
            		</tr>
            		<tr>
            			<td>
            				<WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="Left" Title="loc:Left">
            				    <ZoneTemplate></ZoneTemplate>
            			    </WebPartPages:WebPartZone>
            			</td>
            		</tr>
            		<tr>
            	        <td>
            	            <h3>Partner News</h3>
            	        </td>
            	    </tr>
            		<tr>
            			<td>
            				<PublishingWebControls:RichHtmlField ID="RichHtmlField2" FieldName="PartnerPageContentField" runat="server"/>
            			</td>
            		</tr>
            	</table>
            </td>
            <td valign="top">
	            <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="Right" Title="loc:Right">
	                <ZoneTemplate>
				        <PartnerPromotions:PartnerPromotionsWebPart runat="server" Title="My Promotions" ID="g_0b861666_9c86_47be_8ba5_d9928cd00f9b" __MarkupType="vsattributemarkup" __WebPartId="{0B861666-9C86-47BE-8BA5-D9928CD00F9B}" WebPart="true" __designer:IsClosed="false" partorder="1"></PartnerPromotions:PartnerPromotionsWebPart>
                    </ZoneTemplate>
                </WebPartPages:WebPartZone>
            </td>
        </tr>
    </table>
</asp:Content>
