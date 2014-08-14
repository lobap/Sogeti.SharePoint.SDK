//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Collections.Generic;
using System;
using System.Moles;
using Microsoft.Moles.Framework.Behaviors;

namespace Microsoft.Practices.SharePoint.Common.Configuration.Moles
{
    public class BIPropertyBag 
        : SIPropertyBag 
    {
        readonly BehavedDictionary<string, string> values;
        readonly BehavedValue<ConfigLevel> level;

        public BIPropertyBag()
            : this(ConfigLevel.CurrentSPFarm)
        { }

        public BIPropertyBag(ConfigLevel configLevel)
        {
            this.values = new BehavedDictionary<string, string>(this, "Values");
            this.level = new BehavedValue<ConfigLevel>(this, "Level");

            this.Level = configLevel;
            InitializeStubs();
        }

        private void InitializeStubs()
        {
            this.ContainsString = (key) => this.Values.ContainsKey(key);
            this.ItemGetString = (key) =>
            {
                string value = this.Values.GetValueOrDefault(key);
                return value;
            };
            this.ItemSetStringString = (key, value) => this.Values[key] = value;
            this.RemoveString = (key) => this.Values.Remove(key);
            this.LevelGet = () => this.Level;
        }

        public BehavedDictionary<string, string> Values
        {
            get { return this.values; }
        }

        public ConfigLevel Level
        {
            get { return this.level.Value; }
            set { this.level.Value = value; }
        }
    }
}