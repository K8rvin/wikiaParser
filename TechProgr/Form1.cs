using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace wikiaParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 4;
            tabControl1.SelectedIndex = 1; 
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

            Parser p = new Parser(output.ToString(), "data-pos=(.+)");
            List<string> l = p.Parsing();
            for (int i =0; i< l.Count; i++ )
                textBox3.AppendText(l[i]);            
            textBox3.SelectionStart = 0;
            textBox3.ScrollToCaret();

            List<string> links = p.GetLinks(l);
        }
    }    

    /*[TestFixture]
    public class ParserTestCase
    {
        [Test]
        public void testTwoFinds()
        {
            Parser p = new Parser();
            string HTML = "< a href = \"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >Гарри</a>" + Environment.NewLine +
                "Гарри Поттер — самый знаменитый студент Хогвартса за последние сто лет., Гарри Тригг — волшебник, пропавший без вести в 1997 году.В реальном мире Гарри Меллинг — актёр, исполнивший роль Дадли Дурсля&hellip;" + Environment.NewLine +
                "<li><a href=\"http://ru.harrypotter.wikia.com/wiki/%D0%93%D0%B0%D1%80%D1%80%D0%B8\" class=\"result-link\" data-pos=\"1\" data-event=\"search_click_match\" >http://ru.harrypotter.wikia.com/wiki/Гарри</a></li>";
            Assert.AreEqual("\"1\" [Точное совпадение]  Гарри\r\r\n\"1\" [Точное совпадение]  http://ru.harrypotter.wikia.com/wiki/Гарри\r\n", p.Parsing(HTML, "data-pos=(.+)"));
        }

    }*/
}
