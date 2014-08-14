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
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Practices.SPG.Common.Logging
{
    /// <summary>
    /// This class provides the logging functionality to the ULS. It's based on the implementation on MSDN.
    /// </summary>
    [SuppressUnmanagedCodeSecurity()]
    [SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
    [FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
    [SharePointPermission(SecurityAction.Assert, Unrestricted=true)]
    internal static class ULSTraceProvider
    {
        static UInt64 hTraceLog;
        static TraceRegSafeHandle traceRegSafeHandle;

        class TraceRegSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public UInt64 hTraceReg;

            // Create a SafeHandle, informing the base class
            // that this SafeHandle instance "owns" the handle,
            // and therefore SafeHandle should call
            // our ReleaseHandle method when the SafeHandle
            // is no longer in use.
            public TraceRegSafeHandle()
                : base(true)
            {

            }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            protected override bool ReleaseHandle()
            {
                if (hTraceReg != 0)
                {
                    uint result = NativeMethods.UnregisterTraceGuids(hTraceReg);

                    CheckHResult(result, "Could not unregister trace provider");
                }

                return true;
            }
        }

        static class NativeMethods
        {
            internal const int TRACE_VERSION_CURRENT = 1;
            internal const int ERROR_SUCCESS = 0;
            internal const int ERROR_INVALID_PARAMETER = 87;
            internal const int WNODE_FLAG_TRACED_GUID = 0x00020000;

            internal enum TraceFlags
            {
                TRACE_FLAG_START = 1,
                TRACE_FLAG_END = 2,
                TRACE_FLAG_MIDDLE = 3,
                TRACE_FLAG_ID_AS_ASCII = 4
            }

            // Copied from Win32 APIs
            [StructLayout(LayoutKind.Sequential)]
            internal struct EVENT_TRACE_HEADER_CLASS
            {
                internal byte Type;
                internal byte Level;
                internal ushort Version;
            }

            // Copied from Win32 APIs
            [StructLayout(LayoutKind.Sequential)]
            internal struct EVENT_TRACE_HEADER
            {
                internal ushort Size;
                internal ushort FieldTypeFlags;
                internal EVENT_TRACE_HEADER_CLASS Class;
                internal uint ThreadId;
                internal uint ProcessId;
                internal Int64 TimeStamp;
                internal Guid Guid;
                internal uint ClientContext;
                internal uint Flags;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct ULSTraceHeader
            {
                internal ushort Size;
                internal uint dwVersion;
                internal uint Id;
                internal Guid correlationID;
                internal TraceFlags dwFlags;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                internal string wzExeName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                internal string wzProduct;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                internal string wzCategory;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 800)]
                internal string wzMessage;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct ULSTrace
            {
                internal EVENT_TRACE_HEADER Header;
                internal ULSTraceHeader ULSHeader;
            }

            // Copied from Win32 APIs
            internal enum WMIDPREQUESTCODE
            {
                WMI_GET_ALL_DATA = 0,
                WMI_GET_SINGLE_INSTANCE = 1,
                WMI_SET_SINGLE_INSTANCE = 2,
                WMI_SET_SINGLE_ITEM = 3,
                WMI_ENABLE_EVENTS = 4,
                WMI_DISABLE_EVENTS = 5,
                WMI_ENABLE_COLLECTION = 6,
                WMI_DISABLE_COLLECTION = 7,
                WMI_REGINFO = 8,
                WMI_EXECUTE_METHOD = 9
            }

            // Copied from Win32 APIs
            internal unsafe delegate uint EtwProc(NativeMethods.WMIDPREQUESTCODE requestCode, IntPtr requestContext, uint* bufferSize, IntPtr buffer);

            // Copied from Win32 APIs
            [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
            internal static extern unsafe uint RegisterTraceGuids([In] EtwProc cbFunc, [In] void* context, [In] ref Guid controlGuid, [In] uint guidCount, IntPtr guidReg, [In] string mofImagePath, [In] string mofResourceName, out ulong regHandle);

            // Copied from Win32 APIs
            [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
            internal static extern uint UnregisterTraceGuids([In]ulong regHandle);

            // Copied from Win32 APIs
            [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
            internal static extern UInt64 GetTraceLoggerHandle([In]IntPtr Buffer);

            // Copied from Win32 APIs
            [DllImport("advapi32.dll", SetLastError = true)]
            internal static extern uint TraceEvent([In]UInt64 traceHandle, [In]ref ULSTrace evnt);
        }

        static ULSTraceProvider()
        {
            RegisterTraceProvider();
        }

        internal static void WriteTrace(uint tag, TraceSeverity level, Guid correlationGuid, string exeName, string productName, string categoryName, string message)
        {
            const ushort sizeOfWCHAR = 2;
            NativeMethods.ULSTrace ulsTrace = new NativeMethods.ULSTrace();

            // Pretty standard code needed to make things work
            ulsTrace.Header.Size = (ushort)Marshal.SizeOf(typeof(NativeMethods.ULSTrace));
            ulsTrace.Header.Flags = NativeMethods.WNODE_FLAG_TRACED_GUID;
            ulsTrace.ULSHeader.dwVersion = NativeMethods.TRACE_VERSION_CURRENT;
            ulsTrace.ULSHeader.dwFlags = NativeMethods.TraceFlags.TRACE_FLAG_ID_AS_ASCII;
            ulsTrace.ULSHeader.Size = (ushort)Marshal.SizeOf(typeof(NativeMethods.ULSTraceHeader));

            // Variables communicated to SPTrace
            ulsTrace.ULSHeader.Id = tag;
            ulsTrace.Header.Class.Level = (byte)level;
            ulsTrace.ULSHeader.wzExeName = exeName;
            ulsTrace.ULSHeader.wzProduct = productName;
            ulsTrace.ULSHeader.wzCategory = categoryName;
            ulsTrace.ULSHeader.wzMessage = message;
            ulsTrace.ULSHeader.correlationID = correlationGuid;

            // Optionally, to improve performance by reducing the amount of data copied around,
            // the Size parameters can be reduced by the amount of unused buffer in the Message
            if (message.Length < 800)
            {
                ushort unusedBuffer = (ushort)((800 - (message.Length + 1)) * sizeOfWCHAR);
                ulsTrace.Header.Size -= unusedBuffer;
                ulsTrace.ULSHeader.Size -= unusedBuffer;
            }

            if (hTraceLog != 0)
            {
                uint result = NativeMethods.TraceEvent(hTraceLog, ref ulsTrace);

                CheckHResult(result, "Could not write trace message: " + message);
            }
        }

        private static unsafe void RegisterTraceProvider()
        {
            SPFarm farm = SPFarm.Local;
            Guid traceGuid = farm.TraceSessionGuid;
            UInt64 hTraceReg;
            uint result = NativeMethods.RegisterTraceGuids(ControlCallback, null, ref traceGuid, 0, IntPtr.Zero, null, null, out hTraceReg);

            traceRegSafeHandle = new TraceRegSafeHandle();
            traceRegSafeHandle.hTraceReg = hTraceReg;

            CheckHResult(result, "Could not register trace provider.");
        }

        private static void CheckHResult(uint result, string errorMessage)
        {
            if (result != NativeMethods.ERROR_SUCCESS)
            {
                Exception exception = new UlsTraceProviderException(errorMessage);
                exception.Data["HResult"] = result;

                throw exception;
            }
        }

        static unsafe uint ControlCallback(NativeMethods.WMIDPREQUESTCODE RequestCode, IntPtr Context, uint* InOutBufferSize, IntPtr Buffer)
        {
            uint Status;
            switch (RequestCode)
            {
                case NativeMethods.WMIDPREQUESTCODE.WMI_ENABLE_EVENTS:
                    hTraceLog = NativeMethods.GetTraceLoggerHandle(Buffer);
                    Status = NativeMethods.ERROR_SUCCESS;
                    break;
                case NativeMethods.WMIDPREQUESTCODE.WMI_DISABLE_EVENTS:
                    hTraceLog = 0;
                    Status = NativeMethods.ERROR_SUCCESS;
                    break;
                default:
                    Status = NativeMethods.ERROR_INVALID_PARAMETER;
                    break;
            }

            *InOutBufferSize = 0;
            return Status;
        }
    }

}
