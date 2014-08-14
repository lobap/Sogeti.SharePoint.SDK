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
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Practices.SPG.Common.ListRepository;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.SPG.Common.Tests
{
    [TestClass]
    public class CAMLQueryBuilderFixture
    {
        [TestMethod]
        public void CanConstructSimpleCAMLQuery()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            target.AddEqual("Name", "Value");

            SPQuery query = target.Build();

            Assert.AreEqual("<Where><Eq><FieldRef Name='Name'/><Value Type='Text'>Value</Value></Eq></Where>", query.Query);

        }

        [TestMethod]
        public void CanConstructQueryWithMultipleFields()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            target.AddEqual("Name", "Value");
            target.AddEqual("Name2", "Value2");

            SPQuery query = target.Build();

            Assert.AreEqual("<Where><And><Eq><FieldRef Name='Name'/><Value Type='Text'>Value</Value></Eq><Eq><FieldRef Name='Name2'/><Value Type='Text'>Value2</Value></Eq></And></Where>", query.Query);

        }

        [TestMethod]
        public void CanUseIntegersAndDateTimes()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            target.AddEqual("Name", 1);
            target.AddEqual("Name2", new DateTime(2000, 11, 22));

            SPQuery query = target.Build();

            string expectedDateTime = SPUtility.CreateISO8601DateTimeFromSystemDateTime(new DateTime(2000, 11, 22));
            string expectedQuery =
                string.Format(
                    "<Where><And><Eq><FieldRef Name='Name'/><Value Type='Integer'>1</Value></Eq>" +
                    "<Eq><FieldRef Name='Name2'/><Value Type='DateTime'>{0}</Value></Eq></And></Where>",
                    expectedDateTime);

            Assert.AreEqual(expectedQuery, query.Query);
        }

        [TestMethod]
        public void CanFilterOnContentType()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            target.FilterByContentType("ContentTypeName");

            SPQuery query = target.Build();
            Assert.AreEqual("<Where><Eq><FieldRef Name='ContentType'/><Value Type='Text'>ContentTypeName</Value></Eq></Where>", query.Query);
        }

        [TestMethod]
        public void CanFilterOnContentTypeAndValue()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            target.FilterByContentType("ContentTypeName");
            target.AddEqual("Name", "Value");

            SPQuery query = target.Build();

            Assert.AreEqual("<Where><And><Eq><FieldRef Name='ContentType'/><Value Type='Text'>ContentTypeName</Value></Eq>" +
                "<Eq><FieldRef Name='Name'/><Value Type='Text'>Value</Value></Eq>" +
                "</And></Where>", query.Query);

        }

        [TestMethod]
        public void CanAddCustomEqualFilter()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();

            target.AddEqual("Name", "Test", "CustomType");

            SPQuery query = target.Build();
            Assert.AreEqual("<Where><Eq><FieldRef Name='Name'/><Value Type='CustomType'>Test</Value></Eq></Where>", query.Query);
        }

        [TestMethod]
        public void CanAddCustomFilter()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();

            target.AddFilter(new CAMLFilter { FilterExpression = "foo" });

            SPQuery query = target.Build();
            Assert.AreEqual("<Where>foo</Where>", query.Query);
        }

        [TestMethod]
        public void CanFilterUsingId()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            Guid expectedGuid = Guid.NewGuid();
            target.AddEqual(expectedGuid, "Value", "Text");

            SPQuery query = target.Build();
            Assert.AreEqual(string.Format("<Where><Eq><FieldRef ID='{0}'/><Value Type='Text'>Value</Value></Eq></Where>", expectedGuid.ToString()), query.Query);
        }

        [TestMethod]
        public void CanFilterUsingIdWithString()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            Guid expectedGuid = Guid.NewGuid();
            target.AddEqual(expectedGuid, "Value");

            SPQuery query = target.Build();
            Assert.AreEqual(string.Format("<Where><Eq><FieldRef ID='{0}'/><Value Type='Text'>Value</Value></Eq></Where>", expectedGuid.ToString()), query.Query);
        }

        [TestMethod]
        public void CanFilterUsingIdWithInt()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            Guid expectedGuid = Guid.NewGuid();
            target.AddEqual(expectedGuid, 123);

            SPQuery query = target.Build();
            Assert.AreEqual(string.Format("<Where><Eq><FieldRef ID='{0}'/><Value Type='Integer'>123</Value></Eq></Where>", expectedGuid.ToString()), query.Query);
        }

        [TestMethod]
        public void CanFilterUsingIdWithDateTime()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            Guid expectedGuid = Guid.NewGuid();
            string expectedDateTime = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.MinValue);
            target.AddEqual(expectedGuid, DateTime.MinValue);

            SPQuery query = target.Build();
            Assert.AreEqual(string.Format("<Where><Eq><FieldRef ID='{0}'/><Value Type='DateTime'>{1}</Value></Eq></Where>", expectedGuid.ToString(), expectedDateTime), query.Query);
        }

        [TestMethod]
        public void CanConstructNotEqualsCAMLQuery()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            target.AddNotEqual("Name", "Value");

            SPQuery query = target.Build();

            Assert.AreEqual("<Where><Neq><FieldRef Name='Name'/><Value Type='Text'>Value</Value></Neq></Where>", query.Query);
        }

        [TestMethod]
        public void CanConstructNotEqualsCAMLQueryUsingInt()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            target.AddNotEqual("Name", 123);

            SPQuery query = target.Build();

            Assert.AreEqual("<Where><Neq><FieldRef Name='Name'/><Value Type='Integer'>123</Value></Neq></Where>", query.Query);
        }

        [TestMethod]
        public void CanConstructNotEqualsCAMLQueryUsingDateTime()
        {
            CAMLQueryBuilder target = new CAMLQueryBuilder();
            string expectedDateTime = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.MinValue);
            target.AddNotEqual("Name", DateTime.MinValue);

            SPQuery query = target.Build();

            Assert.AreEqual(string.Format("<Where><Neq><FieldRef Name='Name'/><Value Type='DateTime'>{0}</Value></Neq></Where>", expectedDateTime), query.Query);
        }
        // ToTest: 
        // - Can use integers, datetimes
        // - Partial expressions (begins with, like, etc..)
        // - Can create expressions in the constructor
        // - Can add loose pieces of CAML
        // - Or expressions

    }

    public class TestType { }
}
