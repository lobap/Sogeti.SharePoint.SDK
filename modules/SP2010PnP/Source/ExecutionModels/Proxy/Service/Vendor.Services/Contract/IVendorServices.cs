//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Vendor.Services.Contract
{
    [ServiceContract]
    public interface IVendorServices
    {
        [OperationContract]
        double GetAccountsPayable(string vendorName);

        [OperationContract]
        string WhoAmI();
    }

}