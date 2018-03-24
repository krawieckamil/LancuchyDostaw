using System;
using System.Collections.Generic;
using System.Drawing;

namespace LancuchyDostaw
{
    class Utils
    {
        public static void resetAmounts(TransportConnection[,] matrix)
        {
            resetAmounts(matrix, 0.0);
        }
        public static void resetAmounts(TransportConnection[,] matrix, double val)
        {
            int N = matrix.Length;
            int M = matrix.Length;
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                    matrix[i, j].Amount = val;
        }

        public static List<TransportConnection> getDescendingOrderByProfit(TransportConnection[,] matrix)
        {
            List<TransportConnection> list = new List<TransportConnection>();
            int N = matrix.Length;
            int M = matrix.Rank;

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (matrix[i, j].Blocked) continue;
                    int k = 0;
                    while (k < list.Count && matrix[list[k].X, list[k].Y].UnitProfit() <= matrix[i, j].UnitProfit())
                        k++;
                    list.Insert(k, matrix[i, j]);
                }
            }

            return list;
        }

        public static double getAmountOfRow(TransportConnection[,] matrix, int row)
        {
            int N = matrix.GetLength(1);
            double sum = 0;
            for (int i = 0; i < N; i++)
            {
                sum += matrix[row,i].Amount;
            }
            return sum;
        }

        public static double getAmountOfColumn(TransportConnection[,] matrix, int col)
        {
            int N = matrix.Length;
            double sum = 0;
            for (int i = 0; i < N; i++)
            {
                sum += matrix[i,col].Amount;
            }
            return sum;
        }

        public static void drawAmountMatrix(TransportConnection[,] matrix)
        {
            int N = matrix.Length;
            int M = matrix.GetLength(1);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (j != 0) Console.Write(" ");
                    Console.Write(matrix[i, j].Amount);
                }
                Console.WriteLine();
            }
        }

        public static void drawCostsMatrix(TransportConnection[,] matrix)
        {
            int N = matrix.Length;
            int M = matrix.GetLength(1);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (j != 0) Console.Write(" ");
                    Console.Write(matrix[i, j].TransportCost);
                }
                Console.WriteLine();
            }
        }

        public static void drawUnitProfitsMatrix(TransportConnection[,] matrix)
        {
            int N = matrix.Length;
            int M = matrix.Rank;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (j != 0) Console.WriteLine(" ");
                    Console.Write(matrix[i, j].UnitProfit());
                }
                Console.WriteLine();
            }
        }

        public static void drawMatrix(double[,] matrix)
        {
            int N = matrix.Length;
            int M = matrix.Rank;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (j != 0) Console.Write(" ");
                    Console.Write(matrix[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static double countTotalProfit(TransportConnection[,] matrix)
        {
            int N = matrix.Length;
            int M = matrix.Rank;
            double sum = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    sum += matrix[i, j].TotalProfit();
                }
            }
            return sum;
        }

        public static double countTotalCosts(TransportConnection[,] matrix)
        {
            int N = matrix.Length;
            int M = matrix.Rank;
            double sum = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    sum += matrix[i, j].TransportCost * matrix[i, j].Amount;
                }
            }
            return sum;
        }
        /*
        public static Point getMaximumFromMatrix(double[,] matrix)
        {
            int N = matrix.Length;
            int M = matrix[0].Length;
            Point p = new Point(0, 0);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                    if (matrix[i,j] > matrix[p.X,p.Y])
                        p = new Point(i, j);
            return p;
        }
        */
        public static double getMinimumFromVector(double[] vector)
        {
            double min = vector[0];
            foreach (double d in vector)
                if (d < min)
                    min = d;
            return min;
        }

        public static List<TransportConnection> getColumnAsList(int n, TransportConnection[,] connections)
        {
            List<TransportConnection> list = new List<TransportConnection>();
            int N = connections.Rank;
            for (int i = 0; i < N; i++)
                list.Add(connections[n, i]);
            return list;
        }

        public static List<TransportConnection> getRowAsList(int n, TransportConnection[,] connections)
        {
            List<TransportConnection> list = new List<TransportConnection>();
            int N = connections.Length;
            for (int i = 0; i < N; i++)
                list.Add(connections[i, n]);
            return list;
        }

        public static Point getMaximumFromMatrix(double[,] matrix)
        {
            int N = matrix.Length;
            int M = matrix.GetLength(1);
            Point p = new Point(0, 0);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                    if (matrix[i,j] > matrix[p.X,p.Y])
                        p = new Point(i, j);
            return p;
        }
    }



    public static class ArrayExtensions
    {
        public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                originalArray[i] = with;
            }
        }
    }
}