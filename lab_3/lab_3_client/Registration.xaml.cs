using lab_3_client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public Registration()
        {
            InitializeComponent();
        }

        // Обработчик события для регистрации
        private async void Registrate(object sender, RoutedEventArgs e)
        {
            LoginPassword data = new LoginPassword(textBoxLogin.Text, textBoxPassword.Password);
            var dataJson = JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = new LowerCaseNamingPolicy() });
            var encryptedData = EncryptionHandler.EncryptByteArray(Encoding.UTF8.GetBytes(dataJson));
            HttpContent strContent = new StringContent(Convert.ToBase64String(encryptedData), new MediaTypeHeaderValue("application/json"));
            var response = await client.PostAsync("http://localhost:5000/registration", strContent);

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

        // Обработчик события для отмены регистрации
        private void CancelRegistration_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
