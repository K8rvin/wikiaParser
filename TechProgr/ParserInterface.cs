using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wikiaParser
{
    public interface ParserInterface
    {
        void Parser(string Http, string regexS);
        List<string> Parsing();
        String RemoveDetails(String match);
        void CheckExactMatch();
        void CheckNumberMatches();
        List<string> GetLinks(List<string> ResultAfterParsing);
    }
}
