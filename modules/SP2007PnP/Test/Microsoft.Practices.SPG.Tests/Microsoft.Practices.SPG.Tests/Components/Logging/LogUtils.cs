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
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Microsoft.Practices.SPG.Tests.Components.Logging
{
    class LogUtils
    {
        public static string strEventSourceName = "Office Sharepoint Server";
        private LogUtils()
        { }

        //overloaded method
        public static bool ValidateEventLog(string strEventSourceName, int EID, string strMess)
        {
            bool IsValid = false;
            using (EventLog log = new EventLog("Application", System.Environment.MachineName, strEventSourceName))
            {
                foreach (EventLogEntry entry in log.Entries)
                {
                    if (entry.EntryType == System.Diagnostics.EventLogEntryType.Error)
                    {
                        //if (entry.Message == strMess && entry.EventID==5)
                        if (entry.Source == strEventSourceName)
                            if (entry.InstanceId == EID)
                                if (entry.Message == strMess)
                                    IsValid = true;                                    //ssert.IsTrue(true);                                
                    }
                }
            }
            return IsValid;
        }

        public static bool ValidateEventLog(string strEventSourceName, int EID, string strMess,string strExceptionMsg)
        {
            bool IsValid = false;
            using (EventLog log = new EventLog("Application", System.Environment.MachineName, strEventSourceName))
            {
                foreach (EventLogEntry entry in log.Entries)
                {
                    if (entry.EntryType == System.Diagnostics.EventLogEntryType.Error)
                    {
                        //if (entry.Message == strMess && entry.EventID==5)
                        if (entry.Source == strEventSourceName)
                            if (entry.InstanceId == EID)
                            {
                                string s = entry.Message;
                                if(s.Contains(strMess))
                                    if(s.Contains(strExceptionMsg))
                                        IsValid = true;
                            }
                    }
                }
            }
            return IsValid;
        }


        public static bool ValidateSPLogs(string strMatch)
        {
            bool isValid = false;
            string strLogHive = @"C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\LOGS";

            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.MachineName + "-" + DateTime.Now.Date.ToString("yyyyMMdd") + "-" + DateTime.Now.ToString("HH"));

            string[] Files = Directory.GetFiles(strLogHive, sb.ToString() + "*.log");

            if (Files.Length == 1)
            {
                isValid = SearchLogEntry(Files[0], strMatch);
            }
            else
            {
                int[] Fnames = new int[Files.Length];
                for (int i = 0; i < Fnames.Length; i++)
                {
                    int idx = Files[i].IndexOf(".log");
                    Fnames[i] = int.Parse(Files[i].Substring(idx - 4, 4));
                }
                int max = 0;
                for (int i = 0; i < Fnames.Length; i++)
                {
                    if (Fnames[i] > max)
                        max = Fnames[i];
                }
                for (int i = 0; i < Fnames.Length; i++)
                {
                    if (Fnames[i] == max)
                        isValid = SearchLogEntry(Files[i], strMatch);
                }
            }
            return isValid;
        }

        private static bool SearchLogEntry(string p, string strMatch)
        {
            bool found = false;
            string strval = string.Empty;            

            string strCopyLocation = @"C:\TempMossLogs\";

            if (!Directory.Exists(strCopyLocation))
                Directory.CreateDirectory(strCopyLocation);

            //Degub mode is working fine but run is failing. So delay should be introduced.
            System.Threading.Thread.Sleep(1500);//To Ensure that actual log file is properly written.
            string strfname = Path.GetFileName(p);
            File.Copy(p, strCopyLocation + strfname, true);

            //using (FileStream fs = new FileStream(p, FileMode.Open, FileAccess.Read))
            using (FileStream fs = new FileStream(strCopyLocation + strfname, FileMode.Open, FileAccess.Read))
            {
                System.Threading.Thread.Sleep(100);//To Ensure that file copy is time is taken into account.
                using (StreamReader sr = new StreamReader(fs))
                {
                    while ((strval = sr.ReadLine()) != null)
                    {
                        if (strval.Contains(strMatch))
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }
            //if (found)
            //    Assert.IsTrue(true);
            //else
            //    Assert.Fail("There is no entry of message in Moss Log");

            return found;
        }
    }
}
