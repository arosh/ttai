using System;
using System.Collections.Generic;

namespace TripleTriad
{
    public class Strategy
    {
        public GameRule Rule { get; set; }
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
                        Rule.Apply(state, y, x);
                        next.FlipTurn();
                        yield return next;
                    }
                }
            }
        }
        private bool isEnd(State state)
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
            if (isEnd(state))
            {
                int ret;
                if (my > en) ret = 100;
                else if (my < en) ret = -100;
                else ret = 0;
                return ret * state.NextPlayer;
            }
            return (my - en) * state.NextPlayer;
        }
        private int alphaBeta(State state, int depth, int alpha, int beta)
        {
            if (isEnd(state) || depth == 0) return evaluate(state);
            foreach (var next in nextState(state))
            {
                alpha = Math.Max(alpha, -alphaBeta(next, depth - 1, -beta, -alpha));
                if (alpha >= beta) return alpha;
            }
            return alpha;
        }
    }
}
