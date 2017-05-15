using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace wikiaParser
{
    public class Parser
    {
        string HttpCode;
        string regexSettings;
        List<string> SearchResult = new List<string>();

        public Parser (string Http, string regexS)
        {
            HttpCode = Http;
            regexSettings = regexS;
        }

        public List<string> Parsing()
        {            
            Regex Regex = new Regex(regexSettings);
            Match match = Regex.Match(HttpCode);
            
            // отображаем все совпадения
            while (match.Success)
            {
                // Т.к. мы выделили в шаблоне одну группу (одни круглые скобки),
                // ссылаемся на найденное значение через свойство Groups класса Match
                //result += RemoveDetails(match.Groups[0].Value);
                SearchResult.Add(RemoveDetails(match.Groups[0].Value));
                // Переходим к следующему совпадению
                match = match.NextMatch();
            }
            if (SearchResult.Count != 0)
            {
                CheckExactMatch();
                CheckNumberMatches();
            }
            return SearchResult;
        }

        private String RemoveDetails(String match)
        {
            return match.Replace("</a>", "").Replace("</li>", "").Replace("data-pos=", "").
                    Replace("</b><!--", "").Replace("comment to remove whitespace", "[Лучшие статьи]").
                    Replace("data-event=\"search_click_match\"", "[Точное совпадение]").Replace(">", " ") + Environment.NewLine;
        }

        private void CheckExactMatch()
        {
            if (SearchResult.First().Contains("[Точное совпадение]"))
                SearchResult.Insert(0, "Найдено точное совпадение..."+ Environment.NewLine);
        }

        private void CheckNumberMatches()
        {
            if (SearchResult.Count < 11)
                SearchResult.Insert(0, "Совпадений не найдено..." + Environment.NewLine);
            else if (SearchResult.Count<60)
                SearchResult.Insert(0, "Найдено мало совпадений..." + Environment.NewLine);
            else 
                SearchResult.Insert(0, "Найдено много совпадений :)" + Environment.NewLine);
        }

        //вытащить все ссылки
        public List<string> GetLinks()
        {
            List<string> Links = new List<string>();
            Regex Regex = new Regex("http(.+)");
            string resultString = string.Join(Environment.NewLine, SearchResult.ToArray());
            Match match = Regex.Match(resultString);

            // отображаем все совпадения
            while (match.Success)
            {
                // Т.к. мы выделили в шаблоне одну группу (одни круглые скобки),
                // ссылаемся на найденное значение через свойство Groups класса Match
                //result += RemoveDetails(match.Groups[0].Value);
                Links.Add(match.Groups[0].Value);
                // Переходим к следующему совпадению
                match = match.NextMatch();
            }            
            return Links;
        }
    }
}
