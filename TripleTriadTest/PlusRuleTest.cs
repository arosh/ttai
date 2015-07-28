using System;
using NUnit.Framework;
using TripleTriad;

namespace TripleTriadTest
{
    [TestFixture]
    class PlusRuleTest
    {
        PlusRule rule = new PlusRule();
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
        }

        [Test]
        public void ひっくり返すだけ()
        {
            // MyDeck[1] == ゲロルト
            // EnDeck[1] == ウリエンジェ
            var s = new State(1).SetCardCopy(2, 0, 1);
            rule.Apply(s, 2, 0);
            s.FlipTurn();
            var t = s.SetCardCopy(1, 0, 1);
            rule.Apply(t, 1, 0);
            t.FlipTurn();
            t.NextPlayer.Is(1);
            t.MyUsed[1].Is(true);
            s.EnUsed[1].Is(false);
            t.EnUsed[1].Is(true);
            s.StagePlayer[2, 0].Is(-1);
            s.StageCard[2, 0].Is(1);
        }
    }
}
