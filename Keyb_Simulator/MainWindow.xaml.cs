using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Keyb_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int strLength;

        private string baseString;

        private const string defaultCharsLowCase = "1234567890qwertyuiopasdfghjklzxcvbnm[],./\\`-=;'";
        private const string defaultCharsHightCase = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM[],./\\`-=;'";

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonEnable(false);

            strLength = 70; //длина строки в нижнем регистре 62 символа
            baseString = "";// при инициализации строка пустая

            s_complexity.ValueChanged += S_complexity_ValueChanged;
            b_start.Click += B_start_Click;
            b_stop.Click += B_stop_Click;
            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;

            cb_cases.Checked += Cb_cases_Checked;
            cb_cases.Unchecked += Cb_cases_Unchecked;
            
        }


        private void Cb_cases_Unchecked(object sender, RoutedEventArgs e)
        {
            strLength = 62;
        }

        private void Cb_cases_Checked(object sender, RoutedEventArgs e)
        {
            strLength = 48; //длина строки в верхнем регистре 
        }

        /// <summary>
        /// Поднятия клавиш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            KeyboardKeyUp(e.Key.ToString());
        }

        /// <summary>
        /// Обработка опускания клавиш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            KeyboardKeyDown(e.Key.ToString());
        }

        /// <summary>
        /// Обработка события нажатия кнопки "Стоп"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_stop_Click(object sender, RoutedEventArgs e)
        {

            //Обнуление информации
            l_sQuantity.Content = 0.ToString();
            l_sErrors.Content = 0.ToString();
            s_complexity.Value = 1;
            cb_cases.IsChecked = false;
            tbk_example.Text = "";
            tbk_userInput.Text = "";
            baseString = "";

            ButtonEnable(false);

            //Блокирование/разблокирование кнопок
            b_start.IsEnabled = true;
            b_stop.IsEnabled = false;
        }

        /// <summary>
        /// Обработка события нажатия кнопки "Старт"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_start_Click(object sender, RoutedEventArgs e)
        {
            //Блокирование/разблокирование кнопок
            b_start.IsEnabled = false;
            b_stop.IsEnabled = true;

            tbk_example.Text = "Создание строки";

            GenerateString((int)s_complexity.Value, defaultCharsLowCase);

            ButtonEnable(true);
        }

        /// <summary>
        /// Обработка собыия изменения занчения в слайдере
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void S_complexity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            l_complexity.Content = s_complexity.Value.ToString();
        }

        /// <summary>
        /// Генирация строки из переданной базовой строки и выбранного колличества символов
        /// </summary>
        /// <param name="complexity"></param>
        /// <param name="str"></param>
        private void GenerateString(int complexity, string str)
        {
            int pos; //номер символа
            int isSpace;
            string tmp = "";
            int n = 0;

            while (n < complexity)
            {
                pos = new Random().Next(0, str.Length); //номер символа

                tmp += str[pos];
                n++;
                Thread.Sleep(50);
            }

            for (int i = 0; i < strLength; i++)
            {

                pos = new Random().Next(0, n); //номер символа

                if (i % 2 == 0)
                {
                    //Thread.Sleep(60);
                    isSpace = new Random().Next(0, 2);

                    if (isSpace == 1)
                    {
                        i++;
                        baseString += " ";
                        tbk_example.Text += ".";
                    }
                }

                baseString += tmp[pos];
                tbk_example.Text += ".";
                Thread.Sleep(50);
            }


            tbk_example.Text = baseString;
        }

        /// <summary>
        /// Блокировать / разблокировать клавиатуру
        /// </summary>
        /// <param name="status"></param>
        private void ButtonEnable (bool status)
        {
            int i = 1; //счетчик 

            foreach (UIElement element in (this.Content as Grid).Children)
            {
                
                if (element is Grid && i > 2)
                {
                    foreach (Button b in (element as Grid).Children)
                    {
                        b.IsEnabled = status;
                    }
                }
                else
                {
                    i++;
                }

            }
        }

        private void KeyboardKeyDown (string key)
        {
            int i = 1; //счетчик 

            foreach (UIElement element in (this.Content as Grid).Children)
            {

                if (element is Grid && i > 2)
                {
                    foreach (Button b in (element as Grid).Children)
                    {
                        if (b.Name == key)
                        {
                            b.Opacity = 0.5;
                        }
                        
                    }
                }
                else
                {
                    i++;
                }

            }
        }

        private void KeyboardKeyUp(string key)
        {
            int i = 1; //счетчик 

            foreach (UIElement element in (this.Content as Grid).Children)
            {

                if (element is Grid && i > 2)
                {
                    foreach (Button b in (element as Grid).Children)
                    {   
                        if (b.Name == key)
                        {
                            tbk_userInput.Focus();

                            b.Opacity = 1;

                            if (tbk_userInput.Text.Length <= strLength)
                            {
                                if (b.Name == "Space")
                                {
                                    tbk_userInput.Text += " ";
                                }
                                else
                                {
                                    tbk_userInput.Text += b.Content;
                                }
                                
                            }
                            else
                            {
                                return;
                            }
                        }
                        
                        
                    }
                }
                else
                {
                    i++;
                }

            }
        }
    }
}
