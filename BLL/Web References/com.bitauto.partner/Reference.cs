﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18052
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.18052 版自动生成。
// 
#pragma warning disable 1591

namespace BitAuto.CarChannel.BLL.com.bitauto.partner {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ServiceSoap", Namespace="http://bitauto.com/")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetDealerInfoByCarIdAndCityIdOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service() {
            this.Url = global::BitAuto.CarChannel.BLL.Properties.Settings.Default.BLL_com_bitauto_partner_Service;
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
        public event GetDealerInfoByCarIdAndCityIdCompletedEventHandler GetDealerInfoByCarIdAndCityIdCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://bitauto.com/GetDealerInfoByCarIdAndCityId", RequestNamespace="http://bitauto.com/", ResponseNamespace="http://bitauto.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetDealerInfoByCarIdAndCityId(string AuthorizeCode, int carId, int cityId) {
            object[] results = this.Invoke("GetDealerInfoByCarIdAndCityId", new object[] {
                        AuthorizeCode,
                        carId,
                        cityId});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetDealerInfoByCarIdAndCityIdAsync(string AuthorizeCode, int carId, int cityId) {
            this.GetDealerInfoByCarIdAndCityIdAsync(AuthorizeCode, carId, cityId, null);
        }
        
        /// <remarks/>
        public void GetDealerInfoByCarIdAndCityIdAsync(string AuthorizeCode, int carId, int cityId, object userState) {
            if ((this.GetDealerInfoByCarIdAndCityIdOperationCompleted == null)) {
                this.GetDealerInfoByCarIdAndCityIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetDealerInfoByCarIdAndCityIdOperationCompleted);
            }
            this.InvokeAsync("GetDealerInfoByCarIdAndCityId", new object[] {
                        AuthorizeCode,
                        carId,
                        cityId}, this.GetDealerInfoByCarIdAndCityIdOperationCompleted, userState);
        }
        
        private void OnGetDealerInfoByCarIdAndCityIdOperationCompleted(object arg) {
            if ((this.GetDealerInfoByCarIdAndCityIdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetDealerInfoByCarIdAndCityIdCompleted(this, new GetDealerInfoByCarIdAndCityIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetDealerInfoByCarIdAndCityIdCompletedEventHandler(object sender, GetDealerInfoByCarIdAndCityIdCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetDealerInfoByCarIdAndCityIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetDealerInfoByCarIdAndCityIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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