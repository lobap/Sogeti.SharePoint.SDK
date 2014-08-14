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
    /// This Test verify the text in the Collapsable control in the ExtranetProductCatalogPage page
    /// </summary>



    public class VerifyCollapsableControlExtranetProductCatalog : WebTest
    {

        string fbaURL = CustConfig.GetFBAURL;
        string fbaUserName = CustConfig.Partner1FBAUserName;
        string fbaUserPassword = CustConfig.FBAUserPassword;

        public VerifyCollapsableControlExtranetProductCatalog()
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
            request1.ThinkTime = 28;
            request1.QueryStringParameters.Add("ReturnUrl", "%2f", false, false);
            ExtractHiddenFields extractionRule1 = new ExtractHiddenFields();
            extractionRule1.Required = true;
            extractionRule1.HtmlDecode = true;
            extractionRule1.ContextParameterName = "1";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule1.Extract);
            yield return request1;
            request1 = null;

            WebTestRequest request2 = new WebTestRequest(fbaURL + "/_layouts/login.aspx");
            request2.ThinkTime = 12;
            request2.Method = "POST";
            request2.ExpectedResponseUrl = fbaURL + "/sites/partner1/Pages/default.aspx";
            request2.QueryStringParameters.Add("ReturnUrl", "%2f", false, false);
            FormPostHttpBody request2Body = new FormPostHttpBody();
            request2Body.FormPostParameters.Add("__LASTFOCUS", this.Context["$HIDDEN1.__LASTFOCUS"].ToString());
            request2Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request2Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN1.__EVENTTARGET"].ToString());
            request2Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            request2Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request2Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$UserName", fbaUserName);
            request2Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$password", fbaUserPassword);
            request2Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$login", "Sign In");
            request2Body.FormPostParameters.Add("__spDummyText1", "");
            request2Body.FormPostParameters.Add("__spDummyText2", "");
            request2.Body = request2Body;
            yield return request2;
            request2 = null;

            WebTestRequest request3 = new WebTestRequest(fbaURL + "/sites/productcatalog/category.aspx");
            request3.ThinkTime = 1;
            request3.QueryStringParameters.Add("categoryid", "0", false, false);
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule2 = new ValidationRuleFindText();
                validationRule2.FindText = "This page is the Business Data Catalog (BDC) profile page for the Catalog entity.";
                validationRule2.IgnoreCase = true;
                validationRule2.UseRegularExpression = false;
                validationRule2.PassIfTextFound = true;
                request3.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule3 = new ValidationRuleFindText();
                validationRule3.FindText = "The top of this page shows custom navigation for the hierarchy of product categories.";
                validationRule3.IgnoreCase = true;
                validationRule3.UseRegularExpression = false;
                validationRule3.PassIfTextFound = true;
                request3.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule3.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule4 = new ValidationRuleFindText();
                validationRule4.FindText = @"The Category Detail & Category List Web Parts are standard BDC Web Parts.";
                validationRule4.IgnoreCase = true;
                validationRule4.UseRegularExpression = false;
                validationRule4.PassIfTextFound = true;
                request3.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule4.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule5 = new ValidationRuleFindText();
                validationRule5.FindText = "The Product List Web Part is also a standard Web Part that shows the products associated with the current category.";
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
            yield return request3;
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
            request4Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_b70a53b2_3c1d_4101_88f4_994f8f0aa813");
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
            request5Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_5fc6e986_173f_43a8_91ac_320feebfdccd");
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
            request6Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_a94f60cf_8dc9_489e_9e6b_4b30a6f0be35");
            request6Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request6Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN3.__EVENTVALIDATION"].ToString());
            request6.Body = request6Body;
            yield return request6;
            request6 = null;
        }
    }
}
