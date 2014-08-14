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
<%@ Register TagPrefix="PromotionDiscounts" Namespace="Contoso.PartnerPortal.ProductCatalog.WebParts.Discounts"
    Assembly="Contoso.PartnerPortal.ProductCatalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0dcd9137292eac97" %>
<%@ Register TagPrefix="PromotionPricing" Namespace="Contoso.PartnerPortal.ProductCatalog.WebParts.Pricing" 
    Assembly="Contoso.PartnerPortal.ProductCatalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0dcd9137292eac97"%>
<%@ Register TagPrefix="ProductSku" Namespace="Contoso.PartnerPortal.Promotions.FieldControls"
    Assembly="Contoso.PartnerPortal.Promotions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7dfe266eb414dba8" %>
<%@ Register TagPrefix="ProductDetails" Namespace="Contoso.PartnerPortal.ProductCatalog.WebParts.ProductDetails"
    Assembly="Contoso.PartnerPortal.ProductCatalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0dcd9137292eac97" %>
<%@ Register TagPrefix="PortalWebControls" Namespace="Microsoft.SharePoint.Portal.WebControls" 
    Assembly="Microsoft.SharePoint.Portal, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<%@ Register TagPrefix="WindowsMediaFieldControl" Namespace="Contoso.PartnerPortal.Promotions.FieldControls"
    Assembly="Contoso.PartnerPortal.Promotions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7dfe266eb414dba8" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <SharePointWebControls:FieldValue ID="PageTitle" FieldName="Title" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table>
        <tr>
            <td colspan="2">
	            <PublishingWebControls:EditModePanel runat="server" ID="EditModePanel1" Width="100%">
	                <ProductSku:ProductSku ID="ProductSku" FieldName="ProductSkuField" runat="server">
					</ProductSku:ProductSku>
                </PublishingWebControls:EditModePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <PublishingWebControls:RichHtmlField ID="RichHtmlField1" FieldName="PromotionNameField"
                    runat="server" />
            </td>
        </tr>
        <tr valign="top">
            <td>
                <PublishingWebControls:RichImageField ID="RichImageField1" FieldName="PromotionImageField"
                    runat="server" />
            </td>
            <td>
                <PublishingWebControls:RichHtmlField ID="RichHtmlField3" FieldName="PromotionDescriptionField"
                    runat="server" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <WebPartPages:WebPartZone ID="Left" runat="server">
                    <ZoneTemplate>
                        <ProductDetails:ProductDetailsWebPart runat="server" ID="ProductDetailsWebPart"
                            Description="Product Details"
                            Title="Product Details"
                            partorder="1" />
                    </ZoneTemplate>
                </WebPartPages:WebPartZone>
            </td>
            <td valign=top>
                <WebPartPages:WebPartZone ID="Right" runat="server">
                    <ZoneTemplate>
                        <PromotionDiscounts:DiscountsWebPart runat="server" ID="PromotionDiscountsWebPart"
                            Description="Promotion Discounts" Title="Promotion Discounts" 
                            ImportErrorMessage="Cannot import ProductDetailsWebPart Web Part."
                            partorder="2" />
                        <PromotionPricing:PricingWebPart runat="server" ID="PromotionPricingWebPart" 
                            Description="Displays the price for a product." Title="Product Pricing" 
                            ImportErrorMessage="Cannot import PricingWebPart Web Part." 
                            partorder="3" />
                    </ZoneTemplate>
                </WebPartPages:WebPartZone>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <WindowsMediaFieldControl:WindowsMediaFieldControl  ID="WindowsMediaField" FieldName="WindowsMediaField" runat="server"/>
            </td>
        </tr>
    </table>
</asp:Content>
