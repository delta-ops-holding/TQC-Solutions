using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Routing;

namespace ApiEmily
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new JsonSettings().ContractResolver;
            config.Formatters.Add(new BrowserJsonFormatter());

            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "v1.0-api",
                routeTemplate: "{version}/{controller}/{id}",
                defaults: new { controller = "ClanV1", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "v2.0-api",
                routeTemplate: "{version}/{controller}/{id}",
                defaults: new { controller = "ClanV2", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "v3.0-api",
                routeTemplate: "{version}/{controller}/{id}",
                defaults: new { controller = "ClanV3", id = RouteParameter.Optional }
            );
        }
    }

    public class BrowserJsonFormatter : JsonMediaTypeFormatter
    {
        public BrowserJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            this.SerializerSettings.Formatting = Formatting.Indented;
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }

    public class JsonSettings : JsonSerializerSettings
    {
        public JsonSettings()
        {
            this.Formatting = Formatting.Indented;
            this.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    OverrideSpecifiedNames = true
                },
                SerializeCompilerGeneratedMembers = true
            };
        }
    }
}
