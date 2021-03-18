using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUS.HTTP
{
    public class HttpRequest
    {

        public HttpRequest(string requestString)
        {
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();


            var lines = requestString.Split(new string[] { HttpConstants.NewLine }, System.StringSplitOptions.None);

            //Get /somepage HTTP/1.1
            var headerLine = lines[0];
            var headerLineParts = headerLine.Split(' ');
            this.Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerLineParts[0], true);
            this.Path = headerLineParts[1];

            var lineIndex = 1;
            bool isInHeaders = true;
            var bodyBuilder = new StringBuilder();
            while(lineIndex < lines.Length)
            {
                var line = lines[lineIndex];
                lineIndex++;

                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeaders = false;
                    continue;
                    
                }

                if (isInHeaders)
                {
                    this.Headers.Add(new Header(line));
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }

                if(this.Headers.Any(x => x.Name == HttpConstants.RequestCookieHeader))
                {
                    var cookiesAsString = this.Headers.FirstOrDefault(x => x.Name == HttpConstants.RequestCookieHeader).Value;

                    var cookies = cookiesAsString.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in cookies)
                    {
                        this.Cookies.Add(new Cookie(cookiesAsString));
                    }
                }


            }

            this.Body = bodyBuilder.ToString();

        }



    public string Path { get; set; }

        public string QueryString { get; set; }

        public HttpMethod Method { get; set; }

        public ICollection<Header> Headers { get; set; }

        public ICollection<Cookie> Cookies { get; set; }

        public IDictionary<string, string> FormData { get; set; }

        public IDictionary<string, string> QueryData { get; set; }

        public Dictionary<string, string> Session { get; set; }

        public string Body { get; set; }
    }
}