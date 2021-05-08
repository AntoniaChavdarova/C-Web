using AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var list = new List<string>();

            var id = 2; //11
            try
            {
                for (int i = 1; i < id; i++)
                {
                     var document = await context.OpenAsync($"https://pochivka.bg/apartamenti-a4/{i}");
                   // var document = await context.OpenAsync($"https://pochivka.bg/kashti-a3/{i}");

                    Console.WriteLine($"Page:{i}");
                    var elements = document.QuerySelectorAll(".result-item");

                    foreach (var item in elements)
                    {

                        //var childern = item.ChildElementCount;
                        //Console.WriteLine(childern);
                       // var title = item.QuerySelector(".info > .left-side > .header");
                      
                       //Console.WriteLine(title.TextContent);
                       // if (title.TextContent.Contains(','))
                       // {
                       //     var city = title.TextContent.Split(',');
                       //     Console.WriteLine(city[1]);
                       // }
                        //// var description = item.QuerySelector(".info > .left-side >  div.content.xl");
                        //var description = document.QuerySelector(".result-item > .info > .left-side");
                        //Console.WriteLine("Description");
                        //Console.WriteLine(description.TextContent);

                        //var price = item.QuerySelector(".info > div.right-side > ul > li.price");
                        //Console.WriteLine("Price");
                        //if (price != null)
                        //{

                        //    Console.WriteLine(price.TextContent);

                        //}
                        //Console.WriteLine("Image");
                        ////samo do a - inner html link kum samata kushta
                        //var img = item.QuerySelector(".thumb > a > img ").GetAttribute("src");
                        //Console.WriteLine(img);

                        //Console.WriteLine("Link kum samata oferta");
                        ////tezi linkove gi slagame vuv list i posle obikalqme celiq list
                        var link = item.QuerySelector(".thumb > a ").GetAttribute("href"); ;
                        list.Add(link);
                        Console.WriteLine(link);
                    }
                }

            }
            catch (Exception)
            {


            }

            //za glavnata stranica 
            foreach (var item in list)
            {
                var hhh = "https:" + item;
                var page = await context.OpenAsync(hhh);

                var category = page.QuerySelector("#breadcrumbs > ul > li:nth-child(2) > a");
                Console.WriteLine(category.TextContent);
                var titlePage = page.QuerySelector(" .page-title > .pull-left > h1");
                Console.WriteLine(titlePage.TextContent);

                var city = page.QuerySelector("body > div.container > div > div.property-view.vip > div:nth-child(1) > div > div > div > div.sub-title");
                Console.WriteLine(city.TextContent);

                var descriptionPage = page.QuerySelector("div.col-4.margin-0.pull-right > div.description");
                Console.WriteLine(descriptionPage.TextContent);

                var udobstva = page.QuerySelector("div.col-4 > div.extras > ul");
                Console.WriteLine(udobstva.TextContent);

                var tablewithInfo = page.QuerySelectorAll("#prices > table > tbody > tr > td").Select(x => x.TextContent)
                    .ToList();
                
               Console.WriteLine($" Type: {tablewithInfo[0]}");
               Console.WriteLine($" Mesta: {tablewithInfo[1]}");
               Console.WriteLine($" Broi ot tipa: {tablewithInfo[2]}");
               Console.WriteLine($" Cena lqto: {tablewithInfo[3]}");
               Console.WriteLine($" Cena zima: {tablewithInfo[5]}");
           

                var imags = page.GetElementsByClassName("gallery-slider");

                foreach (var img in imags)
                {
                    Console.WriteLine("Images:");
                    var image = img.QuerySelectorAll("img");
                    foreach (var one in image)
                    {
                        Console.WriteLine(one.GetAttribute("content"));

                    }
                }


            }

        }
    }
}
