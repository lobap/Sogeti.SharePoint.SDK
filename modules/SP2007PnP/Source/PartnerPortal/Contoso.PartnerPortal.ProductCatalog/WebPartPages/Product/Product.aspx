<%@ Page Language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PortalWebControls" Namespace="Microsoft.SharePoint.Portal.WebControls" 
    Assembly="Microsoft.SharePoint.Portal, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<%@ Register TagPrefix="PromotionDiscounts" Namespace="Contoso.PartnerPortal.ProductCatalog.WebParts.Discounts"
    Assembly="Contoso.PartnerPortal.ProductCatalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0dcd9137292eac97" %>
<%@ Register TagPrefix="PromotionPricing" Namespace="Contoso.PartnerPortal.ProductCatalog.WebParts.Pricing" 
    Assembly="Contoso.PartnerPortal.ProductCatalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0dcd9137292eac97"%>
<%@ Register TagPrefix="ProductDetails" Namespace="Contoso.PartnerPortal.ProductCatalog.WebParts.ProductDetails"
    Assembly="Contoso.PartnerPortal.ProductCatalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0dcd9137292eac97" %>
<%@ Register TagPrefix="RelatedParts" Namespace="Contoso.PartnerPortal.ProductCatalog.WebParts.RelatedParts" 
    Assembly="Contoso.PartnerPortal.ProductCatalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0dcd9137292eac97"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" Text="<%$Resources:wss,multipages_homelink_text%>"
        EncodeMethod="HtmlEncode" />
    -
    <SharePoint:ProjectProperty ID="ProjectProperty1" Property="Title" runat="server" />
    - Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageImage" runat="server">
    <img src="/_layouts/images/blank.gif" width="1" height="1" alt=""></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderSiteName" runat="server">
    <h1 class="ms-sitetitle">
        <SharePoint:SPLinkButton runat="server" NavigateUrl="~site/" ID="onetidProjectPropertyTitle">
            <SharePoint:ProjectProperty ID="Title" Property="Title" runat="server" />
        </SharePoint:SPLinkButton>
    </h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    <label class="ms-hidden">
        <SharePoint:ProjectProperty ID="ProjectProperty2" Property="Title" runat="server" />
    </label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderTitleBreadcrumb" runat="server" />
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderTitleAreaClass" runat="server">
    <style type="text/css">
        TD.ms-titleareaframe, .ms-pagetitleareaframe
        {
            height: 10px;
        }
        Div.ms-titleareaframe
        {
            height: 100%;
        }
        .ms-pagetitleareaframe table
        {
            background: none;
            height: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <meta name="CollaborationServer" content="SharePoint Team Web Site">

    <script type="text/javascript">
        var navBarHelpOverrideKey = "wssmain";
    </script>

</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="PlaceHolderSearchArea" runat="server">
    <SharePoint:DelegateControl ID="DelegateControl1" runat="server" ControlId="SmallSearchInputBox" />
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PlaceHolderLeftActions" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server" />
<asp:Content ID="Content11" ContentPlaceHolderID="PlaceHolderBodyAreaClass" runat="server">
    <style type="text/css">
        .ms-bodyareaframe
        {
            padding: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content13" ContentPlaceHolderID="PlaceHolderLeftNavBarTop" runat="server">
</asp:Content>
<asp:Content ID="Content12" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <WebPartPages:SPProxyWebPartManager runat="server" ID="__ProxyWebPartManagerForConnections__">
        <SPWebPartConnections>
            <WebPartPages:SPWebPartConnection ConsumerConnectionPointID="IFilterValueConnection" 
                ConsumerID="ProductDetailsWebPart1" ID="c102772394" ProviderConnectionPointID="ITransformableFilterValues" 
                ProviderID="QueryStringFilterWebPart1">
                <WebPartPages:TransformableFilterValuesToFilterValuesTransformer MappedConsumerParameterName="Sku">
                </WebPartPages:TransformableFilterValuesToFilterValuesTransformer>
            </WebPartPages:SPWebPartConnection>
            <WebPartPages:SPWebPartConnection ConsumerID="PricingWebPart1" ID="c482581249" 
                ProviderConnectionPointID="ITransformableFilterValues" ProviderID="QueryStringFilterWebPart1">
                <WebPartPages:TransformableFilterValuesToFilterValuesTransformer MappedConsumerParameterName="Sku">
            </WebPartPages:TransformableFilterValuesToFilterValuesTransformer>
            </WebPartPages:SPWebPartConnection>
            <WebPartPages:SPWebPartConnection ConsumerID="DiscountsWebPart1" ID="c34931404" 
                ProviderConnectionPointID="ITransformableFilterValues" ProviderID="QueryStringFilterWebPart1">
                <WebPartPages:TransformableFilterValuesToFilterValuesTransformer MappedConsumerParameterName="Sku">
                </WebPartPages:TransformableFilterValuesToFilterValuesTransformer>
            </WebPartPages:SPWebPartConnection>
            <WebPartPages:SPWebPartConnection ConsumerConnectionPointID="IFilterValueConnection" 
                ConsumerID="RelatedPartsWebPart1" ID="c419284813" ProviderConnectionPointID="ITransformableFilterValues" 
                ProviderID="QueryStringFilterWebPart1">
                <WebPartPages:TransformableFilterValuesToFilterValuesTransformer MappedConsumerParameterName="Sku">
                </WebPartPages:TransformableFilterValuesToFilterValuesTransformer>
            </WebPartPages:SPWebPartConnection>
        </SPWebPartConnections>
    </WebPartPages:SPProxyWebPartManager>
    <table cellspacing="0" border="0" width="100%">
        <tr>
            <td class="ms-pagebreadcrumb">
                <asp:SiteMapPath SiteMapProvider="SPContentMapProvider" ID="ContentMap" SkipLinkText=""
                    NodeStyle-CssClass="ms-sitemapdirectional" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="ms-webpartpagedescription">
                <SharePoint:ProjectProperty ID="ProjectProperty3" Property="Description" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="0" style="padding: 5px 10px 10px 10px;">
                    <tr>
                        <td valign="top" width="100%">
                            <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="Left" Title="loc:Left">
                                <ZoneTemplate>
                                    <PortalWebControls:QueryStringFilterWebPart runat="server" CatalogIconImageUrl="/_layouts/images/wp_Filter.gif" 
                                        ZoneID="Left" QueryStringParameterName="Sku" AllowRemove="True" ConnectionID="00000000-0000-0000-0000-000000000000" 
                                        HelpMode="Modeless" DetailLink="" TitleIconImageUrl="/_layouts/images/wp_Filter.gif" 
                                        Description="Filter the contents of Web parts using values passed via the query string." 
                                        ChromeType="None" IsVisible="True" AllowHide="True" AllowZoneChange="True" PartImageSmall="/_layouts/images/wp_Filter.gif" 
                                        FrameType="None" AllowEdit="True" AllowMinimize="True" SendEmptyWhenNoValues="True" artImageLarge="/_layouts/images/wp_Filter.gif" 
                                        ExportControlledProperties="True" HelpLink="" ExportMode="All" Dir="Default" SuppressWebPartChrome="False" 
                                        ID="QueryStringFilterWebPart1" MissingAssembly="Cannot import this Web Part." IsIncludedFilter="" PartOrder="1" 
                                        FilterName="Sku Filter" IsIncluded="True" AllowConnect="True" FrameState="Normal" Title="Query String (URL) Filter" 
                                        __MarkupType="vsattributemarkup" __WebPartId="{ae45c634-8636-4f11-8c5c-07b82292aa16}" WebPart="true" 
                                        Height="" Width="">
                                    </PortalWebControls:QueryStringFilterWebPart>
                                    <ProductDetails:ProductDetailsWebPart runat="server" ID="ProductDetailsWebPart1" 
                                        Description="ProductDetailsWebPart Description" 
                                        Title="ProductDetailsWebPart Web Part" 
                                        ImportErrorMessage="Cannot import ProductDetailsWebPart Web Part." 
                                        __MarkupType="vsattributemarkup" __WebPartId="{2136a455-75ea-4c5d-bd6c-895df351ab2c}" 
                                        WebPart="true" __designer:IsClosed="false" partorder="2">
                                    </ProductDetails:ProductDetailsWebPart>
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>                            
                        </td>
                        <td valign="top">
                            <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="Right" Title="loc:Right" >
                                <ZoneTemplate>
                                    <PromotionPricing:PricingWebPart runat="server" ID="PricingWebPart1" 
                                        Description="Displays the price for a product." 
                                        Title="PricingWebPart Web Part" 
                                        ImportErrorMessage="Cannot import PricingWebPart Web Part." 
                                        __MarkupType="vsattributemarkup" 
                                        __WebPartId="{efff46a7-eeab-443a-b8eb-488e9bca9049}" WebPart="true" 
                                        __designer:IsClosed="false" partorder="1">
                                    </PromotionPricing:PricingWebPart>
                                    <PromotionDiscounts:DiscountsWebPart runat="server" ID="DiscountsWebPart1" 
                                        Description="Displays a list of applicable discounts for a product." 
                                        Title="Discounts Web Part" ImportErrorMessage="Cannot import Discounts Web Part." 
                                        __MarkupType="vsattributemarkup" __WebPartId="{626fbba3-f11f-4727-b431-77581eb4e6de}" 
                                        WebPart="true" __designer:IsClosed="false" partorder="2">
                                    </PromotionDiscounts:DiscountsWebPart>
                                    <RelatedParts:RelatedPartsWebPart runat="server" ID="RelatedPartsWebPart1" 
                                        Description="RelatedPartsWebPart Description" Title="RelatedPartsWebPart Web Part" 
                                        ImportErrorMessage="Cannot import RelatedPartsWebPart Web Part." 
                                        __MarkupType="vsattributemarkup" __WebPartId="{06b50b4d-6329-4d6c-baf5-c43ec4b14ec5}" 
                                        WebPart="true" __designer:IsClosed="false" partorder="3">
                                    </RelatedParts:RelatedPartsWebPart>
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
