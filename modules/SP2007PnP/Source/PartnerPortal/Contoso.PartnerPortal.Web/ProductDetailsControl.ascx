<%@ Control Language="C#" AutoEventWireup="true" Inherits="Contoso.PartnerPortal.ProductCatalog.Controls.ProductDetailsControl, Contoso.PartnerPortal.ProductCatalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0dcd9137292eac97" %>
<%@ Assembly Name="Contoso.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8a83c3e0a2fe6ae9" %>

<table style="width:100%;">
    <tr>
        <td valign="top">
            <img src="<%# this.Product.ImagePath.ToString() %>" alt="product image" />
        </td>
        <td valign="top">
            <h1><%# this.Product.Name %></h1>
            <p><%# this.Product.LongDescription %></p>
            <h2>SKU:&nbsp;<%# this.Product.Sku %></h2>
        </td>
    </tr>
</table>
