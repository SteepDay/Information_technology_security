using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace lab_2_client
{
    /// <summary>
    /// Окно регистрации
    /// </summary>
    public partial class Registration : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public Registration()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод обработки нажатия кнопки "Зарегистрироваться"
        /// </summary>
        private async void Registrate(object sender, RoutedEventArgs e)
        {
            LoginPassword data = new LoginPassword(textBoxLogin.Text, textBoxPassword.Password);
            var response = await client.PostAsJsonAsync("http://localhost:5000/registration", data);

            if (response.IsSuccessStatusCode == true)
            {
                string messageBoxText = "Успешная регистрация!";
                string caption = "Успешно";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(messageBoxText, caption, button, icon);
                this.DialogResult = true;
            }
            else
            {
                string messageBoxText = "Ошибка при регистрации!\nПопробуйте еще раз.";
                string caption = "Ошибка";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }

        /// <summary>
        /// Метод обработки нажатия кнопки "Отмена регистрации"
        /// </summary>
        private void CancelRegistration_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
