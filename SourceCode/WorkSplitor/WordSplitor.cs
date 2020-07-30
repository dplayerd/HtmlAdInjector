using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSplitor
{
    /// <summary> 文字切割器 </summary>
    public class WordSplitor
    {
        /// <summary> 切割文字 </summary>
        /// <param name="sourceText"> 原始文字 </param>
        /// <param name="keywordAndUrl"> 要切割的文字以及要取代的 Url </param>
        /// <returns></returns>
        public static List<IWordElement> SplitWords(string sourceText, Dictionary<string, string> keywordAndUrl)
        {
            // 轉換原文字至資料模型
            var testText = new TextNode() { Context = sourceText };
            List<IWordElement> list = new List<IWordElement>() { testText };

            // 依每個關鍵字切割
            foreach (var item in keywordAndUrl)
            {
                Uri url = new Uri(item.Value);
                WordSplitor.ParseTextNodes(list, item.Key, url);
            }

            return list;
        }


        /// <summary> 切割文字 </summary>
        /// <param name="list"> 要切割的原文 </param>
        /// <param name="keyword"> 目標關鍵字 </param>
        /// <param name="url"> 要更換的 url </param>
        private static void ParseTextNodes(List<IWordElement> list, string keyword, Uri url)
        {
            // 切割原文，關鍵字成為連結物件
            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];

                // 如果是連結物件，不處理 
                if (item is LinkNode)
                    continue;


                // 如果不包含目標關鍵字，不處理
                if (!item.Context.Contains(keyword))
                    continue;


                // 將文字依照關鍵字切成陣列，並且組成文字和連結清單
                string[] arr = item.Context.Split(new string[] { keyword }, StringSplitOptions.None);
                List<IWordElement> tempList = new List<IWordElement>();
                for (var j = 0; j < arr.Length; j++)
                {
                    tempList.Add(new TextNode() { Context = arr[j] });

                    if (j < (arr.Length - 1))
                        tempList.Add(new LinkNode() { Context = keyword, LinkUrl = url });
                }

                // 移除原字，並加入切割後的結果
                list.Remove(item);
                list.InsertRange(i, tempList);
            }
        }
    }
}
