//===============================================================================
// Microsoft patterns & practices
// SharePoint Guidance version 2.0
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================


namespace PartnerPortalBVT
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;
    using PartnerPortalBVT.Settings;

    /// <summary>
    /// This test validate category id =4 details page in the Extranet port.
    /// Browse category page with category id =4
    /// verify Category detail webpart content
    /// verify category list webpart content
    /// verify product list webpart content
    /// verify the details in the productList web part in the Product catalog page with the category id = 15
    /// </summary>


    public class VerifyExtranetProductCatalog_4 : WebTest
    {
        string fbaURL = CustConfig.GetFBAURL;

        public VerifyExtranetProductCatalog_4()
        {
            this.PreAuthenticate = true;
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            // Initialize validation rules that apply to all requests in the WebTest
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidateResponseUrl validationRule1 = new ValidateResponseUrl();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            }

            WebTestRequest request1 = new WebTestRequest(fbaURL + "/_layouts/login.aspx");
            request1.ThinkTime = 16;
            request1.ExpectedResponseUrl = fbaURL + "/_layouts/login.aspx?ReturnUrl=%2f";
            ExtractHiddenFields extractionRule1 = new ExtractHiddenFields();
            extractionRule1.Required = true;
            extractionRule1.HtmlDecode = true;
            extractionRule1.ContextParameterName = "1";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule1.Extract);
            yield return request1;
            request1 = null;

            WebTestRequest request2 = new WebTestRequest(fbaURL + "/_layouts/login.aspx");
            request2.ThinkTime = 13;
            request2.Method = "POST";
            request2.ExpectedResponseUrl = fbaURL + "/sites/partner1/Pages/default.aspx";
            request2.QueryStringParameters.Add("ReturnUrl", "%2f", false, false);
            FormPostHttpBody request2Body = new FormPostHttpBody();
            request2Body.FormPostParameters.Add("__LASTFOCUS", this.Context["$HIDDEN1.__LASTFOCUS"].ToString());
            request2Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request2Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN1.__EVENTTARGET"].ToString());
            request2Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            request2Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request2Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$UserName", "ContosoPartner1User1");
            request2Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$password", "P2ssw0rd$");
            request2Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$login", "Sign In");
            request2Body.FormPostParameters.Add("__spDummyText1", "");
            request2Body.FormPostParameters.Add("__spDummyText2", "");
            request2.Body = request2Body;
            yield return request2;
            request2 = null;

            WebTestRequest request3 = new WebTestRequest(fbaURL + "/sites/productcatalog/category.aspx");
            request3.ThinkTime = 1;
            request3.QueryStringParameters.Add("categoryid", "0", false, false);

            //Check for Category Details Webpart title
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule2 = new ValidationRuleFindText();
                validationRule2.FindText = "Category Details";
                validationRule2.IgnoreCase = true;
                validationRule2.UseRegularExpression = false;
                validationRule2.PassIfTextFound = true;
                request3.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }

            //Check for Category List Webpart title
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule3 = new ValidationRuleFindText();
                validationRule3.FindText = "Category List";
                validationRule3.IgnoreCase = true;
                validationRule3.UseRegularExpression = false;
                validationRule3.PassIfTextFound = true;
                request3.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule3.Validate);
            }

            //Check for Product List Webpart title
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule4 = new ValidationRuleFindText();
                validationRule4.FindText = "Product List";
                validationRule4.IgnoreCase = true;
                validationRule4.UseRegularExpression = false;
                validationRule4.PassIfTextFound = true;
                request3.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule4.Validate);
            }
            ExtractHiddenFields extractionRule2 = new ExtractHiddenFields();
            extractionRule2.Required = true;
            extractionRule2.HtmlDecode = true;
            extractionRule2.ContextParameterName = "1";
            request3.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule2.Extract);
            ExtractHiddenFields extractionRule3 = new ExtractHiddenFields();
            extractionRule3.Required = true;
            extractionRule3.HtmlDecode = true;
            extractionRule3.ContextParameterName = "2";
            request3.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule3.Extract);
            ExtractHiddenFields extractionRule4 = new ExtractHiddenFields();
            extractionRule4.Required = true;
            extractionRule4.HtmlDecode = true;
            extractionRule4.ContextParameterName = "3";
            request3.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule4.Extract);

            //Extract CategoryDetail WebPart guid
            ExtractAttributeValue extractionRule5 = new ExtractAttributeValue();
            extractionRule5.TagName = "div";
            extractionRule5.AttributeName = "WebPartID";
            extractionRule5.MatchAttributeName = "id";
            extractionRule5.MatchAttributeValue = "WebPartWPQ3";
            extractionRule5.HtmlDecode = true;
            extractionRule5.Required = true;
            extractionRule5.Index = 0;
            extractionRule5.ContextParameterName = "";
            extractionRule5.ContextParameterName = "CategoryDetailWebPartGuid";
            request3.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule5.Extract);

            //Extract CategoryList WebPart guid
            ExtractAttributeValue extractionRule6 = new ExtractAttributeValue();
            extractionRule6.TagName = "div";
            extractionRule6.AttributeName = "WebPartID";
            extractionRule6.MatchAttributeName = "id";
            extractionRule6.MatchAttributeValue = "WebPartWPQ4";
            extractionRule6.HtmlDecode = true;
            extractionRule6.Required = true;
            extractionRule6.Index = 0;
            extractionRule6.ContextParameterName = "";
            extractionRule6.ContextParameterName = "CategoryListWebPartGuid";
            request3.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule6.Extract);

            //Extract ProductList WebPart guid
            ExtractAttributeValue extractionRule7 = new ExtractAttributeValue();
            extractionRule7.TagName = "div";
            extractionRule7.AttributeName = "WebPartID";
            extractionRule7.MatchAttributeName = "id";
            extractionRule7.MatchAttributeValue = "WebPartWPQ5";
            extractionRule7.HtmlDecode = true;
            extractionRule7.Required = true;
            extractionRule7.Index = 0;
            extractionRule7.ContextParameterName = "";
            extractionRule7.ContextParameterName = "ProductListWebPartGuid";
            request3.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule7.Extract);
            yield return request3;

            string CategoryDetailWebPartGuid = Context["CategoryDetailWebPartGuid"].ToString();
            string CategoryListWebPartGuid = Context["CategoryListWebPartGuid"].ToString();
            string ProductListWebPartGuid = Context["ProductListWebPartGuid"].ToString();


            request3 = null;

            WebTestRequest request4 = new WebTestRequest(fbaURL + "/sites/productcatalog/category.aspx");
            request4.Method = "POST";
            request4.QueryStringParameters.Add("categoryid", "0", false, false);
            FormPostHttpBody request4Body = new FormPostHttpBody();
            request4Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN1.MSO_PageHashCode"].ToString());
            request4Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN1.MSOWebPartPage_PostbackSource"].ToString());
            request4Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN1.MSOTlPn_SelectedWpId"].ToString());
            request4Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN1.MSOTlPn_View"].ToString());
            request4Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN1.MSOTlPn_ShowSettings"].ToString());
            request4Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN1.MSOGallery_SelectedLibrary"].ToString());
            request4Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN1.MSOGallery_FilterString"].ToString());
            request4Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN1.MSOTlPn_Button"].ToString());
            request4Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN1.__EVENTTARGET"].ToString());
            request4Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            request4Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN1.__REQUESTDIGEST"].ToString());
            request4Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_DisplayModeName"].ToString());
            request4Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN1.MSOWebPartPage_Shared"].ToString());
            request4Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN1.MSOLayout_LayoutChanges"].ToString());
            request4Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN1.MSOLayout_InDesignMode"].ToString());
            request4Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request4Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN1.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request4Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN1.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request4Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request4Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$ctl00", this.Context["$HIDDEN1.ctl00$PlaceHolderSearchArea$ctl00$ctl00"].ToString());
            request4Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$SBScopesDDL", "This Site");
            request4Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$S3031AEBA_InputKeywords", "");
            request4Body.FormPostParameters.Add("showhelp", "Show Help");
            request4Body.FormPostParameters.Add("__spDummyText1", "");
            request4Body.FormPostParameters.Add("__spDummyText2", "");
            request4Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + ProductListWebPartGuid.Replace("-", "_").ToString());
            request4Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request4Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request4.Body = request4Body;
            yield return request4;
            request4 = null;

            WebTestRequest request5 = new WebTestRequest(fbaURL + "/sites/productcatalog/category.aspx");
            request5.Method = "POST";
            request5.QueryStringParameters.Add("categoryid", "0", false, false);
            FormPostHttpBody request5Body = new FormPostHttpBody();
            request5Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN2.MSO_PageHashCode"].ToString());
            request5Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN2.MSOWebPartPage_PostbackSource"].ToString());
            request5Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN2.MSOTlPn_SelectedWpId"].ToString());
            request5Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN2.MSOTlPn_View"].ToString());
            request5Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN2.MSOTlPn_ShowSettings"].ToString());
            request5Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN2.MSOGallery_SelectedLibrary"].ToString());
            request5Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN2.MSOGallery_FilterString"].ToString());
            request5Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN2.MSOTlPn_Button"].ToString());
            request5Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN2.__EVENTTARGET"].ToString());
            request5Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN2.__EVENTARGUMENT"].ToString());
            request5Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN2.__REQUESTDIGEST"].ToString());
            request5Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN2.MSOSPWebPartManager_DisplayModeName"].ToString());
            request5Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN2.MSOWebPartPage_Shared"].ToString());
            request5Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN2.MSOLayout_LayoutChanges"].ToString());
            request5Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN2.MSOLayout_InDesignMode"].ToString());
            request5Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN2.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request5Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN2.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request5Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN2.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request5Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN2.__VIEWSTATE"].ToString());
            request5Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$ctl00", this.Context["$HIDDEN2.ctl00$PlaceHolderSearchArea$ctl00$ctl00"].ToString());
            request5Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$SBScopesDDL", "This Site");
            request5Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$S3031AEBA_InputKeywords", "");
            request5Body.FormPostParameters.Add("showhelp", "Show Help");
            request5Body.FormPostParameters.Add("__spDummyText1", "");
            request5Body.FormPostParameters.Add("__spDummyText2", "");
            request5Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + CategoryListWebPartGuid.Replace("-", "_").ToString());
            request5Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request5Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN2.__EVENTVALIDATION"].ToString());
            request5.Body = request5Body;
            yield return request5;
            request5 = null;

            WebTestRequest request6 = new WebTestRequest(fbaURL + "/sites/productcatalog/category.aspx");
            request6.ThinkTime = 10;
            request6.Method = "POST";
            request6.QueryStringParameters.Add("categoryid", "0", false, false);
            FormPostHttpBody request6Body = new FormPostHttpBody();
            request6Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN3.MSO_PageHashCode"].ToString());
            request6Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN3.MSOWebPartPage_PostbackSource"].ToString());
            request6Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN3.MSOTlPn_SelectedWpId"].ToString());
            request6Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN3.MSOTlPn_View"].ToString());
            request6Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN3.MSOTlPn_ShowSettings"].ToString());
            request6Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN3.MSOGallery_SelectedLibrary"].ToString());
            request6Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN3.MSOGallery_FilterString"].ToString());
            request6Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN3.MSOTlPn_Button"].ToString());
            request6Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN3.__EVENTTARGET"].ToString());
            request6Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN3.__EVENTARGUMENT"].ToString());
            request6Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN3.__REQUESTDIGEST"].ToString());
            request6Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN3.MSOSPWebPartManager_DisplayModeName"].ToString());
            request6Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN3.MSOWebPartPage_Shared"].ToString());
            request6Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN3.MSOLayout_LayoutChanges"].ToString());
            request6Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN3.MSOLayout_InDesignMode"].ToString());
            request6Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN3.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request6Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN3.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request6Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN3.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request6Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN3.__VIEWSTATE"].ToString());
            request6Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$ctl00", this.Context["$HIDDEN3.ctl00$PlaceHolderSearchArea$ctl00$ctl00"].ToString());
            request6Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$SBScopesDDL", "This Site");
            request6Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$S3031AEBA_InputKeywords", "");
            request6Body.FormPostParameters.Add("showhelp", "Show Help");
            request6Body.FormPostParameters.Add("__spDummyText1", "");
            request6Body.FormPostParameters.Add("__spDummyText2", "");
            request6Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + CategoryDetailWebPartGuid.Replace("-", "_").ToString());
            request6Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request6Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN3.__EVENTVALIDATION"].ToString());
            request6.Body = request6Body;
            yield return request6;
            request6 = null;

            WebTestRequest request7 = new WebTestRequest(fbaURL + "/sites/productcatalog/_layouts/ProfileRedirect.aspx");
            request7.ExpectedResponseUrl = fbaURL + "/sites/productcatalog/Category.aspx?CategoryId=4";
            request7.QueryStringParameters.Add("Application", "ContosoProductCatalogService", false, false);
            request7.QueryStringParameters.Add("Entity", "Category", false, false);
            request7.QueryStringParameters.Add("ItemId", "__bk40004300", false, false);
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule5 = new ValidationRuleFindText();
                validationRule5.FindText = @"<span><a class=""ms-sitemapdirectional"" href=""/sites/productcatalog/Category.aspx?CategoryId=0"">Root Category</a></span><span> &gt; </span><span class=""ms-sitemapdirectional"">Dental Equipment</span><a id=""ctl00_PlaceHolderMain_SiteMapPath1_SkipLink""></a></span>";
                validationRule5.IgnoreCase = true;
                validationRule5.UseRegularExpression = false;
                validationRule5.PassIfTextFound = true;
                request7.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule5.Validate);
            }
            ExtractHiddenFields extractionRule8 = new ExtractHiddenFields();
            extractionRule8.Required = true;
            extractionRule8.HtmlDecode = true;
            extractionRule8.ContextParameterName = "1";
            request7.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule8.Extract);
            ExtractHiddenFields extractionRule9 = new ExtractHiddenFields();
            extractionRule9.Required = true;
            extractionRule9.HtmlDecode = true;
            extractionRule9.ContextParameterName = "0";
            request7.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule9.Extract);
            ExtractHiddenFields extractionRule10 = new ExtractHiddenFields();
            extractionRule10.Required = true;
            extractionRule10.HtmlDecode = true;
            extractionRule10.ContextParameterName = "2";
            request7.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule10.Extract);
            yield return request7;
            request7 = null;

            WebTestRequest request8 = new WebTestRequest(fbaURL + "/sites/productcatalog/Category.aspx");
            request8.Method = "POST";
            request8.QueryStringParameters.Add("CategoryId", "4", false, false);
            FormPostHttpBody request8Body = new FormPostHttpBody();
            request8Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN1.MSO_PageHashCode"].ToString());
            request8Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN1.MSOWebPartPage_PostbackSource"].ToString());
            request8Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN1.MSOTlPn_SelectedWpId"].ToString());
            request8Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN1.MSOTlPn_View"].ToString());
            request8Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN1.MSOTlPn_ShowSettings"].ToString());
            request8Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN1.MSOGallery_SelectedLibrary"].ToString());
            request8Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN1.MSOGallery_FilterString"].ToString());
            request8Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN1.MSOTlPn_Button"].ToString());
            request8Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN1.__EVENTTARGET"].ToString());
            request8Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            request8Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN1.__REQUESTDIGEST"].ToString());
            request8Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_DisplayModeName"].ToString());
            request8Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN1.MSOWebPartPage_Shared"].ToString());
            request8Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN1.MSOLayout_LayoutChanges"].ToString());
            request8Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN1.MSOLayout_InDesignMode"].ToString());
            request8Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request8Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN1.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request8Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN1.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request8Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request8Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$ctl00", this.Context["$HIDDEN1.ctl00$PlaceHolderSearchArea$ctl00$ctl00"].ToString());
            request8Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$SBScopesDDL", "This Site");
            request8Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$S3031AEBA_InputKeywords", "");
            request8Body.FormPostParameters.Add("showhelp", "Show Help");
            request8Body.FormPostParameters.Add("__spDummyText1", "");
            request8Body.FormPostParameters.Add("__spDummyText2", "");
            request8Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + ProductListWebPartGuid.Replace("-", "_").ToString());
            request8Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request8Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request8.Body = request8Body;
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule6 = new ValidationRuleFindText();
                validationRule6.FindText = "There are no items to show.";
                validationRule6.IgnoreCase = true;
                validationRule6.UseRegularExpression = false;
                validationRule6.PassIfTextFound = true;
                request8.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule6.Validate);
            }
            yield return request8;
            request8 = null;

            WebTestRequest request9 = new WebTestRequest(fbaURL + "/sites/productcatalog/Category.aspx");
            request9.Method = "POST";
            request9.QueryStringParameters.Add("CategoryId", "4", false, false);
            FormPostHttpBody request9Body = new FormPostHttpBody();
            request9Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN0.MSO_PageHashCode"].ToString());
            request9Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN0.MSOWebPartPage_PostbackSource"].ToString());
            request9Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN0.MSOTlPn_SelectedWpId"].ToString());
            request9Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN0.MSOTlPn_View"].ToString());
            request9Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN0.MSOTlPn_ShowSettings"].ToString());
            request9Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN0.MSOGallery_SelectedLibrary"].ToString());
            request9Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN0.MSOGallery_FilterString"].ToString());
            request9Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN0.MSOTlPn_Button"].ToString());
            request9Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN0.__EVENTTARGET"].ToString());
            request9Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN0.__EVENTARGUMENT"].ToString());
            request9Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN0.__REQUESTDIGEST"].ToString());
            request9Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN0.MSOSPWebPartManager_DisplayModeName"].ToString());
            request9Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN0.MSOWebPartPage_Shared"].ToString());
            request9Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN0.MSOLayout_LayoutChanges"].ToString());
            request9Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN0.MSOLayout_InDesignMode"].ToString());
            request9Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN0.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request9Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN0.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request9Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN0.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request9Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN0.__VIEWSTATE"].ToString());
            request9Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$ctl00", this.Context["$HIDDEN0.ctl00$PlaceHolderSearchArea$ctl00$ctl00"].ToString());
            request9Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$SBScopesDDL", "This Site");
            request9Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$S3031AEBA_InputKeywords", "");
            request9Body.FormPostParameters.Add("showhelp", "Show Help");
            request9Body.FormPostParameters.Add("__spDummyText1", "");
            request9Body.FormPostParameters.Add("__spDummyText2", "");
            request9Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + CategoryListWebPartGuid.Replace("-", "_").ToString());
            request9Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request9Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN0.__EVENTVALIDATION"].ToString());
            request9.Body = request9Body;
            yield return request9;
            request9 = null;

            WebTestRequest request10 = new WebTestRequest(fbaURL + "/sites/productcatalog/Category.aspx");
            request10.ThinkTime = 17;
            request10.Method = "POST";
            request10.QueryStringParameters.Add("CategoryId", "4", false, false);
            FormPostHttpBody request10Body = new FormPostHttpBody();
            request10Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN2.MSO_PageHashCode"].ToString());
            request10Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN2.MSOWebPartPage_PostbackSource"].ToString());
            request10Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN2.MSOTlPn_SelectedWpId"].ToString());
            request10Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN2.MSOTlPn_View"].ToString());
            request10Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN2.MSOTlPn_ShowSettings"].ToString());
            request10Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN2.MSOGallery_SelectedLibrary"].ToString());
            request10Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN2.MSOGallery_FilterString"].ToString());
            request10Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN2.MSOTlPn_Button"].ToString());
            request10Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN2.__EVENTTARGET"].ToString());
            request10Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN2.__EVENTARGUMENT"].ToString());
            request10Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN2.__REQUESTDIGEST"].ToString());
            request10Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN2.MSOSPWebPartManager_DisplayModeName"].ToString());
            request10Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN2.MSOWebPartPage_Shared"].ToString());
            request10Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN2.MSOLayout_LayoutChanges"].ToString());
            request10Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN2.MSOLayout_InDesignMode"].ToString());
            request10Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN2.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request10Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN2.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request10Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN2.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request10Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN2.__VIEWSTATE"].ToString());
            request10Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$ctl00", this.Context["$HIDDEN2.ctl00$PlaceHolderSearchArea$ctl00$ctl00"].ToString());
            request10Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$SBScopesDDL", "This Site");
            request10Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$S3031AEBA_InputKeywords", "");
            request10Body.FormPostParameters.Add("showhelp", "Show Help");
            request10Body.FormPostParameters.Add("__spDummyText1", "");
            request10Body.FormPostParameters.Add("__spDummyText2", "");
            request10Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + CategoryDetailWebPartGuid.Replace("-", "_").ToString());
            request10Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request10Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN2.__EVENTVALIDATION"].ToString());
            request10.Body = request10Body;
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule7 = new ValidationRuleFindText();
                validationRule7.FindText = @"<nobr>CategoryId:</nobr></td><td class=""ms-descriptiontext ms-alignleft"" width=""100%"">4</td>";
                validationRule7.IgnoreCase = true;
                validationRule7.UseRegularExpression = false;
                validationRule7.PassIfTextFound = true;
                request10.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule7.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule8 = new ValidationRuleFindText();
                validationRule8.FindText = @"<nobr>Name:</nobr></td><td class=""ms-descriptiontext ms-alignleft"" width=""100%"">Dental Equipment</td>";
                validationRule8.IgnoreCase = true;
                validationRule8.UseRegularExpression = false;
                validationRule8.PassIfTextFound = true;
                request10.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule8.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule9 = new ValidationRuleFindText();
                validationRule9.FindText = @"<nobr>ParentId:</nobr></td><td class=""ms-descriptiontext ms-alignleft"" width=""100%"">0</td>";
                validationRule9.IgnoreCase = true;
                validationRule9.UseRegularExpression = false;
                validationRule9.PassIfTextFound = true;
                request10.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule9.Validate);
            }
            yield return request10;
            request10 = null;

            WebTestRequest request11 = new WebTestRequest(fbaURL + "/sites/productcatalog/_layouts/ProfileRedirect.aspx");
            request11.ExpectedResponseUrl = fbaURL + "/sites/productcatalog/Category.aspx?CategoryId=15";
            request11.QueryStringParameters.Add("Application", "ContosoProductCatalogService", false, false);
            request11.QueryStringParameters.Add("Entity", "Category", false, false);
            request11.QueryStringParameters.Add("ItemId", "__bk800013005300", false, false);
            ExtractHiddenFields extractionRule11 = new ExtractHiddenFields();
            extractionRule11.Required = true;
            extractionRule11.HtmlDecode = true;
            extractionRule11.ContextParameterName = "1";
            request11.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule11.Extract);
            ExtractHiddenFields extractionRule12 = new ExtractHiddenFields();
            extractionRule12.Required = true;
            extractionRule12.HtmlDecode = true;
            extractionRule12.ContextParameterName = "0";
            request11.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule12.Extract);
            ExtractHiddenFields extractionRule13 = new ExtractHiddenFields();
            extractionRule13.Required = true;
            extractionRule13.HtmlDecode = true;
            extractionRule13.ContextParameterName = "2";
            request11.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule13.Extract);
            yield return request11;
            request11 = null;

            WebTestRequest request12 = new WebTestRequest(fbaURL + "/sites/productcatalog/Category.aspx");
            request12.Method = "POST";
            request12.QueryStringParameters.Add("CategoryId", "15", false, false);
            FormPostHttpBody request12Body = new FormPostHttpBody();
            request12Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN1.MSO_PageHashCode"].ToString());
            request12Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN1.MSOWebPartPage_PostbackSource"].ToString());
            request12Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN1.MSOTlPn_SelectedWpId"].ToString());
            request12Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN1.MSOTlPn_View"].ToString());
            request12Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN1.MSOTlPn_ShowSettings"].ToString());
            request12Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN1.MSOGallery_SelectedLibrary"].ToString());
            request12Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN1.MSOGallery_FilterString"].ToString());
            request12Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN1.MSOTlPn_Button"].ToString());
            request12Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN1.__EVENTTARGET"].ToString());
            request12Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            request12Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN1.__REQUESTDIGEST"].ToString());
            request12Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_DisplayModeName"].ToString());
            request12Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN1.MSOWebPartPage_Shared"].ToString());
            request12Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN1.MSOLayout_LayoutChanges"].ToString());
            request12Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN1.MSOLayout_InDesignMode"].ToString());
            request12Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request12Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN1.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request12Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN1.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request12Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request12Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$ctl00", this.Context["$HIDDEN1.ctl00$PlaceHolderSearchArea$ctl00$ctl00"].ToString());
            request12Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$SBScopesDDL", "This Site");
            request12Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$S3031AEBA_InputKeywords", "");
            request12Body.FormPostParameters.Add("showhelp", "Show Help");
            request12Body.FormPostParameters.Add("__spDummyText1", "");
            request12Body.FormPostParameters.Add("__spDummyText2", "");
            request12Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + ProductListWebPartGuid.Replace("-", "_").ToString());
            request12Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request12Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request12.Body = request12Body;
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule10 = new ValidationRuleFindText();
                validationRule10.FindText = "X-ray machine";
                validationRule10.IgnoreCase = true;
                validationRule10.UseRegularExpression = false;
                validationRule10.PassIfTextFound = true;
                request12.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule10.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule11 = new ValidationRuleFindText();
                validationRule11.FindText = "6000000000";
                validationRule11.IgnoreCase = true;
                validationRule11.UseRegularExpression = false;
                validationRule11.PassIfTextFound = true;
                request12.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule11.Validate);
            }
            yield return request12;
            request12 = null;

            WebTestRequest request13 = new WebTestRequest(fbaURL + "/sites/productcatalog/Category.aspx");
            request13.Method = "POST";
            request13.QueryStringParameters.Add("CategoryId", "15", false, false);
            FormPostHttpBody request13Body = new FormPostHttpBody();
            request13Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN0.MSO_PageHashCode"].ToString());
            request13Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN0.MSOWebPartPage_PostbackSource"].ToString());
            request13Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN0.MSOTlPn_SelectedWpId"].ToString());
            request13Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN0.MSOTlPn_View"].ToString());
            request13Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN0.MSOTlPn_ShowSettings"].ToString());
            request13Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN0.MSOGallery_SelectedLibrary"].ToString());
            request13Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN0.MSOGallery_FilterString"].ToString());
            request13Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN0.MSOTlPn_Button"].ToString());
            request13Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN0.__EVENTTARGET"].ToString());
            request13Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN0.__EVENTARGUMENT"].ToString());
            request13Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN0.__REQUESTDIGEST"].ToString());
            request13Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN0.MSOSPWebPartManager_DisplayModeName"].ToString());
            request13Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN0.MSOWebPartPage_Shared"].ToString());
            request13Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN0.MSOLayout_LayoutChanges"].ToString());
            request13Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN0.MSOLayout_InDesignMode"].ToString());
            request13Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN0.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request13Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN0.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request13Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN0.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request13Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN0.__VIEWSTATE"].ToString());
            request13Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$ctl00", this.Context["$HIDDEN0.ctl00$PlaceHolderSearchArea$ctl00$ctl00"].ToString());
            request13Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$SBScopesDDL", "This Site");
            request13Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$S3031AEBA_InputKeywords", "");
            request13Body.FormPostParameters.Add("showhelp", "Show Help");
            request13Body.FormPostParameters.Add("__spDummyText1", "");
            request13Body.FormPostParameters.Add("__spDummyText2", "");
            request13Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + CategoryListWebPartGuid.Replace("-", "_").ToString());
            request13Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request13Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN0.__EVENTVALIDATION"].ToString());
            request13.Body = request13Body;
            yield return request13;
            request13 = null;

            WebTestRequest request14 = new WebTestRequest(fbaURL + "/sites/productcatalog/Category.aspx");
            request14.Method = "POST";
            request14.QueryStringParameters.Add("CategoryId", "15", false, false);
            FormPostHttpBody request14Body = new FormPostHttpBody();
            request14Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN2.MSO_PageHashCode"].ToString());
            request14Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN2.MSOWebPartPage_PostbackSource"].ToString());
            request14Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN2.MSOTlPn_SelectedWpId"].ToString());
            request14Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN2.MSOTlPn_View"].ToString());
            request14Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN2.MSOTlPn_ShowSettings"].ToString());
            request14Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN2.MSOGallery_SelectedLibrary"].ToString());
            request14Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN2.MSOGallery_FilterString"].ToString());
            request14Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN2.MSOTlPn_Button"].ToString());
            request14Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN2.__EVENTTARGET"].ToString());
            request14Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN2.__EVENTARGUMENT"].ToString());
            request14Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN2.__REQUESTDIGEST"].ToString());
            request14Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN2.MSOSPWebPartManager_DisplayModeName"].ToString());
            request14Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN2.MSOWebPartPage_Shared"].ToString());
            request14Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN2.MSOLayout_LayoutChanges"].ToString());
            request14Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN2.MSOLayout_InDesignMode"].ToString());
            request14Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN2.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request14Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN2.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request14Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN2.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request14Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN2.__VIEWSTATE"].ToString());
            request14Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$ctl00", this.Context["$HIDDEN2.ctl00$PlaceHolderSearchArea$ctl00$ctl00"].ToString());
            request14Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$SBScopesDDL", "This Site");
            request14Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl00$S3031AEBA_InputKeywords", "");
            request14Body.FormPostParameters.Add("showhelp", "Show Help");
            request14Body.FormPostParameters.Add("__spDummyText1", "");
            request14Body.FormPostParameters.Add("__spDummyText2", "");
            request14Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + CategoryDetailWebPartGuid.Replace("-", "_").ToString());
            request14Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request14Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN2.__EVENTVALIDATION"].ToString());
            request14.Body = request14Body;
            yield return request14;
            request14 = null;
        }
    }
}
