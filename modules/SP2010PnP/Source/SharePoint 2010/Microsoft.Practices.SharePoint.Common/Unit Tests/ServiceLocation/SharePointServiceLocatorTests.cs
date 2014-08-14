//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.ServiceLocation.Moles;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.Logging.Moles;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.SharePoint.Administration.Moles;
using Microsoft.SharePoint.Behaviors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Moles;
using Microsoft.Moles.Framework;
using Microsoft.SharePoint.Moles;
using Microsoft.Practices.SharePoint.Common.Tests.Behaviors;

[assembly: MoledType(typeof(System.DateTime))]

namespace Microsoft.Practices.SharePoint.Common.Tests.ServiceLocation
{
    [TestClass]
    public partial class SharePointServiceLocatorTests
    {
        [TestCleanup]        
        public void Cleanup()
        {
            SharePointServiceLocator.Reset();
            SharePointEnvironment.Reset();
        }

        [TestMethod]
        public void ReplaceCurrentServiceLocator_ChangeServiceLocator()
        {
            //Arrange
            var serviceLocator = new SIServiceLocator();

            //Act
            SharePointServiceLocator.ReplaceCurrentServiceLocator(serviceLocator);

            //Assert
            Assert.AreSame(serviceLocator, SharePointServiceLocator.GetCurrent());
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_ServiceLocatorIsLoadedWithTypes()
        {
            //Arrange
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ISomething, Something>("key") };
            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = typeMappings;
            BSPFarm.SetLocal();

            //Act
            var target = sharepointLocator.GetCurrent() as ActivatingServiceLocator;

            //Assert
            Assert.IsInstanceOfType(SharePointServiceLocator.GetCurrent(), typeof(ActivatingServiceLocator));
            Assert.IsTrue(target.IsTypeRegistered<ISomething>());
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_CanRegisterDifferentServiceLocator()
        {
            //Arrange
            var typeMappings = new List<TypeMapping> { 
                TypeMapping.Create<IServiceLocatorFactory, BIServiceLocatorFactory>() 
            };
            BSPFarm.SetLocal();
            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = typeMappings;


            //Act
            IServiceLocator target = sharepointLocator.GetCurrent();

            //Assert
            Assert.IsInstanceOfType(target, typeof(SIServiceLocator));
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_DefaultMappingsAreAdded()
        {
            //Arrange
            var f = new BSPConfiguredFarm();

            //Act
            var target = SharePointServiceLocator.GetCurrent();

            //Assert
            Assert.IsInstanceOfType(target.GetInstance<ILogger>(), typeof(SharePointLogger));
            Assert.IsInstanceOfType(target.GetInstance<ITraceLogger>(), typeof(TraceLogger));
            Assert.IsTrue((target as ActivatingServiceLocator).IsTypeRegistered<IEventLogLogger>());
            Assert.IsInstanceOfType(target.GetInstance<IHierarchicalConfig>(), typeof(HierarchicalConfig));
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_CanOverrideDefaultTypemapping()
        {
            //Arrange
            var typeMappings = new List<TypeMapping>{ TypeMapping.Create<ILogger, SBaseLogger>() };
            BSPFarm.SetLocal();
            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = typeMappings;

            //Act
            var target = sharepointLocator.GetCurrent().GetInstance<ILogger>();

            //Assert
            Assert.IsInstanceOfType(target, typeof(SBaseLogger));
        }

        [TestMethod]
        [HostType("Moles")]        
        public void GetCurrent_CallWithoutSharePoint_ThrowsNoSharePointContextException()
        {
            //Arrange
            MSPFarm.LocalGet = () => null;
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                IServiceLocator target = SharePointServiceLocator.GetCurrent();               
            }
            catch(NoSharePointContextException)
            {
                expectedExceptionThrown = true;
            }

            // Assert 
            Assert.IsTrue(expectedExceptionThrown);
        }

        [HostType("Moles")]
        [TestMethod]
         public void Current_AttemptLoadingFromCommonServiceLocator_ThrowsErrorMessage()
        {
            //Arrange
            var farm = BSPFarm.SetLocal();
            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = null;
            bool expectedException = false;

            var sl = sharepointLocator.GetCurrent();

            //Act
            try
            {
                var sl1 = ServiceLocator.Current;
            }
            catch (NotSupportedException)
            {
                expectedException = true;
            }

            // Assert 
            Assert.IsTrue(expectedException);
        }

        [HostType("Moles")]
        [TestMethod]
        public void GetCurrent_WithoutContext_UsesFarmMappings()
        {
            //Arrange
            BSPFarm.SetLocal();
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            var siteTypeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SBaseLogger>() };
            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = typeMappings;
            sharepointLocator.SiteTypeMappings = siteTypeMappings;
            

            //Act
            var target = sharepointLocator.GetCurrent().GetInstance<ILogger>();

            // Assert
            Assert.IsInstanceOfType(target, typeof(SharePointLogger));
        }

        [HostType("Moles")]
        [TestMethod]
        public void GetCurrent_WithContext_UsesSiteMappings()
        {
            //Arrange
            BSPFarm.SetLocal();
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            var siteTypeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SBaseLogger>() };
            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = typeMappings;
            sharepointLocator.SiteTypeMappings = siteTypeMappings;

            var site = new MSPSite()
            {
                IDGet = () => TestsConstants.TestGuid,
            };

            var context = new MSPContext
            {
                SiteGet = () => site,
            };

            MSPContext.CurrentGet = () => context;

            //Act
            var target = sharepointLocator.GetCurrent().GetInstance<ILogger>();

            // Assert
            Assert.IsInstanceOfType(target, typeof(SBaseLogger));
        }


        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_LoadsSiteTypeMappings()
        {
            //Arrange
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            var siteTypeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SBaseLogger>() };

            BSPFarm.SetLocal();
            var expectedSite = new BSPSite();
            expectedSite.ID = TestsConstants.TestGuid;

            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = typeMappings;
            sharepointLocator.SiteTypeMappings = siteTypeMappings;

            //Act
            var target = sharepointLocator.GetCurrent(expectedSite);
            var targetLogger = target.GetInstance<ILogger>();

            //Assert
            Assert.IsInstanceOfType(targetLogger, typeof(SBaseLogger));
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_LoadsFarmTypeMappings()
        {
            //Setup
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            BSPFarm.SetLocal();

            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = typeMappings;

            //Act
            var target = sharepointLocator.GetCurrent();
            var targetLogger = target.GetInstance<ILogger>();

            //Assert
            Assert.IsInstanceOfType(targetLogger, typeof(SharePointLogger));
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_AddNewFarmMappingIsAlsoAddedToSite()
        {
            //Setup
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            var farm = new MSPFarm();
            MSPFarm.LocalGet = ()=> farm;

            var expectedSite = new MSPSite()
            {
                 IDGet = () => TestsConstants.TestGuid
            };

            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = typeMappings;
            sharepointLocator.SiteTypeMappings = new List<TypeMapping>();

            var target = sharepointLocator.GetCurrent(expectedSite);     // get current to make sure it is in memory
            var farmLocator = sharepointLocator.GetCurrent() as ActivatingServiceLocator;
 
            //Act
            farmLocator.RegisterTypeMapping<ILogger, SBaseLogger>();
            var targetLogger = target.GetInstance<ILogger>();

            //Assert
            Assert.IsInstanceOfType(targetLogger, typeof(SBaseLogger));
        }



        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_AddNewFarmMappingAlreadyDefinedInSiteUsesSite()
        {
            //Setup
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            BSPFarm.SetLocal();
            var expectedSite = new BSPSite {ID = TestsConstants.TestGuid};

            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = new List<TypeMapping>();
            sharepointLocator.SiteTypeMappings = typeMappings;

            var target = sharepointLocator.GetCurrent(expectedSite);     // get current to make sure it is in memory
            var farmLocator = sharepointLocator.GetCurrent() as ActivatingServiceLocator;

            //Act
            farmLocator.RegisterTypeMapping<ILogger, SBaseLogger>();
            var targetLogger = target.GetInstance<ILogger>();

            //Assert
            Assert.IsInstanceOfType(targetLogger, typeof(SharePointLogger));
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_SiteUsesCachedInstance()
        {
            //Setup
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            BSPFarm.SetLocal();
            var expectedSite = new BSPSite {ID = TestsConstants.TestGuid};

            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = new List<TypeMapping>();
            sharepointLocator.SiteTypeMappings = typeMappings;
            sharepointLocator.SiteLastUpdatedRetVal = DateTime.Now;

            var target = sharepointLocator.GetCurrent(expectedSite);     // get current to make sure it is in memory
            Assert.IsTrue(sharepointLocator.SiteGotTypeMappingsFromConfig); //getting from config and populating cache
            
            sharepointLocator.SiteGotTypeMappingsFromConfig = false;  // reset getting config...

            //Act
            sharepointLocator.GetCurrent(expectedSite);


            //Assert
            Assert.IsFalse(sharepointLocator.SiteGotTypeMappingsFromConfig);
        }


        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_SiteTimeoutReloadWorks()
        {
            //Setup
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            var f = new BSPConfiguredFarm();

            var expectedSite = new BSPSite {ID = TestsConstants.TestGuid};
            var time = new DateTime(2010, 5, 30);

            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = new List<TypeMapping>();
            sharepointLocator.SiteTypeMappings = typeMappings;
            
            MDateTime.NowGet = () => time;
            var target = sharepointLocator.GetCurrent(expectedSite);     // get current to make sure it is in memory
            time = time.AddSeconds(SharePointServiceLocator.SiteCachingTimeoutInSeconds + 1);
            sharepointLocator.SiteLastUpdatedRetVal = time;

            //Act
            var initialLogger = target.GetInstance<ILogger>();
            sharepointLocator.SiteTypeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SBaseLogger>() };
            var targetLogger = sharepointLocator.GetCurrent(expectedSite).GetInstance<ILogger>();

            //Assert
            Assert.IsInstanceOfType(initialLogger, typeof(SharePointLogger));
            Assert.IsInstanceOfType(targetLogger, typeof(SBaseLogger));
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_SiteTimeoutReloadCachesInstanceAfterReload()
        {
            //Setup
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            var f = new BSPConfiguredFarm();

            var expectedSite = new BSPSite {ID = TestsConstants.TestGuid};
            var time = new DateTime(2010, 5, 30);

            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.FarmTypeMappings = new List<TypeMapping>();
            sharepointLocator.SiteTypeMappings = typeMappings;
            MDateTime.NowGet = () => time;

            sharepointLocator.SiteGotTypeMappingsFromConfig = false;

            sharepointLocator.GetCurrent(expectedSite);

            Assert.IsTrue(sharepointLocator.SiteGotTypeMappingsFromConfig);

            sharepointLocator.SiteGotTypeMappingsFromConfig = false;
            time = time.AddSeconds(SharePointServiceLocator.SiteCachingTimeoutInSeconds + 1);
            sharepointLocator.SiteLastUpdatedRetVal = time;
            var targetLogger = sharepointLocator.GetCurrent(expectedSite).GetInstance<ILogger>();

            //Act
            sharepointLocator.SiteGotTypeMappingsFromConfig = false;
            targetLogger = sharepointLocator.GetCurrent(expectedSite).GetInstance<ILogger>();
            
            //Assert
            Assert.IsFalse(sharepointLocator.SiteGotTypeMappingsFromConfig);
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_SiteTimeoutReloadDoesntDropProgrammaticAdds()
        {
            //Setup
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            BSPFarm.SetLocal();
            var expectedSite = new BSPSite {ID = TestsConstants.TestGuid};
            var time = new DateTime(2010, 5, 30);

            var sharepointLocator = new TestableSharePointServiceLocator
                                        {
                                            FarmTypeMappings = new List<TypeMapping>(),
                                            SiteTypeMappings = typeMappings
                                        };
            MDateTime.NowGet = () => time;
            var target = sharepointLocator.GetCurrent(expectedSite);     // get current to make sure it is in memory
            ((ActivatingServiceLocator)target).RegisterTypeMapping<ISomething, Something>();

            time = time.AddSeconds(SharePointServiceLocator.SiteCachingTimeoutInSeconds + 1);
            sharepointLocator.SiteLastUpdatedRetVal = time;

            //Act
            var initialLogger = target.GetInstance<ILogger>();
            var targetSomething = sharepointLocator.GetCurrent(expectedSite).GetInstance<ISomething>();

            //Assert
            Assert.IsInstanceOfType(initialLogger, typeof(SharePointLogger));
            Assert.IsInstanceOfType(targetSomething, typeof(Something));
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_SiteTimeoutReloadDoesntDropProgrammaticOverride()
        {
            //Setup
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            BSPFarm.SetLocal();
            var expectedSite = new BSPSite {ID = TestsConstants.TestGuid};
            var time = new DateTime(2010, 5, 30);

            var sharepointLocator = new TestableSharePointServiceLocator
                                        {
                                            FarmTypeMappings = new List<TypeMapping>(),
                                            SiteTypeMappings = typeMappings
                                        };

            MDateTime.NowGet = () => time;
            var target = sharepointLocator.GetCurrent(expectedSite);     // get current to make sure it is in memory
            time = time.AddSeconds(SharePointServiceLocator.SiteCachingTimeoutInSeconds+1);
            sharepointLocator.SiteLastUpdatedRetVal = time;

            //Act
            var initialLogger = target.GetInstance<ILogger>();
            ((ActivatingServiceLocator)target).RegisterTypeMapping<ILogger, SBaseLogger>();
            var targetLogger = sharepointLocator.GetCurrent(expectedSite).GetInstance<ILogger>();
 
            //Assert
            Assert.IsInstanceOfType(initialLogger, typeof(SharePointLogger));
            Assert.IsInstanceOfType(targetLogger, typeof(SBaseLogger));
        }

        class FakeLocator: SIServiceLocator
        {
            public FakeLocator()
            {
                this.GetInstance<ILogger>(()=> new SILogger());
            }
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetCurrent_CustomServiceLocatorReloadsSiteOnChange()
        {
            //Arrange
            var f = new BSPConfiguredFarm(); ;
            var sharepointLocator = new TestableSharePointServiceLocator();
            //Arrange
            BIServiceLocatorFactory.Reset();
            BIServiceLocatorFactory.LocatorTypeToUse = typeof (FakeLocator);
            var farmTypeMappings = new List<TypeMapping> { 
                TypeMapping.Create<IServiceLocatorFactory, BIServiceLocatorFactory>() 
            };

            sharepointLocator.FarmTypeMappings = farmTypeMappings;
            var expectedSite = new BSPSite();
            var typeMappings = new List<TypeMapping> { TypeMapping.Create<ILogger, SharePointLogger>() };
            expectedSite.ID = TestsConstants.TestGuid;
            var time = new DateTime(2010, 5, 30);
            sharepointLocator.SiteTypeMappings = typeMappings;
            MDateTime.NowGet = () => time;
            var target = sharepointLocator.GetCurrent(expectedSite);     // get current to make sure it is in memory
            time = time.AddSeconds(SharePointServiceLocator.SiteCachingTimeoutInSeconds + 1);
            sharepointLocator.SiteLastUpdatedRetVal = time;

            //Act
            var targetLogger = sharepointLocator.GetCurrent(expectedSite).GetInstance<ILogger>();

            //Assert
            Assert.IsTrue(BIServiceLocatorFactory.LoadCount == 2);
        }

        [TestMethod]
        public void SiteCachingTimeoutInSeconds_WithNoConfiguration_ReturnsDefaultValue()
        {
            //Arrange
            int expected = 60;
            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.SiteCachingTimeoutInSecondsRetVal = -1;

            //Act
            int target = sharepointLocator.GetSiteCacheInterval();

            //Assert
            Assert.AreEqual<int>(expected, target);
        }

        [TestMethod]
        public void SiteCachingTimeoutInSeconds_WithConfiguration_ReturnsConfiguredValue()
        {
            //Arrange
            int expected = 30;
            var sharepointLocator = new TestableSharePointServiceLocator();
            sharepointLocator.SiteCachingTimeoutInSecondsRetVal = 30;

            //Act
            int target = sharepointLocator.GetSiteCacheInterval();

            //Assert
            Assert.AreEqual<int>(expected, target);
        }

        [TestMethod]
        public void SiteCachingTimeoutInSeconds_SetWithInvalidValue_ThrowsArgumentException()
        {
            //Arrange
             bool exceptionThrown = false;
           
            //Act
             try
             {
                 SharePointServiceLocator.SiteCachingTimeoutInSeconds = -1;
             }
             catch (ArgumentException)
             {
                 exceptionThrown = true;
             }

            //Assert
             Assert.IsTrue(exceptionThrown, "ArgumentException expected on negative cache timeout");
        }
    }
}
