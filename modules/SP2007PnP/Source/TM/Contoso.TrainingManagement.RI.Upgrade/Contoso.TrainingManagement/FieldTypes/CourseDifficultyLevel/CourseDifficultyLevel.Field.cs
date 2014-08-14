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
using System.Runtime.InteropServices;
using System.Security.Permissions;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Security;

namespace Contoso.TrainingManagement.FieldTypes
{
    /// <summary>
    /// The CourseDifficultyLevelField is a custom SharePoint field type.
    /// It is based on the SPFieldNumber type and meant to carry
    /// a numeric value. This class defines a custom field rendering control.
    /// It also overrides the GetValidatedString method in the base class
    /// to provide custom validation.
    /// </summary>
    [Guid("f6b8f646-2180-4dc7-b7f9-b108ed7abc56")]
    public class CourseDifficultyLevelField : SPFieldNumber
    {
        #region Constructor

        public CourseDifficultyLevelField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public CourseDifficultyLevelField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The CourseDifficultyLevelField utilizes the CourseDifficultyLevelFieldControl class.
        /// </summary>
        public override BaseFieldControl FieldRenderingControl
        {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                BaseFieldControl fieldControl = new CourseDifficultyLevelFieldControl();
                fieldControl.FieldName = this.InternalName;

                return fieldControl;
            }
        }

        /// <summary>
        /// The GetValidatedString method checks that the value of the field
        /// is between 0 and 5.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override string GetValidatedString(object value)
        {
            string difficultyLevel = value.ToString();
            short difficultyLevelValue = Int16.Parse(difficultyLevel);
            if (difficultyLevelValue < 0 || difficultyLevelValue > 5)
            {
                throw new SPFieldValidationException("Invalid difficulty level. Difficulty level must be between 0 and 5.");
            }

            return difficultyLevel;
        }

        #endregion
    }
}
