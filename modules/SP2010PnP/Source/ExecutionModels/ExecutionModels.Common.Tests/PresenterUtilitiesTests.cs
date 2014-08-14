//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace ExecutionModels.Common.Tests
{
    using ExecutionModels;
    using ExecutionModels.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    /// <summary>
    ///This is a test class for PresenterUtilitiesTest and is intended
    ///to contain all PresenterUtilitiesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PresenterUtilitiesTests
    {

        ///// <summary>
        /////A test for FormatGridDisplay
        /////</summary>
        //[TestMethod()]
        //public void FormatGridDisplayTest()
        //{
        //    GridView gridView = null; 
        //    GridView gridViewExpected = null; 
        //    PresenterUtilities.FormatGridDisplay(ref gridView);
        //    Assert.AreEqual(gridViewExpected, gridView);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        /// <summary>
        ///A test for IsNotSystemColumn
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ExecutionModels.Common.dll")]
        public void IsNotSystemColumnTest()
        {
            //Arrange
            string[] columnNames = Constants.GetSystemColumns();
            bool expected = false;
            bool actual;


            foreach (string str in columnNames)
            {
                //Act
                actual = PresenterUtilities_Accessor.IsNotSystemColumn(str);

                //Assert
                Assert.AreEqual(expected, actual);
            }

        }

        [TestMethod]
        public void ConstantsGetSystemColumns_ReturnsNewStringArrayEveryTime()
        {
            var syscolumns = Constants.GetSystemColumns();
            var updatedsyscolumns = Constants.GetSystemColumns();

            Assert.AreNotSame(syscolumns, updatedsyscolumns);
        }
    }
}
