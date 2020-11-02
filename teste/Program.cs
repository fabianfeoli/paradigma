using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste
{
    class Program
    {
        static void Main(string[] args)
        {
            PairTree ptree = new PairTree("[A,B] [A,C] [B,G] [C,H] [E,F] [B,D] [C,E]");
            //PairTree ptree = new PairTree("[B,D] [D,E] [Z,B] [C,F] [E,G] [Z,C]");
            Console.WriteLine( ptree.Print() );
            Console.ReadKey();
        }
    }
}
