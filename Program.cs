using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication6
{
    class Program
    {
       static void Main(string[] args)
        {

              #region display string
           string [] lines=System.IO.File.ReadAllLines(@"C:\Users\Mohammad Zeeshan\Desktop\Code.txt.txt");
            foreach (Lexemes lex in TokenGenerator.splitFunc(lines))
            {

                Console.WriteLine(lex.lexemes + " " + lex.lineNo);
            }
            #endregion


        }
    }
}
