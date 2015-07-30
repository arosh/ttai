using System;
using NUnit.Framework;
using TripleTriad;

namespace TripleTriadTest
{
    [TestFixture]
    class PlusRuleTest
    {
        GameRule rule = new PlusRule();
        [Test]
        public void 置くだけ()
        {
            var s = new State(1).SetCardCopy(0, 0, 0);
            rule.Apply(s, 0, 0);
            s.FlipTurn();
            s.NextPlayer.Is(-1);
            s.MyUsed[0].Is(true);
            s.StagePlayer[0, 0].Is(1);
            s.StageCard[0, 0].Is(0);
            s.StageOwner[0, 0].Is(1);
        }
        [Test]
        public void ひっくり返すだけ()
        {
            // MyDeck[1] == ゲロルト 7173
            // EnDeck[1] == ウリエンジェ 1874
            var s = new State(1).SetCardCopy(2, 0, 1);
            rule.Apply(s, 2, 0);
            s.FlipTurn();
            var t = s.SetCardCopy(1, 0, 1);
            rule.Apply(t, 1, 0);
            t.FlipTurn();
            t.NextPlayer.Is(1, message: "NextPlayer.Is(1)");
            t.MyUsed[1].IsTrue();
            s.EnUsed[1].IsFalse();
            t.EnUsed[1].IsTrue();
            t.StageOwner[2, 0].Is(1);
            t.StagePlayer[2, 0].Is(-1, message: "StagePlayer[2, 0].Is(-1)");
            t.StageCard[2, 0].Is(1);
        }
        [Test]
        public void コンボ発動()
        {
            var s = new State(-1);
            s = s.SetCardCopy(0, 1, 3);
            rule.Apply(s, 0, 1);
            s.FlipTurn();
            s = s.SetCardCopy(0, 0, 3);
            rule.Apply(s, 0, 0);
            s.FlipTurn();
            s.StagePlayer[0, 0].Is(1, "s.StagePlayer[0, 0].Is(1) (A)");
            s = s.SetCardCopy(1, 0, 0);
            rule.Apply(s, 1, 0);
            s.FlipTurn();
            s.StagePlayer[0, 0].Is(1, "s.StagePlayer[0, 0].Is(1) (B)");
            s = s.SetCardCopy(1, 1, 2);
            rule.Apply(s, 1, 1);
            s.FlipTurn();
            s.NextPlayer.Is(-1);
            Console.Error.WriteLine(s.CreateScreen());
            s.StageCard[0, 1].Is(3);
            s.MyUsed[3].IsTrue();
            s.MyUsed[2].IsTrue();
            s.EnUsed[0].IsTrue();
            s.EnUsed[3].IsTrue();
            s.StagePlayer[0, 0].Is(1, "s.StagePlayer[0, 0].Is(1)");
            s.StagePlayer[0, 1].Is(1, "s.StagePlayer[0, 1].Is(1)");
            s.StagePlayer[1, 0].Is(1, "s.StagePlayer[1, 0].Is(1)");
            s.StagePlayer[1, 1].Is(1, "s.StagePlayer[1, 1].Is(1)");
        }
    }
}
