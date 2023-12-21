using System;
using System.Windows;
using System.Windows.Controls;

namespace lab_1_client
{
    /// Класс, представляющий окно для отправки данных
    public partial class PostDataWindow : Window
    {
        public PostDataWindow()
        {
            InitializeComponent();
        }

        // Обработчик клика по кнопке "ОК"
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        // Обработчик клика по кнопке "Отмена"
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        // Обработчик изменения текста в текстовом поле
        private void valueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // фильтрация сообщения ...
        }
    }
}
