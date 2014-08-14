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
namespace Contoso.TrainingManagement.Common.Constants
{
    /// <summary>
    /// The Fields class contains the unique identifiers of
    /// fields from SharePoint lists. The names of the constants correspond
    /// to the "Name" attribute of either the Field or FieldRef element of a
    /// content type definition. The Guid value corresponds to the "ID" attribute
    /// of Field or FieldRef element of a content type definition.
    /// </summary>
    public static class Fields
    {
        #region Item Base Content Type

        //The following Guids can be found in the ctypewss.xml file locatedin the 
        //%programfiles"\common files\microsoft shared\web server extensions\12\TEMPLATE\FEATURES\cytpes folder.
        public const string Id = "1D22EA11-1E32-424E-89AB-9FEDBADB6CE1";
        public const string Title = "FA564E0F-0C70-4AB9-B863-0177E6DDD247";

        #endregion

        #region Training Course Content Type

        //The following Guids can be found in the TrainingCourseContentType.xml file located in the
        //Contoso.TrainingManagement project under the ContentTypes\TrainingCourseContentType folder
        public const string TrainingCourseCode = "E5509750-CB71-4DE3-873D-171BA6448FA5";
        public const string TrainingCourseDescription = "8E39DAD4-65FA-4395-BA0C-43BF52586B3E";
        public const string TrainingCourseEnrollmentDate = "43568365-8448-4130-831C-98C074B61E89";
        public const string TrainingCourseStartDate = "AE2A0BBD-F22E-41DC-8753-451067122318";
        public const string TrainingCourseEndDate = "F5E6F566-FA7C-4883-BF7F-006727760E22";
        public const string TrainingCourseCost = "7E4004FA-D0BE-4611-A817-65D17CF11A6A";
        public const string TrainingCourseInstructor = "65f64276-ef6f-4357-a33a-183e6e6a5863";

        #endregion

        #region Registration Content Type

        //The following Guids can be found in the RegistrationContentType.xml file located in the
        //Contoso.TrainingManagement project under the ContentTypes\RegistrationContentType folder
        public const string CourseId = "11B6EBA7-D1A1-4D15-9770-645052681E40";
        public const string RegistrationStatus = "0711F3FA-7D65-4653-8E46-7835D48063EA";
        public const string UserId = "7277EB32-0529-4F5A-9F40-42220C327C55";
        public const string User = "4782FC5D-5A82-4F7E-BD4D-BB4407A3D473";

        #endregion

        #region Workflow Task Content Type

        //The following Guids can be found in the ctypewss.xml file locatedin the 
        //%programfiles"\common files\microsoft shared\web server extensions\12\TEMPLATE\FEATURES\cytpes folder.
        public const string WorkflowItemId = "8e234c69-02b0-42d9-8046-d5f49bf0174f";

        #endregion
    }
}