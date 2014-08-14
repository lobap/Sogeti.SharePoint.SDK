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
using Microsoft.SharePoint;
using TypeMock;

namespace Contoso.TrainingManagement.Mocks
{
    /// <summary>
    /// The MockHelper class contains common SharePoint behaviors that needs mocking.
    /// </summary>
    public static class MockHelper
    {
        /// <summary>
        /// RecordSPItemEventDataCollection mocks the Get behavior of the
        /// SPItemEventDataCollection collection.
        /// </summary>
        /// <param name="properties">A mocked instance of an SPItemEventDataCollection object</param>
        /// <param name="fieldName">the name of the SPField that will be used as the indexer</param>
        /// <param name="value">the mocked value to return</param>
        public static void RecordSPItemEventDataCollection(SPItemEventDataCollection properties, string fieldName, object value)
        {
            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                object val = properties[fieldName];
                recorder.Return(value).RepeatAlways().WhenArgumentsMatch();
            }
        }

        /// <summary>
        /// The RecordFieldTitle method will mock the Get of the Title property
        /// of an SPField
        /// </summary>
        /// <param name="list">a mocked SPList object</param>
        /// <param name="id">the unique identifier of the SPField</param>
        /// <param name="fieldTitle">the mocked title value</param>
        public static void RecordFieldTitle(SPList list, Guid id, string fieldTitle)
        {
            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                SPField field = list.Fields[id];
                recorder.CheckArguments();
                string fieldName = field.Title;
                recorder.Return(fieldTitle).RepeatAlways();
            }
        }

        /// <summary>
        /// The RecordFieldInternalName method will mock the Get of the InternalName property
        /// of an SPField
        /// </summary>
        /// <param name="list">a mocked SPList object</param>
        /// <param name="id">the unique identifier of the SPField</param>
        /// <param name="internalName">the mocked InternalName</param>
        internal static void RecordFieldInternalName(SPList list, Guid id, string internalName)
        {
            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                SPField field = list.Fields[id];
                recorder.CheckArguments();
                string fieldName = field.InternalName;
                recorder.Return(internalName).RepeatAlways();
            }
        }
    }
}
