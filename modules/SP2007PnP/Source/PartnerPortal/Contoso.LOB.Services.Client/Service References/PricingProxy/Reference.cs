﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3521
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Contoso.LOB.Services.Client.PricingProxy {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Price", Namespace="http://Contoso.LOB.Services/2009/01/BusinessEntities")]
    [System.SerializableAttribute()]
    internal partial class Price : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PartnerIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProductSkuField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal ValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string PartnerId {
            get {
                return this.PartnerIdField;
            }
            set {
                if ((object.ReferenceEquals(this.PartnerIdField, value) != true)) {
                    this.PartnerIdField = value;
                    this.RaisePropertyChanged("PartnerId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string ProductSku {
            get {
                return this.ProductSkuField;
            }
            set {
                if ((object.ReferenceEquals(this.ProductSkuField, value) != true)) {
                    this.ProductSkuField = value;
                    this.RaisePropertyChanged("ProductSku");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal decimal Value {
            get {
                return this.ValueField;
            }
            set {
                if ((this.ValueField.Equals(value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Discount", Namespace="http://Contoso.LOB.Services/2009/01/BusinessEntities")]
    [System.SerializableAttribute()]
    internal partial class Discount : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PartnerIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProductSkuField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal ValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string PartnerId {
            get {
                return this.PartnerIdField;
            }
            set {
                if ((object.ReferenceEquals(this.PartnerIdField, value) != true)) {
                    this.PartnerIdField = value;
                    this.RaisePropertyChanged("PartnerId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string ProductSku {
            get {
                return this.ProductSkuField;
            }
            set {
                if ((object.ReferenceEquals(this.ProductSkuField, value) != true)) {
                    this.ProductSkuField = value;
                    this.RaisePropertyChanged("ProductSku");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal decimal Value {
            get {
                return this.ValueField;
            }
            set {
                if ((this.ValueField.Equals(value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Contoso.LOB.Services/2009/01", ConfigurationName="PricingProxy.IPricing")]
    internal interface IPricing {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Contoso.LOB.Services/2009/01/IPricing/GetPriceBySku", ReplyAction="http://Contoso.LOB.Services/2009/01/IPricing/GetPriceBySkuResponse")]
        Contoso.LOB.Services.Client.PricingProxy.Price GetPriceBySku(string sku);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Contoso.LOB.Services/2009/01/IPricing/GetDiscountById", ReplyAction="http://Contoso.LOB.Services/2009/01/IPricing/GetDiscountByIdResponse")]
        Contoso.LOB.Services.Client.PricingProxy.Discount GetDiscountById(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Contoso.LOB.Services/2009/01/IPricing/GetDiscountByName", ReplyAction="http://Contoso.LOB.Services/2009/01/IPricing/GetDiscountByNameResponse")]
        Contoso.LOB.Services.Client.PricingProxy.Discount GetDiscountByName(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Contoso.LOB.Services/2009/01/IPricing/GetDiscounts", ReplyAction="http://Contoso.LOB.Services/2009/01/IPricing/GetDiscountsResponse")]
        Contoso.LOB.Services.Client.PricingProxy.Discount[] GetDiscounts();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Contoso.LOB.Services/2009/01/IPricing/GetDiscountsBySku", ReplyAction="http://Contoso.LOB.Services/2009/01/IPricing/GetDiscountsBySkuResponse")]
        Contoso.LOB.Services.Client.PricingProxy.Discount[] GetDiscountsBySku(string sku);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    internal interface IPricingChannel : Contoso.LOB.Services.Client.PricingProxy.IPricing, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    internal partial class PricingClient : System.ServiceModel.ClientBase<Contoso.LOB.Services.Client.PricingProxy.IPricing>, Contoso.LOB.Services.Client.PricingProxy.IPricing {
        
        public PricingClient() {
        }
        
        public PricingClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PricingClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PricingClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PricingClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Contoso.LOB.Services.Client.PricingProxy.Price GetPriceBySku(string sku) {
            return base.Channel.GetPriceBySku(sku);
        }
        
        public Contoso.LOB.Services.Client.PricingProxy.Discount GetDiscountById(string id) {
            return base.Channel.GetDiscountById(id);
        }
        
        public Contoso.LOB.Services.Client.PricingProxy.Discount GetDiscountByName(string name) {
            return base.Channel.GetDiscountByName(name);
        }
        
        public Contoso.LOB.Services.Client.PricingProxy.Discount[] GetDiscounts() {
            return base.Channel.GetDiscounts();
        }
        
        public Contoso.LOB.Services.Client.PricingProxy.Discount[] GetDiscountsBySku(string sku) {
            return base.Channel.GetDiscountsBySku(sku);
        }
    }
}
