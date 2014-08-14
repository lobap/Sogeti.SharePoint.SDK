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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Moles;
using Microsoft.SharePoint.Utilities.Moles;
using Microsoft.Practices.SharePoint.Common.Configuration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    [TestClass]
    public class SandboxWebAppPropertyBagTests
    {
        [TestMethod]
        [HostType("Moles")]
        public void ContainsSucceeds()
        {
            //Arrange
            string assemblyName = null;
            string typeName = null;
            string key = TestsConstants.TestGuidName;
              string resultKey = null;
            ConfigLevel resultLevel = ConfigLevel.CurrentSPWeb;

            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (assembly, type, args) =>
            {
                assemblyName = assembly;
                typeName = type;
                var proxyArgs = args as ProxyArgs.ContainsKeyDataArgs;
                resultKey = proxyArgs.Key;
                resultLevel = (ConfigLevel)proxyArgs.Level;
                return true;
            };

            var target = new SandboxWebAppPropertyBag(TestsConstants.TestGuid); 

            //Act
            bool result = target.Contains(key);

            //Assert
            Assert.AreEqual<string>(assemblyName, ProxyArgs.ContainsKeyDataArgs.OperationAssemblyName);
            Assert.AreEqual<string>(typeName, ProxyArgs.ContainsKeyDataArgs.OperationTypeName);
            Assert.AreEqual(key, resultKey);
            Assert.AreEqual(ConfigLevel.CurrentSPWebApplication, resultLevel);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContainsKeyWithNullKeyThrowsArgumentNullException()
        {
            //Arrange
            var target = new SandboxWebAppPropertyBag(TestsConstants.TestGuid);

            //Act
            bool result = target.Contains(null);

            //Assert caught by expected exception 
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetIndexerSucceeds()
        {
            //Arrange
            string assemblyName = null;
            string typeName = null;
            string key = TestsConstants.TestGuidName;
            string resultKey = null;
            ConfigLevel resultLevel = ConfigLevel.CurrentSPWeb;
            string targetValue = "foobar";

            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (assembly, type, args) =>
            {
                assemblyName = assembly;
                typeName = type;
                var proxyArgs = args as ProxyArgs.ReadConfigArgs;
                resultKey = proxyArgs.Key;
                resultLevel = (ConfigLevel)proxyArgs.Level;
                return targetValue;
            };

            var target = new SandboxWebAppPropertyBag(TestsConstants.TestGuid);

            //Act
            string result = target[key];

            //Assert
            Assert.AreEqual<string>(assemblyName, ProxyArgs.ReadConfigArgs.OperationAssemblyName);
            Assert.AreEqual<string>(typeName, ProxyArgs.ReadConfigArgs.OperationTypeName);
            Assert.AreEqual(key, resultKey);
            Assert.AreEqual(ConfigLevel.CurrentSPWebApplication, resultLevel);
            Assert.AreEqual(targetValue, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetIndexerWithNullThrowsArgumentNullException()
        {
            //Arrange
            var target = new SandboxWebAppPropertyBag(TestsConstants.TestGuid);

            //Act
            string result = target[null];

            //Assert caught by expected exception 
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetIndexerThrowsInvalidOperationException()
        {
            //Arrange
            var target = new SandboxWebAppPropertyBag(TestsConstants.TestGuid);

            //Act
            target["foo"] = "bar";

            //Assert caught by expected exception 
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetIndexerWithNullThrowsArgumentNullException()
        {
            //Arrange
            var target = new SandboxWebAppPropertyBag(TestsConstants.TestGuid);

            //Act
            string result = target[null];

            //Assert caught by expected exception 
        }

        [TestMethod]
        public void GetLevelSucceeds()
        {
            //Arrange
            var target = new SandboxWebAppPropertyBag(TestsConstants.TestGuid);

            //Act
            ConfigLevel level = target.Level;

            //Assert
            Assert.AreEqual<ConfigLevel>(ConfigLevel.CurrentSPWebApplication, level);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveThrowsInvalidOperationException()
        {
            //Arrange
            var target = new SandboxWebAppPropertyBag(TestsConstants.TestGuid);

            //Act
            target.Remove("foobar");

            //Assert caught by exception

        }
    }
}
