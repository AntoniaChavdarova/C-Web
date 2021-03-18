using SUS.HTTP;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstMvcApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IHttpServer server = new HttpServer();
           

            server.AddRoute("/", HomePage);
            server.AddRoute("/about", About);
            server.AddRoute("/favicon.ico", FavIcon);
            await server.StartAsync(80);
        }

        private static HttpResponse About(HttpRequest request)
        {
            var responseHTML = "About Us...";
                
            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHTML);
            var response = new HttpResponse("text/html", responseBodyBytes);


            return response;
        }

        static HttpResponse HomePage(HttpRequest request)
        {
            var responseHTML = "Welcome!" +
                request.Headers.FirstOrDefault(x => x.Name == "UserAgent")?.Value;
            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHTML);
            var response = new HttpResponse("text/html", responseBodyBytes);
         

            return response;

        }

        static HttpResponse FavIcon(HttpRequest request)
        {
            var fileBytes = File.ReadAllBytes("wwwroot/favicon.ico");
            var response = new HttpResponse("image/vnd.microsoft.icon", fileBytes);
            return response;
        }

    }

   
}
