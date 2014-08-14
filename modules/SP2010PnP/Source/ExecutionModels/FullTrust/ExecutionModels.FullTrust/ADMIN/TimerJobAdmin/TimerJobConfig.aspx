<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimerJobConfig.aspx.cs"
    Inherits="ExecutionModels.FullTrust.CentralAdminForms.TimerJobConfig" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:content id="PageHead" contentplaceholderid="PlaceHolderAdditionalPageHead" runat="server">
</asp:content>
<asp:content id="Main" contentplaceholderid="PlaceHolderMain" runat="server">
    <asp:UpdatePanel ID="TimerJobUpdatePanel" runat="server" ChildrenAsTriggers="false"
        UpdateMode="Conditional">
        <ContentTemplate>
            <table width="525px" cellpadding="1" cellspacing="1">
                <tr>
                    <td valign="top" align="left" colspan="2">
                        <asp:Label ID="TitleLabel" runat="server" ForeColor="Black" Font-Bold="true" Font-Size="Small"
                            Text="Approved Estimates Aggregation Timer Job Properties"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left" colspan="2" style="border: solid 2px Black; background: #e9e9e9; height:45px; overflow:auto; white-space:normal">
                      <asp:DataList id="PropertyInfoDataList" runat="server" width="100%">                           
                           <ItemTemplate>
                              <%# DataBinder.Eval(Container.DataItem,"Key") %> :  <%# DataBinder.Eval(Container.DataItem,"Value") %>
                            </ItemTemplate>
                         </asp:DataList>

                        <asp:Label ID="PropertyInfoLabel" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left">
                        <asp:Label ID="DestinationSiteLabel" runat="server" Text="Destination site:"></asp:Label>
                    </td>
                    <td valign="top" align="left">
                        <asp:TextBox Width="300px" ID="DestinationSiteTextBox" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left">
                        <asp:Label ID="ListNameLabel" runat="server" Text="Approved Estimates List Name:"></asp:Label>
                    </td>
                    <td valign="top" align="left">
                        <asp:TextBox Width="300px" ID="ListNameTextBox" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left">
                        <asp:Label ID="SiteNamesLabel" runat="server" Text="Sites To Query"></asp:Label><br />
                        <asp:Label ID="DetailsLabel" runat="server" Font-Size="XX-Small" Text="(root-relative, semi-colon separated list)"></asp:Label>
                    </td>
                    <td valign="top" align="left">
                        <asp:TextBox Width="300px" ID="SiteNamesTextBox" runat="server" TextMode="MultiLine"
                            Height="75px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="ApplyButton" runat="server" Text="Apply Changes" />
                        <asp:Button ID="RunButton" runat="server" Text="Run Now >>" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ApplyButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="RunButton" EventName="Click" />            
        </Triggers>
    </asp:UpdatePanel>
</asp:content>
<asp:content id="PageTitle" contentplaceholderid="PlaceHolderPageTitle" runat="server">
    Approved Estimates Aggregation Timer Job
</asp:content>
<asp:content id="PageTitleInTitleArea" contentplaceholderid="PlaceHolderPageTitleInTitleArea"
    runat="server">
    Approved Estimates Aggregation Timer Job
</asp:content>
