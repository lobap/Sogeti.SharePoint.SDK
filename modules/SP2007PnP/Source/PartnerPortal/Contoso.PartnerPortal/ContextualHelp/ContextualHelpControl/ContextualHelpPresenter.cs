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
using System.Globalization;

using Contoso.PartnerPortal.Properties;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace Contoso.PartnerPortal.ContextualHelp
{
    /// <summary>
    /// 
    /// </summary>
    public class ContextualHelpPresenter
    {
        #region Private Constants

        private const string ContextualHelpContentXml = "Contoso.PartnerPortal.ContextualHelp.HelpContent.xml";

        #endregion

        private readonly object syncRoot = new object();
        private static Dictionary<string, string> helpDictionary;

        IContextualHelpView view;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public ContextualHelpPresenter(IContextualHelpView view)
        {
            this.view = view;

            lock (syncRoot)
            {
                if (helpDictionary == null)
                {
                    //serialize the dictionary of content
                    HelpContent[] helpContents;
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ContextualHelpContentXml))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(HelpContent[]));
                        helpContents = (HelpContent[])xmlSerializer.Deserialize(stream);
                    }
                    helpDictionary = new Dictionary<string, string>(helpContents.Count());
                    foreach (HelpContent helpContent in helpContents)
                    {
                        helpDictionary.Add(helpContent.Url.ToLowerInvariant(), helpContent.Content);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageUrl"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public void SetContent(string pageUrl)
        {
            if (helpDictionary.Keys.Contains(pageUrl.ToLower(CultureInfo.InvariantCulture)))
            {
                this.view.SetContent(helpDictionary[pageUrl.ToLowerInvariant()]);
            }
        }
    }
}
