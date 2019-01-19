using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace p
{
    class napravlenie : alg
    {
        int n, m;
        int[] t;

        public void set(int n1, int m1)
        {
            n = n1;
            m = m1;
        }

        public void func(int[] t1)
        {
            t = new int[m];
            t1.CopyTo(t, 0);
            Array.Sort(t);
            Array.Reverse(t);

            LinkedList<int>[] mas = new LinkedList<int>[n];

            for (int i = 0; i < n; i++)
            {
                mas[i] = new LinkedList<int>();
            }

            int sum1, sum2, max = 0;

            int j = -1;
            int direction = 1;

            for (int i = 0; i < m; i++)
            {
                if (j == n - 1)
                {
                    sum1 = 0; sum2 = 0;
                    for (LinkedListNode<int> node = mas[0].First; node != null; node = node.Next)
                        sum1 += node.Value;
                    for (LinkedListNode<int> node = mas[n - 1].First; node != null; node = node.Next)
                        sum2 += node.Value;
                    if (sum2 > sum1)
                        direction = -1;
                    else
                        direction = 0;
                }

                if (j == 0)
                {
                    sum1 = 0; sum2 = 0;
                    for (LinkedListNode<int> node = mas[0].First; node != null; node = node.Next)
                        sum1 += node.Value;
                    for (LinkedListNode<int> node = mas[n - 1].First; node != null; node = node.Next)
                        sum2 += node.Value;
                    if (sum2 < sum1)
                        direction = 1;
                    else
                        direction = 0;
                }
                j += direction;
                mas[j].AddLast(t[i]);
            }

            int summ = 0;
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
    }
}
