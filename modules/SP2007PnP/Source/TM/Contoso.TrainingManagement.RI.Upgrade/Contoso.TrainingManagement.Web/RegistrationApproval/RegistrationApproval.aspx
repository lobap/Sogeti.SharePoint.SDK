<%@ Page Language="C#" Inherits="Contoso.TrainingManagement.Forms.RegistrationApproval, Contoso.TrainingManagement, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5"
    MasterPageFile="~/_layouts/application.master" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <SharePoint:EncodedLiteral ID="ListFormPageTitle" runat="server" EncodeMethod='HtmlEncode' />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    <SharePoint:EncodedLiteral ID="LinkTitle" runat="server" EncodeMethod='HtmlEncode' />:
    <SharePoint:EncodedLiteral ID="ItemProperty" runat="server" EncodeMethod='HtmlEncode' />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderPageImage" runat="server" />
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderPageDescriptionRowAttr"
    runat="server">
    style="display:none;"
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderPageDescriptionRowAttr2"
    runat="server">
    style="display:none;"
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
    <asp:Label ID="LabelPageDescription" runat="server" />
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <style type="text/css">
        table.ms-propertysheet
        {
            height: 100%;
        }
    </style>
    <table cellpadding="0" cellspacing="0" id="onetIDListForm">
        <tr>
            <td>
                <asp:Image ID="ContosoImage" ImageUrl="/_layouts/images/Contoso/imagePills.jpg" Style="float: left;
                    width: 150px;" runat="server" />
                <asp:Label ID="ContentMessage" runat="server"></asp:Label>
                <asp:Panel ID="Confirmation" Visible="false" runat="server">
                    <br />
                    <asp:DropDownList ID="Status" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="StatusValidator" ControlToValidate="Status" Display="Dynamic"
                        runat="server" ErrorMessage="(*) Required" />
                    <br />
                    <br />
                    <asp:Button ID="Submit" OnClick="Submit_Click" Text="Submit" runat="server" />
                    <asp:Button ID="Cancel" UseSubmitBehavior="false" OnClientClick="history.back(); return false;"
                        Text="Cancel" runat="server" />
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
