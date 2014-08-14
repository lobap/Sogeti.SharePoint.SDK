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
    /// This Test verify the creation of promotion page.
    /// </summary>

    public class VerifyCreatePromotionPage : WebTest
    {
        string hostURL = CustConfig.GetHostURL;

        public VerifyCreatePromotionPage()
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

            WebTestRequest request1 = new WebTestRequest(hostURL + "/");
            request1.ThinkTime = 4;
            request1.ExpectedResponseUrl = hostURL + "/sites/partner1/Pages/default.aspx";
            yield return request1;
            request1 = null;

            WebTestRequest request2 = new WebTestRequest(hostURL + "/sites/promotions");
            request2.ThinkTime = 2;
            request2.ExpectedResponseUrl = hostURL + "/sites/promotions/Pages/default.aspx";
            yield return request2;
            request2 = null;

            //WebTestRequest request3 = new WebTestRequest(hostURL + "/_layouts/blank.htm");
            //request3.ThinkTime = 2;
            //yield return request3;
            //request3 = null;

            WebTestRequest request4 = new WebTestRequest(hostURL + "/sites/promotions/_layouts/CreatePage.aspx");
            request4.ThinkTime = 8;
            request4.QueryStringParameters.Add("CancelSource", "%2Fsites%2Fpromotions%2FPages%2Fdefault%2Easpx", false, false);
            ExtractHiddenFields extractionRule1 = new ExtractHiddenFields();
            extractionRule1.Required = true;
            extractionRule1.HtmlDecode = true;
            extractionRule1.ContextParameterName = "1";
            request4.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule1.Extract);
            yield return request4;
            request4 = null;

            WebTestRequest request5 = new WebTestRequest(hostURL + "/sites/promotions/_layouts/CreatePage.aspx");
            request5.ThinkTime = 29;
            request5.Method = "POST";
            request5.ExpectedResponseUrl = hostURL + "/sites/promotions/Pages/MyPromo1.aspx?ControlMode=Edit&D" +
                "isplayMode=Design";
            request5.QueryStringParameters.Add("CancelSource", "%2fsites%2fpromotions%2fPages%2fdefault.aspx", false, false);
            FormPostHttpBody request5Body = new FormPostHttpBody();
            request5Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN1.__EVENTTARGET"].ToString());
            request5Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            request5Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN1.__REQUESTDIGEST"].ToString());
            request5Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request5Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request5Body.FormPostParameters.Add("ctl00$PlaceHolderMain$pageTitleSection$ctl00$titleTextBox", "MyPromo1");
            request5Body.FormPostParameters.Add("ctl00$PlaceHolderMain$pageTitleSection$ctl01$descriptionTextBox", "");
            request5Body.FormPostParameters.Add("ctl00$PlaceHolderMain$pageTitleSection$ctl02$urlNameTextBox", "MyPromo1");
            request5Body.FormPostParameters.Add("ctl00$PlaceHolderMain$layoutPickerSection$ctl01$pageTemplatePicker$ctl00$dropdown" +
                    "List", "84");
            request5Body.FormPostParameters.Add("ctl00$PlaceHolderMain$ctl00$RptControls$buttonCreatePage", "Create");
            request5Body.FormPostParameters.Add("__spDummyText1", "");
            request5Body.FormPostParameters.Add("__spDummyText2", "");
            request5.Body = request5Body;
            ExtractHiddenFields extractionRule2 = new ExtractHiddenFields();
            extractionRule2.Required = true;
            extractionRule2.HtmlDecode = true;
            extractionRule2.ContextParameterName = "1";
            request5.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule2.Extract);
            yield return request5;
            request5 = null;

            //WebTestRequest request6 = new WebTestRequest(hostURL + "/_layouts/blank.html");
            //request6.ThinkTime = 10;
            //yield return request6;
            //request6 = null;

            //WebTestRequest request7 = new WebTestRequest(hostURL + "/_layouts/blank.htm");
            //request7.ThinkTime = 1;
            //yield return request7;
            //request7 = null;

            //WebTestRequest request8 = new WebTestRequest(hostURL + "/_layouts/blank.htm");
            //request8.ThinkTime = 1;
            //yield return request8;
            //request8 = null;

            //WebTestRequest request9 = new WebTestRequest(hostURL + "/_layouts/blank.htm");
            //request9.ThinkTime = 2;
            //yield return request9;
            //request9 = null;

            WebTestRequest request10 = new WebTestRequest(hostURL + "/sites/promotions/Pages/MyPromo1.aspx");
            request10.ThinkTime = 1;
            request10.Method = "POST";
            request10.QueryStringParameters.Add("ControlMode", "Edit", false, false);
            request10.QueryStringParameters.Add("DisplayMode", "Design", false, false);
            FormPostHttpBody request10Body = new FormPostHttpBody();
            request10Body.FormPostParameters.Add("__SPSCEditMenu", this.Context["$HIDDEN1.__SPSCEditMenu"].ToString());
            request10Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN1.MSOWebPartPage_PostbackSource"].ToString());
            request10Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN1.MSOTlPn_SelectedWpId"].ToString());
            request10Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN1.MSOTlPn_View"].ToString());
            request10Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN1.MSOTlPn_ShowSettings"].ToString());
            request10Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN1.MSOGallery_SelectedLibrary"].ToString());
            request10Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN1.MSOGallery_FilterString"].ToString());
            request10Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN1.MSOTlPn_Button"].ToString());
            request10Body.FormPostParameters.Add("__EVENTTARGET", "aspnetForm");
            request10Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            request10Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN1.__REQUESTDIGEST"].ToString());
            request10Body.FormPostParameters.Add("MSO_PageAlreadySaved", "1");
            request10Body.FormPostParameters.Add("MSOAuthoringConsole_FormContext", this.Context["$HIDDEN1.MSOAuthoringConsole_FormContext"].ToString());
            request10Body.FormPostParameters.Add("MSOAC_EditDuringWorkflow", this.Context["$HIDDEN1.MSOAC_EditDuringWorkflow"].ToString());
            request10Body.FormPostParameters.Add("_ListSchemaVersion_{c80571e3-6097-429f-8455-773b75a6cc70}", this.Context["$HIDDEN1._ListSchemaVersion_{c80571e3-6097-429f-8455-773b75a6cc70"].ToString());
            request10Body.FormPostParameters.Add("MSOConn_SWpId", "ProductDetailsWebPart");
            request10Body.FormPostParameters.Add("MSOConn_TWpId", "g_b54328f5_63ea_4a47_b094_04cc4790beb5");
            request10Body.FormPostParameters.Add("MSOConn_SGroupId", "IFilterValueConnection");
            request10Body.FormPostParameters.Add("MSOConn_TGroupId", "ITransformableFilterValues");
            request10Body.FormPostParameters.Add("MSOConn_XForm1", this.Context["$HIDDEN1.MSOConn_XForm1"].ToString());
            request10Body.FormPostParameters.Add("MSOConn_XForm2", this.Context["$HIDDEN1.MSOConn_XForm2"].ToString());
            request10Body.FormPostParameters.Add("MSOConn_AspXForm", this.Context["$HIDDEN1.MSOConn_AspXForm"].ToString());
            request10Body.FormPostParameters.Add("MSOConn_CreationStep", "1");
            request10Body.FormPostParameters.Add("MSOConn_Button", "save");
            request10Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_DisplayModeName"].ToString());
            request10Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN1.MSOWebPartPage_Shared"].ToString());
            request10Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN1.MSOLayout_LayoutChanges"].ToString());
            request10Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN1.MSOLayout_InDesignMode"].ToString());
            request10Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request10Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN1.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request10Body.FormPostParameters.Add("MSOShowUnapproved_Xml", this.Context["$HIDDEN1.MSOShowUnapproved_Xml"].ToString());
            request10Body.FormPostParameters.Add("__LASTFOCUS", this.Context["$HIDDEN1.__LASTFOCUS"].ToString());
            request10Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichImageField1$ctl00$RichImageField", this.Context["$HIDDEN1.ctl00$PlaceHolderMain$RichImageField1$ctl00$RichImageField"].ToString());
            request10Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichImageField1$ctl00$RichImageField__ValContext", "FONTS=true&REUSABLECONTENT=true&HEADINGS=true&HYPERLINKS=true&IMAGES=true&LISTS=t" +
                    "rue&TABLES=true&TEXTMARKUP=true&ISREQUIRED=false&RESTRICTURLSTOSITECOLLECTION=fa" +
                    "lse&");
            request10Body.FormPostParameters.Add("__spPickerHasReturnValue", this.Context["$HIDDEN1.__spPickerHasReturnValue"].ToString());
            request10Body.FormPostParameters.Add("__spPickerReturnValueHolder", this.Context["$HIDDEN1.__spPickerReturnValueHolder"].ToString());
            request10Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request10Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request10Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl01$ctl00", this.Context["$HIDDEN1.ctl00$PlaceHolderSearchArea$ctl01$ctl00"].ToString());
            request10Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl01$SBScopesDDL", "This Site");
            request10Body.FormPostParameters.Add("InputKeywords", "");
            request10Body.FormPostParameters.Add("ctl00$PlaceHolderMain$EditModePanel1$ProductSku$ctl00$TextField", "6000000000");
            request10Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichHtmlField1$ctl00$RichHtmlField", "Promotion Text");
            request10Body.FormPostParameters.Add("ctl00_PlaceHolderMain_RichHtmlField1_ctl00_RichHtmlField_hiddenDisplay", "Promotion Text");
            request10Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichHtmlField3$ctl00$RichHtmlField", this.Context["$HIDDEN1.ctl00$PlaceHolderMain$RichHtmlField3$ctl00$RichHtmlField"].ToString());
            request10Body.FormPostParameters.Add("ctl00_PlaceHolderMain_RichHtmlField3_ctl00_RichHtmlField_hiddenDisplay", this.Context["$HIDDEN1.ctl00_PlaceHolderMain_RichHtmlField3_ctl00_RichHtmlField_hiddenDisplay"].ToString());
            request10Body.FormPostParameters.Add("ctl00$PlaceHolderMain$WindowsMediaField$ctl00$TextField", "/sites/pssportal/Promotions/Movie Libraries/DemoMedia/intro.wmv");
            request10Body.FormPostParameters.Add("__spDummyText1", "");
            request10Body.FormPostParameters.Add("__spDummyText2", "");
            request10.Body = request10Body;
            ExtractHiddenFields extractionRule3 = new ExtractHiddenFields();
            extractionRule3.Required = true;
            extractionRule3.HtmlDecode = true;
            extractionRule3.ContextParameterName = "1";
            request10.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule3.Extract);
            yield return request10;
            request10 = null;

            WebTestRequest request11 = new WebTestRequest(hostURL + "/sites/promotions/_layouts/AspXform.aspx");
            request11.ThinkTime = 2;
            request11.QueryStringParameters.Add("pageUrl", "http%3A%2F%2F" + hostURL + "%2Fsites%2Fpromotions%2FPages%2FMyPromo1%2Easpx%" +
                    "3FControlMode%3DEdit%26DisplayMode%3DDesign", false, false);
            request11.QueryStringParameters.Add("sWpId", "ProductDetailsWebPart", false, false);
            request11.QueryStringParameters.Add("sGroupId", "IFilterValueConnection", false, false);
            request11.QueryStringParameters.Add("tWpId", "g%5Fb54328f5%5F63ea%5F4a47%5Fb094%5F04cc4790beb5", false, false);
            request11.QueryStringParameters.Add("tGroupId", "ITransformableFilterValues", false, false);
            request11.QueryStringParameters.Add("xFormType", "Microsoft%2ESharePoint%2EWebPartPages%2ETransformableFilterValuesToFilterValuesTr" +
                    "ansformer%2C%20Microsoft%2ESharePoint%2C%20Version%3D12%2E0%2E0%2E0%2C%20Culture" +
                    "%3Dneutral%2C%20PublicKeyToken%3D71e9bce111e9429c", false, false);
            request11.QueryStringParameters.Add("xFormInfo", "", false, false);
            request11.QueryStringParameters.Add("isMultiGroup", "False", false, false);
            request11.QueryStringParameters.Add("isConnected", "False", false, false);
            ExtractHiddenFields extractionRule4 = new ExtractHiddenFields();
            extractionRule4.Required = true;
            extractionRule4.HtmlDecode = true;
            extractionRule4.ContextParameterName = "0";
            request11.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule4.Extract);
            yield return request11;
            request11 = null;

            WebTestRequest request12 = new WebTestRequest(hostURL + "/sites/promotions/_layouts/AspXform.aspx");
            request12.Method = "POST";
            request12.QueryStringParameters.Add("pageUrl", "http%3a%2f%2f" + hostURL + "%2fsites%2fpromotions%2fPages%2fMyPromo1.aspx%3f" +
                    "ControlMode%3dEdit%26DisplayMode%3dDesign", false, false);
            request12.QueryStringParameters.Add("sWpId", "ProductDetailsWebPart", false, false);
            request12.QueryStringParameters.Add("sGroupId", "IFilterValueConnection", false, false);
            request12.QueryStringParameters.Add("tWpId", "g_b54328f5_63ea_4a47_b094_04cc4790beb5", false, false);
            request12.QueryStringParameters.Add("tGroupId", "ITransformableFilterValues", false, false);
            request12.QueryStringParameters.Add("xFormType", "Microsoft.SharePoint.WebPartPages.TransformableFilterValuesToFilterValuesTransfor" +
                    "mer%2c+Microsoft.SharePoint%2c+Version%3d12.0.0.0%2c+Culture%3dneutral%2c+Public" +
                    "KeyToken%3d71e9bce111e9429c", false, false);
            request12.QueryStringParameters.Add("xFormInfo", "", false, false);
            request12.QueryStringParameters.Add("isMultiGroup", "False", false, false);
            request12.QueryStringParameters.Add("isConnected", "False", false, false);
            FormPostHttpBody request12Body = new FormPostHttpBody();
            request12Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN0.__VIEWSTATE"].ToString());
            //request12Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN0.__EVENTVALIDATION"].ToString());
            request12Body.FormPostParameters.Add("ctl11$MappedConsumerParameterName0", "Sku");
            request12Body.FormPostParameters.Add("ctl11$FinishNavigationTemplateContainerID$FinishButton", "Finish");
            request12.Body = request12Body;
            yield return request12;
            request12 = null;

            WebTestRequest request13 = new WebTestRequest(hostURL + "/sites/promotions/Pages/MyPromo1.aspx");
            request13.ThinkTime = 29;
            request13.Method = "POST";
            request13.QueryStringParameters.Add("ControlMode", "Edit", false, false);
            request13.QueryStringParameters.Add("DisplayMode", "Design", false, false);
            FormPostHttpBody request13Body = new FormPostHttpBody();
            request13Body.FormPostParameters.Add("__SPSCEditMenu", this.Context["$HIDDEN1.__SPSCEditMenu"].ToString());
            request13Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN1.MSOWebPartPage_PostbackSource"].ToString());
            request13Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN1.MSOTlPn_SelectedWpId"].ToString());
            request13Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN1.MSOTlPn_View"].ToString());
            request13Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN1.MSOTlPn_ShowSettings"].ToString());
            request13Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN1.MSOGallery_SelectedLibrary"].ToString());
            request13Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN1.MSOGallery_FilterString"].ToString());
            request13Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN1.MSOTlPn_Button"].ToString());
            request13Body.FormPostParameters.Add("__EVENTTARGET", "aspnetForm");
            request13Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            request13Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN1.__REQUESTDIGEST"].ToString());
            request13Body.FormPostParameters.Add("MSO_PageAlreadySaved", "1");
            request13Body.FormPostParameters.Add("MSOAuthoringConsole_FormContext", this.Context["$HIDDEN1.MSOAuthoringConsole_FormContext"].ToString());
            request13Body.FormPostParameters.Add("MSOAC_EditDuringWorkflow", this.Context["$HIDDEN1.MSOAC_EditDuringWorkflow"].ToString());
            //request13Body.FormPostParameters.Add("MSOConn_GroupUrl", this.Context["$HIDDEN1.MSOConn_GroupUrl"].ToString());
            //request13Body.FormPostParameters.Add("MSOConn_RCXform", this.Context["$HIDDEN1.MSOConn_RCXform"].ToString());
            //request13Body.FormPostParameters.Add("MSOConn_RFProXform", this.Context["$HIDDEN1.MSOConn_RFProXform"].ToString());
            //request13Body.FormPostParameters.Add("MSOConn_RFConXform", this.Context["$HIDDEN1.MSOConn_RFConXform"].ToString());
            //request13Body.FormPostParameters.Add("MSOConn_AspXformUrl", this.Context["$HIDDEN1.MSOConn_AspXformUrl"].ToString());
            request13Body.FormPostParameters.Add("MSOConn_SWpId", "ProductDetailsWebPart");
            request13Body.FormPostParameters.Add("MSOConn_TWpId", "g_b54328f5_63ea_4a47_b094_04cc4790beb5");
            request13Body.FormPostParameters.Add("MSOConn_SGroupId", "IFilterValueConnection");
            request13Body.FormPostParameters.Add("MSOConn_TGroupId", "ITransformableFilterValues");
            request13Body.FormPostParameters.Add("MSOConn_XForm1", this.Context["$HIDDEN1.MSOConn_XForm1"].ToString());
            request13Body.FormPostParameters.Add("MSOConn_XForm2", this.Context["$HIDDEN1.MSOConn_XForm2"].ToString());
            request13Body.FormPostParameters.Add("MSOConn_AspXForm", "/wEFA1NrdQ==");
            request13Body.FormPostParameters.Add("MSOConn_CreationStep", this.Context["$HIDDEN1.MSOConn_CreationStep"].ToString());
            request13Body.FormPostParameters.Add("MSOConn_Button", "save");
            request13Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_DisplayModeName"].ToString());
            request13Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN1.MSOWebPartPage_Shared"].ToString());
            request13Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN1.MSOLayout_LayoutChanges"].ToString());
            request13Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN1.MSOLayout_InDesignMode"].ToString());
            request13Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request13Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN1.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request13Body.FormPostParameters.Add("MSOShowUnapproved_Xml", this.Context["$HIDDEN1.MSOShowUnapproved_Xml"].ToString());
            request13Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichImageField1$ctl00$RichImageField", this.Context["$HIDDEN1.ctl00$PlaceHolderMain$RichImageField1$ctl00$RichImageField"].ToString());
            request13Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichImageField1$ctl00$RichImageField__ValContext", "FONTS=true&REUSABLECONTENT=true&HEADINGS=true&HYPERLINKS=true&IMAGES=true&LISTS=t" +
                    "rue&TABLES=true&TEXTMARKUP=true&ISREQUIRED=false&RESTRICTURLSTOSITECOLLECTION=fa" +
                    "lse&");
            request13Body.FormPostParameters.Add("__spPickerHasReturnValue", this.Context["$HIDDEN1.__spPickerHasReturnValue"].ToString());
            request13Body.FormPostParameters.Add("__spPickerReturnValueHolder", this.Context["$HIDDEN1.__spPickerReturnValueHolder"].ToString());
            request13Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request13Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request13Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl01$ctl00", this.Context["$HIDDEN1.ctl00$PlaceHolderSearchArea$ctl01$ctl00"].ToString());
            request13Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl01$SBScopesDDL", "This Site");
            request13Body.FormPostParameters.Add("InputKeywords", "");
            request13Body.FormPostParameters.Add("ctl00$PlaceHolderMain$EditModePanel1$ProductSku$ctl00$TextField", "6000000000");
            request13Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichHtmlField1$ctl00$RichHtmlField", this.Context["$HIDDEN1.ctl00$PlaceHolderMain$RichHtmlField1$ctl00$RichHtmlField"].ToString());
            request13Body.FormPostParameters.Add("ctl00_PlaceHolderMain_RichHtmlField1_ctl00_RichHtmlField_hiddenDisplay", this.Context["$HIDDEN1.ctl00_PlaceHolderMain_RichHtmlField1_ctl00_RichHtmlField_hiddenDisplay"].ToString());
            request13Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichHtmlField3$ctl00$RichHtmlField", this.Context["$HIDDEN1.ctl00$PlaceHolderMain$RichHtmlField3$ctl00$RichHtmlField"].ToString());
            request13Body.FormPostParameters.Add("ctl00_PlaceHolderMain_RichHtmlField3_ctl00_RichHtmlField_hiddenDisplay", this.Context["$HIDDEN1.ctl00_PlaceHolderMain_RichHtmlField3_ctl00_RichHtmlField_hiddenDisplay"].ToString());
            request13Body.FormPostParameters.Add("ctl00$PlaceHolderMain$WindowsMediaField$ctl00$TextField", "/sites/pssportal/Promotions/Movie Libraries/DemoMedia/intro.wmv");
            request13Body.FormPostParameters.Add("__spDummyText1", "");
            request13Body.FormPostParameters.Add("__spDummyText2", "");
            request13.Body = request13Body;
            ExtractHiddenFields extractionRule5 = new ExtractHiddenFields();
            extractionRule5.Required = true;
            extractionRule5.HtmlDecode = true;
            extractionRule5.ContextParameterName = "1";
            request13.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule5.Extract);
            yield return request13;
            request13 = null;

            WebTestRequest request14 = new WebTestRequest(hostURL + "/sites/promotions/Pages/MyPromo1.aspx");
            request14.ThinkTime = 1;
            request14.Method = "POST";
            request14.ExpectedResponseUrl = hostURL + "/sites/promotions/Pages/MyPromo1.aspx";
            request14.QueryStringParameters.Add("ControlMode", "Edit", false, false);
            request14.QueryStringParameters.Add("DisplayMode", "Design", false, false);
            FormPostHttpBody request14Body = new FormPostHttpBody();
            request14Body.FormPostParameters.Add("__SPSCEditMenu", this.Context["$HIDDEN1.__SPSCEditMenu"].ToString());
            request14Body.FormPostParameters.Add("MSOWebPartPage_PostbackSource", this.Context["$HIDDEN1.MSOWebPartPage_PostbackSource"].ToString());
            request14Body.FormPostParameters.Add("MSOTlPn_SelectedWpId", this.Context["$HIDDEN1.MSOTlPn_SelectedWpId"].ToString());
            request14Body.FormPostParameters.Add("MSOTlPn_View", this.Context["$HIDDEN1.MSOTlPn_View"].ToString());
            request14Body.FormPostParameters.Add("MSOTlPn_ShowSettings", this.Context["$HIDDEN1.MSOTlPn_ShowSettings"].ToString());
            request14Body.FormPostParameters.Add("MSOGallery_SelectedLibrary", this.Context["$HIDDEN1.MSOGallery_SelectedLibrary"].ToString());
            request14Body.FormPostParameters.Add("MSOGallery_FilterString", this.Context["$HIDDEN1.MSOGallery_FilterString"].ToString());
            request14Body.FormPostParameters.Add("MSOTlPn_Button", this.Context["$HIDDEN1.MSOTlPn_Button"].ToString());
            request14Body.FormPostParameters.Add("__EVENTTARGET", "ctl00$SPNavigation$ctl01$qaPublish_CmsActionControl");
            request14Body.FormPostParameters.Add("__EVENTARGUMENT", "publishPage");
            request14Body.FormPostParameters.Add("__REQUESTDIGEST", this.Context["$HIDDEN1.__REQUESTDIGEST"].ToString());
            request14Body.FormPostParameters.Add("MSO_PageAlreadySaved", "1");
            request14Body.FormPostParameters.Add("MSOAuthoringConsole_FormContext", this.Context["$HIDDEN1.MSOAuthoringConsole_FormContext"].ToString());
            request14Body.FormPostParameters.Add("MSOAC_EditDuringWorkflow", this.Context["$HIDDEN1.MSOAC_EditDuringWorkflow"].ToString());
            request14Body.FormPostParameters.Add("_ListSchemaVersion_{c80571e3-6097-429f-8455-773b75a6cc70}", this.Context["$HIDDEN1._ListSchemaVersion_{c80571e3-6097-429f-8455-773b75a6cc70"].ToString());
            request14Body.FormPostParameters.Add("MSOConn_SWpId", this.Context["$HIDDEN1.MSOConn_SWpId"].ToString());
            request14Body.FormPostParameters.Add("MSOConn_TWpId", this.Context["$HIDDEN1.MSOConn_TWpId"].ToString());
            request14Body.FormPostParameters.Add("MSOConn_SGroupId", this.Context["$HIDDEN1.MSOConn_SGroupId"].ToString());
            request14Body.FormPostParameters.Add("MSOConn_TGroupId", this.Context["$HIDDEN1.MSOConn_TGroupId"].ToString());
            request14Body.FormPostParameters.Add("MSOConn_XForm1", this.Context["$HIDDEN1.MSOConn_XForm1"].ToString());
            request14Body.FormPostParameters.Add("MSOConn_XForm2", this.Context["$HIDDEN1.MSOConn_XForm2"].ToString());
            request14Body.FormPostParameters.Add("MSOConn_AspXForm", this.Context["$HIDDEN1.MSOConn_AspXForm"].ToString());
            request14Body.FormPostParameters.Add("MSOConn_CreationStep", this.Context["$HIDDEN1.MSOConn_CreationStep"].ToString());
            request14Body.FormPostParameters.Add("MSOConn_Button", this.Context["$HIDDEN1.MSOConn_Button"].ToString());
            request14Body.FormPostParameters.Add("MSOSPWebPartManager_DisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_DisplayModeName"].ToString());
            request14Body.FormPostParameters.Add("MSOWebPartPage_Shared", this.Context["$HIDDEN1.MSOWebPartPage_Shared"].ToString());
            request14Body.FormPostParameters.Add("MSOLayout_LayoutChanges", this.Context["$HIDDEN1.MSOLayout_LayoutChanges"].ToString());
            request14Body.FormPostParameters.Add("MSOLayout_InDesignMode", this.Context["$HIDDEN1.MSOLayout_InDesignMode"].ToString());
            request14Body.FormPostParameters.Add("MSOSPWebPartManager_OldDisplayModeName", this.Context["$HIDDEN1.MSOSPWebPartManager_OldDisplayModeName"].ToString());
            request14Body.FormPostParameters.Add("MSOSPWebPartManager_StartWebPartEditingName", this.Context["$HIDDEN1.MSOSPWebPartManager_StartWebPartEditingName"].ToString());
            request14Body.FormPostParameters.Add("MSOShowUnapproved_Xml", this.Context["$HIDDEN1.MSOShowUnapproved_Xml"].ToString());
            request14Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichImageField1$ctl00$RichImageField", this.Context["$HIDDEN1.ctl00$PlaceHolderMain$RichImageField1$ctl00$RichImageField"].ToString());
            request14Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichImageField1$ctl00$RichImageField__ValContext", "FONTS=true&REUSABLECONTENT=true&HEADINGS=true&HYPERLINKS=true&IMAGES=true&LISTS=t" +
                    "rue&TABLES=true&TEXTMARKUP=true&ISREQUIRED=false&RESTRICTURLSTOSITECOLLECTION=fa" +
                    "lse&");
            request14Body.FormPostParameters.Add("__spPickerHasReturnValue", this.Context["$HIDDEN1.__spPickerHasReturnValue"].ToString());
            request14Body.FormPostParameters.Add("__spPickerReturnValueHolder", this.Context["$HIDDEN1.__spPickerReturnValueHolder"].ToString());
            request14Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request14Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request14Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl01$ctl00", this.Context["$HIDDEN1.ctl00$PlaceHolderSearchArea$ctl01$ctl00"].ToString());
            request14Body.FormPostParameters.Add("ctl00$PlaceHolderSearchArea$ctl01$SBScopesDDL", "This Site");
            request14Body.FormPostParameters.Add("InputKeywords", "");
            request14Body.FormPostParameters.Add("ctl00$PlaceHolderMain$EditModePanel1$ProductSku$ctl00$TextField", "6000000000");
            request14Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichHtmlField1$ctl00$RichHtmlField", this.Context["$HIDDEN1.ctl00$PlaceHolderMain$RichHtmlField1$ctl00$RichHtmlField"].ToString());
            request14Body.FormPostParameters.Add("ctl00_PlaceHolderMain_RichHtmlField1_ctl00_RichHtmlField_hiddenDisplay", this.Context["$HIDDEN1.ctl00_PlaceHolderMain_RichHtmlField1_ctl00_RichHtmlField_hiddenDisplay"].ToString());
            request14Body.FormPostParameters.Add("ctl00$PlaceHolderMain$RichHtmlField3$ctl00$RichHtmlField", this.Context["$HIDDEN1.ctl00$PlaceHolderMain$RichHtmlField3$ctl00$RichHtmlField"].ToString());
            request14Body.FormPostParameters.Add("ctl00_PlaceHolderMain_RichHtmlField3_ctl00_RichHtmlField_hiddenDisplay", this.Context["$HIDDEN1.ctl00_PlaceHolderMain_RichHtmlField3_ctl00_RichHtmlField_hiddenDisplay"].ToString());
            request14Body.FormPostParameters.Add("ctl00$PlaceHolderMain$WindowsMediaField$ctl00$TextField", "/sites/pssportal/Promotions/Movie Libraries/DemoMedia/intro.wmv");
            request14Body.FormPostParameters.Add("__spDummyText1", "");
            request14Body.FormPostParameters.Add("__spDummyText2", "");
            request14.Body = request14Body;
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule2 = new ValidationRuleFindText();
                validationRule2.FindText = "X-ray Machine";
                validationRule2.IgnoreCase = true;
                validationRule2.UseRegularExpression = false;
                validationRule2.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule3 = new ValidationRuleFindText();
                validationRule3.FindText = "6000000000";
                validationRule3.IgnoreCase = true;
                validationRule3.UseRegularExpression = false;
                validationRule3.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule3.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule4 = new ValidationRuleFindText();
                validationRule4.FindText = "Product Details";
                validationRule4.IgnoreCase = true;
                validationRule4.UseRegularExpression = false;
                validationRule4.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule4.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule5 = new ValidationRuleFindText();
                validationRule5.FindText = "Promotion Discounts";
                validationRule5.IgnoreCase = true;
                validationRule5.UseRegularExpression = false;
                validationRule5.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule5.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule6 = new ValidationRuleFindText();
                validationRule6.FindText = "Promotion Text";
                validationRule6.IgnoreCase = true;
                validationRule6.UseRegularExpression = false;
                validationRule6.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule6.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule7 = new ValidationRuleFindText();
                validationRule7.FindText = "MyPromo1.aspx";
                validationRule7.IgnoreCase = true;
                validationRule7.UseRegularExpression = false;
                validationRule7.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule7.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule8 = new ValidationRuleFindText();
                validationRule8.FindText = "Product Pricing";
                validationRule8.IgnoreCase = true;
                validationRule8.UseRegularExpression = false;
                validationRule8.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule8.Validate);
            }
            yield return request14;
            request14 = null;

            WebTestRequest request15 = new WebTestRequest(hostURL + "/WebResource.axd");
            request15.QueryStringParameters.Add("d", "3zfQbNlR1haIrlcHlwg1Lns7-KkiaRBIC_wQp9O7M8EaAFG7RANEHNfrEAEISUzVYM6-pSIduig25fnTV" +
                    "C1XaA2", false, false);
            request15.QueryStringParameters.Add("t", "633831381381926872", false, false);
            yield return request15;
            request15 = null;

            WebTestRequest request16 = new WebTestRequest(hostURL + "/sites/pssportal/Promotions/Movie%20Libraries/DemoMedia/" +
                    "intro.wmv");
            yield return request16;
            request16 = null;
        }
    }
}
