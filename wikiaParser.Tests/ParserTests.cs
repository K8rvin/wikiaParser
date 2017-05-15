using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NSubstitute;

namespace wikiaParser.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Parser_ParsingNullHttpCode_ExpectedZeroResults()
        {
            Parser p = new Parser("", "data-pos=(.+)");
            List<string> resultAfterParsing = p.Parsing();            
            Assert.AreEqual(resultAfterParsing.Count, 0);
        }

        [TestMethod]
        public void Parser_ParsingSmallHttpCode_ExpectedExactMatch()
        {
            string HTML = "<h1> < a href = class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >Гарри</a> ";
            Parser p = new Parser(HTML, "data-pos=(.+)");
            List<string> resultAfterParsing = p.Parsing();
            Assert.AreEqual(resultAfterParsing[1], "Найдено точное совпадение...\r\n");
        }

        [TestMethod]
        public void Parser_ParsingHttpCode_ExpectedHarryMatch()
        {
            string HTML = "< a href = \"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >Гарри</a>" + Environment.NewLine +
                "Гарри Поттер — самый знаменитый студент Хогвартса за последние сто лет., Гарри Тригг — волшебник, пропавший без вести в 1997 году.В реальном мире Гарри Меллинг — актёр, исполнивший роль Дадли Дурсля&hellip;" + Environment.NewLine +
                "<li><a href=\"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >http://ru.harrypotter.wikia.com/wiki/Гарри</a></li>";
            Parser p = new Parser(HTML, "data-pos=(.+)");
            List<string> resultAfterParsing = p.Parsing();
            Assert.AreEqual(resultAfterParsing[2], "\"1\" [Точное совпадение]  Гарри\r\r\n");
        }

        [TestMethod]
        public void Parser_ParsingHttpCode_ExpectedLinkMatch()
        {
            string HTML = "< a href = \"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >Гарри</a>" + Environment.NewLine +
                "Гарри Поттер — самый знаменитый студент Хогвартса за последние сто лет., Гарри Тригг — волшебник, пропавший без вести в 1997 году.В реальном мире Гарри Меллинг — актёр, исполнивший роль Дадли Дурсля&hellip;" + Environment.NewLine +
                "<li><a href=\"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >http://ru.harrypotter.wikia.com/wiki/Гарри</a></li>";
            Parser p = new Parser(HTML, "data-pos=(.+)");
            List<string> resultAfterParsing = p.Parsing();
            Assert.AreEqual(resultAfterParsing[3], "\"1\" [Точное совпадение]  http://ru.harrypotter.wikia.com/wiki/Гарри\r\n");
        }

        [TestMethod]
        public void Parser_ParsingHttpCodeAndGetLinks_ExpectedOneLink()
        {
            string HTML = "class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >http://ru.harrypotter.wikia.com/wiki/Гарри</a></li>";
            Parser p = new Parser(HTML, "data-pos=(.+)");
            List<string> resultAfterParsing = p.Parsing();
            List<string> links = p.GetLinks(resultAfterParsing);
            Assert.AreEqual(links[0], "http://ru.harrypotter.wikia.com/wiki/Гарри\r");
        }

        [TestMethod]
        public void Parser_ParsingWithoutHTTP_ExpectedThreeMatches()
        {

            var calculator = Substitute.For<ParserInterface>();
            List<string> FakeResultAfterParsing = new List<string>();
            FakeResultAfterParsing.Add("\"1\" [Точное совпадение]  Гарри\r\n");
            FakeResultAfterParsing.Add("\"1\" [Точное совпадение]  http://ru.harrypotter.wikia.com/wiki/Гарри\r\n");
            FakeResultAfterParsing.Add("\"2\"  Гарри Тейлор\r\n");

            calculator.Parsing().Returns(FakeResultAfterParsing);
            var ResultAfterParsing = calculator.Parsing();
            Assert.AreEqual(ResultAfterParsing.Count, 3);            
        }

        [TestMethod]
        public void Parser_ParsingWithoutHTTPAndGetLinks_ExpectedTwoLinks()
        {

            var calculator = Substitute.For<ParserInterface>();
            List<string> FakeResultAfterParsing = new List<string>();
            FakeResultAfterParsing.Add("\"1\" [Точное совпадение]  Гарри\r\n");
            FakeResultAfterParsing.Add("\"1\" [Точное совпадение]  http://ru.harrypotter.wikia.com/wiki/Гарри\r\n");
            FakeResultAfterParsing.Add("\"2\"  Гарри Тейлор\r\n");
            FakeResultAfterParsing.Add("\"2\"  http://ru.harrypotter.wikia.com/wiki/Гарри_Тейлор\r\n");

            calculator.Parsing().Returns(FakeResultAfterParsing);
            var ResultAfterParsing = calculator.Parsing();
            Parser p = new Parser("", "data-pos=(.+)");
            List<string> links = p.GetLinks(ResultAfterParsing);
            Assert.AreEqual(links.Count, 2);
        }

        [TestMethod]
        public void Parser_ParsingWithoutHTTPAndGetLinks_ExpectedEightLinks()
        {
            var calculator = Substitute.For<ParserInterface>();
            List<string> FakeResultAfterParsing = new List<string>();
            FakeResultAfterParsing.Add("\"1\" [Точное совпадение]  Гарри\r\n");
            FakeResultAfterParsing.Add("\"1\" [Точное совпадение]  http://ru.harrypotter.wikia.com/wiki/Гарри\r\n");
            FakeResultAfterParsing.Add("\"2\"  Гарри Тейлор\r\n");
            FakeResultAfterParsing.Add("\"2\"  http://ru.harrypotter.wikia.com/wiki/Гарри_Тейлор\r\n");
            FakeResultAfterParsing.Add("\"3\"  Гарри Поттер и я\r\n");
            FakeResultAfterParsing.Add("\"3\"  http://ru.harrypotter.wikia.com/wiki/Гарри_Поттер_и_я\r\n");
            FakeResultAfterParsing.Add("\"4\"  Гарри Поттер\r\n");
            FakeResultAfterParsing.Add("\"4\"  http://ru.harrypotter.wikia.com/wiki/Гарри_Поттер\r\n");
            FakeResultAfterParsing.Add("\"5\"  Гарри Меллинг\r\n");
            FakeResultAfterParsing.Add("\"5\"  http://ru.harrypotter.wikia.com/wiki/Гарри_Меллинг\r\n");
            FakeResultAfterParsing.Add("\"14\"  Гарри Поттер и Дары Смерти\r\n");
            FakeResultAfterParsing.Add("\"14\"  http://ru.harrypotter.wikia.com/wiki/Гарри_Поттер_и_Дары_Смерти\r\n");
            FakeResultAfterParsing.Add("\"17\"  Комната Гарри в доме Дурслей\r\n");
            FakeResultAfterParsing.Add("\"17\"  http://ru.harrypotter.wikia.com/wiki/Комната_Гарри_в_доме_Дурслей\r\n");
            FakeResultAfterParsing.Add("\"24\"  Палочка Гарри Поттера\r\n");
            FakeResultAfterParsing.Add("\"24\"  http://ru.harrypotter.wikia.com/wiki/Палочка_Гарри_Поттера\r\n");

            calculator.Parsing().Returns(FakeResultAfterParsing);
            var ResultAfterParsing = calculator.Parsing();
            Parser p = new Parser("", "");
            List<string> links = p.GetLinks(ResultAfterParsing);
            Assert.AreEqual(links.Count, 8);
        }
    }
}
