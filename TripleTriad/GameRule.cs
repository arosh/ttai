using System;

namespace TripleTriad
{
    public abstract class GameRule
    {
        protected static int[] dy = { 0, -1, 0, 1 };
        protected static int[] dx = { 1, 0, -1, 0 };
        public abstract void Apply(State state, int y, int x);
    }
    public class PlusRule : GameRule
    {
        public override void Apply(State state, int y, int x)
        {
            bool enemyExist = false;
            for (int i = 0; i < 4; i++)
            {
                int ny = y + GameRule.dy[i];
                int nx = x + GameRule.dx[i];
                if (ny < 0 || ny >= 3 || nx < 0 || nx >= 3) continue;
                if (state.StagePlayer[ny, nx] == state.NextPlayer * -1)
                {
                    enemyExist = true;
                    int mypower, enpower;
                    if (state.NextPlayer == 1)
                    {
                        mypower = State.MyDeck[state.StageCard[y, x], i];
                        enpower = State.EnDeck[state.StageCard[ny, nx], (i + 2) % 4];
                    }
                    else
                    {
                        mypower = State.EnDeck[state.StageCard[y, x], i];
                        enpower = State.MyDeck[state.StageCard[ny, nx], (i + 2) % 4];
                    }
                    if (mypower > enpower)
                    {
                        state.FlipCard(ny, nx);
                    }
                }
            }
            if (enemyExist == false) return;
            bool applyPlus = false;
            int[] sum = new int[21];
            for (int i = 0; i < 4; i++)
            {
                int ny = y + GameRule.dy[i];
                int nx = x + GameRule.dx[i];
                if (ny < 0 || ny >= 3 || nx < 0 || nx >= 3) continue;
                if (state.StagePlayer[ny, nx] == 0) continue;
                int mypower, enpower;
                if (state.NextPlayer == 1)
                {
                    mypower = State.MyDeck[state.StageCard[y, x], i];
                    if (state.StagePlayer[ny, nx] == state.NextPlayer)
                    {
                        enpower = State.MyDeck[state.StageCard[ny, nx], (i + 2) % 4];
                    }
                    else
                    {
                        enpower = State.EnDeck[state.StageCard[ny, nx], (i + 2) % 4];
                    }
                }
                else
                {
                    mypower = State.EnDeck[state.StageCard[y, x], i];
                    if (state.StagePlayer[ny, nx] == state.NextPlayer)
                    {
                        enpower = State.EnDeck[state.StageCard[ny, nx], (i + 2) % 4];
                    }
                    else
                    {
                        enpower = State.MyDeck[state.StageCard[ny, nx], (i + 2) % 4];
                    }
                }
#if DEBUG
                if (mypower + enpower > 20) throw new Exception("mypower + enpower > 20");
#endif
                sum[mypower + enpower]++;
            }
            for (int i = 0; i <= 20; i++)
            {
                if (sum[i] >= 2) applyPlus = true;
            }
            if (applyPlus == false) return;
        }
    }
}
