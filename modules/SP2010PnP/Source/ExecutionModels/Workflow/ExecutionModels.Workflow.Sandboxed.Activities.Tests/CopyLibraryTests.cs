//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Collections.Generic;
using System.IO;
using Microsoft.SharePoint.Moles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;

namespace ExecutionModels.Workflow.Sandboxed.Activities.Tests
{
    [TestClass]
    public class CopyLibraryTests
    {
        [TestMethod]
        [Ignore]
        [HostType("Moles")]
        public void CopyLibraryActivity_CopiesLibrary()
        {
            var copyLibrary = new CopyLibrary();
            var sourceFolder = new MSPFolder();
            var targetFolder = new MSPFolder();
            var sourceFileCollection = new MSPFileCollection();
            var targetFileCollection = new MSPFileCollection();
            var targetCollection = new List<SPFile>();
            var file1 = new MSPFile()
                            {
                                NameGet = () => "file1;",
                                OpenBinaryStream = () => Stream.Null
                            };
            var file2 = new MSPFile()
                            {
                                NameGet = () => "file2;",
                                OpenBinaryStream = () => Stream.Null
                            };
            sourceFileCollection.Bind(new List<SPFile>() { file1.Instance, file2.Instance });
            targetFileCollection.Bind(targetCollection);
            targetFileCollection.AddStringStream = (string stg, Stream stm) =>
                                                       {
                                                           var file = new MSPFile();
                                                           file.NameGet = ()=> stg;
                                                           file.OpenBinaryStream = () => stm;
                                                           return file.Instance;
                                                       }; 
            sourceFolder.FilesGet = () => sourceFileCollection;
            targetFolder.FilesGet = () => targetFileCollection;
            
            int filesCopied = copyLibrary.CopyFolder(sourceFolder, targetFolder, false);

            Assert.AreEqual(2, filesCopied);
            Assert.AreEqual(2, targetFolder.Instance.Files.Count);
            Assert.AreEqual("file1", targetFolder.Instance.Files[0].Name);
            Assert.AreEqual("file2", targetFolder.Instance.Files[1].Name);
        }

       
        //[TestMethod]
        //public void CopyLibraryActivity_CopiesDeepFoldersAndFiles()
        //{
        //}
    }
}
