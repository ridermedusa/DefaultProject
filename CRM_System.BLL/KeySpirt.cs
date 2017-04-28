using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using JiebaNet.Segmenter;
using System.Collections.Generic;
using JiebaNet.Analyser;
using LoTLib.Word.Split;

namespace CRM_System.BLL
{
    public class KeySpirt
    {
        public KeySpirt()
        { }
        public static List<string> SpirtKeyword(string Keyword)
        {
            List<string> stringN = new List<string>();
            if (!string.IsNullOrEmpty(Keyword))
            {
                string A = Keyword.GetSplitWordStr();
                //string B = Keyword.GetArticleKeywordStr();
                stringN.Add(A);
                //stringN.Add(B);
            }
            return stringN;
        }
    }
} 
