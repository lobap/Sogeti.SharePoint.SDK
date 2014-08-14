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
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SPG.Common;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ServiceLocation;

namespace Microsoft.Practices.SPG.Tests.Components.Logging
{
    class LogUtils
    {
        private static string strEventSourceName = "Office SharePoint Server";
        private LogUtils()
        { }

        public static string GetEventSource()
        {
            //public static readonly string EventSourceNameConfigKey = "Microsoft.Practices.SPG.Common.EventSourceName";            
            string strEventSource_Name = string.Empty;
            IConfigManager configMgr = SharePointServiceLocator.Current.GetInstance<IConfigManager>();
            strEventSource_Name = configMgr.GetFromPropertyBag<string>(Common.Constants.EventSourceNameConfigKey, SPFarm.Local);
            if (string.IsNullOrEmpty(strEventSource_Name))
                strEventSource_Name = strEventSourceName;
            return strEventSource_Name;
        }

        //overloaded method
        public static bool ValidateEventLog(string strEventSourceName, int EID, string strMess, EventLogEntryType Severity)
        {
            bool IsValid = false;
            using (EventLog log = new EventLog("Application", System.Environment.MachineName, strEventSourceName))
            {
                foreach (EventLogEntry entry in log.Entries)
                {                    
                    if (entry.EntryType == Severity)
                    {                        
                        if (entry.Source == strEventSourceName)
                            if (entry.InstanceId == EID)
                                if (entry.Message.Contains(strMess))//if (entry.Message == strMess)
                                    IsValid = true;                                                                    
                    }
                }
            }
            return IsValid;
        }

        public static bool ValidateEventLog(string strEventSourceName, int EID, string strMess, string strExceptionMsg, EventLogEntryType Severity)
        {
            bool IsValid = false;
            using (EventLog log = new EventLog("Application", System.Environment.MachineName, strEventSourceName))
            {
                foreach (EventLogEntry entry in log.Entries)
                {                    
                    if (entry.EntryType == Severity)
                    {                       
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


        public static bool ValidateSPLogs(string strMatch, string strCategory, TraceSeverity Severity, int EID)
        {
            bool isValid = false;
            string strLogHive = @"C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\LOGS";
            
            string strFilename = Environment.MachineName + "-" + DateTime.Now.Date.ToString("yyyyMMdd") + "-" + DateTime.Now.ToString("HH");
            string strPattern =strFilename+"*.log";

            string[] Files = Directory.GetFiles(strLogHive, strPattern);//sb.ToString() + "*.log");
            
            if (Files.Length == 1)
            {
                isValid = SearchLogEntry(Files[0], strMatch,strCategory,Severity,EID);
            }
            else
            if (Files.Length == 0)//Compensation if Hour just got incrermented.
            {
                string PH = DateTime.Now.ToString("HH");
                int h = int.Parse(PH);
                h = h - 1;
                PH = h.ToString();
                strFilename = Environment.MachineName + "-" + DateTime.Now.Date.ToString("yyyyMMdd") + "-" +
                    PH + "*.log";// DateTime.Now.ToString("HH");
                Files = Directory.GetFiles(strLogHive, strFilename);
                if (Files.Length == 1)//recheck if only one file is there for that period.
                    isValid = SearchLogEntry(Files[0], strMatch, strCategory, Severity, EID);
                else
                    isValid = SearchFromFiles(Files, strMatch, strCategory, Severity, EID);
            }
            else
            {                
                isValid = SearchFromFiles(Files, strMatch, strCategory, Severity, EID);
            }
            return isValid;
        }

        private static bool SearchFromFiles(string[] Files, string strMatch, string strCategory, TraceSeverity Severity, int EID)
        {
            bool isValid = false;
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
                {
                    isValid = SearchLogEntry(Files[i], strMatch, strCategory, Severity, EID);
                    break;
                }
            }
            return isValid;
        }

        private static bool SearchLogEntry(string p, string strMatch, string strCategory, TraceSeverity Severity,int EID)
        {
            bool found = false;
            string strLogEntry = string.Empty;
            string strColumnRow = string.Empty;

            string strCopyLocation = @"C:\TempMossLogs\";

            if (!Directory.Exists(strCopyLocation))
                Directory.CreateDirectory(strCopyLocation);

            //Degub mode is working fine but run is failing. So delay should be introduced.
            System.Threading.Thread.Sleep(3000);//To Ensure that actual log file is properly written.
            string strfname = Path.GetFileName(p);
            File.Copy(p, strCopyLocation + strfname, true);

            //using (FileStream fs = new FileStream(p, FileMode.Open, FileAccess.Read))
            using (FileStream fs = new FileStream(strCopyLocation + strfname, FileMode.Open, FileAccess.Read))
            {
                System.Threading.Thread.Sleep(2000);//To Ensure that file copy is time is taken into account.
                using (StreamReader sr = new StreamReader(fs))
                {
                    strColumnRow = sr.ReadLine();
                    while ((strLogEntry = sr.ReadLine()) != null)
                    {
                        if (FindStrings(strLogEntry, strMatch))
                        {
                            if (FindStrings(strLogEntry, Severity.ToString()))//FindMatch(strLogEntry, strColumnRow, LogColumn.Level, Severity.ToString())
                            {
                                if (FindStrings(strLogEntry, EID.ToString()))//if (FindMatch(strLogEntry, strColumnRow, LogColumn.EventID, EID.ToString()))
                                {
                                    if (!string.IsNullOrEmpty(strCategory))
                                    {
                                        if (FindStrings(strLogEntry, strCategory))
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (Directory.Exists(strCopyLocation))
                Directory.Delete(strCopyLocation, true);
            
            return found;
        }

        private static bool FindStrings(string strLogEntry,string strKey)
        {
            bool found = false;

            if (strLogEntry.Contains(strKey))
                found = true;

            return found;
        }

        private static bool FindMatch(string strLogEntry, string strColumnRow,LogColumn Column, string ColVal)
        {
            bool found = false;
            int x = strColumnRow.IndexOf(Column.ToString());
            string value = strLogEntry.Substring(x, ColVal.Length);
            if (value == ColVal)
                found = true;
            return found;
        }

        public enum LogColumn
        {
            Category,
            EventID,
            Level,
        }

        public static  TraceSeverity MapEventLogEntryTypesToTraceLogSeverity(EventLogEntryType severity)
        {
            switch (severity)
            {
                case EventLogEntryType.Error:
                    return TraceSeverity.High;

                case EventLogEntryType.Warning:
                    return TraceSeverity.Medium;

                case EventLogEntryType.Information:
                case EventLogEntryType.FailureAudit:
                case EventLogEntryType.SuccessAudit:
                    return TraceSeverity.Verbose;

                default:
                    // assume worst case scenario if unknown.
                    return TraceSeverity.High;
            }
        }
    }
}
