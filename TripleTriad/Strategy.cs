using System;
using System.Collections.Generic;

namespace TripleTriad
{
    public class Strategy
    {
        public int FirstPlayer;
        public GameRule Rule { get; set; }
        public int SearchCount;
        public int MoveCard, MoveY, MoveX;
        public static Random rnd = new Random();
        private IEnumerable<State> nextState(State state)
        {
            bool[] used;
            if (state.NextPlayer == 1) used = state.MyUsed;
            else used = state.EnUsed;
            for (int card = 0; card < 5; card++)
            {
                if (used[card]) continue;
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (state.StagePlayer[y, x] != 0) continue;
                        var next = state.SetCardCopy(y, x, card);
                        Rule.Apply(next, y, x);
                        next.FlipTurn();
                        MoveCard = card;
                        MoveY = y;
                        MoveX = x;
                        yield return next;
                    }
                }
            }
        }
        private State randomNextState(State state)
        {
            bool[] used;
            if (state.NextPlayer == 1) used = state.MyUsed;
            else used = state.EnUsed;
            var ret = new List<Func<State>>();
            for (int card = 0; card < 5; card++)
            {
                if (used[card]) continue;
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (state.StagePlayer[y, x] != 0) continue;
                        var next = state.SetCardCopy(y, x, card);
                        Rule.Apply(next, y, x);
                        next.FlipTurn();
                        int tmpCard = card;
                        int tmpY = y;
                        int tmpX = x;
                        ret.Add(() =>
                        {
                            MoveCard = tmpCard;
                            MoveY = tmpY;
                            MoveX = tmpX;
                            return next;
                        });
                    }
                }
            }
            return ret[rnd.Next(ret.Count)]();
        }
        private static bool isEnd(State state)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (state.StagePlayer[y, x] == 0) return false;
                }
            }
            return true;
        }
        private int evaluate(State state)
        {
            int my = 0, en = 0;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (state.StagePlayer[y, x] == 1) my++;
                    if (state.StagePlayer[y, x] == -1) en++;
                }
            }
            if (FirstPlayer == 1)
            {
                en++;
            }
            if (FirstPlayer == -1)
            {
                my++;
            }
            if (isEnd(state))
            {
                return (my - en) * 100 * state.NextPlayer;
            }
            return (my - en) * state.NextPlayer;
        }
        private int alphaBeta(State state, int depth, int alpha, int beta, out int bestCard, out int bestY, out int bestX)
        {
            bestCard = -1;
            bestY = -1;
            bestX = -1;
            if (depth == 0 || isEnd(state))
            {
                SearchCount++;
                return evaluate(state);
            }
            int bestScore = int.MinValue;
            int card, y, x;
            foreach (var next in nextState(state))
            {
                int tmpCard = MoveCard, tmpY = MoveY, tmpX = MoveX;
                alpha = Math.Max(alpha, -alphaBeta(next, depth - 1, -beta, -alpha, out card, out y, out x));
                if (bestScore < alpha)
                {
                    bestScore = alpha;
                    bestCard = tmpCard;
                    bestY = tmpY;
                    bestX = tmpX;
                }
                if (alpha >= beta) break;
            }
            return alpha;
        }
        public void MiniMax()
        {
            FirstPlayer = -1;
            // FirstPlayer = -1;
            var s = new State(FirstPlayer);
            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine(s.CreateScreen());
                Console.WriteLine("Turn = {0}", s.NextPlayer == 1 ? '<' : '>');
                int bestCard, bestY, bestX;
                if (i % 2 == 0)
                {
                    SearchCount = 0;
                    int score = alphaBeta(s, 9, int.MinValue, int.MaxValue, out bestCard, out bestY, out bestX);
                    Console.WriteLine("score = {0}, search = {1}", score, SearchCount);
                }
                else
                {
                    randomNextState(s);
                    bestCard = MoveCard;
                    bestY = MoveY;
                    bestX = MoveX;
                }
                s = s.SetCardCopy(bestY, bestX, bestCard);
                Rule.Apply(s, bestY, bestX);
                s.FlipTurn();
            }
            Console.WriteLine(s.CreateScreen());
        }
    }
}
