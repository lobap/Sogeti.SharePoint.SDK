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
using System.Runtime.Serialization;
using System;

namespace Contoso.LOB.Services.BusinessEntities
{
    [DataContract(Namespace = "http://Contoso.LOB.Services/2009/01/BusinessEntities")]
    public class Product
    {
        [DataMember]
        public string Name{ get; set; }

        [DataMember]
        public string ShortDescription { get; set; }

        [DataMember]
        public string LongDescription { get; set; }

        [DataMember]
        public string Sku { get; set; }

        [DataMember]
        public string CategoryId { get; set; }

        [DataMember]
        public string ThumbnailImagePath { get; set; }

        [DataMember]
        public string ImagePath { get; set; }
    }
}
