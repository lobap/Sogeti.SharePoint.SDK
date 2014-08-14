<%@ Control Language="C#" AutoEventWireup="true" Inherits="Contoso.PartnerPortal.PartnerCentral.Controls.PartnerRollupControl, Contoso.PartnerPortal.PartnerCentral, Version=1.0.0.0, Culture=neutral, PublicKeyToken=019398de256d0b98" %>
<%@ Import Namespace="Contoso.Common.BusinessEntities"%>
<%@ Assembly Name="Contoso.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8a83c3e0a2fe6ae9" %>
<asp:Repeater runat="server" ID="PartnerRepeater">
    <HeaderTemplate>
        <table cellspacing="0" cellpadding="5" width="100%" style="border:thin black solid">
            <tr style="background-color: D7D7D7;" >
	        	<td colspan="4" style="border-bottom: thin black dotted">Partner Name</td>
	       	</tr>
            <tr style="background-color: D7D7D7;">
                <td style="border-bottom:thin black solid">Title</td>
                <td style="border-bottom:thin black solid">Assigned To</td>
                <td style="border-bottom:thin black solid">Status</td>
                <td style="border-bottom:thin black solid">Priority</td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td colspan="4" style="border-bottom: thin black dotted;">
                <a href="<%# DataBinder.Eval(Container.DataItem, "Partner.SiteCollectionUrl") %>" ><%# DataBinder.Eval(Container.DataItem, "Partner.PartnerId") %></a></td>
        </tr>
        <h1>
        <asp:Repeater runat="server" ID="IncidentTaskRepeater" DataSource='<%# ((Contoso.PartnerPortal.PartnerCentral.PartnerRollup.PartnerRollupSearchResult)Container.DataItem).IncidentTasks %>'>
            <ItemTemplate>
                <tr>
                    <td style="border-bottom:thin black solid; background-color:#FFFF99">
                        <a href="<%# DataBinder.Eval(Container.DataItem, "Path") %>" ><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
                    </td>
                    <td style="border-bottom:thin black solid; background-color:#FFFF99" >
                        <%# DataBinder.Eval(Container.DataItem, "AssignedTo" ) %>
                    </td>
                    <td style="border-bottom:thin black solid; background-color:#FFFF99">
                        <%# DataBinder.Eval(Container.DataItem, "Status") %>
                    </td>
                    <td style="border-bottom:thin black solid; background-color:#FFFF99">
                        <%# DataBinder.Eval(Container.DataItem, "Priority" ) %>
                    </td>                    
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>