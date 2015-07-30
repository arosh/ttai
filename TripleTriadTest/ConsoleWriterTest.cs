using System;
using NUnit.Framework;
using TripleTriad;

namespace TripleTriadTest
{
    [TestFixture]
    public class ConsoleWriterTest
    {
        [Test]
        public void CreateScreenで空のステージが表示できること()
        {
            var cw = new ConsoleWriter();
            string screen = cw.CreateScreen();
            screen.Is(@"*---*---*---*
|...|...|...|
|...|...|...|
|...|...|...|
*---*---*---*
|...|...|...|
|...|...|...|
|...|...|...|
*---*---*---*
|...|...|...|
|...|...|...|
|...|...|...|
*---*---*---*
");
        }
        [Test]
        public void SetCardでモモディ・モディを置いてCreateScreenでステージが表示できること()
        {
            var cw = new ConsoleWriter();
            cw.SetCard(1, 2, 2, new int[] { 5, 7, 3, 5 });
            string screen = cw.CreateScreen();
            screen.Is(@"*---*---*---*
|...|...|...|
|...|...|...|
|...|...|...|
*---*---*---*
|...|...|...|
|...|...|...|
|...|...|...|
*---*---*---*
|...|...|.7.|
|...|...|3<5|
|...|...|.5.|
*---*---*---*
");
        }
        [Test]
        public void カード一覧()
        {
            {
                var cw = new ConsoleWriter();
                cw.SetCard(1, 0, 0, State.GetPowers(1, 0));
                cw.SetCard(1, 0, 1, State.GetPowers(1, 1));
                cw.SetCard(1, 0, 2, State.GetPowers(1, 2));
                cw.SetCard(1, 1, 0, State.GetPowers(1, 3));
                cw.SetCard(1, 1, 1, State.GetPowers(1, 4));
                Console.WriteLine(cw.CreateScreen());
            }
            {
                var cw = new ConsoleWriter();
                cw.SetCard(-1, 0, 0, State.GetPowers(-1, 0));
                cw.SetCard(-1, 0, 1, State.GetPowers(-1, 1));
                cw.SetCard(-1, 0, 2, State.GetPowers(-1, 2));
                cw.SetCard(-1, 1, 0, State.GetPowers(-1, 3));
                cw.SetCard(-1, 1, 1, State.GetPowers(-1, 4));
                Console.WriteLine(cw.CreateScreen());
            }
        }
    }
}
