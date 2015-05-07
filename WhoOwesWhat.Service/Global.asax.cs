using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface.Cors;
using ServiceStack.WebHost.Endpoints;
using WhoOwesWhat.Ninject;

namespace WhoOwesWhat.Service
{
    public class Global : System.Web.HttpApplication
    {
        public class WhoOwesWhatServiceHost : AppHostBase
        {
            //Register your web service with ServiceStack.
            public WhoOwesWhatServiceHost()
                : base("WhoOwesWhat Service", typeof(WhoOwesWhatService).Assembly)
            { }

            public override void Configure(Funq.Container container)
            {
                //Register any dependencies your services use here.
                NinjectKernel.Load(container);

                Plugins.Add(new CorsFeature());

                SetConfig(new EndpointHostConfig
                {
                    DefaultContentType = ContentType.Json
                });

                RequestFilters.Add((httpReq, httpRes, requestDto) =>
                {
                    if (httpReq.HttpMethod == "OPTIONS")
                    {
                        httpRes.AddHeader("Access-Control-Allow-Origin", "*");
                        httpRes.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                        httpRes.AddHeader("Access-Control-Allow-Headers", "X-Requested-With, Content-Type");
                        httpRes.End();
                    }
                });
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your web service on startup.
            new WhoOwesWhatServiceHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}