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
namespace Contoso.AccountingManagement.BusinessEntities
{
    /// <summary>
    /// Transaction object contains data related to a individual debit/credit to cost center's account.
    /// </summary>
    public class Transaction
    {
        #region public properties

        /// <summary>
        /// Bucket is the category of credit/debit
        /// </summary>
        public string Bucket
        {
            get; 
            set;
        }

        /// <summary>
        /// Cost Center is the organization or department related to a credit/debit
        /// </summary>
        public string CostCenter 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// The Description of the credit/debit
        /// </summary>
        public string Description
        {
            get; 
            set;
        }

        /// <summary>
        /// the amount of debit/credit
        /// </summary>
        public float Amount
        {
            get; 
            set;
        }

        #endregion
    }
}
