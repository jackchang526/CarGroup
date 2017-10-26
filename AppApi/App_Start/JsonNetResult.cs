using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace AppApi
{
    public class JsonNetResult : JsonResult
    {
        public List<JsonConverter> Converters { get; set; }

        public IContractResolver ContractResolver { get; set; }

        public JsonNetResult(params JsonConverter[] converters)
            : this(null, converters)
        {

        }

        public JsonNetResult(IContractResolver contractResover, params JsonConverter[] converters)
        {
            this.Converters = new List<JsonConverter>();
            this.ContractResolver = contractResover;
            if (converters != null && converters.Length > 0)
            {
                Converters.AddRange(converters);
            }
            else
            {
                Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss", Culture = System.Globalization.CultureInfo.CurrentCulture });
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if ((this.JsonRequestBehavior == System.Web.Mvc.JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                //throw new InvalidOperationException(MvcResources.JsonRequest_GetNotAllowed);
                base.ExecuteResult(context);
                return;
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                var settings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, Converters = Converters.ToArray() };
                if (ContractResolver != null)
                {
                    settings.ContractResolver = this.ContractResolver;
                }
                var serializedObject = JsonConvert.SerializeObject(Data, settings);
                response.Write(serializedObject);
            }
        }
    }
}