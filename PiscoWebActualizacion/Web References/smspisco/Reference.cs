﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace PiscoWebActualizacion.smspisco {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SigmaSMSBinding", Namespace="urn:miserviciowsdl")]
    public partial class SigmaSMS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback EnviarSMSOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConsultarCupoOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SigmaSMS() {
            this.Url = global::PiscoWebActualizacion.Properties.Settings.Default.PiscoWebActualizacion_smspisco_SigmaSMS;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event EnviarSMSCompletedEventHandler EnviarSMSCompleted;
        
        /// <remarks/>
        public event ConsultarCupoCompletedEventHandler ConsultarCupoCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://sms.piscotics.com/WebServiceSigmaSMS/servicio.php/EnviarSMS", RequestNamespace="urn:miserviciowsdl", ResponseNamespace="urn:miserviciowsdl")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string EnviarSMS(string phone, string message, string apiclave, string apisecret) {
            object[] results = this.Invoke("EnviarSMS", new object[] {
                        phone,
                        message,
                        apiclave,
                        apisecret});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void EnviarSMSAsync(string phone, string message, string apiclave, string apisecret) {
            this.EnviarSMSAsync(phone, message, apiclave, apisecret, null);
        }
        
        /// <remarks/>
        public void EnviarSMSAsync(string phone, string message, string apiclave, string apisecret, object userState) {
            if ((this.EnviarSMSOperationCompleted == null)) {
                this.EnviarSMSOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnviarSMSOperationCompleted);
            }
            this.InvokeAsync("EnviarSMS", new object[] {
                        phone,
                        message,
                        apiclave,
                        apisecret}, this.EnviarSMSOperationCompleted, userState);
        }
        
        private void OnEnviarSMSOperationCompleted(object arg) {
            if ((this.EnviarSMSCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.EnviarSMSCompleted(this, new EnviarSMSCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://sms.piscotics.com/WebServiceSigmaSMS/servicio.php/ConsultarCupo", RequestNamespace="urn:miserviciowsdl", ResponseNamespace="urn:miserviciowsdl")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string ConsultarCupo(string apiclave, string apisecret) {
            object[] results = this.Invoke("ConsultarCupo", new object[] {
                        apiclave,
                        apisecret});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ConsultarCupoAsync(string apiclave, string apisecret) {
            this.ConsultarCupoAsync(apiclave, apisecret, null);
        }
        
        /// <remarks/>
        public void ConsultarCupoAsync(string apiclave, string apisecret, object userState) {
            if ((this.ConsultarCupoOperationCompleted == null)) {
                this.ConsultarCupoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultarCupoOperationCompleted);
            }
            this.InvokeAsync("ConsultarCupo", new object[] {
                        apiclave,
                        apisecret}, this.ConsultarCupoOperationCompleted, userState);
        }
        
        private void OnConsultarCupoOperationCompleted(object arg) {
            if ((this.ConsultarCupoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultarCupoCompleted(this, new ConsultarCupoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0")]
    public delegate void EnviarSMSCompletedEventHandler(object sender, EnviarSMSCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EnviarSMSCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal EnviarSMSCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0")]
    public delegate void ConsultarCupoCompletedEventHandler(object sender, ConsultarCupoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultarCupoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConsultarCupoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591