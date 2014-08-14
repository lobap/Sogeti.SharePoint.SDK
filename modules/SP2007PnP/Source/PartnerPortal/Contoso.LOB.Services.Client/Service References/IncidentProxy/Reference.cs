﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3521
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Contoso.LOB.Services.Client.IncidentProxy {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Incident", Namespace="http://Contoso.LOB.Services/2009/01/BusinessEntities")]
    [System.SerializableAttribute()]
    internal partial class Incident : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] HistoryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PartnerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProductField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;
        
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
        internal string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string[] History {
            get {
                return this.HistoryField;
            }
            set {
                if ((object.ReferenceEquals(this.HistoryField, value) != true)) {
                    this.HistoryField = value;
                    this.RaisePropertyChanged("History");
                }
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
        internal string Partner {
            get {
                return this.PartnerField;
            }
            set {
                if ((object.ReferenceEquals(this.PartnerField, value) != true)) {
                    this.PartnerField = value;
                    this.RaisePropertyChanged("Partner");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string Product {
            get {
                return this.ProductField;
            }
            set {
                if ((object.ReferenceEquals(this.ProductField, value) != true)) {
                    this.ProductField = value;
                    this.RaisePropertyChanged("Product");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string Status {
            get {
                return this.StatusField;
            }
            set {
                if ((object.ReferenceEquals(this.StatusField, value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
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
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Contoso.LOB.Services/2009/01", ConfigurationName="IncidentProxy.IIncidentManagement")]
    internal interface IIncidentManagement {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Contoso.LOB.Services/2009/01/IIncidentManagement/GetIncident", ReplyAction="http://Contoso.LOB.Services/2009/01/IIncidentManagement/GetIncidentResponse")]
        Contoso.LOB.Services.Client.IncidentProxy.Incident GetIncident(string incidentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Contoso.LOB.Services/2009/01/IIncidentManagement/WriteToHistory", ReplyAction="http://Contoso.LOB.Services/2009/01/IIncidentManagement/WriteToHistoryResponse")]
        void WriteToHistory(string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    internal interface IIncidentManagementChannel : Contoso.LOB.Services.Client.IncidentProxy.IIncidentManagement, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    internal partial class IncidentManagementClient : System.ServiceModel.ClientBase<Contoso.LOB.Services.Client.IncidentProxy.IIncidentManagement>, Contoso.LOB.Services.Client.IncidentProxy.IIncidentManagement {
        
        public IncidentManagementClient() {
        }
        
        public IncidentManagementClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IncidentManagementClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IncidentManagementClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IncidentManagementClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Contoso.LOB.Services.Client.IncidentProxy.Incident GetIncident(string incidentId) {
            return base.Channel.GetIncident(incidentId);
        }
        
        public void WriteToHistory(string message) {
            base.Channel.WriteToHistory(message);
        }
    }
}