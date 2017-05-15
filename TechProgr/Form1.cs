using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace TechProgr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 4;            
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            String Text = textBox1.Text;
            String Site="";

            switch (comboBox1.SelectedIndex)
            {
                case 0: Site = "http://ru.elderscrolls.wikia.com/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:Search?query="; break;
                case 1: Site = "http://ru.terraria.wikia.com/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:Search?query="; break;
                case 2: Site = "http://ru.darksouls.wikia.com/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:Search?query="; break;
                case 3: Site = "http://ru.fallout.wikia.com/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:Search?query="; break;
                case 4: Site = "http://ru.harrypotter.wikia.com/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:Search?query="; break;
                case 5: Site = "http://ru.starwars.wikia.com/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:Search?query="; break;
                case 6: Site = "http://ru.dont-starve.wikia.com/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:Search?query="; break;
                case 7: Site = "http://ru.bindingofisaac.wikia.com/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:Search?query="; break;
                case 8: Site = "http://vedmak.wikia.com/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:Search?query="; break;
                case 9: Site = "http://ru.gameofthrones.wikia.com/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:Search?query="; break;
                case 10: Site= "http://ru.007-pedia.wikia.com/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:Search?query="; break;                
            }                

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Site + Text);
            request.Method = "GET";
            request.Accept = "application/json";            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            StringBuilder output = new StringBuilder();
            output.Append(reader.ReadToEnd());

            textBox2.Text = output.ToString();
            textBox3.Text = "";

            Parser p = new Parser();
            textBox3.Text = p.Parsing(output.ToString(), "data-pos=(.+)");
        }        
    }

    public class Parser
    {
        public String Parsing(string Http, string regexS)
        {
            String result = "";
            Regex Regex = new Regex(regexS);
            Match match = Regex.Match(Http);
            // отображаем все совпадения
            while (match.Success)
            {
                // Т.к. мы выделили в шаблоне одну группу (одни круглые скобки),
                // ссылаемся на найденное значение через свойство Groups класса Match
                result += RemoveDetails(match.Groups[0].Value);                
                // Переходим к следующему совпадению
                match = match.NextMatch();
            }
            return result;
        }

        public String RemoveDetails(String match)
        {            
            return match.Replace("</a>", "").Replace("</li>", "").Replace("data-pos=", "").
                    Replace("</b><!--", "").Replace("comment to remove whitespace", "[Лучшие статьи]").
                    Replace("data-event=\"search_click_match\"", "[Точное совпадение]").Replace(">", " ") + Environment.NewLine;
        }
    }

    [TestFixture]
    public class ParserTestCase
    {
        [Test]
        public void testTrue()
        {
            Parser p = new Parser();
            string HTML = "";
            Assert.AreEqual("", p.Parsing(HTML, "data-pos=(.+)"));            
        }

        [Test]
        public void testFalse()
        {
            Parser p = new Parser();
            string HTML = "";
            Assert.AreEqual("1", p.Parsing(HTML, "data-pos=(.+)"));            
        }

        [Test]
        public void testFirstFind()
        {
            Parser p = new Parser();
            string HTML = "<h1> < a href = class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >Гарри</a> ";
            Assert.AreEqual("\"1\" [Точное совпадение]  Гарри \r\n", p.Parsing(HTML, "data-pos=(.+)"));            
        }

        [Test]
        public void testFullFind()
        {
            Parser p = new Parser();
            string HTML = "< a href = \"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >Гарри</a>" + Environment.NewLine +
                "Гарри Поттер — самый знаменитый студент Хогвартса за последние сто лет., Гарри Тригг — волшебник, пропавший без вести в 1997 году.В реальном мире Гарри Меллинг — актёр, исполнивший роль Дадли Дурсля&hellip;" + Environment.NewLine +
                "<li><a href=\"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >http://ru.harrypotter.wikia.com/wiki/Гарри</a></li>";
            Assert.AreEqual("\"1\" [Точное совпадение]  Гарри\r\r\n\"1\" [Точное совпадение]  http://ru.harrypotter.wikia.com/wiki/Гарри\r\n", p.Parsing(HTML, "data-pos=(.+)"));
        }

        [Test]
        public void testTwoFinds()
        {
            Parser p = new Parser();
            string HTML = "< a href = \"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >Гарри</a>" + Environment.NewLine +
                "Гарри Поттер — самый знаменитый студент Хогвартса за последние сто лет., Гарри Тригг — волшебник, пропавший без вести в 1997 году.В реальном мире Гарри Меллинг — актёр, исполнивший роль Дадли Дурсля&hellip;" + Environment.NewLine +
                "<li><a href=\"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >http://ru.harrypotter.wikia.com/wiki/Гарри</a></li>";
            Assert.AreEqual("\"1\" [Точное совпадение]  Гарри\r\r\n\"1\" [Точное совпадение]  http://ru.harrypotter.wikia.com/wiki/Гарри\r\n", p.Parsing(HTML, "data-pos=(.+)"));
        }

    }
}
