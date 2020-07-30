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
using WorkSplitor.Models;

namespace TryAngelSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
        }





        private static void Test1()
        {
            Dictionary<string, string> dicKeywordMap = new Dictionary<string, string>()
            {
                { "維基百科", "https://www.wikipedia.org/" },
            };

            var content = File.ReadAllText("TestData.html");
            var result = ProcessTextElement(content, dicKeywordMap);


            File.WriteAllText("TestData3.html", result);

            Console.WriteLine("Press Enter to exist. ");
            Console.ReadLine();
        }


        private static string ProcessTextElement(string htmlContent, Dictionary<string, string> dicKeywordMap)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(htmlContent);

            string[] targetTags = { "p", "td", "th", "li", "span", "div", "strong", "em", "s" };


            foreach (var element in document.All)
            {
                var childs = element.ChildNodes;


                if (targetTags.Contains(element.LocalName))
                {
                    ProcessChildNode(document, childs, dicKeywordMap);
                }
            }

            var result = document.QuerySelector("body").InnerHtml;
            return result;
        }


        private static void ProcessChildNode(AngleSharp.Html.Dom.IHtmlDocument document, INodeList childs, Dictionary<string, string> dicKeywordMap)
        {
            for (var i = 0; i < childs.Length; i++)
            {
                var childNode = childs[i];
                
                // 只跑純文字
                if (childNode.NodeType != NodeType.Text)
                    continue;

                // 如果沒有可閱讀文字，跳過
                if (string.IsNullOrWhiteSpace(childNode.TextContent))
                    continue;


                // 切割文字
                var splitedResult = WordSplitor.SplitWords(childNode.TextContent, dicKeywordMap);

                // 如果沒有找到目標
                if (!splitedResult.Where(obj => obj.NodeType == TextNodeType.Link).Any())
                    continue;


                List<INode> nodeList = new List<INode>();

                foreach (var item in splitedResult)
                {
                    if (item is TextNode)
                        nodeList.Add(document.CreateTextNode(item.Context));
                    else
                    {
                        var linkItem = item as LinkNode;

                        var link = document.CreateElement("a");
                        link.SetAttribute("href", linkItem.LinkUrl.ToString());
                        link.SetAttribute("data-autolink", "bot");
                        link.TextContent = linkItem.Context;

                        nodeList.Add(link);
                    }
                }

                childNode.ReplaceWith(nodeList.ToArray());
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
                    childNode.ReplaceWith(document.CreateTextNode("kk123"));
                }

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
    }
}
