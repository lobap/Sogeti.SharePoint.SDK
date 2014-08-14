//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;

namespace Service.IntegrationTests
{
    using Vendor.Services.Contract;

    [TestClass]
    public class VendorSystemProxyFixture
    {
        [TestMethod]
        public void CanGetAccountsPayable()
        {
            // Arrange
            const string vendorId = "Contoso";
            var binding = new BasicHttpBinding();
            binding.Security = new BasicHttpSecurity()
            {
                Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                Transport = new HttpTransportSecurity() { ClientCredentialType = HttpClientCredentialType.Windows }
            };
            EndpointAddress addr = new EndpointAddress("http://localhost:81/Vendor/Service.svc");
            ChannelFactory<IVendorServices> factory = new ChannelFactory<IVendorServices>(binding, addr);
            IVendorServices proxy = factory.CreateChannel();

            // Act
            double accountsPayable = proxy.GetAccountsPayable(vendorId);

            // Assert
            Assert.IsTrue(accountsPayable >= 0.0);

            factory.Close();
        }
    }
}
