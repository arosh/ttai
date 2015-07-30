using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleTriad
{
    static class Program
    {
        static void Main(string[] args)
        {
            var s = new Strategy();
            s.Rule = new PlusRule();
            s.MiniMax();
            return;
        }
    }
}
