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
    /// This test validate category id =0 page for the Extranet user
    /// Browse category page with category id =0
    /// verify Category detail webpart content
    /// verify category list webpart content
    /// verify product list webpart content
    /// </summary>
    /// 
    public class VerifyExtranetProductCatalog_0 : WebTest
    {
        string fbaURL = CustConfig.GetFBAURL;

        public VerifyExtranetProductCatalog_0()
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
            request1.ThinkTime = 18;
            request1.QueryStringParameters.Add("ReturnUrl", "%2f", false, false);
            ExtractHiddenFields extractionRule1 = new ExtractHiddenFields();
            extractionRule1.Required = true;
            extractionRule1.HtmlDecode = true;
            extractionRule1.ContextParameterName = "1";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule1.Extract);
            yield return request1;
            request1 = null;

            WebTestRequest request2 = new WebTestRequest(fbaURL + "/_layouts/login.aspx");
            request2.ThinkTime = 7;
            request2.Method = "POST";
            request2.ExpectedResponseUrl = fbaURL + "/sites/partner1/Pages/default.aspx";
            request2.QueryStringParameters.Add("ReturnUrl", "%2f", false, false);
            FormPostHttpBody request2Body = new FormPostHttpBody();
            request2Body.FormPostParameters.Add("__LASTFOCUS", this.Context["$HIDDEN1.__LASTFOCUS"].ToString());
            request2Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request2Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN1.__EVENTTARGET"].ToString());
            request2Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            request2Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request2Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$UserName", "contosopartner1user1");
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

            //Check for Root Category 
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule2 = new ValidationRuleFindText();
                validationRule2.FindText = "Root Category";
                validationRule2.IgnoreCase = true;
                validationRule2.UseRegularExpression = false;
                validationRule2.PassIfTextFound = true;
                request3.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }

            //Check for Category Details Webpart title
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule3 = new ValidationRuleFindText();
                validationRule3.FindText = "Category Details";
                validationRule3.IgnoreCase = true;
                validationRule3.UseRegularExpression = false;
                validationRule3.PassIfTextFound = true;
                request3.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule3.Validate);
            }

            //Check for Category List Webpart title
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule4 = new ValidationRuleFindText();
                validationRule4.FindText = "Category List";
                validationRule4.IgnoreCase = true;
                validationRule4.UseRegularExpression = false;
                validationRule4.PassIfTextFound = true;
                request3.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule4.Validate);
            }

            //Check for Product List Webpart title
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule5 = new ValidationRuleFindText();
                validationRule5.FindText = "Product List";
                validationRule5.IgnoreCase = true;
                validationRule5.UseRegularExpression = false;
                validationRule5.PassIfTextFound = true;
                request3.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule5.Validate);
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
            request4Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + CategoryDetailWebPartGuid.Replace("-", "_").ToString());
            request4Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request4Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request4.Body = request4Body;
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule6 = new ValidationRuleFindText();
                validationRule6.FindText = "CategoryId:";
                validationRule6.IgnoreCase = true;
                validationRule6.UseRegularExpression = false;
                validationRule6.PassIfTextFound = true;
                request4.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule6.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule7 = new ValidationRuleFindText();
                validationRule7.FindText = "Name:";
                validationRule7.IgnoreCase = true;
                validationRule7.UseRegularExpression = false;
                validationRule7.PassIfTextFound = true;
                request4.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule7.Validate);
            }
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
            request6Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + ProductListWebPartGuid.Replace("-", "_").ToString());
            request6Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request6Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN3.__EVENTVALIDATION"].ToString());
            request6.Body = request6Body;
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule8 = new ValidationRuleFindText();
                validationRule8.FindText = "There are no items to show.";
                validationRule8.IgnoreCase = true;
                validationRule8.UseRegularExpression = false;
                validationRule8.PassIfTextFound = true;
                request6.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule8.Validate);
            }
            yield return request6;
            request6 = null;
        }
    }
}
