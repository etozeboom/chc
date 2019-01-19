using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace p
{
    class Generation
    {
        Individual [] individual;
        int inds;
        int n;
        int m;
		float HEMEN;
		int index=0;
        int counter = 0;
        Random myRnd;
		int vivod;
        int FlagElite;
		int CrossElite;
		
        public void set(int N, int M, int INDS,int[,] Tasks, float hem,int vib, int FElite, int FCross)
        {
			FlagElite=FElite;
			CrossElite=FCross;
			vivod=vib;
			HEMEN = hem;
            n = N;
            m = M;
            inds = INDS;
            individual = new Individual[inds*2];
            for (int i = 0; i < inds*2; i++)
            {
                individual[i] = new Individual(n, m, Tasks);
            }
            setFirstGeneration();
        }
      
        public  void setFirstGeneration()
        {
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine ( "Первое поколение: родители");
						//writer.WriteLine();
						writer.Close();
					}
			}
			int FL=0;
			
			if (FlagElite==2)
			{
				individual[0].elite(0);
				FlagElite=1;
				FL=1;
			}
			if (FlagElite==3)
			{
				individual[0].elite(1);
				FlagElite=1;
				FL=1;
			}
			if (FlagElite==4)
			{
				individual[0].elite(2);
				FlagElite=1;
				FL=1;
			} 
            myRnd = new Random();
	        for (int i = FL; i < inds; i++) //для каждой особи
	        {
		        for (int j = 0; j < m; j++) //для каждого задания
		        {
			        while (true)
			        {
                        individual[i].getChr()[j].number = myRnd.Next(255)+1;
                        individual[i].getChr()[j].device = individual[i].getDeviceNumber(individual[i].getChr()[j]);
                        individual[i].getChr()[j].task = individual[i].getTask(individual[i].getChr()[j], j);
				        if (individual[i].getTask(individual[i].getChr()[j], j) > 0)
				        {
					        break;
				        }
			        }
		        }
		        individual[i].tmax();
		
	        }
	        counter++;  //счётчик поколений
			
			if (vivod==1)
			{
                this.print2();
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.WriteLine( ".............................................................." );
					writer.Close();
				}
			}
			
			if (FlagElite==1)
			{
				this.BubbleSort();
				this.peremesh();
				if (vivod==1)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine();
						writer.WriteLine( "после рандом");
						writer.Close();
					}
					this.print2();
				}
			}
			this.selectionCrossover();
			
			
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.WriteLine();
					writer.WriteLine( "Потомки:" );
					writer.Close();
				}
                this.print();
			}
        }

			public void selectionCrossover()
		{
			myRnd = new Random();
			index=0;
			int tochka=m/2;
			int FlagNoChet=0;
            if  (inds % 2 == 0){ FlagNoChet = 0; } else { FlagNoChet = 1; }
            if ((inds % 2 == 0) & FlagElite == 1) { FlagNoChet = 2; } 
           // if ((inds % 2 == 0) & FlagElite == 1) { FlagNoChet = 1; }

			for (int i = FlagElite; i < inds-FlagNoChet; i=i+2)
				{
					if (hemeng(individual[i],individual[i+1])>HEMEN)
					{
						if (vivod==1)
						{
							using (StreamWriter writer = File.AppendText("Zurnal.txt"))
							{
								writer.WriteLine();
                                writer.WriteLine("селектируют {0} и {1}", (i + 1 ), (i + 2 ));
								writer.Close();
							}
						}
						if (CrossElite==1)
						{
							tochka=myRnd.Next(2,m)-1; 
						}
						individual[index + inds].prisvoenie(individual[index + inds], individual[i]);
						individual[inds + index + 1].prisvoenie(individual[inds + index + 1], individual[i + 1]);
						individual[index + inds]    =crossover(individual[index + inds], individual[i + 1], tochka);
						
						if ((CrossElite==1) &&(vivod==1))
						{
							using (StreamWriter writer = File.AppendText("Zurnal.txt"))
								{
									writer.WriteLine();
									writer.WriteLine("кроссовер в точке {0} ", tochka);
									writer.Close();
								}
						}
						
						individual[inds + index + 1]=crossover(individual[inds+index+1], individual[i], tochka);
						index+=2;
					}
				}
          
		  
			for (int i = 0; i < inds+index; i++) //для каждой особи
                {
                    for (int j = 0; j < m; j++) //для каждого задания
                    {
                        while (true)
                        {
                            //individual[i].getChr()[j].number = myRnd.Next(255) + 1;
                            individual[i].getChr()[j].device = individual[i].getDeviceNumber(individual[i].getChr()[j]);
                            individual[i].getChr()[j].task = individual[i].getTask(individual[i].getChr()[j], j);
                            if (individual[i].getTask(individual[i].getChr()[j], j) > 0)
                            {
                                break;
                            }
                        }
                    }
                    individual[i].tmax();

                }
		}
		
		public void BubbleSort()
		{
			for (int i = 0; i < inds; i++)
			{
				for (int j = i+1; j < inds; j++)
				{
					if (individual[j].getTmax() <individual[i].getTmax())
					{
						var temp = individual[i];
						individual[i] = individual[j];
						individual[j] = temp;
					}
				}
			}
		}
		
		public void peremesh()
		{
			myRnd = new Random();
           // for (int i = 0; i < inds; i++)
           // int i = FlagElite;
            for (int i = FlagElite; i < inds; i++)
			{
				int r = myRnd.Next(i, inds);
				var temp = individual[r];
				individual[r] = individual[i];
				individual[i] = temp;
			}
		}
		
		public void BubbleSort2()
		{
			for (int i = 0; i < inds+index; i++)
			{
				for (int j = i+1; j < inds+index; j++)
				{
					if (individual[j].getTmax() <individual[i].getTmax())
					{
						var temp = individual[i];
						individual[i] = individual[j];
						individual[j] = temp;
					}
				}
			}	
		}
		
        public  void newGeneration (int sbros)
        {
			
            if (sbros == 1) { counter = 0; };
			counter++;
			this.BubbleSort2();
	       // int j = 0;
           // myRnd = new Random();
		    if (vivod==1 & sbros==0)
			{
				this.BubbleSort2();
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.WriteLine("..............................................................");
					writer.WriteLine( "Следующее поколение № {0}: Родители",  counter+1 );
					writer.Close();
				}
			   // this.BubbleSort2();
				this.print2();  
			}
			this.peremesh();
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.WriteLine();
					writer.WriteLine( "после рандом");
					writer.Close();
				}
				this.print2();
			}
			this.selectionCrossover();	
				
	        if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.WriteLine();
					writer.WriteLine( "Поколение №{0} : Потомки",  counter+1 );
					writer.Close();
				}
				this.print();
			}
	       // this = newGeneration;
        }
 
        public void print ()
        {
           
                for (int i = inds; i < inds+index; i++)
                {	
					
                    using (StreamWriter writer = File.AppendText("Zurnal.txt"))
                    {
						writer.WriteLine();
						writer.WriteLine( "Особь №{0}", i + 1);
						writer.Close();
                    }
                     individual[i].print();
                }
            using (StreamWriter writer = File.AppendText("Zurnal.txt"))
                    {
						writer.WriteLine();
						writer.WriteLine("Лучший tmax: {0}",bestTmax());
						writer.Close();
                    }
        }

		public void print2 ()
        {
            
                for (int i = 0; i < inds; i++)
                {
                    using (StreamWriter writer = File.AppendText("Zurnal.txt"))
                    {
						writer.WriteLine();
						writer.WriteLine( "Особь №{0}", i + 1);
						writer.Close();
                    }
                     individual[i].print();
                }
            using (StreamWriter writer = File.AppendText("Zurnal.txt"))
                    {
						writer.WriteLine();
						writer.WriteLine("Лучший tmax: {0}",bestTmax());
						writer.Close();
                    }
        }
		
        public Individual crossover(Individual ind1, Individual ind2, int tohka)
        {
			int k = tohka ;
			
		
			for (int i = k; i < m; i++)    //от черты до конца заданий
			{
				ind1.getChr()[i].number = ind2.getChr()[i].number;
			}	
			
	        return ind1;
        }
		
		
		
		public int hemeng(Individual ind1, Individual ind2)
        {
			int h=0;
		        for (int i = 0; i < m; i++)    
		        {
					if (ind1.getChr()[i].device != ind2.getChr()[i].device){h++;}
		        }	
	        return h;
        }
		
		 public  void mutationC1()
        {
			myRnd = new Random();
			int j = myRnd.Next(m);
			for (int i =1; i<inds; i++)
			{
				if (vivod==1)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine();
						writer.WriteLine( "Особь №{0}:  ",  i+1 );
						writer.Close();
					}
				}
				mutation1(individual[i],j);
				j = myRnd.Next(m);
			}
		}
		
		public  Individual mutation1(Individual ind,int j)
        {
			int device = ind.getChr()[j].device;
			int number = ind.getChr()[j].number;

			byte[] b = new byte[8];
			byte g=0, temp=0;
			byte f1 = 1, f2 = 5;
		
			f1 = (byte)myRnd.Next(7);
			int flag=0;
			while(flag==0)
			{
				f2 = (byte)myRnd.Next(7);
				if(((byte)(Math.Abs(f2-f1))!=1) && ((byte)(Math.Abs(f2-f1))!=0))  
				{
					flag = 1;
				}
			}
			for (byte i = 0; i < 8; i++)
			{
				b[i]=(byte)((ind.getChr()[j].number >> i) & 1);
			}
			
			temp = b[f1];
			b[f1] = b[f2];
			b[f2] = temp;
			temp = b[f1+1];
			b[f1+1] = b[f2+1];
			b[f2+1] = temp;
			
			for (byte i = 0; i < 8; i++)
			{
				g =(byte)( g | (b[i]<<i ));
			}
			ind.getChr()[j].number=g;
			
			ind.getDeviceNumber(ind.getChr()[j]);
			 if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.Write("биты {0},{1} и {2},{3} меняются местами в {4}-ой хромосоме" , f1 + 1, f1+2,f2+1,f2+2, j + 1);
					writer.Close();
				}
			}
			ind.getChr()[j].device = ind.getDeviceNumber(ind.getChr()[j]);
			ind.getChr()[j].task = ind.getTask(ind.getChr()[j], j);
			if (vivod==1)
			{
				if (ind.getChr()[j].device != device)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.Write( " --------процессор изменился с {0} на {1} ", device ,ind.getChr()[j].device);
						writer.Close();
					}
				}
			}
			ind.tmax();
			
	        return ind;
        }
		
		 public  void mutationC2()
        {
			myRnd = new Random();
			int j = myRnd.Next(m);
			for (int i =1; i<inds; i++)
			{
				if (vivod==1)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine();
						writer.WriteLine( "Особь №{0}:  ",  i+1 );
						writer.Close();
					}
				}
				mutation2(individual[i],j);
				j = myRnd.Next(m);
			}
		}
		
		
		public  Individual mutation2(Individual ind, int j)
        {
			int device = ind.getChr()[j].device;
			int number = ind.getChr()[j].number;

			byte[] b = new byte[8];
			byte g=0, temp=0;
			byte f1 = 1, f2 = 5;
		
			f1 = (byte)myRnd.Next(6);
			int flag=0;
			while(flag==0)
			{
				f2 = (byte)myRnd.Next(6);
				if(((byte)(Math.Abs(f2-f1))!=1) && ((byte)(Math.Abs(f2-f1))!=0))  
				{
					flag = 1;
				}
			}
			for (byte i = 0; i < 8; i++)
			{
				b[i]=(byte)((ind.getChr()[j].number >> i) & 1);
			}
			
			temp = b[f1];
			b[f1] = b[f2];
			b[f2] = temp;
			temp = b[f1+1];
			b[f1+1] = b[f2+1];
			b[f2+1] = temp;
			temp = b[f1+2];
			b[f1+2] = b[f2+2];
			b[f2+2] = temp;
			
			for (byte i = 0; i < 8; i++)
			{
				g =(byte)( g | (b[i]<<i ));
			}
			ind.getChr()[j].number=g;
			
			ind.getDeviceNumber(ind.getChr()[j]);
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.Write("биты {0},{1},{2} и {3},{4},{5} меняются местами в {6}-ой хромосоме   " , f1 + 1, f1+2,f1+3,f2+1,f2+2,f2+3, j + 1);
					writer.Close();
				}
			}
			ind.getChr()[j].device = ind.getDeviceNumber(ind.getChr()[j]);
			ind.getChr()[j].task = ind.getTask(ind.getChr()[j], j);
			
			if (vivod==1)
			{
				if (ind.getChr()[j].device != device)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.Write( "--------процессор изменился с {0} на {1} ", device ,ind.getChr()[j].device);
						writer.Close();
					}
				}
			}
			
			ind.tmax();
			
	        return ind;
        }
		
		public  void mutationC3()
        {
			myRnd = new Random();
			int j = myRnd.Next(m);
			for (int i =1; i<inds; i++)
			{
				if (vivod==1)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine();
						writer.WriteLine( "Особь №{0}:  ",  i+1 );
						writer.Close();
					}
				}
				mutation3(individual[i],j);
				j = myRnd.Next(m);
			}
		}
		
		
		public  Individual mutation3(Individual ind, int j)
        {
			int device = ind.getChr()[j].device;
			int number = ind.getChr()[j].number;

			byte[] b = new byte[8];
			byte g=0, temp=0;
			byte f1 = 1, f2 = 5;
		
			f1 = (byte)myRnd.Next(7);
			int flag=0;
			while(flag==0)
			{
				f2 = (byte)myRnd.Next(7);
				if(((byte)(Math.Abs(f2-f1))!=1) && ((byte)(Math.Abs(f2-f1))!=0))  
				{
					flag = 1;
				}
			}
			for (byte i = 0; i < 8; i++)
			{
				b[i]=(byte)((ind.getChr()[j].number >> i) & 1);
			}
			
			if ((b[f1] & 1) == 0)
			{
				b[f1] |= 1;
			}
			else
			{
				b[f1] &= 0;
			}
			if ((b[f1+1] & 1) == 0)
			{
				b[f1+1] |= 1;
			}
			else
			{
				b[f1+1] &= 0;
			}
			
			temp = b[f1];
			b[f1] = b[f2];
			b[f2] = temp;
			temp = b[f1+1];
			b[f1+1] = b[f2+1];
			b[f2+1] = temp;
			
			for (byte i = 0; i < 8; i++)
			{
				g =(byte)( g | (b[i]<<i ));
			}
			ind.getChr()[j].number=g;
			
			ind.getDeviceNumber(ind.getChr()[j]);
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.Write("биты {0},{1} инвертируются и меняются местами с {2},{3} в {4}-ой хромосоме   " , f1 + 1, f1+2,f2+1,f2+2, j + 1);
					writer.Close();
				}
			}
			ind.getChr()[j].device = ind.getDeviceNumber(ind.getChr()[j]);

			if (ind.getChr()[j].device == 0)
			{
				ind.getChr()[j].device = device;
				ind.getChr()[j].number = number;
				ind.getTask(ind.getChr()[j], j);
			}
			else
			{
				ind.getChr()[j].task = ind.getTask(ind.getChr()[j], j);
			}
		   
			if (vivod==1)
			{
				if (ind.getChr()[j].device != device)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.Write( "--------процессор изменился с {0} на {1} ", device ,ind.getChr()[j].device);
						writer.Close();
					}
				}
			}
			
			ind.tmax();
		   
	        return ind;
        }
		
		
		public  void mutationC4()
        {
			myRnd = new Random();
			int j = myRnd.Next(m);
			for (int i =1; i<inds; i++)
			{
				if (vivod==1)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine();
						writer.WriteLine( "Особь №{0}:  ",  i+1 );
						writer.Close();
					}
				}
				mutation4(individual[i],j);
				j = myRnd.Next(m);
			}
		}
		
		
		public  Individual mutation4(Individual ind, int j)
		{
			int device = ind.getChr()[j].device;
			int number = ind.getChr()[j].number;

			byte[] b = new byte[8];
			byte g=0, temp=0;
			byte f1 = 1, f2 = 5;
		
			f1 = (byte)myRnd.Next(7);
			int flag=0;
			while(flag==0)
			{
				f2 = (byte)myRnd.Next(7);
				if(((byte)(Math.Abs(f2-f1))!=1) && ((byte)(Math.Abs(f2-f1))!=0))  
				{
					flag = 1;
				}
			}
			for (byte i = 0; i < 8; i++)
			{
				b[i]=(byte)((ind.getChr()[j].number >> i) & 1);
			}
			
			if ((b[f1] & 1) == 0)
			{
				b[f1] |= 1;
			}
			else
			{
				b[f1] &= 0;
			}
			if ((b[f1+1] & 1) == 0)
			{
				b[f1+1] |= 1;
			}
			else
			{
				b[f1+1] &= 0;
			}
			
			temp = b[f1];
			b[f1] = b[f2];
			b[f2] = temp;
			temp = b[f1+1];
			b[f1+1] = b[f2+1];
			b[f2+1] = temp;
			
			if ((b[f1] & 1) == 0)
			{
				b[f1] |= 1;
			}
			else
			{
				b[f1] &= 0;
			}
			if ((b[f1+1] & 1) == 0)
			{
				b[f1+1] |= 1;
			}
			else
			{
				b[f1+1] &= 0;
			}
			
			for (byte i = 0; i < 8; i++)
			{
				g =(byte)( g | (b[i]<<i ));
			}
			ind.getChr()[j].number=g;
			
			ind.getDeviceNumber(ind.getChr()[j]);
			 if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.Write("биты {0},{1} и {2},{3} меняются местами после инверсии в {4}-ой хромосоме   " , f1 + 1, f1+2,f2+1,f2+2, j + 1);
					writer.Close();
				}
			}
			ind.getChr()[j].device = ind.getDeviceNumber(ind.getChr()[j]);

			if (ind.getChr()[j].device == 0)
			{
				ind.getChr()[j].device = device;
				ind.getChr()[j].number = number;
				ind.getTask(ind.getChr()[j], j);
			}
			else
			{
				ind.getChr()[j].task = ind.getTask(ind.getChr()[j], j);
			}
			
			if (vivod==1)
			{
				if (ind.getChr()[j].device != device)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.Write( "--------процессор изменился с {0} на {1} ", device ,ind.getChr()[j].device);
						writer.Close();
					}
				}
			}
			
			ind.tmax();
		   
			
	   
	        return ind;
        }
		
		public  void mutationC5()
        {
			myRnd = new Random();
			int j = myRnd.Next(m);
			for (int i =1; i<inds; i++)
			{
				if (vivod==1)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine();
						writer.WriteLine( "Особь №{0}:  ",  i+1 );
						writer.Close();
					}
				}
				mutation5(individual[i],j);
				j = myRnd.Next(m);
			}
		}
		
		
		public  Individual mutation5(Individual ind, int j)
        {
			int device = ind.getChr()[j].device;
			int number = ind.getChr()[j].number;

			byte[] b = new byte[8];
			byte g=0, temp=0;
			byte f1 = 1, f2 = 5;
		
			f1 = (byte)myRnd.Next(6);
			int flag=0;
			while(flag==0)
			{
				f2 = (byte)myRnd.Next(6);
				if(((byte)(Math.Abs(f2-f1))!=1) && ((byte)(Math.Abs(f2-f1))!=0))  
				{
					flag = 1;
				}
			}
			for (byte i = 0; i < 8; i++)
			{
				b[i]=(byte)((ind.getChr()[j].number >> i) & 1);
			}
			
			if ((b[f1] & 1) == 0)
			{
				b[f1] |= 1;
			}
			else
			{
				b[f1] &= 0;
			}
			if ((b[f1+1] & 1) == 0)
			{
				b[f1+1] |= 1;
			}
			else
			{
				b[f1+1] &= 0;
			}
			if ((b[f1+2] & 1) == 0)
			{
				b[f1+2] |= 1;
			}
			else
			{
				b[f1+2] &= 0;
			}
			
			temp = b[f1];
			b[f1] = b[f2];
			b[f2] = temp;
			temp = b[f1+1];
			b[f1+1] = b[f2+1];
			b[f2+1] = temp;
			temp = b[f1+2];
			b[f1+2] = b[f2+2];
			b[f2+2] = temp;
			
			for (byte i = 0; i < 8; i++)
			{
				g =(byte)( g | (b[i]<<i ));
			}
			ind.getChr()[j].number=g;
			
			ind.getDeviceNumber(ind.getChr()[j]);
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.Write("биты {0},{1},{2} инвертируются и  меняются местами с {3},{4},{5} в {6}-ой хромосоме   " , f1 + 1, f1+2,f1+3,f2+1,f2+2,f2+3, j + 1);
					writer.Close();
				}
			}
			ind.getChr()[j].device = ind.getDeviceNumber(ind.getChr()[j]);

			if (ind.getChr()[j].device == 0)
			{
				ind.getChr()[j].device = device;
				ind.getChr()[j].number = number;
				ind.getTask(ind.getChr()[j], j);
			}
			else
			{
				ind.getChr()[j].task = ind.getTask(ind.getChr()[j], j);
			}
			
			if (vivod==1)
			{
				if (ind.getChr()[j].device != device)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.Write( "--------процессор изменился с {0} на {1} ", device ,ind.getChr()[j].device);
						writer.Close();
					}
				}
			}
			
			ind.tmax();
		   
	        return ind;
        }
		
		public  void mutationC6()
        {
			myRnd = new Random();
			int j = myRnd.Next(m);
			for (int i =1; i<inds; i++)
			{
				if (vivod==1)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine();
						writer.WriteLine( "Особь №{0}:  ",  i+1 );
						writer.Close();
					}
				}
				mutation6(individual[i],j);
				j = myRnd.Next(m);
			}
		}
		
		
		public  Individual mutation6(Individual ind, int j)
        {
			int device = ind.getChr()[j].device;
			int number = ind.getChr()[j].number;

			byte[] b = new byte[8];
			byte g=0, temp=0;
			byte f1 = 1, f2 = 5;
		
			f1 = (byte)myRnd.Next(6);
			int flag=0;
			while(flag==0)
			{
				f2 = (byte)myRnd.Next(6);
				if(((byte)(Math.Abs(f2-f1))!=1) && ((byte)(Math.Abs(f2-f1))!=0))  
				{
					flag = 1;
				}
			}
			for (byte i = 0; i < 8; i++)
			{
				b[i]=(byte)((ind.getChr()[j].number >> i) & 1);
			}
			
			if ((b[f1] & 1) == 0)
			{
				b[f1] |= 1;
			}
			else
			{
				b[f1] &= 0;
			}
			if ((b[f1+1] & 1) == 0)
			{
				b[f1+1] |= 1;
			}
			else
			{
				b[f1+1] &= 0;
			}
			if ((b[f1+2] & 1) == 0)
			{
				b[f1+2] |= 1;
			}
			else
			{
				b[f1+2] &= 0;
			}
			
			temp = b[f1];
			b[f1] = b[f2];
			b[f2] = temp;
			temp = b[f1+1];
			b[f1+1] = b[f2+1];
			b[f2+1] = temp;
			temp = b[f1+2];
			b[f1+2] = b[f2+2];
			b[f2+2] = temp;
			
			if ((b[f1] & 1) == 0)
			{
				b[f1] |= 1;
			}
			else
			{
				b[f1] &= 0;
			}
			if ((b[f1+1] & 1) == 0)
			{
				b[f1+1] |= 1;
			}
			else
			{
				b[f1+1] &= 0;
			}
			if ((b[f1+2] & 1) == 0)
			{
				b[f1+2] |= 1;
			}
			else
			{
				b[f1+2] &= 0;
			}
			
			for (byte i = 0; i < 8; i++)
			{
				g =(byte)( g | (b[i]<<i ));
			}
			ind.getChr()[j].number=g;
			
			ind.getDeviceNumber(ind.getChr()[j]);
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.Write("биты {0},{1},{2} и {3},{4},{5} меняются местами после инверсии в {6}-ой хромосоме   " , f1 + 1, f1+2,f1+3,f2+1,f2+2,f2+3, j + 1);
					writer.Close();
				}
			}
			ind.getChr()[j].device = ind.getDeviceNumber(ind.getChr()[j]);

			if (ind.getChr()[j].device == 0)
			{
				ind.getChr()[j].device = device;
				ind.getChr()[j].number = number;
				ind.getTask(ind.getChr()[j], j);
			}
			else
			{
				ind.getChr()[j].task = ind.getTask(ind.getChr()[j], j);
			}
			if (vivod==1)
			{
				if (ind.getChr()[j].device != device)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.Write( "--------процессор изменился с {0} на {1} ", device ,ind.getChr()[j].device);
						writer.Close();
					}
				}
			}
			
			ind.tmax();
		   
	        return ind;
        }
		
		 public  void mutationC7()
        {
			myRnd = new Random();
			int j = myRnd.Next(m);
			for (int i =1; i<inds; i++)
			{
				if (vivod==1)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine();
						writer.WriteLine( "Особь №{0}:  ",  i+1 );
						writer.Close();
					}
				}
				mutation1(individual[i],j);
				j = myRnd.Next(m);
			}
		}
		
		
		public  Individual mutation7(Individual ind,int j)
        {
			int device = ind.getChr()[j].device;
			int number = ind.getChr()[j].number;

			int f;
			int c;
			f = myRnd.Next(8);
			c = 0x01 << f;

			if ((ind.getChr()[j].number & c) == 0)
			{
				ind.getChr()[j].number |= c;
			}
			else
			{
				ind.getChr()[j].number &= ~c;
			}

			ind.getDeviceNumber(ind.getChr()[j]);
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.Write("мутирует {0}-й бит в  {1}-ой хромосоме   " , f + 1 , j + 1);
					writer.Close();
				}
			}
			ind.getChr()[j].device = ind.getDeviceNumber(ind.getChr()[j]);

			if (ind.getChr()[j].device == 0)
			{
				ind.getChr()[j].device = device;
				ind.getChr()[j].number = number;
				ind.getTask(ind.getChr()[j], j);
			}
			else
			{
				ind.getChr()[j].task = ind.getTask(ind.getChr()[j], j);
			}
			if (vivod==1)
			{
				if (ind.getChr()[j].device != device)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.Write( "--------процессор изменился с {0} на {1} ", device ,ind.getChr()[j].device);
						writer.Close();
					}
				}
			}
			
			ind.tmax();
		   
	        return ind;
        }
		
		 public  void mutationC8()
        {
			myRnd = new Random();
			int j = myRnd.Next(m);
			for (int i =1; i<inds; i++)
			{
				if (vivod==1)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine();
						writer.WriteLine( "Особь №{0}:  ",  i+1 );
						writer.Close();
					}
				}
				mutation2(individual[i],j);
				j = myRnd.Next(m);
			}
		}
		
		
		public  Individual mutation8(Individual ind, int j)
        {
			int device = ind.getChr()[j].device;
			int number = ind.getChr()[j].number;

			int f;
			int c;
			f = myRnd.Next(8);
			c = 0x01 << f;

			if ((ind.getChr()[j].number & c) == 0)
			{
				ind.getChr()[j].number |= c;
			}
			else
			{
				ind.getChr()[j].number &= ~c;
			}
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.Write("мутирует {0}-й бит в  {1}-ой хромосоме   " , f + 1 , j + 1);
					writer.Close();
				}
			}
			c = 0x01 << f+1;

			if ((ind.getChr()[j].number & c) == 0)
			{
				ind.getChr()[j].number |= c;
			}
			else
			{
				ind.getChr()[j].number &= ~c;
			}
			
			ind.getDeviceNumber(ind.getChr()[j]);
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.Write("мутирует {0}-й бит в  {1}-ой хромосоме   " , f + 2 , j + 1);
					writer.Close();
				}
			}
			ind.getChr()[j].device = ind.getDeviceNumber(ind.getChr()[j]);

			if (ind.getChr()[j].device == 0)
			{
				ind.getChr()[j].device = device;
				ind.getChr()[j].number = number;
				ind.getTask(ind.getChr()[j], j);
			}
			else
			{
				ind.getChr()[j].task = ind.getTask(ind.getChr()[j], j);
			}
			if (vivod==1)
			{
				if (ind.getChr()[j].device != device)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.Write( "--------процессор изменился с {0} на {1} ", device ,ind.getChr()[j].device);
						writer.Close();
					}
				}
			}
			
			ind.tmax();
		   
	        return ind;
        }
		
		 public  void mutationC9()
        {
			myRnd = new Random();
			int j = myRnd.Next(m);
			for (int i =1; i<inds; i++)
			{
				if (vivod==1)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine();
						writer.WriteLine( "Особь №{0}:  ",  i+1 );
						writer.Close();
					}
				}
				mutation3(individual[i],j);
				j = myRnd.Next(m);
			}
		}
		
		
		public  Individual mutation9(Individual ind, int j)
        {
			int device = ind.getChr()[j].device;
			int number = ind.getChr()[j].number;

			int f;
			int c;
			f = myRnd.Next(8);
			c = 0x01 << f;

			if ((ind.getChr()[j].number & c) == 0)
			{
				ind.getChr()[j].number |= c;
			}
			else
			{
				ind.getChr()[j].number &= ~c;
			}

			ind.getDeviceNumber(ind.getChr()[j]);
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.Write("мутирует {0}-й бит в  {1}-ой хромосоме   " , f + 1 , j + 1);
					writer.Close();
				}
			}
			f = myRnd.Next(8);
			c = 0x01 << f;

			if ((ind.getChr()[j].number & c) == 0)
			{
				ind.getChr()[j].number |= c;
			}
			else
			{
				ind.getChr()[j].number &= ~c;
			}
			
			ind.getDeviceNumber(ind.getChr()[j]);
			if (vivod==1)
			{
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.Write("мутирует {0}-й бит в  {1}-ой хромосоме   " , f + 1 , j + 1);
					writer.Close();
				}
			}
			ind.getChr()[j].device = ind.getDeviceNumber(ind.getChr()[j]);

			if (ind.getChr()[j].device == 0)
			{
				ind.getChr()[j].device = device;
				ind.getChr()[j].number = number;
				ind.getTask(ind.getChr()[j], j);
			}
			else
			{
				ind.getChr()[j].task = ind.getTask(ind.getChr()[j], j);
			}
			if (vivod==1)
			{
				if (ind.getChr()[j].device != device)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.Write( "--------процессор изменился с {0} на {1} ", device ,ind.getChr()[j].device);
						writer.Close();
					}
				}
			}
			
			ind.tmax();
		   
	        return ind;
        }
		
        public  Individual selection(Individual ind1, Individual ind2)
        {
	        if (ind1.getTmax() < ind2.getTmax())
	        {
		        return ind1;
	        }
	        else
	        {
		        return ind2;
	        }
        }

        public  int bestTmax ()
        {
	        int bestTmax = 0;
	        int t = 0;

	        for ( int i = 0; i < inds+index; i++) //для каждой особи
	        {
		        t = individual[i].getTmax();

                if ((t == 0) && (i == inds)) 
                {
                    return bestTmax; ;
                }

		        if (bestTmax == 0)
		        {
			        bestTmax = t;
		        }

		        if (t < bestTmax)
		        {
			        bestTmax = t;
		        }
	        }
	        return bestTmax;
        }

    }
}
