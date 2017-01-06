using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stones.Model;

namespace Stones
{
    public class MinMaxLogic : Logic
    {
        private SimpleLogic l;
        private int depth = 4;

        public MinMaxLogic(Board b, int gameSize) : base(b, gameSize)
        {
            l = new SimpleLogic(b, gameSize);
        }

        private double Eval(CellState player, Board board, double @base = 10, double a = 1, double b = 1)
        {
            var startsWith0 = false;
            var currentLineLen = 0;
            var currentPlayer = CellState.None;
            var e = 0.0;


            // Подсчёт линий по ширине
            for (var i = 0; i < board.Size; i++)
            {
                startsWith0 = false;
                currentLineLen = 0;

                for (var j = 0; j < board.Size; j++)
                {
                    var c = board[i, j];

                    if (c != 0)
                    {
                        if ((startsWith0 || c == 0) && currentPlayer != CellState.None && c != currentPlayer)
                        {
                            e += (startsWith0 ? 2 : 1) *
                            Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);

                            startsWith0 = false;
                            currentLineLen = 0;
                        }
                        if (currentPlayer == 0)
                        {
                            startsWith0 = true;
                            currentLineLen = 0;
                        }


                        currentPlayer = c;
                        currentLineLen++;

                        //

                        if (currentLineLen == 4)
                            e *= 1000;
                    }
                    else if (currentPlayer != 0)
                    {
                        if (currentLineLen > 0)
                            e += (startsWith0 || c == 0 ? 2 : 1) *
                                Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);

                        startsWith0 = true;
                        currentLineLen = 0;
                        currentPlayer = 0;
                    }
                }

                if (currentLineLen == 5)
                    e += ((currentPlayer == player) ? a : -b) * 10000000000000;
                if (currentPlayer != 0 && startsWith0)
                    if (currentLineLen > 0)
                        e += Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);
            }

            // Подсчёт линий по высоте
            for (var i = 0; i < board.Size; i++)
            {
                startsWith0 = false;
                currentLineLen = 0;

                for (var j = 0; j < board.Size; j++)
                {
                    var c = board[j, i];

                    if (c != 0)
                    {
                        if ((startsWith0 || c == 0) && currentPlayer != 0 && c != currentPlayer)
                        {
                            e += (startsWith0 || c == 0 ? 2 : 1) *
                            Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);

                            startsWith0 = false;
                            currentLineLen = 0;
                        }
                        if (currentPlayer == 0)
                        {
                            startsWith0 = true;
                            currentLineLen = 0;
                        }

                        currentPlayer = c;
                        currentLineLen++;

                        //
                        if (currentLineLen == 4)
                            e *= 1000;
                    }
                    else if (currentPlayer != 0)
                    {
                        if (currentLineLen > 0)
                            e += (startsWith0 || c == 0 ? 2 : 1) *
                                Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);

                        startsWith0 = true;
                        currentLineLen = 0;
                        currentPlayer = 0;
                    }
                }

                if (currentLineLen == 5)
                    e += ((currentPlayer == player) ? a : -b) * 100000000000;
                if (currentPlayer != 0 && startsWith0)
                    e += Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);
            }

            // Подсчёт линий вдоль главной диагонали
            for (var i = 0; i < board.Size * 2; i++)
            {
                startsWith0 = false;
                currentLineLen = 0;

                if (i == board.Size) i++;

                for (var j = 0; (i % board.Size) + j < board.Size; j++)
                {
                    var c = i > board.Size ? board[j, i % board.Size + j] : board[j + i % board.Size, j];

                    if (c != 0)
                    {
                        if ((startsWith0 || c == 0) && currentPlayer != 0 && c != currentPlayer)
                        {
                            e += (startsWith0 || c == 0 ? 2 : 1) *
                            Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);

                            startsWith0 = false;
                            currentLineLen = 0;
                        }
                        if (currentPlayer == 0)
                        {
                            startsWith0 = true;
                            currentLineLen = 0;
                        }


                        currentPlayer = c;
                        currentLineLen++;

                        //

                        if (currentLineLen == 4)
                            e *= 1000;

                    }
                    else if (currentPlayer != 0)
                    {
                        if (currentLineLen > 0)
                            e += (startsWith0 || c == 0 ? 2 : 1) *
                                Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);

                        startsWith0 = true;
                        currentLineLen = 0;
                        currentPlayer = 0;
                    }
                }


                if (currentLineLen == 5)
                    e += ((currentPlayer == player) ? a : -b) * 1000000000000;
                if (currentPlayer != 0 && startsWith0)
                    e += Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);
            }

            // Подсчёт линий вдоль побочной диагонали
            for (var i = 0; i < board.Size * 2; i++)
            {
                startsWith0 = false;
                currentLineLen = 0;

                if (i == board.Size) i++;

                for (var j = 0; (i % board.Size) + j < board.Size; j++)
                {
                    var c = i > board.Size ?
                        board[board.Size - 1 - j, i % board.Size + j]
                        : board[board.Size - 1 - j - i % board.Size, j];

                    if (c != 0)
                    {
                        if ((startsWith0 || c == 0) && currentPlayer != 0 && c != currentPlayer)
                        {
                            e += (startsWith0 && c == 0 ? 2 : 1)
                            * Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);

                            startsWith0 = false;
                            currentLineLen = 0;
                        }
                        if (currentPlayer == 0)
                        {
                            startsWith0 = true;
                            currentLineLen = 0;
                        }


                        currentPlayer = c;
                        currentLineLen++;

                        //

                        if (currentLineLen == 4)
                            e *= 1000;
                    }
                    else if (currentPlayer != 0)
                    {
                        if (currentLineLen > 0)
                            e += (startsWith0 && c == 0 ? 2 : 1) *
                                Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);

                        startsWith0 = true;
                        currentLineLen = 0;
                        currentPlayer = 0;
                    }
                }


                if (currentLineLen == 5)
                    e += ((currentPlayer == player) ? a : -b) * 10000000000000;
                if (currentPlayer != 0 && startsWith0)
                    e += Math.Pow(@base, currentLineLen - 1) * ((player == currentPlayer) ? a : -b);
            }

            return e;
        }
 
        private CellState Swap(CellState s)
        {
            if (s == CellState.Black) return CellState.White;
            return CellState.Black;
        }

        private double Minimax(CellState color, Board b, Point p, int depth)
        {
            if (depth == 0)
                return Eval(color, b);

            var nb = b.Copy();
            nb.PutStone(color, p);

            var candidates = l.FilterCellsStageThree(Swap(color));
            if (candidates.Count == 0)
                return 0.0;
            var c = candidates
                .Select(x => new { Value = x, Ev = Minimax(Swap(color), l.Board, x, depth - 1) });
            return c.Max(x => x.Ev);
        }

        public override Point SelectBestCell(CellState color)
        {
            var candidates = l.FilterCellsStageThree(color);

            if (candidates.Count == 0)
                return Draw;

            if (candidates.Count == 1)
                return candidates[0];

            var c = candidates
                .Select(x => new { Value = x, Ev = Minimax(color, l.Board, x, depth) })
                .OrderBy(x => x.Ev)
                .Select(x => x.Value).First();
            return c;
        }
    }
}
