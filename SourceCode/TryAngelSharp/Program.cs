using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using WorkSplitor;

namespace TryAngelSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test1();
            SplitTextTest();
        }


        #region "Test2"
        private static void SplitTextTest()
        {
            Dictionary<string, string> map = new Dictionary<string, string>()
            {
                { "warcraft", "http://www.google.com.tw" },
                { "starcraft", "http://www.google.com.tw" },
            };

            string sourceText = "blizzard is a great company, have build much great games, like 'warcraft', 'starcraft', 'starcraft', 'warcraft', diablo.";

            List<IWordElement> totalNodeList = WordSplitor.SplitWords(sourceText, map);

            Console.WriteLine("Input: " + sourceText);
            Console.WriteLine("  keyword: warcraft, starcraft ");


            foreach (var item in totalNodeList)
            {
                if (item is TextNode)
                    Console.ForegroundColor = ConsoleColor.Red;

                if (item is LinkNode)
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("   " + item.ToString());
            }
        }
        #endregion




        private static void Test1()
        {

            //var config = Configuration.Default.WithDefaultLoader();
            //var address = "https://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes";
            //var context = BrowsingContext.New(config);

            //var document = context.OpenAsync(address).Result;
            //var cellSelector = "tr.vevent td:nth-child(3)";
            //var cells = document.QuerySelectorAll(cellSelector);
            //var titles = cells.Select(m => m.TextContent);
            //---------------------



            var content = File.ReadAllText("TestData2.html");
            var result = RefineImageElement(content);


            File.WriteAllText("TestData3.html", result);

            Console.ReadLine();


            //---------------------
            //var i = 1;
            //var titles = new List<string>();
            //while (true)
            //{
            //	string url = "https://dotblogs.com.tw/kinanson/" + i;
            //	var config = Configuration.Default.WithDefaultLoader();
            //	var dom = BrowsingContext.New(config).OpenAsync(url).Result;
            //	//下面是檢查分頁這個區塊
            //	var page = dom.QuerySelector("body > div.top-wrapper > div > div > div.content-wrapper > div > ul");
            //	if (page == null) break;
            //	//下面需注意一下，因為我要爬的是全部，而不是用chrome工具找出的那一筆而已，所以我把選擇第一筆的部份刪掉
            //	var title = dom.QuerySelectorAll("body > div.top-wrapper > div > div > div.content-wrapper > article > header > h3 > a").Select(x => x.TextContent);
            //	titles.AddRange(title);
            //	i++;
            //}
            //titles.Dump();
        }

        private static string ProcessKeyword(string htmlContent)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(htmlContent);

            var page = document.QuerySelector("body");

            var result = page.InnerHtml.Replace("22", @"<a href=""name1"">22</a>");
            return result;
        }



        private static string TryProcess(string htmlContent)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(htmlContent);
            

            foreach (var element in document.All)
            {
                Console.WriteLine(element.NodeName);

                //if (element.LocalName == "img")
                //{
                //    var newElement = document.CreateElement("v-img");
                //    newElement.SetAttribute("src", element.Attributes["src"] == null ? "" :
                //     element.Attributes["src"].Value);
                //    newElement.SetAttribute("alt", "Article Image");

                //    element.Insert(AdjacentPosition.BeforeBegin, newElement.OuterHtml);
                //    element.Remove();
                //}
            }
            return document.FirstElementChild.OuterHtml;
        }


        private static void TryProces(IElement element)
        {
            if (string.Compare("a", element.LocalName, true) == 0)
                return;

            foreach (var item in element.Children)
            {
                
            }
        }




        private static string RefineImageElement(string htmlContent)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(htmlContent);


            bool hasChanged = false;

            foreach (var element in document.All)
            {
                var childs = element.ChildNodes;

                foreach (var childNode in childs)
                {
                    if (childNode.NodeType == NodeType.Text)
                    {
                        //childNode.Text = childNode.Text.Replace("22", @"<a href=""name1"">22</a>"); 
                        var aa = childNode.TextContent;

                        //var ele1 = document.CreateTextNode("kk123");
                        
                        if (!hasChanged)
                        {
                            element.Insert(AdjacentPosition.BeforeBegin, "kk123");
                            hasChanged = true;
                        }
                        //element.Remove();
                    }

                    //childNode.ReplaceWith(new AngleSharp.Html.Dom.HtmlElement(document, "#text"))
                }

                if (element.LocalName == "img")
                {
                    var newElement = document.CreateElement("v-img");
                    newElement.SetAttribute("src", element.Attributes["src"] == null ? "" :
                     element.Attributes["src"].Value);
                    newElement.SetAttribute("alt", "Article Image");

                    element.Insert(AdjacentPosition.BeforeBegin, newElement.OuterHtml);
                    element.Remove();
                }
            }
            return document.FirstElementChild.OuterHtml;
        }
    }
}
