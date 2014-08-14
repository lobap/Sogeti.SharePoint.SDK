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
using System.Linq;
using System.Text;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Tests.Logging;
using Microsoft.SharePoint.Moles;
using Microsoft.SharePoint.Behaviors;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using Microsoft.SharePoint.Administration.Moles;
using Microsoft.SharePoint.Utilities.Moles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.Practices.SharePoint.Common.Moles;
using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SharePoint.Common.Tests.ServiceLocation
{
    [TestClass]
    public class ServiceLocatorConfigTests
    {
        [TestCleanup]
        public void TearDown()
        {
            SharePointEnvironment.Reset();
        }

        [TestMethod]
        public void CTOR_ThrowsArgumentNullException_WithNullManager()
        {
            //Arrange
            bool argumentNullExceptionThrown = false;

            //act
            try
            {
                var target = new ServiceLocatorConfig((IConfigManager)null);
            }
            catch (ArgumentNullException)
            {
                argumentNullExceptionThrown = true;
            }

            //assert
            Assert.IsTrue(argumentNullExceptionThrown);
        }

        [TestMethod]
        public void SitePropertySetter_ThrowsArgumentNullException_WithNullSite()
        {
            //Arrange
            bool argumentNullExceptionThrown = false;
            var target = new ServiceLocatorConfig();

            //act
            try
            {
                target.Site = null;
            }
            catch (ArgumentNullException)
            {
                argumentNullExceptionThrown = true;
            }

            //assert
            Assert.IsTrue(argumentNullExceptionThrown);
        }

        [TestMethod]
        [HostType("Moles")]
        public void RegisterTypeMapping_SaveNewMapping()
        {
            // Arrange
            object value = null;
            var rootConfig = new BIPropertyBag();
            BSPFarm.SetLocal();

            var mgr = new SIConfigManager
            {
                ContainsKeyInPropertyBagStringIPropertyBag = (stringValue, propertyBag) => false,
                SetInPropertyBagStringObjectIPropertyBag = (strValue, obj, propertyBag) => value = obj as List<TypeMapping>,
                 GetPropertyBagConfigLevel = (level) => rootConfig,
                CanAccessFarmConfigGet = () => true
            };

            mgr.GetFromPropertyBagStringIPropertyBag<ServiceLocationConfigData>((key, bag) => null);
 
            //Act
            var target = new ServiceLocatorConfig(mgr);
            target.RegisterTypeMapping<ISomething, Something>();
            var typeMappings = value as List<TypeMapping>;


            //Assert
            Assert.IsNotNull(typeMappings);
            Assert.IsTrue(typeMappings.Count >= 1);
            TypeMapping mapping = typeMappings[0];
            Assert.AreEqual("Microsoft.Practices.SharePoint.Common.Tests, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null"
                , mapping.FromAssembly);
            Assert.AreEqual("Microsoft.Practices.SharePoint.Common.Tests.ServiceLocation.ISomething, Microsoft.Practices.SharePoint.Common.Tests, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null"
                , mapping.FromType);
            Assert.AreEqual("Microsoft.Practices.SharePoint.Common.Tests, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null"
                , mapping.ToAssembly);
            Assert.AreEqual("Microsoft.Practices.SharePoint.Common.Tests.ServiceLocation.Something, Microsoft.Practices.SharePoint.Common.Tests, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null"
                , mapping.ToType);
        }

        [TestMethod]
        [HostType("Moles")]
        public void RegisterTypeMapping_SaveNewMappingWithNoContext_ThrowsInvalidOperationException()
        {
            // Arrange
            object value = null;
            var rootConfig = new BIPropertyBag();
            bool expectedExceptionThrown = false;

            var mgr = new SIConfigManager
            {
                ContainsKeyInPropertyBagStringIPropertyBag = (stringValue, propertyBag) => false,
                SetInPropertyBagStringObjectIPropertyBag = (strValue, obj, propertyBag) => value = obj as List<TypeMapping>,
                GetPropertyBagConfigLevel = (level) => rootConfig,
                CanAccessFarmConfigGet = () => false
            };

            mgr.GetFromPropertyBagStringIPropertyBag<ServiceLocationConfigData>((key, bag) => null);

            //Act
            var target = new ServiceLocatorConfig(mgr);
            try
            {
                target.RegisterTypeMapping<ISomething, Something>();
            }
            catch (InvalidOperationException)
            {
                expectedExceptionThrown = true;
            }

            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        [HostType("Moles")]
        public void RegisterTypeMapping_SaveNewMappingWithSite()
        {
            var mappings = new List<TypeMapping>();
            string savedkey = null;
            object savedValue = null;
            var configData = new ServiceLocationConfigData(mappings);
            BSPFarm.SetLocal();
            const string expectedKey = "Microsoft.Practices.SharePoint.Common.TypeMappings";
            var web = new MSPWeb();
            var propBag = new BIPropertyBag();

            var mgr = new SIConfigManager()
            {
                  SetInPropertyBagStringObjectIPropertyBag= (key, value, bag) =>
                    {
                        savedkey = key;
                        savedValue = value;
                    },

                ContainsKeyInPropertyBagStringIPropertyBag = (key, bag) =>
                    {
                        if(key == expectedKey)
                            return true;
                        return false;
                    },
                CanAccessFarmConfigGet = ()=>true,
                GetPropertyBagConfigLevel = (level) => propBag,
                SetWebSPWeb = (w) => { },
            };

            mgr.GetFromPropertyBagStringIPropertyBag<ServiceLocationConfigData>((key, bag) =>
                {
                    if(key == expectedKey)
                        return configData;
                    return null;
                });

            var configSite = new MSPSite()
            {
                 RootWebGet = ()=> web
            }; 

            var target = new ServiceLocatorConfig(mgr);
            target.Site = configSite;

            //act
            target.RegisterTypeMapping<ISomething, Something>();

            //assert
            Assert.IsNotNull(savedValue);
            Assert.AreEqual(expectedKey, savedkey);
        }



        [TestMethod]
        [HostType("Moles")]
        public void GetTypeMappings_GetAllMappings()
        {
            //Arrange
            var setMappings = new List<TypeMapping> {
                new TypeMapping { FromAssembly = "1" },
                new TypeMapping { FromAssembly = "2" },
                new TypeMapping { FromAssembly = "3" } 
            };
            var configData = new ServiceLocationConfigData(setMappings);
            var bag = new BIPropertyBag();
            BSPFarm.SetLocal();

            var cfgMgr = new SIConfigManager
            {
                ContainsKeyInPropertyBagStringIPropertyBag = (strValue, propertyBag) => true,
                CanAccessFarmConfigGet = () => true,
                GetPropertyBagConfigLevel = (configlevel) => bag
             };

            cfgMgr.BehaveAsDefaultValue();
            cfgMgr.GetFromPropertyBagStringIPropertyBag<ServiceLocationConfigData>((strValue, property) => configData);

            //Act
            var target = new ServiceLocatorConfig(cfgMgr);
            IEnumerable<TypeMapping> registeredTypeMappings = target.GetTypeMappings();

            //Assert
            Assert.AreEqual(3, registeredTypeMappings.Count());
        }

        [TestMethod]
        [HostType("Moles")]
        public void RegisterTypeMapping_SetANewMappingForExistingMapping()
        {
            //Arrange
            List<TypeMapping> mappings = null;
            var propertyBag = new BIPropertyBag();
            BSPFarm.SetLocal();

            var configMgr = new SIConfigManager
            {
                ContainsKeyInPropertyBagStringIPropertyBag = (stringValue, farm) => false,
                SetInPropertyBagStringObjectIPropertyBag = (strValue, obj, farm) => mappings = obj as List<TypeMapping>,
                CanAccessFarmConfigGet = () => true,
                GetPropertyBagConfigLevel = (configlevel) => propertyBag
            };
            configMgr.BehaveAsDefaultValue();

            //Act
            var target = new ServiceLocatorConfig(configMgr);
            target.RegisterTypeMapping<ISomething, Something>();
            target.RegisterTypeMapping<ISomething, SomethingElse>();
            TypeMapping mapping = mappings.First();

            //Assert
            Assert.IsTrue(mapping.ToType.Contains("SomethingElse"));
        }

        [TestMethod]
        [HostType("Moles")]
        public void RegisterTypeMapping_RegisterMappingWithAKey()
        {
            //Arrange
            BSPFarm.SetLocal();
            List<TypeMapping> typeMappings = new List<TypeMapping>();
            var configData = new ServiceLocationConfigData();
            var propBag = new BIPropertyBag();

            int savedCnt = 0;
            var configMgr = new SIConfigManager
            {
                ContainsKeyInPropertyBagStringIPropertyBag = (s, propertybag) => typeMappings != null,
                SetInPropertyBagStringObjectIPropertyBag = (s, obj, propertybag) => 
                { 
                    savedCnt++;
                },
                CanAccessFarmConfigGet = () => true,
                GetPropertyBagConfigLevel = (configlevel) => propBag,
            };
            configMgr.GetFromPropertyBagStringIPropertyBag<ServiceLocationConfigData>((key, propertybag) => configData);

            //Act
            var target = new ServiceLocatorConfig(configMgr as IConfigManager);
            target.RegisterTypeMapping<ISomething, Something>("key1");
            target.RegisterTypeMapping<ISomething, Something>("key2");

            //Assert
            Assert.IsTrue(configData.Count == 2);
            Assert.IsTrue(savedCnt == 2);
            TypeMapping mapping1 = configData[0];
            TypeMapping mapping2 = configData[1];
            Assert.AreEqual("key1", configData[0].Key);
            Assert.AreEqual("key2", configData[1].Key);
        }

        [TestMethod]
        [HostType("Moles")]
        public void RemoveTypeMapping_RemoveAMapping()
        {
            //Arrange
            var typeMappings = new List<TypeMapping> {
                new TypeMapping() { FromAssembly = "1" },
                new TypeMapping() { FromAssembly = "2" },
                new TypeMapping() { FromAssembly = "3" }
            };
            var config = new ServiceLocationConfigData(typeMappings);
            var bag = new BIPropertyBag();
            BSPFarm.SetLocal();
            var cfgMgr = new SIConfigManager
            {
                ContainsKeyInPropertyBagStringIPropertyBag = (key, propertyBag) => true,
                CanAccessFarmConfigGet = () => true,
                GetPropertyBagConfigLevel = (configlevel) => bag,
            };

            cfgMgr.GetFromPropertyBagStringIPropertyBag<ServiceLocationConfigData>((key, propetyBag) => config);
            cfgMgr.BehaveAsDefaultValue();

            //Act
            var target = new ServiceLocatorConfig(cfgMgr);
            target.RemoveTypeMapping(typeMappings[0]);
            List<TypeMapping> registeredTypeMappings =
                target.GetTypeMappings().ToList();

            //Assert
            Assert.AreEqual(2, registeredTypeMappings.Count);
            Assert.AreSame(typeMappings[1], registeredTypeMappings[0]);
            Assert.AreSame(typeMappings[2], registeredTypeMappings[1]);
        }

        [TestMethod]
        [HostType("Moles")]
        public void RemoveTypeMappingsGeneric_RemoveAMapping()
        {
            //Arrange
            var typeMappings = new List<TypeMapping> {
                TypeMapping.Create<ILogger, SharePointLogger>(),
                TypeMapping.Create<IConfigManager, ConfigManager>(),
                TypeMapping.Create<IHierarchicalConfig, HierarchicalConfig>()
            };

            var config = new ServiceLocationConfigData(typeMappings);
            var bag = new BIPropertyBag();
            BSPFarm.SetLocal();
            var cfgMgr = new SIConfigManager
            {
                ContainsKeyInPropertyBagStringIPropertyBag = (key, propertyBag) => true,
                CanAccessFarmConfigGet = () => true,
                GetPropertyBagConfigLevel = (configlevel) => bag,
            };

            cfgMgr.GetFromPropertyBagStringIPropertyBag<ServiceLocationConfigData>((key, propetyBag) => config);
            cfgMgr.BehaveAsDefaultValue();

            //Act
            var target = new ServiceLocatorConfig(cfgMgr);
            target.RemoveTypeMappings<ILogger>();
            List<TypeMapping> registeredTypeMappings =
                target.GetTypeMappings().ToList();

            //Assert
            Assert.AreEqual(2, registeredTypeMappings.Count);
            Assert.AreSame(typeMappings[1], registeredTypeMappings[0]);
            Assert.AreSame(typeMappings[2], registeredTypeMappings[1]);
        }


        [TestMethod]
        [HostType("Moles")]
        public void GetSiteCacheInterval_WithConfiguredFarmValue_ReturnsValue()
        {
            //Arrange
            int expected = 30;
            string expectedKey = "Microsoft.Practices.SharePoint.Common.SiteLocatorCacheInterval";
            var bag = new BIPropertyBag();
            BSPFarm.SetLocal();

            var configMgr = new SIConfigManager
            {
                ContainsKeyInPropertyBagStringIPropertyBag = (key, propertyBag) => 
                    {
                        if(key == expectedKey)
                            return true;
                        return false;
                    },
                CanAccessFarmConfigGet = () => true,
                GetPropertyBagConfigLevel = (configlevel) => bag,
            };
            configMgr.GetFromPropertyBagStringIPropertyBag<int>((key, propetyBag) => 
                {
                    if(key == expectedKey)
                        return expected;
                    return -1;
                });

            var config = new ServiceLocatorConfig(configMgr);

            //Act
            int target = config.GetSiteCacheInterval();

            //Assert
            Assert.AreEqual(expected, target);
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetSiteCacheInterval_WithoutConfiguredFarmValue_ReturnsNegativeOne()
        {
            //Arrange
            int expected = -1;
            string expectedKey = "Microsoft.Practices.SharePoint.Common.SiteLocatorCacheInterval";
            var bag = new BIPropertyBag();
            SPFarm farm = BSPFarm.SetLocal();

            var context = new SIApplicationContextProvider();
            context.GetCurrentAppDomainFriendlyName = () => "FullTrust";
            context.GetSPFarmLocal = () => farm;
            SharePointEnvironment.ApplicationContextProvider = context;

            var configMgr = new SIConfigManager();
            configMgr.ContainsKeyInPropertyBagStringIPropertyBag = (key, propertyBag) =>
                {
                    if (key == expectedKey)
                        return false;
                    return true;
                };
            configMgr.GetPropertyBagConfigLevel = (cfgLevel) => bag;

            var config = new ServiceLocatorConfig(configMgr);

            //Act
            int target = config.GetSiteCacheInterval();

            //Assert
            Assert.AreEqual(expected, target);
        }


        [TestMethod]
        public void SetSiteCacheInterval_WithValidValue_UpdatesConfiguration()
        {
            //Arrange
            int expected = 30;
            string expectedKey = "Microsoft.Practices.SharePoint.Common.SiteLocatorCacheInterval";
            var bag = new BIPropertyBag();
            int target = -1;

            var cfgMgr = new SIConfigManager();
            cfgMgr.SetInPropertyBagStringObjectIPropertyBag = (key, value, propBag) =>
                        {
                            if(key == expectedKey)
                                target = (int) value;
                        };
            cfgMgr.GetPropertyBagConfigLevel = (configlevel) => bag;

            var config = new ServiceLocatorConfig(cfgMgr);

            //Act
            config.SetSiteCacheInterval(expected);

            //Assert
            Assert.AreEqual(expected, target);
        }

        [TestMethod]
        public void SetSiteCacheInterval_WithInvalidValue_ThrowsArgumentException()
        {
            //Arrange
            bool threwException = false;

            var hierarchicalConfig = new SIConfigManager();

            var config = new ServiceLocatorConfig(hierarchicalConfig);

            //Act
            try
            {
                config.SetSiteCacheInterval(-1);
            }
            catch (ArgumentException)
            {
                threwException = true;
            }

            //Assert
            Assert.IsTrue(threwException, "Expected ArgumentException not thrown");
        }

        public Microsoft.Moles.Framework.Behaviors.IBehavior MolesBehavior { get; set; }
    }

    interface ISomething
    {

    }

    class Something : ISomething
    {

    }

    class SomethingElse : ISomething
    {

    }

}
