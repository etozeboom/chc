using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace p
{
    class Task
    {
        int row, col;
        int[,] T, backup;

        public Task(int ROW, int COL, int a, int b, int[,]R)
        {
            row = ROW;
            col = COL;
            T = (int[,])R.Clone();
            backup = (int[,])T.Clone();
        }

        public int Get(int numTask, int processor)
        {
            return T[numTask, processor];
        }

        int weight(int index)
        {
            for (int j = 0; j < col; j++)
                if (T[index, j] != -1) return T[index, j];
            return 0;
        }

        int infinities(int index)
        {
            int sum = 0;
            for (int j = 0; j < col; j++)
                if (T[index, j] == -1) sum++;
            return sum;
        }

        void SwapRows(int rowA, int rowB)
        {
            for (int j = 0; j < col; j++)
            {
                int buf = T[rowA, j];
                T[rowA, j] = T[rowB, j];
                T[rowB, j] = buf;
            }
        }

        void WeightSort(int leftRange, int rightRange)
        {
            //В диапазоне есть только одно число
            if (leftRange == rightRange)
                return;
            //В диапазоне есть два числа
            if (rightRange - leftRange == 1)
            {
                if (weight(rightRange) > weight(leftRange))
                    SwapRows(leftRange, rightRange);
                return;
            }
            //В диапазоне более двух чисел
            int l = leftRange;
            int r = rightRange;
            int middle = weight((l + r) / 2);

            do
            {
                while (weight(l) > middle) l++;
                while (weight(r) < middle) r--;

                if (l <= r)
                {
                    SwapRows(l, r);
                    l++;
                    r--;
                }
            } while (l < r);

            if (leftRange < r) WeightSort(leftRange, r);
            if (l < rightRange) WeightSort(l, rightRange);
        }

        void InfinitySort(int leftRange, int rightRange)
        {
            //В диапазоне есть только одно число
            if (leftRange == rightRange)
                return;
            //В диапазоне есть два числа
            if (rightRange - leftRange == 1)
            {
                if (infinities(rightRange) > infinities(leftRange))
                    SwapRows(leftRange, rightRange);
                return;
            }
            //В диапазоне более двух чисел
            int l = leftRange;
            int r = rightRange;
            int middle = infinities((l + r) / 2);

            do
            {
                while (infinities(l) > middle) l++;
                while (infinities(r) < middle) r--;

                if (l <= r)
                {
                    SwapRows(l, r);
                    l++;
                    r--;
                }
            } while (l < r);

            if (leftRange < r) InfinitySort(leftRange, r);
            if (l < rightRange) InfinitySort(l, rightRange);
        }
        // Простая сортировка без учета бесконечности
        public void SimpleSort()
        {
            WeightSort(0, row - 1);
        }
        // Сортировка с учетом бесконечности
        public void SortWithRegardToInfinity()
        {
            //Сортируем по количеству бесконечностей
            InfinitySort(0, row - 1);

            //Определим сколько строк имеют хотя бы одну бесконечность
            int range = row - 1;
            for (int i = 0; i < row; i++)
                if (infinities(i) == 0)
                {
                    range = i - 1;

                    break;
                }
            //Сортируем строки с бескончностями
            WeightSort(0, range);
            //Сортируем строки без бесконечностей
            if (row - range - 1 >= 2)
                WeightSort(range + 1, row - 1);
        }
        // Сортировка с учетом количества бесконечностей
        public void SortByWeightAndInfinities()
        {
            //Сортируем по количеству бесконечностей
            InfinitySort(0, row - 1);
            int range = 0;
            int infinitiesNum = infinities(0);
            //Группируем по количеству бесконечностей в строке 
            for (int i = 0; i < row; i++)
            {
                int K = infinities(i);
                //Достигнута следующая группа строк
                if (infinitiesNum != K)
                {
                    //Сортируем
                    //Console.WriteLine("[{0}, {1}]", 0, range);
                    WeightSort(range, i - 1);
                    range = i;
                    infinitiesNum = K;
                }
                //Это последняя итерация
                if (i == row - 1)
                {
                    WeightSort(range, row - 1);
                }
            }
        }
    }
}
