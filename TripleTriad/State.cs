using System;

namespace TripleTriad
{
    public class State
    {
        public int NextPlayer { get; set; }
        public static int[,] MyDeck = {
            {7, 3, 5, 5},
            {7, 1, 7, 3},
            {5, 5, 7, 3},
            {5, 7, 3, 5},
            {7, 9, 1, 8},
        };
        public static int[,] EnDeck = {
            {7, 1, 6, 7},
            {1, 8, 7, 4},
            {3, 8, 3, 7},
            {5, 9, 6, 9},
            {6, 6, 6, 5},
        };
        public bool[] MyUsed = new bool[5];
        public bool[] EnUsed = new bool[5];
        /// <summary>
        /// カードの元々の持ち主
        /// </summary>
        public int[,] StageOwner = new int[3, 3];
        /// <summary>
        /// カードのゲーム上の持ち主
        /// </summary>
        public int[,] StagePlayer = new int[3, 3];
        public int[,] StageCard = new int[3, 3];
        public State(int nextPlayer)
        {
#if DEBUG
            if (nextPlayer != 1 && nextPlayer != -1)
            {
                throw new ArgumentException("nextPlayer != 1 && nextPlayer != -1");
            }
#endif
            this.NextPlayer = nextPlayer;
        }
        public State(State state)
        {
            this.NextPlayer = state.NextPlayer;
            state.MyUsed.CopyTo(this.MyUsed, 0);
            state.EnUsed.CopyTo(this.EnUsed, 0);
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    this.StageOwner[y, x] = state.StageOwner[y, x];
                    this.StagePlayer[y, x] = state.StagePlayer[y, x];
                    this.StageCard[y, x] = state.StageCard[y, x];
                }
            }
        }
        public State SetCardCopy(int y, int x, int cardIndex)
        {
#if DEBUG
            checkYX(y, x);
            if (cardIndex < 0 || cardIndex >= 5)
            {
                throw new ArgumentException("cardIndex < 0 || cardIndex >= 5");
            }
            if (StageOwner[y, x] != 0)
            {
                throw new ArgumentException("StageOwner[y, x] != 0");
            }
            if (StagePlayer[y, x] != 0)
            {
                throw new ArgumentException("StagePlayer[y, x] != 0");
            }
            if (NextPlayer == 1 && MyUsed[cardIndex])
            {
                throw new ArgumentException("NextPlayer == 1 && MyUsed[cardIndex]");
            }
            if (NextPlayer == -1 && EnUsed[cardIndex])
            {
                throw new ArgumentException("NextPlayer == -1 && EnUsed[cardIndex]");
            }
#endif
            var state = new State(this);
            if (NextPlayer == 1)
            {
                state.MyUsed[cardIndex] = true;
            }
            else
            {
                state.EnUsed[cardIndex] = true;
            }
            state.StageOwner[y, x] = NextPlayer;
            state.StagePlayer[y, x] = NextPlayer;
            state.StageCard[y, x] = cardIndex;
            return state;
        }
        public void FlipTurn()
        {
            NextPlayer *= -1;
        }
        public void FlipCard(int y, int x)
        {
#if DEBUG
            checkYX(y, x);
            if (StagePlayer[y, x] == 0)
            {
                throw new ArgumentException("StagePlayer[y, x] == 0");
            }
#endif
            StagePlayer[y, x] *= -1;
        }
        public int GetPower(int y, int x, int dir)
        {
#if DEBUG
            checkYX(y, x);
            if (StageOwner[y, x] == 0)
            {
                throw new ArgumentException("StageOwner[y, x] == 0");
            }
            if (dir < 0 || dir >= 4)
            {
                throw new ArgumentException("dir < 0 || dir >= 4");
            }
#endif
            if (StageOwner[y, x] == 1)
            {
                return State.MyDeck[StageCard[y, x], dir];
            }
            else
            {
                return State.EnDeck[StageCard[y, x], dir];
            }
        }
        private static void checkYX(int y, int x)
        {
            if (y < 0 || y >= 3)
            {
                throw new ArgumentException("y < 0 || y >= 3");
            }
            if (x < 0 || x >= 3)
            {
                throw new ArgumentException("x < 0 || x >= 3");
            }
        }
        public static int[] GetPowers(int player, int cardIndex)
        {
#if DEBUG
            if (player != 1 && player != -1)
            {
                throw new ArgumentException("player != 1 && player != -1");
            }
#endif
            int[] ret = new int[4];
            int[,] deck;
            if (player == 1)
            {
                deck = MyDeck;
            }
            else
            {
                deck = EnDeck;
            }
            for (int i = 0; i < 4; i++)
            {
                ret[i] = deck[cardIndex, i];
            }
            return ret;
        }
        public string CreateScreen()
        {
            var cw = new ConsoleWriter();
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (StagePlayer[y, x] != 0)
                    {
                        cw.SetCard(StagePlayer[y, x], y, x, GetPowers(StageOwner[y, x], StageCard[y, x]));
                    }
                }
            }
            return cw.CreateScreen();
        }
    }
}
