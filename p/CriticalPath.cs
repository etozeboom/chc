using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace p
{
    class CriticalPath
    {
        Task T;
        int N, M;
        int[,] R;
        int[] SumWeight;

        public void Set(int n, int m, int a, int b,int[,] H)
        {
            N = n;
            M = m;
			R = new int[N, M];
            T = new Task(M, N, a, b,H);
            SumWeight = new int[N];
        }

        void Add(int index, int value)
        {
            for (int j = 0; j < M; j++)
                if (R[index, j] == 0)
                {
                    R[index, j] = value;
                    return;
                }
        }

        int MinSumWeight()
        {
            int min = Int32.MaxValue;
            int index = 0;
            for (int i = 0; i < N; i++)
            {
                if (SumWeight[i] < min)
                {
                    min = SumWeight[i];
                    index = i;
                }
            }
            return index;
        }

        void RecomputeSumWeight()
        {
            for (int i = 0; i < N; i++)
            {
                SumWeight[i] = 0;
                for (int j = 0; j < M; j++)
                    SumWeight[i] += R[i, j];
            }
        }

        void BuildSchedule()
        {
            const int LOCK = Int32.MaxValue;
            for (int i = 0; i < M; i++)
            {
                do
                {
                    int k = MinSumWeight();
                    if (T.Get(i, k) == -1)
                    {
                        SumWeight[k] = LOCK;
                    }
                    else
                    {
                        Add(k, T.Get(i, k));
                        RecomputeSumWeight();
                        break;
                    }
                } while (true);
            }
        }

        public void CP_mode1()
        {
            T.SimpleSort();
            BuildSchedule();
            PrintR();
        }

        public void CP_mode2()
        {
            T.SortWithRegardToInfinity();
            BuildSchedule();
            PrintR();
        }

        public void CP_mode3()
        {
            T.SortByWeightAndInfinities();
            BuildSchedule();
            PrintR();
        }

        public void PrintR()
        {
            using (StreamWriter writer = File.CreateText("newfile.txt"))
            {
                writer.WriteLine();
                for (int i = 0; i < N; i++)
                {
                    writer.Write("P{0}: ", i + 1);
                    for (int j = 0; j < M; j++)
                        if (R[i, j] != 0) writer.Write("{0} ", R[i, j]);
                    writer.WriteLine("");
                }
                writer.Write("max(");
                for (int i = 0; i < N; i++)
                    writer.Write("{0},", SumWeight[i]);
                writer.WriteLine(") = {0}", SumWeight.Max());
            }
        }
    }
}
