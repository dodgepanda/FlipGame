using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlipGame
{
    enum PieceColor { BLACK = -1, WHITE = 1 };
    class FlipBoard
    {
        int numXPieces, numYPieces;
        int[,] board;
        FlipBoard parentFlipBoard;
        public FlipBoard(int x, int y)
        {
            parentFlipBoard = null;
            numXPieces = (int)Microsoft.Xna.Framework.MathHelper.Clamp(x, 3, 5);
            numYPieces = (int)Microsoft.Xna.Framework.MathHelper.Clamp(y, 3, 5);
            board = new int[numXPieces, numYPieces];
            for (int i = 0; i < numXPieces; i++ )
            {
                for (int j = 0; j < numYPieces; j++)
                {
                    board[i, j] = -1;
                }
            }
        }
        public void newGame()
        {
            parentFlipBoard = null;
            Random rand = new Random();
            for (int i = 0; i < numXPieces; i++)
            {
                for (int j = 0; j < numYPieces; j++)
                {
                    board[i, j] = -1;
                }
            }
            while (isGameOver())
            {
                int numFlips = rand.Next(100, 100);
                while (numFlips != 0)
                {
                    flipPiece(rand.Next(0, numXPieces), rand.Next(0, numXPieces));
                    numFlips--;
                }
            }
        }
        public void test()
        {
            board[0, 1] = 1;
            //board[1, 1] = 1;
            //board[1, 2] = 1;
            //board[2, 1] = 1;
            //board[1, 0] = 1;
        }
        public FlipBoard getParent() { return parentFlipBoard; }
        public Microsoft.Xna.Framework.Vector2 getNumXY()
        {
            return new Microsoft.Xna.Framework.Vector2(numXPieces, numYPieces);
        }
        public Microsoft.Xna.Framework.Color getPieceColor(int x, int y)
        {
            if ((PieceColor)board[x, y] == PieceColor.WHITE)
                return Microsoft.Xna.Framework.Color.White;
            return Microsoft.Xna.Framework.Color.Black;
        }
        public bool isGameOver(int color = 0)
        {
            int first = board[0,0];
            if (color == 1 || color == -1)
                first = color;
            for (int i = 0; i < numXPieces; i++)
            {
                for (int j = 0; j < numYPieces; j++)
                {
                    if (board[i, j] != first)
                        return false;
                }
            }
            return true;
        }
        public void flipPiece(int x, int y)
        {
            board[x, y] *= -1;
            if(x > 0 )
            {
                board[x - 1, y] *= -1;
            }
            if(x < numXPieces-1)
            {
                board[x + 1, y] *= -1;
            }
            if (y > 0 )
            {
                board[x, y - 1] *= -1;
            }
            if (y < numYPieces-1)
            {
                board[x, y + 1] *= -1;
            }
        }
        public bool isEqual(FlipBoard fb)
        {
            if(numXPieces!=fb.numXPieces || numYPieces!=fb.numYPieces)
                return false;
            for (int i = 0; i < numXPieces; i++)
            {
                for (int j = 0; j < numYPieces; j++)
                {
                    if (board[i, j] != fb.board[i,j])
                        return false;
                }
            }
            return true;
        }
        public List<FlipBoard> generateChildren()
        {
            List<FlipBoard> children = new List<FlipBoard>();
            for (int i = 0; i < numXPieces; i++)
            {
                for (int j = 0; j < numYPieces; j++)
                {
                    FlipBoard temp = new FlipBoard(numXPieces, numYPieces);
                    //temp.parentFlipBoard = (FlipBoard)this.MemberwiseClone();
                    //temp.board = (int[,])this.board.Clone();
                    temp.parentFlipBoard = this;
                    temp.board = (int[,])this.board.Clone();
                    temp.flipPiece(i, j);
                    children.Add(temp);
                }
            }
            return children;
        }
        public string toString()
        {
            string s = "";

            for (int i = 0; i < numXPieces; i++)
            {
                for (int j = 0; j < numYPieces; j++)
                {
                    if (board[i, j] > 0)
                        s += "w" + " ";
                    else
                        s += "b" + " ";
                }
                s += "\n";
            }
            return s;
        }
    }
}
