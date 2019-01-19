using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace p
{
    public struct Chromosome
    {
	    public int task;	//задание
	    public int number; //число
	    public int device;	//номер процессора, на который попадает задание с данной хромосомы
    }
    class Individual
    {
        int n;
        int m;
        int[,] tasks;
        Chromosome[] chr;
        int Tmax;
        //ofstream out;

        public  Individual(int N, int M, int[,] Tasks)
        {
            n = N;
            m = M;
            tasks = Tasks;
	        chr = new Chromosome [m];
	        for (int i = 0; i < m; i++)
	        {
		        chr[i].task = 0;
		        chr[i].number = 0;
		        chr[i].device = 0;
	        }

	        Tmax = 0;
        }

		public Individual prisvoenie(Individual ob1, Individual ob2)
		{
			chr = new Chromosome [m];
	        for (int i = 0; i < m; i++)
	        {
		        ob1.chr[i].task = ob2.chr[i].task;
		        ob1.chr[i].number = ob2.chr[i].number;
		        ob1.chr[i].device = ob2.chr[i].device;
	        }
	        ob1.Tmax = ob2.Tmax;
            return ob1;
		}
		
        public Individual(Individual ob)
        {
	         chr = new Chromosome [m];
	        for (int i = 0; i < m; i++)
	        {
		        chr[i].task = ob.chr[i].task;
		        chr[i].number = ob.chr[i].number;
		        chr[i].device = ob.chr[i].device;
	        }
	        Tmax = ob.Tmax;
        }

        public  Chromosome[] getChr ()
        {
	        return chr;
        }

        public  int getTmax ()
        {
	        return Tmax;
        }

        public  int tmax ()
        {
	        int[] t = new int [n];
	        Tmax = 0;
	        for (int i = 0; i < n; i++) //для каждого процессора
	        {
		        t[i] = 0;
		        for (int j = 0; j < m; j++) // для каждой хромосомы
		        {
			        if (chr[j].device == i + 1)
			        {
				        t[i] += chr[j].task;
			        }
		        }
		
		        if (Tmax == 0)
		        {
			        Tmax = t[i];
		        }

		        if(t[i] > Tmax)
		        {
			        Tmax = t[i];
		        }
	        }
	        return Tmax;
        }

        public  int getDeviceNumber(Chromosome chr)
        {
	        for (int i = 0; i < n; i++) //для каждого процессора
	        {
		        if( (chr.number > ((255/n)*i)) && (chr.number <= ((255/n)*(i+1)))) //если число в данном промежутке 
		        {
			        chr.device = i + 1;  //записываем номер процессора
			        return chr.device;
		        }
                else{ if (chr.number >= ( 255-255/n)) { chr.device = n; return chr.device; }}
	        }
	        return 0;
        }

        public  int getTask(Chromosome chr, int j)
        {
	        //return chr.task = tasks[j][chr.device - 1];
            return chr.task = tasks[j,(chr.device-1 )];
        }

        public  void print()
        {
            
            using (StreamWriter writer = File.AppendText("Zurnal.txt"))
            {
               // writer.WriteLine();
                writer.Write("Task:      ");
                for (int i = 0; i < m; i++)
                {
                  // writer.Write( "{0} - {1}; ", chr[i].task,chr[i].number) ;
                    writer.Write("{0,5}", chr[i].task);
                }
				writer.WriteLine();
                writer.Write( "Number:    ");
                for (int i = 0; i < m; i++)
                {
                    writer.Write("{0,5}", chr[i].number);
                }
				writer.WriteLine();
                writer.Write( "Device:    ");
                for (int i = 0; i < m; i++)
                {
                     writer.Write("{0,5}", chr[i].device);
                }
				writer.WriteLine();
                writer.WriteLine("Tmax = {0}   ", Tmax);
                writer.Write("...............................");
                writer.Close();
            }
        }
		
		public void  elite(int Tuda)
		{
               
			int[] p = new int [n]; //загруженности процессоров

			for (int i = 0; i < n; i++)
			{
				p[i] = 0;
			}
			
			int [] t = new int [m]; //отсортированные номера
			t = sort(Tuda);

			for (int i = 0; i < m; i++) //для каждой хромосомы
			{
				int min = 0;          //номер процессора с минимальной загруженностью

				for (int j = 0; j < n; j++) //ищем процессор с минимальной загруженностью
				{
					if (p[j] < p[min])  
					{
						min = j;			//записываем его номер в min
					}
				}

				chr[t[i]].task = tasks[t[i],0];
				p[min] += chr[t[i]].task;
				chr[t[i]].device = min + 1;
				chr[t[i]].number = (min * (256 / n)) + (256 / (2 * n));
			}
			
			tmax();
		}
 
		public int[]  sort (int Tuda)
		{
			int[] t = new int [m];        //создаём массив с номерами заданий
			for (int i = 0; i < m; i++)
			{
				t[i] = i;				//заполняем номерами от 0 до m
			}

			if (Tuda==2)
			{
				return t;
			}
			int [] tempTask = new int [m];
			for (int i = 0; i < m; i++)
			{
				tempTask[i] = tasks[i,0];
			}

			int tmp;
			
			if (Tuda==0)
			{
				for (int i = m - 1; i >= 1; i--)
				{
					for (int j = 0; j < i; j++)
					{
						if ( tempTask[j] < tempTask[j+1] )
						{
							tmp = tempTask[j];
							tempTask[j] = tempTask[j+1];
							tempTask[j+1] = tmp;

							tmp = t[j];
							t[j] = t[j+1];
							t[j+1] = tmp;
						}
					}
				}
			}
			else
			{
				for (int i = m - 1; i >= 1; i--)
				{
					for (int j = 0; j < i; j++)
					{
						if ( tempTask[j] > tempTask[j+1] )
						{
							tmp = tempTask[j];
							tempTask[j] = tempTask[j+1];
							tempTask[j+1] = tmp;

							tmp = t[j];
							t[j] = t[j+1];
							t[j+1] = tmp;
						}
					}
				}
			}
			return t;
		}
		
    }
}
