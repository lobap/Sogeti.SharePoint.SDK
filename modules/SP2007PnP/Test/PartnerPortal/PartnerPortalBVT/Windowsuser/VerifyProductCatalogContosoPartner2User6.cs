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
    /// This test validate category id =0 details page.
    /// Browse category page with category id =0
    /// verify Category detail webpart content
    /// verify category list webpart content
    /// verify product list webpart content
    /// </summary>
    public class VerifyProductCatalogContosoPartner2User6 : WebTest
    {
        string hostURL = CustConfig.GetHostURL;
        string winUserPassword = CustConfig.WINUserPassword;

        public VerifyProductCatalogContosoPartner2User6()
        {
            this.PreAuthenticate = true;
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            this.UserName = System.Environment.MachineName + "\\ContosoPartner2User6";
            this.Password = winUserPassword;

            // Initialize validation rules that apply to all requests in the WebTest
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidateResponseUrl validationRule1 = new ValidateResponseUrl();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            }

            WebTestRequest request1 = new WebTestRequest(hostURL + "/sites/productcatalog/category.aspx");
            request1.QueryStringParameters.Add("categoryid", "0", false, false);
            ExtractHiddenFields extractionRule1 = new ExtractHiddenFields();
            extractionRule1.Required = true;
            extractionRule1.HtmlDecode = true;
            extractionRule1.ContextParameterName = "1";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule1.Extract);
            ExtractHiddenFields extractionRule2 = new ExtractHiddenFields();
            extractionRule2.Required = true;
            extractionRule2.HtmlDecode = true;
            extractionRule2.ContextParameterName = "2";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule2.Extract);
            ExtractHiddenFields extractionRule3 = new ExtractHiddenFields();
            extractionRule3.Required = true;
            extractionRule3.HtmlDecode = true;
            extractionRule3.ContextParameterName = "3";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule3.Extract);

            //Extract CategoryDetail WebPart guid
            ExtractAttributeValue extractionRule23 = new ExtractAttributeValue();
            extractionRule23.TagName = "div";
            extractionRule23.AttributeName = "WebPartID";
            extractionRule23.MatchAttributeName = "id";
            extractionRule23.MatchAttributeValue = "WebPartWPQ3";
            extractionRule23.HtmlDecode = true;
            extractionRule23.Required = true;
            extractionRule23.Index = 0;
            extractionRule23.ContextParameterName = "CategoryDetailWebPartGuid";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule23.Extract);

            //Extract CategoryList WebPart guid
            ExtractAttributeValue extractionRule33 = new ExtractAttributeValue();
            extractionRule33.TagName = "div";
            extractionRule33.AttributeName = "WebPartID";
            extractionRule33.MatchAttributeName = "id";
            extractionRule33.MatchAttributeValue = "WebPartWPQ4";
            extractionRule33.HtmlDecode = true;
            extractionRule33.Required = true;
            extractionRule33.Index = 0;
            extractionRule33.ContextParameterName = "CategoryListWebPartGuid";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule33.Extract);

            //Extract CategoryList WebPart guid
            ExtractAttributeValue extractionRule43 = new ExtractAttributeValue();
            extractionRule43.TagName = "div";
            extractionRule43.AttributeName = "WebPartID";
            extractionRule43.MatchAttributeName = "id";
            extractionRule43.MatchAttributeValue = "WebPartWPQ5";
            extractionRule43.HtmlDecode = true;
            extractionRule43.Required = true;
            extractionRule43.Index = 0;
            extractionRule43.ContextParameterName = "ProductListWebPartGuid";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule43.Extract);
            //Check for Root Category 
            ValidationRuleFindText validationRule11 = new ValidationRuleFindText();
            validationRule11.FindText = "Root Category";
            validationRule11.IgnoreCase = true;
            validationRule11.UseRegularExpression = false;
            validationRule11.PassIfTextFound = true;
            request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule11.Validate);

            //Check for Category Details Webpart title
            ValidationRuleFindText validationRule12 = new ValidationRuleFindText();
            validationRule12.FindText = "Category Details";
            validationRule12.IgnoreCase = true;
            validationRule12.UseRegularExpression = false;
            validationRule12.PassIfTextFound = true;
            request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule12.Validate);

            //Check for Category List Webpart title
            ValidationRuleFindText validationRule13 = new ValidationRuleFindText();
            validationRule13.FindText = "Category List";
            validationRule13.IgnoreCase = true;
            validationRule13.UseRegularExpression = false;
            validationRule13.PassIfTextFound = true;
            request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule13.Validate);

            //Check for Product List Webpart title
            ValidationRuleFindText validationRule14 = new ValidationRuleFindText();
            validationRule14.FindText = "Product List";
            validationRule14.IgnoreCase = true;
            validationRule14.UseRegularExpression = false;
            validationRule14.PassIfTextFound = true;
            request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule14.Validate);

            yield return request1;

            string CategoryDetailWebPartGuid = Context["CategoryDetailWebPartGuid"].ToString();
            string CategoryListWebPartGuid = Context["CategoryListWebPartGuid"].ToString();
            string ProductListWebPartGuid = Context["ProductListWebPartGuid"].ToString();


            request1 = null;

            WebTestRequest request2 = new WebTestRequest(hostURL + "/sites/productcatalog/category.aspx");
            request2.Method = "POST";
            request2.QueryStringParameters.Add("categoryid", "0", false, false);
            FormPostHttpBody request2Body = new FormPostHttpBody();
            request2Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN1.MSO_PageHashCode"].ToString());
            // request2Body.FormPostParameters.Add("__SPSCEditMenu", this.Context["$HIDDEN1.__SPSCEditMenu"].ToString());
            //request2Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN1.MSOWebPartPage_PostbackSource"].ToString());
            request2Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN1.MSOTlPn_SelectedWpId"].ToString());
            request2Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN1.MSOTlPn_View"].ToString());
            request2Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN1.MSOTlPn_ShowSettings"].ToString());
            request2Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN1.MSOGallery_SelectedLibrary"].ToString());
            request2Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN1.MSOGallery_FilterString"].ToString());
            request2Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN1.MSOTlPn_Button"].ToString());
            request2Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN1.__EVENTTARGET"].ToString());
            request2Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            request2Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN1.__REQUESTDIGEST"].ToString());
            //request2Body.FormPostParameters.Add("MSOAuthoringConsole_FormContext", this.Context["$HIDDEN1.MSOAuthoringConsole_FormContext"].ToString());
            //request2Body.FormPostParameters.Add("MSOAC_EditDuringWorkflow", this.Context["$HIDDEN1.MSOAC_EditDuringWorkflow"].ToString());
            request2Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_DisplayModeName"].ToString());
            request2Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN1.MSOWebPartPage_Shared"].ToString());
            request2Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN1.MSOLayout_LayoutChanges"].ToString());
            request2Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN1.MSOLayout_InDesignMode"].ToString());
            request2Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request2Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN1.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request2Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN1.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request2Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request2Body.FormPostParameters.Add("__spDummyText1", "");
            request2Body.FormPostParameters.Add("__spDummyText2", "");
            request2Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + CategoryDetailWebPartGuid.Replace("-", "_").ToString());
            request2Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request2Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request2.Body = request2Body;

            //Check for CategoryId
            ValidationRuleFindText validationRule21 = new ValidationRuleFindText();
            validationRule21.FindText = @"<nobr>CategoryId:</nobr></td><td class=""ms-descriptiontext ms-alignleft"" width=""100%"">0</td>";
            validationRule21.IgnoreCase = true;
            validationRule21.UseRegularExpression = false;
            validationRule21.PassIfTextFound = true;
            request2.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule21.Validate);

            //Check for CategoryName
            ValidationRuleFindText validationRule22 = new ValidationRuleFindText();
            validationRule22.FindText = @"<nobr>Name:</nobr></td><td class=""ms-descriptiontext ms-alignleft"" width=""100%"">Root Category</td>";
            validationRule22.IgnoreCase = true;
            validationRule22.UseRegularExpression = false;
            validationRule22.PassIfTextFound = true;
            request2.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule22.Validate);
            yield return request2;
            request2 = null;

            WebTestRequest request3 = new WebTestRequest(hostURL + "/sites/productcatalog/category.aspx");
            request3.Method = "POST";
            request3.QueryStringParameters.Add("categoryid", "0", false, false);
            FormPostHttpBody request3Body = new FormPostHttpBody();
            request3Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN2.MSO_PageHashCode"].ToString());
            //request3Body.FormPostParameters.Add("__SPSCEditMenu", this.Context["$HIDDEN2.__SPSCEditMenu"].ToString());
            //request3Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN2.MSOWebPartPage_PostbackSource"].ToString());
            request3Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN2.MSOTlPn_SelectedWpId"].ToString());
            request3Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN2.MSOTlPn_View"].ToString());
            request3Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN2.MSOTlPn_ShowSettings"].ToString());
            request3Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN2.MSOGallery_SelectedLibrary"].ToString());
            request3Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN2.MSOGallery_FilterString"].ToString());
            request3Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN2.MSOTlPn_Button"].ToString());
            request3Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN2.__EVENTTARGET"].ToString());
            request3Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN2.__EVENTARGUMENT"].ToString());
            request3Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN2.__REQUESTDIGEST"].ToString());
            //request3Body.FormPostParameters.Add("MSOAuthoringConsole_FormContext", this.Context["$HIDDEN2.MSOAuthoringConsole_FormContext"].ToString());
            //request3Body.FormPostParameters.Add("MSOAC_EditDuringWorkflow", this.Context["$HIDDEN2.MSOAC_EditDuringWorkflow"].ToString());
            request3Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN2.MSOSPWebPartManager_DisplayModeName"].ToString());
            request3Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN2.MSOWebPartPage_Shared"].ToString());
            request3Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN2.MSOLayout_LayoutChanges"].ToString());
            request3Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN2.MSOLayout_InDesignMode"].ToString());
            request3Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN2.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request3Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN2.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request3Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN2.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request3Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN2.__VIEWSTATE"].ToString());
            request3Body.FormPostParameters.Add("__spDummyText1", "");
            request3Body.FormPostParameters.Add("__spDummyText2", "");
            request3Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + CategoryListWebPartGuid.Replace("-", "_").ToString());
            request3Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request3Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN2.__EVENTVALIDATION"].ToString());

            request3.Body = request3Body;
            ValidationRuleFindText validationRule31 = new ValidationRuleFindText();

            validationRule31.FindText = @"<a onclick=""event.cancelBubble=true"" href=""" + hostURL + "/sites/productcatalog/_layouts/ProfileRedirect.aspx?Application=ContosoProductCatalogService&amp;Entity=Category&amp;ItemId=__bk40003300\" onkeydown=\"actionMenuOnKeyDown('Loading...','Physician Equipment',false,'ContosoProductCatalogService','Category','__bk40003300');\">Physician Equipment<img src=\"/_layouts/images/blank.gif\" border=\"0\" alt=\"\"></a>";
            //validationRule31.FindText = @"<a onclick=""event.cancelBubble=true"" onkeydown=""actionMenuOnKeyDown('Loading...','Dental Equipment',false,'ContosoProductCatalogService','Category','__bk40001300');"">Dental Equipment<img src=""/_layouts/images/blank.gif"" border=""0"" alt=""""></a>";
            validationRule31.IgnoreCase = true;
            validationRule31.UseRegularExpression = false;
            validationRule31.PassIfTextFound = true;
            request3.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule31.Validate);

            yield return request3;
            request3 = null;

            WebTestRequest request4 = new WebTestRequest(hostURL + "/sites/productcatalog/category.aspx");
            request4.Method = "POST";
            request4.QueryStringParameters.Add("categoryid", "0", false, false);
            FormPostHttpBody request4Body = new FormPostHttpBody();
            request4Body.FormPostParameters.Add("MSO_PageHashCode", this.Context["$HIDDEN3.MSO_PageHashCode"].ToString());
            //request4Body.FormPostParameters.Add("__SPSCEditMenu", this.Context["$HIDDEN3.__SPSCEditMenu"].ToString());
            //request4Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN3.MSOWebPartPage_PostbackSource"].ToString());
            request4Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN3.MSOTlPn_SelectedWpId"].ToString());
            request4Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN3.MSOTlPn_View"].ToString());
            request4Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN3.MSOTlPn_ShowSettings"].ToString());
            request4Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN3.MSOGallery_SelectedLibrary"].ToString());
            request4Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN3.MSOGallery_FilterString"].ToString());
            request4Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN3.MSOTlPn_Button"].ToString());
            request4Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN3.__EVENTTARGET"].ToString());
            request4Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN3.__EVENTARGUMENT"].ToString());
            request4Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN3.__REQUESTDIGEST"].ToString());
            //request4Body.FormPostParameters.Add("MSOAuthoringConsole_FormContext", this.Context["$HIDDEN3.MSOAuthoringConsole_FormContext"].ToString());
            //request4Body.FormPostParameters.Add("MSOAC_EditDuringWorkflow", this.Context["$HIDDEN3.MSOAC_EditDuringWorkflow"].ToString());
            request4Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN3.MSOSPWebPartManager_DisplayModeName"].ToString());
            request4Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN3.MSOWebPartPage_Shared"].ToString());
            request4Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN3.MSOLayout_LayoutChanges"].ToString());
            request4Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN3.MSOLayout_InDesignMode"].ToString());
            request4Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN3.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request4Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN3.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request4Body.FormPostParameters.Add("BDC_ActionsMenuProxyPageWebUrl", this.Context["$HIDDEN3.BDC_ActionsMenuProxyPageWebUrl"].ToString());
            request4Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN3.__VIEWSTATE"].ToString());
            request4Body.FormPostParameters.Add("__spDummyText1", "");
            request4Body.FormPostParameters.Add("__spDummyText2", "");
            request4Body.FormPostParameters.Add("__CALLBACKID", "ctl00$m$g_" + ProductListWebPartGuid.Replace("-", "_").ToString());
            request4Body.FormPostParameters.Add("__CALLBACKPARAM", "GetLongRunningUI");
            request4Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN3.__EVENTVALIDATION"].ToString());
            request4.Body = request4Body;

            //check for There are no items to show.
            ValidationRuleFindText validationRule41 = new ValidationRuleFindText();
            validationRule41.FindText = "There are no items to show.";
            validationRule41.IgnoreCase = true;
            validationRule41.UseRegularExpression = false;
            validationRule41.PassIfTextFound = true;
            request4.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule41.Validate);

            yield return request4;
            request4 = null;
        }
    }
}
