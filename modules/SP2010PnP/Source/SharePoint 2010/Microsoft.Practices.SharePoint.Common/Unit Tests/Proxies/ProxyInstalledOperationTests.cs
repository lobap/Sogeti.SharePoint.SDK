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
using Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.SharePoint.Administration.Moles;
using Microsoft.SharePoint.UserCode;
using Microsoft.Practices.SharePoint.Common.Configuration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Proxies
{
    [TestClass]
    public class ProxyInstalledOperationTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Execute_ThrowsArgumentNullException_WhenArgsAreNull()
        {
            var proxyOp = new ProxyInstalledOperation();

            //Act 
            proxyOp.Execute(null);

            //Assert caught by exception...
        }

        [TestMethod]
        public void Execute_ReturnsArgumentNullException_WhenAssemblyNameIsNull()
        {
            var proxyOp = new ProxyInstalledOperation();
            var args = new ProxyInstalledArgs();
            args.AssemblyName = null;
            args.TypeName = ProxyInstalledArgs.OperationTypeName;

            //Act 
            object result = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ArgumentNullException));
        }


        [TestMethod]
        public void Execute_ReturnsArgumentNullException_WhenTypeNameIsNull()
        {
            var proxyOp = new ProxyInstalledOperation();
            var args = new ProxyInstalledArgs();
            args.AssemblyName = ProxyInstalledArgs.OperationAssemblyName;
            args.TypeName = null;

            //Act 
            object result = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void Execute_ReturnsConfigurationException_WhenWrongArgTypeProvided()
        {
            var proxyOp = new ProxyInstalledOperation();
            var args = new ReadConfigArgs();

            //Act 
            object result = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ConfigurationException));
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_ReturnsTrue_WhenProxyExists()
        {
            var args = new ProxyInstalledArgs();
            var opsList = new List<SPProxyOperationType>();
            opsList.Add(new SPProxyOperationType(ReadConfigArgs.OperationAssemblyName, ReadConfigArgs.OperationTypeName));
            opsList.Add(new SPProxyOperationType(ContainsKeyDataArgs.OperationAssemblyName, ContainsKeyDataArgs.OperationTypeName));
            opsList.Add(new SPProxyOperationType(LoggingOperationArgs.OperationAssemblyName, LoggingOperationArgs.OperationTypeName));
            var userService = new MSPUserCodeService();
            userService.ProxyOperationTypesGet= ()=> opsList;
            MSPUserCodeService.LocalGet = () => userService;

            args.AssemblyName = LoggingOperationArgs.OperationAssemblyName;
            args.TypeName = LoggingOperationArgs.OperationTypeName;
            var proxyOp = new ProxyInstalledOperation();

            //Act
            var result = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsTrue((bool)result);

        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_ReturnsFalse_WhenProxyDoesntExist()
        {
            var args = new ProxyInstalledArgs();
            var opsList = new List<SPProxyOperationType>();
            opsList.Add(new SPProxyOperationType(ReadConfigArgs.OperationAssemblyName, ReadConfigArgs.OperationTypeName));
            opsList.Add(new SPProxyOperationType(ContainsKeyDataArgs.OperationAssemblyName, ContainsKeyDataArgs.OperationTypeName));
            opsList.Add(new SPProxyOperationType(LoggingOperationArgs.OperationAssemblyName, LoggingOperationArgs.OperationTypeName));
            var userService = new MSPUserCodeService();
            userService.ProxyOperationTypesGet = () => opsList;
            MSPUserCodeService.LocalGet = () => userService;

            args.AssemblyName = LoggingOperationArgs.OperationAssemblyName;
            args.TypeName = ReadConfigArgs.OperationTypeName;
            var proxyOp = new ProxyInstalledOperation();

            //Act
            var result = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsFalse((bool)result);
        }
    }
}
