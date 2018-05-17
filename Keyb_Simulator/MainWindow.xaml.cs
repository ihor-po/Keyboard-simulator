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
using System.Windows.Threading;

namespace Keyb_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int strLength; //длина строки для текст бокса

        private string baseString; //хранит сформированную строку

        bool isUpper; //флаг большой буквы
        bool CapsLook; //флаг включения CapsLook

        int errors; //колличество допущенных ошибок

        DispatcherTimer timer; //таймер
        int sec; //секунды в миллисекундах

        bool isTour; //флаг тура

        private const string defaultCharsLowCase = "1234567890qwertyuiopasdfghjklzxcvbnm[],./\\`-=;'";
        private const string defaultCharsHightCase = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM[],./\\`-=;'";

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonEnable(false);

            isUpper = false;
            CapsLook = false;

            strLength = 65; //длина строки в нижнем регистре 65 символа
            baseString = "";// при инициализации строка пустая
            errors = 0; //колличество ошибок

            sec = 0;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            s_complexity.ValueChanged += S_complexity_ValueChanged;
            b_start.Click += B_start_Click;
            b_stop.Click += B_stop_Click;
            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;

            cb_cases.Checked += Cb_cases_Checked;
            cb_cases.Unchecked += Cb_cases_Unchecked;

            timer.Tick += Timer_Tick;
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            sec += 1;

            CalcSymbols();

        }

        /// <summary>
        /// Чек бокс - не выбран
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_cases_Unchecked(object sender, RoutedEventArgs e)
        {
            strLength = 65;
        }

        /// <summary>
        /// Чек бокс - выбран
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_cases_Checked(object sender, RoutedEventArgs e)
        {
            strLength = 58; //длина строки в верхнем регистре 
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
            
            if (isTour)
            {
                EndOfCours(false);
            }

            //Обнуление информации
            l_sQuantity.Content = 0.ToString();
            l_sErrors.Content = 0.ToString();
            s_complexity.Value = 1;
            cb_cases.IsChecked = false;
            cb_cases.IsEnabled = true;
            s_complexity.IsEnabled = true;
            s_complexity.Value = 1;
            tbk_example.Text = "";
            tbk_userInput.Text = "";
            baseString = "";

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

            isTour = true;
            sec = 0;
            errors = 0;

            cb_cases.IsEnabled = false;
            s_complexity.IsEnabled = false;
            errors = 0;

            if (cb_cases.IsChecked == true)
            {
                GenerateString((int)s_complexity.Value, defaultCharsHightCase);
            }
            else
            {
                GenerateString((int)s_complexity.Value, defaultCharsLowCase);
            }

            ButtonEnable(true);

            timer.Start();
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

        /// <summary>
        /// Обработка нажатия клавиш
        /// </summary>
        /// <param name="key"></param>
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
                            if (b.Name == "LeftShift" || b.Name == "RightShift")
                            {
                                if (isUpper == false)
                                {
                                    isUpper = true;
                                }
                                else
                                {
                                    isUpper = false;
                                }
                            }
                            else if (b.Name == "Capital")
                            {
                                if (CapsLook == false)
                                {
                                    CapsLook = true;
                                }
                                else
                                {
                                    CapsLook = false;
                                }
                            }
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

        /// <summary>
        /// Обработка отпуска клавиш
        /// </summary>
        /// <param name="key"></param>
        private void KeyboardKeyUp(string key)
        {
            if (isTour)
            {

                int i = 1; //счетчик 
                if (tbk_userInput.Text.Length != strLength)
                {
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

                                    if (tbk_userInput.Text.Length < strLength)
                                    {
                                        if (b.Name == "Space") //проверка нажатия пробела
                                        {
                                            tbk_userInput.Text += " ";
                                        }
                                        else if (b.Name == "LeftShift" || b.Name == "RightShift") //проверка нажатия шифтов
                                        {
                                            if (isUpper == false)
                                            {
                                                isUpper = true;
                                            }
                                            else
                                            {
                                                isUpper = false;
                                            }
                                        }
                                        else if (b.Name == "Capital")//проверка нажатия капс лука
                                        {
                                            if (CapsLook == false)
                                            {
                                                KeysToUpper(false);
                                            }
                                            else
                                            {
                                                KeysToUpper(true);
                                            }
                                            continue;
                                        }
                                        else if (b.Name == "Tab")
                                        {
                                            tbk_userInput.Text += "    ";
                                        }
                                        else if (b.Name == "Back")
                                        {
                                            if (tbk_userInput.Text.Length > 0)
                                            {
                                                tbk_userInput.Text = tbk_userInput.Text.Substring(0, tbk_userInput.Text.Length - 1);
                                            }
                                        }
                                        else if (b.Name == "Return")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            string s = "";

                                            if (isUpper || CapsLook)
                                            {
                                                s = b.Content.ToString().ToUpper();
                                            }
                                            else
                                            {
                                                s = b.Content.ToString();
                                            }

                                            int index = tbk_userInput.Text.Length;

                                            if (s == baseString[index].ToString())
                                            {
                                                tbk_userInput.Foreground = new SolidColorBrush(Colors.Green);
                                            }
                                            else
                                            {
                                                tbk_userInput.Foreground = new SolidColorBrush(Colors.Red);
                                                l_sErrors.Content = (++errors).ToString();
                                            }

                                            tbk_userInput.Text += s;

                                            if (tbk_userInput.Text.Length == baseString.Length)
                                            {
                                                EndOfCours(true);
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        EndOfCours(true);
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
                else
                {
                    EndOfCours(true);
                    return;
                }
            }
        }

        /// <summary>
        /// Перевод клавишь в верхний / нижний регистр
        /// </summary>
        /// <param name="status"></param>
        private void KeysToUpper(bool status)
        {
            int i = 1; //счетчик 

            foreach (UIElement element in (this.Content as Grid).Children)
            {

                if (element is Grid && i > 2)
                {
                    foreach (Button b in (element as Grid).Children)
                    {
                        if (b.Name == "Space" ||
                            b.Name == "LeftShift" ||
                            b.Name == "RightShift" ||
                            b.Name == "System1" ||
                            b.Name == "System2" ||
                            b.Name == "Capital" ||
                            b.Name == "Tab" ||
                            b.Name == "LeftControl" ||
                            b.Name == "RightControl" ||
                            b.Name == "RWin" ||
                            b.Name == "LWin" ||
                            b.Name == "Return" ||
                            b.Name == "Back")
                        {
                            continue;
                        }

                        if (status)
                        {
                            b.Content = b.Content.ToString().ToUpper();
                        }
                        else
                        {
                            b.Content = b.Content.ToString().ToLower();
                        }
                    }
                }
                else
                {
                    i++;
                }

            }
        }

        /// <summary>
        /// Окончание этапа
        /// </summary>
        /// <param name="fin"></param>
        private void EndOfCours(bool fin)
        {
            isTour = false;

            timer.Stop();
            ButtonEnable(false);

            string msg;

            if (fin)
            {
                msg = "Вы завершили этап!\r\r";
            }
            else
            {
                msg = "Вы НЕ завершили этап!\r\r";
            }

            CalcSymbols();

            msg += $"Ваш результат:\rСкорость набора: {l_sQuantity.Content} симв / мин\r";
            msg += $"Колличество ошибок: {errors.ToString()}\r";
            msg += $"Правильность набора { (tbk_userInput.Text.Length - errors) * 100 / baseString.Length } %";

            MessageBox.Show(msg, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CalcSymbols()
        {
            float min = 0;

            int res = tbk_userInput.Text.Length;

            if (sec / 1000 > 60)
            {

                min = (float)(sec / (float)1000) / (float)60;
                res = (int)(res / min);
            }

            l_sQuantity.Content = res.ToString();
        }
    }
}
