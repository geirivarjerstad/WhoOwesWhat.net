using Nancy.ModelBinding;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.NancyServer
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return View["index"];
            };

            Get["/authenticateUser"] = parameters =>
            {
                var response = new BasicResponse();
                response.isSuccess = true;

                return Response.AsJson(response);
            };

            Post["/user/new"] = parameters =>
            {
                var model = this.Bind<DynamicDictionary>();

                var response = new BasicResponse();
                response.isSuccess = true;

                return Response.AsJson(response);
            };
        }
    }

}