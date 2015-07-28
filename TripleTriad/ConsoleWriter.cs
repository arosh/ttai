using System;
using System.Text;

namespace TripleTriad
{
    public class ConsoleWriter
    {
        private static int[] dy = { 0, -1, 0, 1 };
        private static int[] dx = { 1, 0, -1, 0 };
        private int[,] _player = new int[3, 3];
        private int[, ,] _power = new int[3, 3, 4];
        public void SetCard(int player, int y, int x, int[] power)
        {
#if DEBUG
            if (player != 1 && player != -1)
            {
                throw new ArgumentException("player != 1 && player != -1");
            }
            if (y < 0 || y >= 3)
            {
                throw new ArgumentException("y < 0 || y >= 3");
            }
            if (x < 0 || x >= 3)
            {
                throw new ArgumentException("x < 0 || x >= 3");
            }
#endif
            _player[y, x] = player;
            for (int k = 0; k < 4; k++)
            {
                _power[y, x, k] = power[k];
            }
        }
        public string CreateScreen()
        {
            StringBuilder[] sbs = initScreen();
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (_player[y, x] != 0)
                    {
                        putCard(sbs, y, x);
                    }
                }
            }
            return concatStringBuilder(sbs);
        }
        private void putCard(StringBuilder[] sbs, int y, int x)
        {
#if DEBUG
            if (_player[y, x] != 1 && _player[y, x] != -1)
            {
                throw new ArgumentException("_player[y, x] != 1 && _player[y, x] != -1");
            }
#endif
            int cy = 4 * y + 2;
            int cx = 4 * x + 2;
            sbs[cy][cx] = _player[y, x] == 1 ? '<' : '>';
            for (int k = 0; k < 4; k++)
            {

                sbs[cy + dy[k]][cx + dx[k]] = powerToChar(_power[y, x, k]);
            }
        }
        private static char powerToChar(int p)
        {
#if DEBUG
            if (p <= 0 || p >= 11)
            {
                throw new ArgumentException("p < 0 || p >= 11");
            }
#endif
            if (p == 10)
            {
                return 'A';
            }
            return p.ToString()[0];
        }
        private static string concatStringBuilder(StringBuilder[] sbs)
        {
            var ret = new StringBuilder();
            foreach (var sb in sbs)
            {
                ret.AppendLine(sb.ToString());
            }
            return ret.ToString();
        }
        private static StringBuilder[] initScreen()
        {
            int len = 1 + 3 + 1 + 3 + 1 + 3 + 1;
            StringBuilder[] sbs = new StringBuilder[len];
            for (int y = 0; y < len; y++)
            {
                sbs[y] = new StringBuilder();
                for (int x = 0; x < len; x++)
                {
                    sbs[y].Append('.');
                }
            }
            for (int k = 0; k < len; k++)
            {
                sbs[0][k] = '-';
                sbs[4][k] = '-';
                sbs[8][k] = '-';
                sbs[12][k] = '-';
                sbs[k][0] = '|';
                sbs[k][4] = '|';
                sbs[k][8] = '|';
                sbs[k][12] = '|';
            }
            sbs[0][0] = '*';
            sbs[0][4] = '*';
            sbs[0][8] = '*';
            sbs[0][12] = '*';
            sbs[4][0] = '*';
            sbs[4][4] = '*';
            sbs[4][8] = '*';
            sbs[4][12] = '*';
            sbs[8][0] = '*';
            sbs[8][4] = '*';
            sbs[8][8] = '*';
            sbs[8][12] = '*';
            sbs[12][0] = '*';
            sbs[12][4] = '*';
            sbs[12][8] = '*';
            sbs[12][12] = '*';
            return sbs;
        }
    }
}
