using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace p
{
    class kt:alg
    {
        int n, m;
        int[] t;

        public void set(int n1, int m1)
        {
            n = n1;
            m = m1;
        }
        public void method()
        {
            LinkedList<int>[] mas = new LinkedList<int>[n];
            for (int i = 0; i < n; i++)
            {
                mas[i] = new LinkedList<int>();
            }
            int[] sum = new int[n];
            int min = 0, index = 0;

            int summ = 0;
            int max = 0;

            mas[0].AddLast(t[0]);

            for (int i = 1; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                    for (LinkedListNode<int> node = mas[j].First; node != null; node = node.Next)
                        sum[j] += node.Value;

                min = sum[0];
                index = 0;
                for (int j = 1; j < n; j++)
                    if (sum[j] < min)
                    {
                        min = sum[j];
                        index = j;
                    }
                mas[index].AddLast(t[i]);

                for (int j = 0; j < n; j++)
                    sum[j] = 0;
            }

             using (StreamWriter writer = File.CreateText("newfile.txt"))
            {
                for (int i = 0; i < n; i++)
                {
                    writer.WriteLine();
                    writer.Write("p{0} ", i + 1);
                    for (LinkedListNode<int> node = mas[i].First; node != null; node = node.Next)
                    {
                        writer.Write("{0} ", node.Value);
                        summ += node.Value;
                    }
                    writer.Write(" сумма f(p{0})= {1}", i + 1, summ);
                    if (summ > max) max = summ;
                    summ = 0;
                }
                writer.WriteLine();
                writer.WriteLine("максимум = {0}", max);

                writer.WriteLine();
                writer.WriteLine();
            }

        }
        public void func(int[] t1)
        {
            t = new int[m];
            t1.CopyTo(t, 0);
            Array.Sort(t);
            Array.Reverse(t);
            method();
        }
    }
}
