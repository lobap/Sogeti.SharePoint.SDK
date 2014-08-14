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
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Permissions.Moles;
using System.Security.Moles;
using Microsoft.SharePoint.Utilities.Moles;
using Microsoft.Moles.Framework;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.Practices.SharePoint.Common.Configuration;

[assembly: MoledType(typeof(System.Security.Permissions.SecurityPermission))]
[assembly: MoledType(typeof(System.Security.CodeAccessPermission))]

namespace Microsoft.Practices.SharePoint.Common.Tests
{
    [TestClass]
    public class ApplicationContextProviderTests
    {
        [TestMethod]
        [HostType("Moles")]
        public void IsProxyCheckerInstalled_ReturnsTrueWhenInGac()
        {
            //Arrange
            SetupInGac();

            var target = new TestableApplicationContextProvider(true);

            //Act
            bool result = target.IsProxyCheckerInstalled();

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [HostType("Moles")]
        public void IIsProxyCheckerInstalled_ReturnsFalse_WhenNotInGac()
        {
            //Arrange
            MCodeAccessPermission mock; 
            MSecurityPermission.ConstructorSecurityPermissionFlag = (instance, flags) =>
            {
                mock = new MCodeAccessPermission(instance)
                {
                    Assert = () => {throw new SecurityException(); }
                };

            };

            var target = new ApplicationContextProvider();

            //Act
            bool result = target.IsProxyCheckerInstalled();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [HostType("Moles")]
        public void IsProxyCheckerInstalled_ReturnsFalseWhenArgumentExceptionThrown()
        {
            //Arrange
            SetupInGac();
            var target = new ApplicationContextProvider();
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) =>
                                                                                             {
                                                                                                 throw new ArgumentException();
                                                                                             };
            //Act
            bool result = target.IsProxyCheckerInstalled();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [HostType("Moles")]
        public void IsProxyCheckerInstalled_ReturnsFalseWhenTypeLoadExceptionThrown()
        {
            //Arrange
            SetupInGac();
            var target = new ApplicationContextProvider();
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) =>
            {
                throw new TypeLoadException();
            };
            //Act
            bool result = target.IsProxyCheckerInstalled();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [HostType("Moles")]
        public void IsProxyCheckerInstalled_ReturnsFalseWhenInvalidOperationExceptionThrown()
        {
            //Arrange
            SetupInGac();
            var target = new ApplicationContextProvider();
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) =>
            {
                throw new InvalidOperationException();
            };
            //Act
            bool result = target.IsProxyCheckerInstalled();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [HostType("Moles")]
        public void IsProxyCheckerInstalled_GeneralExceptionNotCaught()
        {
            //Arrange
            SetupInGac();
            var target = new TestableApplicationContextProvider(true);
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) =>
            {
                throw new LoggingException();
            };
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                bool result = target.IsProxyCheckerInstalled();

            }
            catch (LoggingException)
            {
                expectedExceptionThrown = true;
            }

            //Assert
            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        [HostType("Moles")]
        public void IsProxyInstalled_ReturnsTrueWhenTrueReturned()
        {
            //Arrange
            SetupInGac();

            var target = new ApplicationContextProvider();
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) =>
                                                 {
                                                     var checkargs = args as ProxyInstalledArgs;

                                                     if (checkargs.AssemblyName == ProxyInstalledArgs.OperationAssemblyName &&
                                                         checkargs.TypeName == ProxyInstalledArgs.OperationTypeName)
                                                         return true;
                                                     else if (checkargs.AssemblyName == "foo" && checkargs.TypeName == "bar")
                                                         return true;
                                                     return false;
                                                 };
            //Act
            bool result = target.IsProxyInstalled("foo", "bar");

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [HostType("Moles")]
        public void IsProxyInstalled_ReturnsFalseWhenFalseReturned()
        {
            //Arrange
            SetupInGac();

            var target = new TestableApplicationContextProvider(true);
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) =>
            {
                var checkargs = args as ProxyInstalledArgs;

                if (checkargs.AssemblyName == ProxyInstalledArgs.OperationAssemblyName &&
                    checkargs.TypeName == ProxyInstalledArgs.OperationTypeName)
                    return true;

                return false;
            };
            //Act
            bool result = target.IsProxyInstalled("foo", "bar");

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [HostType("Moles")]
        public void IsProxyInstalled_ThrowsExceptionWhenExceptionReturned()
        {
            //Arrange
            SetupInGac();

            var target = new TestableApplicationContextProvider(true);
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) =>
            {
                var checkargs = args as ProxyInstalledArgs;

                if (checkargs.AssemblyName == ProxyInstalledArgs.OperationAssemblyName &&
                    checkargs.TypeName == ProxyInstalledArgs.OperationTypeName)
                    return true;

                return new LoggingException();
            };

            ConfigurationException configEx = null;

            //Act
            try
            {
                bool result = target.IsProxyInstalled("foo", "bar");
            }
            catch (ConfigurationException ex)
            {
                configEx = ex;
            }

            //Assert
            Assert.IsNotNull(configEx);
        }

        private void SetupInGac()
        {
            
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) => null;

        }
    }

    public class TestableApplicationContextProvider : ApplicationContextProvider
    {
        bool isInGac = false;

        public TestableApplicationContextProvider(bool inGac)
        {
            isInGac = inGac;
        }

        protected override bool CheckInGAC()
        {
            return isInGac;
        }
    }
}
