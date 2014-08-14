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
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Contoso.TrainingManagement.FieldTypes;

namespace Contoso.TrainingManagement.SharePoint.Tests
{
    /// <summary>
    /// Summary description for CourseRatingFieldFixture
    /// </summary>
    [TestClass]
    public class CourseRatingFieldFixture
    {
        #region Test Methods

        [TestMethod]
        [Ignore]
        public void GetValidatedString_ValidValue_ReturnValue()
        {
            SPFieldCollection coll  = new SPFieldCollection(null, "");
            CourseDifficultyLevelField courseDifficultyLevelField = new CourseDifficultyLevelField(coll, "");
            //CourseDifficultyLevelField courseDifficultyLevelField = RecorderManager.CreateMockedObject<CourseDifficultyLevelField>();
            object value = "1";

            string returnValue = courseDifficultyLevelField.GetValidatedString(value);

            Assert.AreEqual("1", returnValue);
        }

        [ExpectedException(typeof(SPFieldValidationException), "Invalid rating. Rating must be between 0 and 5.")]
        [TestMethod]
        [Ignore]
        public void GetValidatedString_InValidValue_ThrowException()
        {
            //CourseDifficultyLevelField courseDifficultyLevelField = RecorderManager.CreateMockedObject<CourseDifficultyLevelField>();
            //object value = "-1";

            //string returnValue = courseDifficultyLevelField.GetValidatedString(value);            
        }

        #endregion
    }
}
