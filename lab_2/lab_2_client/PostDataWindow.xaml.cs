using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace lab_2_client
{
    /// <summary>
    /// Логика взаимодействия для окна для отправки данных (PostDataWindow.xaml)
    /// </summary>
    public partial class PostDataWindow : Window
    {
        public PostDataWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "OK"
        /// </summary>
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Отмена"
        /// </summary>
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Обработчик изменения текста в текстовом поле valueTextBox
        /// </summary>
        private void valueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // фильтрация сообщения ...
        }
    }
}
