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
using Contoso.TrainingManagement.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Contoso.AccountingManagement.Services;
using Contoso.HRManagement.Services;

namespace Contoso.TrainingManagement.Tests
{
    [TestClass]
    public class ServiceLocatorFixture
    {
        private ServiceLocator serviceLocator = ServiceLocator.GetInstance();

        [TestInitialize]
        public void TestInit()
        {
            ConfigurationService.LoadTypeMetadata();
            serviceLocator.Reset();
        }

        [TestMethod]
        public void CanRegisterTypeByInterface()
        {
            serviceLocator.Register<ITestInterface>(typeof(TestInstance));

            Assert.IsInstanceOfType(serviceLocator.Get<ITestInterface>(), typeof(TestInstance));
        }

        [TestMethod]
        public void GetInstanceReturnsSameInstance()
        {
            ServiceLocator serviceLocator1 = ServiceLocator.GetInstance();
            ServiceLocator serviceLocator2 = ServiceLocator.GetInstance();

            Assert.AreSame(serviceLocator2, serviceLocator1);
        }

        [TestMethod]
        public void ServiceLocatorInitializesServices()
        {
            Assert.IsNotNull(serviceLocator.Get<IHRManager>());
            Assert.IsNotNull(serviceLocator.Get<IAccountingManager>());
            Assert.IsNotNull(serviceLocator.Get<IListItemRepository>());
            Assert.IsNotNull(serviceLocator.Get<IRegistrationRepository>());
            Assert.IsNotNull(serviceLocator.Get<ITrainingCourseRepository>());
            Assert.IsNotNull(serviceLocator.Get<IRegistrationApprovalTaskRepository>());
        }

        [TestMethod]
        public void RegisterServiceOfExistingType()
        {
            ServiceLocator.Clear();
            serviceLocator.Register<ITestInterface>(typeof(TestInstance));
            serviceLocator.Register<ITestInterface>(typeof(TestInstanceThree));
            Assert.AreEqual(1, serviceLocator.Count);
            Assert.IsInstanceOfType(serviceLocator.Get<ITestInterface>(), typeof(TestInstanceThree));
        }
    }

    internal class TestInstanceTwo : ITestInterfaceTwo
    {

    }

    internal interface ITestInterfaceTwo
    {
    }


    internal class TestInstance : ITestInterface
    {
    }

    internal class TestInstanceThree : ITestInterface
    {
    }

    internal interface ITestInterface
    {
    }
}
