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
using System.Linq;
using System.Text;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using Microsoft.Moles.Framework.Behaviors;

namespace Microsoft.Practices.SharePoint.Common.Tests.Mocks
{
    /// <summary>
    /// A behaved type of the IConfigSettingSerializer. 
    /// This serializer is implemented as 
    /// a table to (key, type, object) tuples. 
    /// Serialization is done as a lookup.
    /// </summary>
    public class BIConfigSettingSerializer
        : SIConfigSettingSerializer
    {
        public struct Setting
        {
            public readonly string Key;
            public readonly Type Type;
            public readonly Object Value;
            public Setting(string key, Type type, object value)
            {
                this.Key = key;
                this.Type = type;
                this.Value = value;
            }
        }

        readonly BehavedCollection<Setting> settings;
        public BIConfigSettingSerializer()
        {
            this.settings = new BehavedCollection<Setting>(this, "Settings");
            this.SerializeTypeObject = (t, o) =>
                {
                    if (o == null) return null;

                    var key = this.settings.Count.ToString();
                    var setting = new Setting(key, t, o);
                    this.settings.SetOne(setting);
                    return key;
                };
            this.DeserializeTypeString = (t, k) =>
                {
                    if (k == null) return null;

                    foreach (var setting in this.settings)
                    {
                        if (String.Equals(setting.Key, k, StringComparison.Ordinal))
                        {
                            if (setting.Type != t)
                                // TODO review which exception is thrown here
                                throw new InvalidCastException();
                            return setting.Value;
                        }
                    }
                    throw new BehaviorMissingValueException(this, k);
                };
        }

        public BehavedCollection<Setting> Settings
        {
            get { return this.settings; }
        }
    }
}
