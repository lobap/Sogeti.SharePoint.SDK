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
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml.Serialization;

using Contoso.LOB.Services.BusinessEntities;

namespace Contoso.LOB.Services.Contracts
{
    [ServiceContract(Namespace = "http://Contoso.LOB.Services/2009/01")]
    interface IProductCatalog
    {
        [OperationContract]
        IList<string> GetProductSkus();

        [OperationContract]
        [FaultContract(typeof(LOBServicesFault))]
        Product GetProductBySku(string sku);

        [OperationContract]
        IList<Product> GetProductsByCategory(string categoryId);

        [OperationContract]
        IEnumerable<Product> GetProducts();

        [OperationContract]
        Category GetCategoryById(string categoryId);

        [OperationContract]
        IList<Category> GetCategories();

        [OperationContract]
        IList<Category> GetChildCategoriesByCategory(string categoryId);

        [OperationContract]
        IEnumerable<Part> GetPartsByProductSku(string productSku);
    }
}