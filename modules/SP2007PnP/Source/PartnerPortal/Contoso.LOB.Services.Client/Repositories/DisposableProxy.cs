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

namespace Contoso.LOB.Services.Client.Repositories
{
    /// <summary>
    /// This class maks it easier to dispose of proxies. By default, the WCF proxy interfaces don't implement 
    /// IDisposable, whereas the proxies themselves do. This wrapper makes it easier to use them in a 'using' statement
    /// </summary>
    /// <typeparam name="TProxy">The type of the proxy that's disposable</typeparam>
    public class DisposableProxy<TProxy> : IDisposable
        where TProxy : class
    {
        /// <summary>
        /// The proxy that's used.
        /// </summary>
        public TProxy Proxy { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableProxy&lt;TProxy&gt;"/> class.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        public DisposableProxy(TProxy proxy)
        {
            this.Proxy = proxy;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources. This disposes of the proxy, if the proxy implements idisposable. 
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                IDisposable disposableProxy = this.Proxy as IDisposable;
                if (disposableProxy != null)
                {
                    disposableProxy.Dispose();
                }
                this.Proxy = null;
            }
        }
    }
}
