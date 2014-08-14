//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using System.Globalization;
using Microsoft.Practices.SharePoint.Common.Properties;
using Microsoft.Practices.SharePoint.Common.Logging;
using System.Threading;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// Provides a property bag for a web application based upon the SPWebApplication property bag. 
    /// </summary>
    public class SPWebAppPropertyBag : IPropertyBag
    {
        private readonly SPWebApplication webApplication;
        private static ReaderWriterLockSlim rrLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        static WebAppSettingStore _settingStore;
        static DateTime lastLoad = DateTime.Now.AddDays(-1);
        static int cacheInterval = 10;

        /// <summary>
        /// For internal use, clears the cached setting store data
        /// </summary>
        public static void ClearCache()
        {
            _settingStore = null;
        }

        /// <summary>
        /// Forces the property bag to reload settings on next access.
        /// </summary>
        public void Reset()
        {
            rrLock.EnterWriteLock();

            try
            {
                _settingStore = null;
            }
            finally
            {
                rrLock.ExitWriteLock();
            }
        }

        private WebAppSettingStore GetWriteSettingStore()
        {
            rrLock.EnterUpgradeableReadLock();

            try
            {
                GetSettingStore();

                if (_settingStore == null) // setting store not yet created
                {
                    rrLock.EnterWriteLock();

                    try
                    {
                        _settingStore = WebAppSettingStore.Create(this.webApplication);
                    }
                    finally
                    {
                        rrLock.ExitWriteLock();
                    }
                }
                return _settingStore;
            }
            finally
            {
                rrLock.ExitUpgradeableReadLock();
            }
        }

        private WebAppSettingStore GetSettingStore()
        {
            rrLock.EnterUpgradeableReadLock();

            try
            {
                if (_settingStore == null || (DateTime.Now.Subtract(lastLoad).TotalSeconds) > cacheInterval)
                {
                    //Need to exist so don't deadlock.

                    rrLock.EnterWriteLock();
                    try
                    {
                        //make sure first another thread didn't already load...
                        if (_settingStore == null || (DateTime.Now.Subtract(lastLoad).TotalSeconds) > cacheInterval)
                        {
                            _settingStore = WebAppSettingStore.Load(this.webApplication);
                            lastLoad = DateTime.Now;
                        }
                    }
                    finally
                    {
                        rrLock.ExitWriteLock();
                    }
                }

                return _settingStore;
            }
            finally
            {
                rrLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// The config level this property bag operates on. Returns <see cref="ConfigLevel.CurrentSPWebApplication"/>.
        /// </summary>
        public ConfigLevel Level
        {
            get { return ConfigLevel.CurrentSPWebApplication; }
        }

         /// <summary>
        /// Initializes a new instance of the <see cref="SPWebAppPropertyBag"/> class.
        /// </summary>
        /// <param name="webApplication">The Web Application.</param>
         public SPWebAppPropertyBag(SPWebApplication webApplication)
        {
            Validation.ArgumentNotNull(webApplication, "webApplication");
            this.webApplication = webApplication;
        }

        /// <summary>
        /// Does a specific key exist in the PropertyBag.
        /// </summary>
        /// <param name="key">the key to check.</param>
        /// <returns>true if the key exists, else false.</returns>
         [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
         [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool Contains(string key)
        {
            WebAppSettingStore store = GetSettingStore();

            if (store == null)      //store doesn't exist, so setting doesn't exist...
                return false;

            rrLock.EnterReadLock();

            try
            {
                return store.Settings.ContainsKey(key);
            }
            finally
            {
                rrLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets or sets a value based on the key. If the value is not defined in this PropertyBag, it will look in it's
        /// parent property bag.
        /// </summary>
        /// <value></value>
        /// <returns>The config value defined in the property bag. </returns>
        public string this[string key]
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                WebAppSettingStore store = GetSettingStore();

                if (store == null)      //store doesn't exist, so setting doesn't exist...
                    return null;

                rrLock.EnterReadLock();

                try
                {
                    if (!store.Settings.ContainsKey(key))
                        return null;

                    return store.Settings[key];
                }
                finally
                {
                    rrLock.ExitReadLock();
                }
            }
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            set
            {
                rrLock.EnterWriteLock();

                try
                {

                    //intentionally nested, Reset is also getting a write lock, is safe and ensures no race.
                    //Force the reload of the persisted object, which minimizes chances of a failure due
                    // to a concurrency failure.
                    Reset();
                    WebAppSettingStore store = GetWriteSettingStore();

                    store.Settings[key] = value;
                    store.Update();
                }
                catch (SPUpdatedConcurrencyException)
                {
                    Reset();

                    throw;
                }
                finally
                {
                    rrLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Remove a particular config setting from this property bag.
        /// </summary>
        /// <param name="key">The key to remove</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Remove(string key)
        {
            var logger = SharePointServiceLocator.GetCurrent().GetInstance<ILogger>();

            rrLock.EnterWriteLock();

            try
            {
                //intentionally nested, both cases getting a write lock, is safe and ensures no race.
                //Force the reload of the persisted object, which minimizes chances of a failure due
                // to a concurrency failure.

                Reset();
                WebAppSettingStore store = GetWriteSettingStore();

                if (store.Settings.ContainsKey(key))
                {
                    store.Settings.Remove(key);
                    store.Update();
                }
            }
            catch (SPUpdatedConcurrencyException)
            {
                Reset();
                throw;
            }
            finally
            {
                rrLock.ExitWriteLock();
            }
        }
    }
}