using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            s_complexity.ValueChanged += S_complexity_ValueChanged;
            b_start.Click += B_start_Click;
            b_stop.Click += B_stop_Click;
            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.SystemKey.ToString());
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

            //Блокирование/разблокирование кнопок
            b_start.IsEnabled = false;
            b_stop.IsEnabled = true;
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
    }
}
