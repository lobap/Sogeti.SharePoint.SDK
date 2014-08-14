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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Contoso.Common.BusinessEntities;
using Contoso.Common.ExceptionHandling;
using Contoso.Common.Repositories;
using Contoso.PartnerPortal.ProductCatalog.WebParts.RelatedParts;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contoso.PartnerPortal.ProductCatalog.Tests
{
    [TestClass]
    public class RelatedPartsPresenterFixture
    {
        [TestCleanup]
        public void Cleanup()
        {
            SharePointServiceLocator.Reset();
        }

        [TestMethod]
        public void CanLoadRelatedPartsOnClick()
        {
            MockRelatedPartsView mockView = new MockRelatedPartsView();

            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                .RegisterTypeMapping<IProductCatalogRepository, MockProductCatalog>(InstantiationType.AsSingleton));

            MockProductCatalog catalog = SharePointServiceLocator.Current.GetInstance<IProductCatalogRepository>() as MockProductCatalog;
            catalog.Parts = new List<Part> { new Part() };

            RelatedPartsPresenter target = new RelatedPartsPresenter(mockView);

            target.LoadParts("123456");

            Assert.AreEqual(1, mockView.Parts.Count());
            Assert.IsTrue(mockView.DataBound);

        }

        [TestMethod]
        public void WhenExceptionOccursErrorIsDisplayedInVisualizer()
        {
            MockRelatedPartsView mockView = new MockRelatedPartsView();

            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                .RegisterTypeMapping<IProductCatalogRepository, MockProductCatalog>()
                .RegisterTypeMapping<ILogger, MockLogger>());

            MockErrorVisualizer errorVisualizer = new MockErrorVisualizer();

            RelatedPartsPresenter target = new RelatedPartsPresenter(mockView);
            target.ErrorVisualizer = errorVisualizer;

            target.LoadParts("Error");

            Assert.IsTrue(errorVisualizer.DefaultErrorDisplayed);
        }

        [TestMethod]
        public void WhenNoPartsErrorMessageIsSetOnView()
        {
            MockRelatedPartsView mockView = new MockRelatedPartsView();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                .RegisterTypeMapping<IProductCatalogRepository, MockProductCatalog>()
                .RegisterTypeMapping<ILogger, MockLogger>());

            MockErrorVisualizer errorVisualizer = new MockErrorVisualizer();

            RelatedPartsPresenter target = new RelatedPartsPresenter(mockView);
            target.ErrorVisualizer = errorVisualizer;

            target.LoadParts("sku");

            Assert.AreEqual("No parts found.", mockView.ErrorMessage);
        }


    }

    public class MockLogger : BaseLogger
    {
        protected override void WriteToOperationsLog(string message, int eventId, EventLogEntryType severity, string category)
        {
            
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category)
        {
            
        }
    }

    internal class MockErrorVisualizer : IErrorVisualizer
    {
        public bool DefaultErrorDisplayed;

        public void ShowDefaultErrorMessage()
        {
            DefaultErrorDisplayed = true;
        }

        public void ShowErrorMessage(string errorMessage)
        {
        }
    }

    internal class MockRelatedPartsView : IRelatedPartsView
    {
        public bool DataBound;

        public IEnumerable<Part> Parts
        {
            get; set;

        }

        public void DataBind()
        {
            DataBound = true;
        }

        public string ErrorMessage
        {
            get; set;
        }
    }

    internal class MockProductCatalog : IProductCatalogRepository
    {
        public IList<Part> Parts;

        public Product GetProductBySku(string sku)
        {
            return null;
        }

        public IEnumerable<Product> GetProductsByCategory(string categoryId)
        {
            yield break;
        }

        public Category GetCategoryById(string categoryId)
        {
            return null;
        }

        public IEnumerable<Category> GetChildCategoriesByCategory(string categoryId)
        {
            yield break;
        }

        public IEnumerable<Part> GetPartsByProductSku(string productSku)
        {
            if (productSku == "Error")
            {
                throw new AbandonedMutexException("Something goes horribly wrong!");
            }

            return Parts;
        }

        public string GetCategoryProfileUrl()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }

}
