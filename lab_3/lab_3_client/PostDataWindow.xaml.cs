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
using System.Windows.Shapes;

namespace lab_3_client
{
    /// <summary>
    /// Логика взаимодействия для PostDataWindow.xaml
    /// </summary>
    public partial class PostDataWindow : Window
    {
        public PostDataWindow()
        {
            InitializeComponent();
        }

        void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void valueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // фильтрация сообщения ...
        }
    }
}
