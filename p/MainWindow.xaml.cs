using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Markup;
using System.Windows.Controls.DataVisualization.Charting;
using System.Diagnostics;

namespace p
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //textBox9.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viborAlg = 0;
           //textBox9.Visibility = Visibility.Hidden;
        }
        public Dictionary<string, float> Chardata;  
        int viborAlg=0;
        Generation gen = new Generation();
        int FlagElite = 0;
		int CrossElite = 0;
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e) {}
        private void textBox2_TextChanged(object sender, TextChangedEventArgs e) {}
        private void textBox3_TextChanged(object sender, TextChangedEventArgs e) {}
        private void textBox4_TextChanged(object sender, TextChangedEventArgs e) {}
        private void radioButton1_Checked(object sender, RoutedEventArgs e) { }
        private void radioButton2_Checked(object sender, RoutedEventArgs e) { }
        private void textBox5_TextChanged(object sender, TextChangedEventArgs e) { }

        private int vipolnenie(int con, ref float averageTmax, ref int successfulMutt, int [] valueTmax)
        {
            int N=1;
            int M=1;
            int a=1;
            int b=1;
			int hem=0;
			N = Convert.ToInt32(textBox1.Text);
			M = Convert.ToInt32(textBox2.Text);
			a = Convert.ToInt32(textBox3.Text);
			b = Convert.ToInt32(textBox4.Text);
			hem = Convert.ToInt32(textBox9.Text);
			int[,] T = new int[M, N];
	 
			string buff;
			for (int i = 0; i < M; i++)
			{
				buff = textBox7.GetLineText(i);
				string[] strbuff = buff.Split(' ', '\n');

				for (int j = 0; j < N; j++)
				{
					 T[i, j] = Convert.ToInt32(strbuff[j]); 
				}
			}
			int inds = Convert.ToInt32(textBox10.Text);
			int reps = Convert.ToInt32(textBox11.Text);
		 
			if (radioButton1.IsChecked == true)
			{
				gen.set(N, M, inds, T, hem, 0, FlagElite,CrossElite);
			}
			else
			{
				gen.set(N, M, inds, T, hem, 1, FlagElite,CrossElite);
			}
			
			int bestTmax = gen.bestTmax();
			int r = 0;
			int r2 = 0;
			int counte = 1;
			
			if (radioButton1.IsChecked != true)
			{
				gen.BubbleSort2();
				using (StreamWriter writer = File.AppendText("Zurnal.txt"))
				{
					writer.WriteLine("..............................................................");
					writer.WriteLine( "Следующее поколение № 2: Родители" );
					writer.Close();
				}
				gen.print2();  
			}
			gen.newGeneration(1);
			bestTmax = gen.bestTmax();
			
			while ( r < reps-1 )
			{
				gen.newGeneration(0);
				if ( gen.bestTmax() == bestTmax )
				{
					r++;
				}
				else
				{
					r = 0;
				}
				bestTmax = gen.bestTmax();
			}
			int bestbestTmax=bestTmax;
			
			while ( r2 != 1 )
			{
				r = 0;
				if (radioButton1.IsChecked != true)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine("****************************************************************************************************");
						writer.WriteLine( "Перед мутацией: {0}",  counte );
						writer.Close();
					}
				}
				
				gen.BubbleSort2();
				
				if (radioButton1.IsChecked != true)
				{
					gen.print2();
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine("****************************************************************************************************");
						writer.WriteLine( "Макромутация №: {0}",  counte );
						writer.Close();
					}
				}
				
				counte++;
				
				
				switch (viborAlg)
				{
					case 0:
						gen.mutationC1();
						break;
					case 1:
						gen.mutationC2();
						break;
					case 2:
						gen.mutationC3();
						break;
					case 3:
						gen.mutationC4();
						break;
					case 4:
						gen.mutationC5();
						break;
					case 5:
						gen.mutationC6();
						break; 
					case 6:
						gen.mutationC7();
						break; 
					case 7:
						gen.mutationC8();
						break; 
					case 8:
						gen.mutationC9();
						break; 
				}
				
				if (radioButton1.IsChecked != true)
				{
					using (StreamWriter writer = File.AppendText("Zurnal.txt"))
					{
						writer.WriteLine();
						writer.WriteLine("..............................................................");
						writer.WriteLine( "Следующее поколение № 2: Родители");
						writer.Close();
					}
					gen.print2();  
				}
				gen.newGeneration(1);
				bestTmax = gen.bestTmax();
				while ( r < reps-1 )
				{
					gen.newGeneration(0);
					if ( gen.bestTmax() == bestTmax )
					{
						r++;
					}
					else
					{
						r = 0;
					}
					bestTmax = gen.bestTmax();
				}
				if ( gen.bestTmax() == bestbestTmax )
				{
					r2=1;
				}
				else{successfulMutt++;}
				bestbestTmax = gen.bestTmax();
			}
			averageTmax=averageTmax+gen.bestTmax();
			valueTmax[con]=gen.bestTmax();
            return 0;
        }

       
      private void vibor(int viborAlgM)
        {
			
			viborAlg=viborAlgM;
			int repOpit;
			float averageTmax=0;
			float Variance=0;
			int successfulMutt=0;
			repOpit = Convert.ToInt32(textBox8.Text);
			int [] valueTmax=new int [repOpit];
			for (int con = 0; con < repOpit; con++)
			{
				int ER=vipolnenie( con, ref averageTmax, ref successfulMutt, valueTmax);
				if (ER==3){while(ER==3){ER= vipolnenie( con, ref averageTmax, ref successfulMutt, valueTmax);}}
			}
			averageTmax=averageTmax/repOpit;
			
			for (int ii = 0; ii < repOpit; ii++)
			{
				Variance=Variance+ (valueTmax[ii]-averageTmax)*(valueTmax[ii]-averageTmax);
			}
			Variance=Variance/repOpit;
			
			switch (viborAlg)
			{
				case 0:
					Chardata.Add("Обмен 2-х бит", averageTmax);
					textBox6.Text = textBox6.Text + "Обмен 2-х бит:                 " + "среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) +") Успешных мутаций: " + Convert.ToString(successfulMutt) + "\n";
					using (StreamWriter writer = File.AppendText("Rezult.txt"))
					{
						writer.WriteLine( "Обмен 2-х бит:                 " + "среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " + Convert.ToString(successfulMutt));
						writer.Close();
					}
					break;
				case 1:
					Chardata.Add("Обмен 3-х бит", averageTmax);
					textBox6.Text = textBox6.Text + "Обмен 3-х бит:                 " + "среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " + Convert.ToString(successfulMutt) + "\n";
					using (StreamWriter writer = File.AppendText("Rezult.txt"))
					{
						writer.WriteLine("Обмен 3-х бит:                 " + "среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " + Convert.ToString(successfulMutt));
						writer.Close();
					}
					break;
				case 2:
					Chardata.Add("Обмен 2-х бит с инверсией", averageTmax);
					textBox6.Text = textBox6.Text + "Обмен 2-х бит с инверсией:     " + "среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " + Convert.ToString(successfulMutt) + "\n";
					using (StreamWriter writer = File.AppendText("Rezult.txt"))
					{
						writer.WriteLine( "Обмен 2-х бит с инверсией:     " + "среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " + Convert.ToString(successfulMutt));
						writer.Close();
					}
					break;
				case 3:
					Chardata.Add("Обмен 2-х бит с 2 инверс", averageTmax);
					textBox6.Text = textBox6.Text + "Обмен 2-х бит с 2 инверсией:   " + "среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " + Convert.ToString(successfulMutt) + "\n";
					using (StreamWriter writer = File.AppendText("Rezult.txt"))
					{
						writer.WriteLine( "Обмен 2-х бит с 2 инверсией:   " + "среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " + Convert.ToString(successfulMutt));
						writer.Close();
					}
					break;
				case 4:
					Chardata.Add("Обмен 3-х бит с инверсией", averageTmax);
					textBox6.Text =textBox6.Text+"Обмен 3-х бит с инверсией:     " +"среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " +Convert.ToString(successfulMutt)+"\n";
					using (StreamWriter writer = File.AppendText("Rezult.txt"))
					{
						writer.WriteLine( "Обмен 3-х бит с инверсией:     " +"среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " +Convert.ToString(successfulMutt));
						writer.Close();
					}
					break;
				case 5:
					Chardata.Add("Обмен 3-х бит с 2 инверс", averageTmax);
					textBox6.Text =textBox6.Text+"Обмен 3-х бит с 2 инверсией:   " +"среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " +Convert.ToString(successfulMutt)+"\n";
					using (StreamWriter writer = File.AppendText("Rezult.txt"))
					{
						writer.WriteLine( "Обмен 3-х бит с 2 инверсией:   " +"среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " +Convert.ToString(successfulMutt));
						writer.Close();
					}
					break; 
				case 6:
					Chardata.Add("1 случайный бит", averageTmax);
					textBox6.Text =textBox6.Text+"1 случайный бит:               " +"среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " +Convert.ToString(successfulMutt)+"\n";
					using (StreamWriter writer = File.AppendText("Rezult.txt"))
					{
						writer.WriteLine("1 случайный бит:               " +"среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " +Convert.ToString(successfulMutt));
						writer.Close();
					}
					break; 
				case 7:
					Chardata.Add("2 бита рядом", averageTmax);
					textBox6.Text =textBox6.Text+"2 бита рядом:                  " +"среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " +Convert.ToString(successfulMutt)+"\n";
					using (StreamWriter writer = File.AppendText("Rezult.txt"))
					{
						writer.WriteLine( "2 бита рядом:                  " +"среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " +Convert.ToString(successfulMutt));
						writer.Close();
					}
					break; 
				case 8:
					Chardata.Add("2 бита случайно", averageTmax);
					textBox6.Text =textBox6.Text+"2 бита случайно:               " +"среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " +Convert.ToString(successfulMutt);
					using (StreamWriter writer = File.AppendText("Rezult.txt"))
					{
						writer.WriteLine( "2 бита случайно:               " +"среднее Tmax: (" + Convert.ToString(averageTmax) + ")     Дисперсия: ("+ Convert.ToString(Variance) + ") Успешных мутаций: " +Convert.ToString(successfulMutt));
						writer.Close();
					}
					break; 
			}
		}
 
        private void button_Zapusk_Click(object sender, RoutedEventArgs e)
        {
			
			try
            {
				Stopwatch stopWatch = new Stopwatch();
				stopWatch.Start();
				
				using (StreamWriter writer = File.CreateText("Rezult.txt"))
						{ writer.Close(); }

				if (radioButtonNoElite.IsChecked == true) { FlagElite = 0; };
				if (radioButtonEliteCp.IsChecked == true) { FlagElite = 2; };
				if (radioButtonEliteCpVozrast.IsChecked == true) { FlagElite = 3; };
				if (radioButtonElite.IsChecked == true) { FlagElite = 4; };
				
				if (radioButtonHux.IsChecked == true) { CrossElite = 0; };
				if (radioButtonRand.IsChecked == true) { CrossElite = 1; };
				
				Chardata = new Dictionary<string, float>();

				if ( radioButton2.IsChecked == true)
					{
						using (StreamWriter writer = File.CreateText("Zurnal.txt"))
						{ writer.Close(); }
					}
				textBox6.Text = "";
				if (checkBox1.IsChecked == true) { vibor(0); };
				if (checkBox2.IsChecked == true) {vibor(1); };
				if (checkBox3.IsChecked == true) { vibor(2); };
				if (checkBox4.IsChecked == true) { vibor(3);};
				if (checkBox5.IsChecked == true) { vibor(4);};
				if (checkBox6.IsChecked == true) {vibor(5); };
				if (checkBox7.IsChecked == true) { vibor(6);};
				if (checkBox8.IsChecked == true) {vibor(7);};
				if (checkBox9.IsChecked == true) { vibor(8); };
				((ColumnSeries)mcChart.Series[0]).ItemsSource = Chardata;

				stopWatch.Stop();
				TimeSpan ts = stopWatch.Elapsed;
				string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",ts.Hours, ts.Minutes, ts.Seconds,
					ts.Milliseconds / 10);
				textBox6.Text =textBox6.Text+"\nВремя выполнения " + elapsedTime;
				using (StreamWriter writer = File.AppendText("Rezult.txt"))
					{
						writer.WriteLine( "Время выполнения " + elapsedTime);
						writer.Close();
					}
			}
			catch (System.FormatException)
            {
                MessageBox.Show("Проверьте правильность данных");
            }
		
			catch (System.OverflowException)
            {
                MessageBox.Show("Проверьте правильность данных");
            
            }
			catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Проверьте правильность данных");
            }
		}

        private void button_Generaziya_Click(object sender, RoutedEventArgs e)
        {
            
            Random myRnd;
            myRnd = new Random();
            int M;
            int N;
			int hem ;
            int min, max;
            int  inds, reps;
			int repOpit;
			try
            {
				if (textBox1.Text == "")
				{
					N = myRnd.Next(3,10);
				}
				else
				{
					N = Convert.ToInt32(textBox1.Text);
				}
				if (textBox2.Text == "")
				{
					M = myRnd.Next(12,30);
				}
				else
				{M = Convert.ToInt32(textBox2.Text);}
				if (textBox3.Text == "")
				{
					min = myRnd.Next(1,11);
				}
				else
				{min = Convert.ToInt32(textBox3.Text);}
				if (textBox4.Text == "")
				{
					max = myRnd.Next(12,30);
				}
				else
				{ max = Convert.ToInt32(textBox4.Text);}
				textBox6.Text = "";
				textBox7.Text = "";

			 
				textBox6.Text = "";
				textBox7.Text = "";
				//textBox8.Text = "";
				
				if (textBox8.Text == "")
				{
					repOpit = myRnd.Next(2, 4);
					textBox8.Text = Convert.ToString(repOpit);
				}
				
				if (textBox10.Text == "")
				{
					inds = myRnd.Next(3,10);
					textBox10.Text = Convert.ToString(inds);
				}
				
				if (textBox11.Text == "")
				{
					reps = myRnd.Next(2, 10);
					textBox11.Text = Convert.ToString(reps);
				}
				if (textBox9.Text == "")
				{
					hem = myRnd.Next(M/2,M-1);
					textBox9.Text = Convert.ToString(hem);
				}
				
				int[,] Tga = new int[M, N];
				for (int i = 0; i < M; i++) 
				{
					Tga[i, 0] = myRnd.Next(min, max);   
					for (int j = 0; j < N; j++)
					{
						Tga[i,j] = Tga[i,0];
						textBox7.Text = textBox7.Text + Tga[i, j];
						if (j != (N - 1)) { textBox7.Text = textBox7.Text + " "; }
					}
					if (i != (M - 1)) { textBox7.Text = textBox7.Text + "\n"; }
				}
				
			   
				textBox1.Text = Convert.ToString(N);
				textBox2.Text = Convert.ToString(M);
				textBox3.Text = Convert.ToString(min);
				textBox4.Text = Convert.ToString(max);
				//textBox9.Text = Convert.ToString(hem);
            }
			 catch (System.FormatException)
            {
                MessageBox.Show("Проверьте правильность данных");
            }
             catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Проверьте правильность данных");
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e) {}
        private void checkBox1_Checked(object sender, RoutedEventArgs e) {}
        private void checkBox2_Checked(object sender, RoutedEventArgs e){}
        private void checkBox3_Checked(object sender, RoutedEventArgs e)  { }
        private void checkBox4_Checked(object sender, RoutedEventArgs e)  {}
        private void checkBox5_Checked(object sender, RoutedEventArgs e)  { }
        private void checkBox6_Checked(object sender, RoutedEventArgs e) {}
		private void RadioButtonClicked(object sender, RoutedEventArgs e) {}
        private void checkBox7_Checked(object sender, RoutedEventArgs e) {  }
        private void checkBox8_Checked(object sender, RoutedEventArgs e) {}
        private void checkBox9_Checked(object sender, RoutedEventArgs e) { }

        private void Button_VibratVse_Click(object sender, RoutedEventArgs e)
        {
			checkBox1.IsChecked = true;
            checkBox2.IsChecked = true;
            checkBox3.IsChecked = true;
            checkBox4.IsChecked = true;
            checkBox5.IsChecked = true;
            checkBox6.IsChecked = true;
            checkBox7.IsChecked = true;
            checkBox8.IsChecked = true;
            checkBox9.IsChecked = true;
        }

    }
}
