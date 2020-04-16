using Owin;
using System.Web.Http;

namespace RandomJokeClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Used to configure the Web API for self-host
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}