using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
//using NUnit.Framework;

namespace wikiaParser.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Parser_ParsingNullHttpCode_ExpectedZeroResults()
        {
            Parser p = new Parser("", "data-pos=(.+)");
            List<string> l = p.Parsing();            
            Assert.AreEqual(l.Count, 0);
        }

        [TestMethod]
        public void Parser_ParsingSmallHttpCode_ExpectedExactMatch()
        {
            string HTML = "<h1> < a href = class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >Гарри</a> ";
            Parser p = new Parser(HTML, "data-pos=(.+)");
            List<string> l = p.Parsing();
            Assert.AreEqual(l[1], "Найдено точное совпадение...\r\n");
        }

        [TestMethod]
        public void Parser_ParsingHttpCode_ExpectedHarryMatch()
        {
            string HTML = "< a href = \"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >Гарри</a>" + Environment.NewLine +
                "Гарри Поттер — самый знаменитый студент Хогвартса за последние сто лет., Гарри Тригг — волшебник, пропавший без вести в 1997 году.В реальном мире Гарри Меллинг — актёр, исполнивший роль Дадли Дурсля&hellip;" + Environment.NewLine +
                "<li><a href=\"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >http://ru.harrypotter.wikia.com/wiki/Гарри</a></li>";
            Parser p = new Parser(HTML, "data-pos=(.+)");
            List<string> l = p.Parsing();
            Assert.AreEqual(l[2], "\"1\" [Точное совпадение]  Гарри\r\r\n");
        }

        [TestMethod]
        public void Parser_ParsingHttpCode_ExpectedLinkMatch()
        {
            string HTML = "< a href = \"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >Гарри</a>" + Environment.NewLine +
                "Гарри Поттер — самый знаменитый студент Хогвартса за последние сто лет., Гарри Тригг — волшебник, пропавший без вести в 1997 году.В реальном мире Гарри Меллинг — актёр, исполнивший роль Дадли Дурсля&hellip;" + Environment.NewLine +
                "<li><a href=\"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >http://ru.harrypotter.wikia.com/wiki/Гарри</a></li>";
            Parser p = new Parser(HTML, "data-pos=(.+)");
            List<string> l = p.Parsing();
            Assert.AreEqual(l[3], "\"1\" [Точное совпадение]  http://ru.harrypotter.wikia.com/wiki/Гарри\r\n");
        }

        [TestMethod]
        public void Parser_ParsingHttpCodeAndGetLinks_ExpectedSomeLinks()
        {
            string HTML = "< a href = \"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >Гарри</a>" + Environment.NewLine +
                "Гарри Поттер — самый знаменитый студент Хогвартса за последние сто лет., Гарри Тригг — волшебник, пропавший без вести в 1997 году.В реальном мире Гарри Меллинг — актёр, исполнивший роль Дадли Дурсля&hellip;" + Environment.NewLine +
                "<li><a href=\"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >http://ru.harrypotter.wikia.com/wiki/Гарри</a></li>";
            Parser p = new Parser(HTML, "data-pos=(.+)");            
            p.Parsing();
            List<string> links = p.GetLinks();
            Assert.AreEqual(links[0], "http://ru.harrypotter.wikia.com/wiki/Гарри\r");
        }
    }
}
