using System;

namespace TripleTriad
{
    public abstract class GameRule
    {
        protected static int[] dy = { 0, -1, 0, 1 };
        protected static int[] dx = { 1, 0, -1, 0 };
        public abstract void Apply(State state, int y, int x);
        protected static bool isOutOfRange(int ny, int nx)
        {
            return ny < 0 || ny >= 3 || nx < 0 || nx >= 3;
        }
    }
    public class PlusRule : GameRule
    {
        public override void Apply(State state, int y, int x)
        {
            bool enemyExist = false;
            for (int i = 0; i < 4; i++)
            {
                int ny = y + dy[i];
                int nx = x + dx[i];
                if (isOutOfRange(ny, nx)) continue;
                if (state.StagePlayer[ny, nx] != state.NextPlayer * -1) continue;

                enemyExist = true;
                int mypower = state.GetPower(y, x, i);
                int enpower = state.GetPower(ny, nx, (i + 2) % 4);
                if (mypower > enpower)
                {
                    state.FlipCard(ny, nx);
                }
            }
            if (enemyExist == false) return;
            bool applyPlus = false;
            int[] sum = new int[21];
            for (int i = 0; i < 4; i++)
            {
                int ny = y + dy[i];
                int nx = x + dx[i];
                if (isOutOfRange(ny, nx)) continue;
                if (state.StagePlayer[ny, nx] == 0) continue;
                int mypower = state.GetPower(y, x, i);
                int enpower = state.GetPower(ny, nx, (i + 2) % 4);
#if DEBUG
                if (mypower + enpower < 2) throw new Exception("mypower + enpower < 2");
                if (mypower + enpower > 20) throw new Exception("mypower + enpower > 20");
#endif
                sum[mypower + enpower]++;
            }
            for (int i = 0; i <= 20; i++)
            {
                if (sum[i] >= 2) applyPlus = true;
            }
            if (applyPlus == false) return;
            for (int i = 0; i < 4; i++)
            {
                int ny = y + dy[i];
                int nx = x + dx[i];
                if (isOutOfRange(ny, nx)) continue;
                if (state.StagePlayer[ny, nx] == 0) continue;
                int mypower = state.GetPower(y, x, i);
                int enpower = state.GetPower(ny, nx, (i + 2) % 4);
#if DEBUG
                if (mypower + enpower > 20) throw new Exception("mypower + enpower > 20");
#endif
                if (sum[mypower + enpower] < 2) continue;
                if (state.StagePlayer[ny, nx] == state.NextPlayer * -1)
                {
                    state.FlipCard(ny, nx);
                }
                for (int j = 0; j < 4; j++)
                {
                    int my = ny + dy[i];
                    int mx = nx + dx[i];
                    if (isOutOfRange(my, mx)) continue;
                    if (state.StagePlayer[my, mx] != state.NextPlayer * -1) continue;
                    int mypower2 = state.GetPower(ny, nx, j);
                    int enpower2 = state.GetPower(my, mx, (j + 2) % 4);
                    if (mypower2 > enpower2)
                    {
                        state.FlipCard(my, mx);
                    }
                }
            }
        }
    }
}
