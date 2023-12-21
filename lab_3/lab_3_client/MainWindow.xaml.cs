using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace lab_3_client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        private string SavedLogin = "";
        private string SavedPassword = "";

        public MainWindow()
        {
            InitializeComponent();
            SetVisibilityElements();
        }

        // Обработчик события для добавления данных
        private async void AddData(object sender, RoutedEventArgs e)
        {
            PostDataWindow dlg = new PostDataWindow();
            dlg.Owner = this;
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                DataValue data = new DataValue(dlg.valueTextBox.Text);
                var dataJson = JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = new LowerCaseNamingPolicy() });
                var encryptedData = EncryptionHandler.EncryptByteArray(Encoding.UTF8.GetBytes(dataJson));
                HttpContent strContent = new StringContent(Convert.ToBase64String(encryptedData), new MediaTypeHeaderValue("application/json"));
                var response = await client.PostAsync("http://localhost:5000/data/write", strContent);

                if (response.IsSuccessStatusCode == true)
                {
                    GetDataToListView();
                }
            }
        }

        // Метод для получения данных и отображения в ListView
        private async void GetDataToListView()
        {
            var response = await client.GetAsync("http://localhost:5000/data/read");
            var enryptedStr = await response.Content.ReadAsStringAsync();
            var decryptedStr = EncryptionHandler.DecryptString(enryptedStr);
            List<Data>? items = JsonSerializer.Deserialize<List<Data>>(decryptedStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            listviewData.ItemsSource = items;
        }

        // Метод для установки видимости элементов интерфейса
        private void SetVisibilityElements()
        {
            authForm.Visibility = Visibility.Visible;
            exitButton.Visibility = Visibility.Collapsed;
            addButton.Visibility = Visibility.Collapsed;
            listviewData.Visibility = Visibility.Collapsed;
        }

        // Обработчик события для входа пользователя
        private async void SignIn(object sender, RoutedEventArgs e)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{textBoxLogin.Text}:{textBoxPassword.Password}")));
            var response = await client.GetAsync("http://localhost:5000/login");

            if (response.IsSuccessStatusCode == true)
            {
                var enryptedStr = await response.Content.ReadAsStreamAsync();
                var decryptedStr = EncryptionHandler.DecryptStream(enryptedStr);
                var rights = JsonSerializer.Deserialize<List<string>>(decryptedStr);

                SavedLogin = textBoxLogin.Text;
                SavedPassword = textBoxPassword.Password;
                authForm.Visibility = Visibility.Collapsed;
                exitButton.Visibility = Visibility.Visible;
                if (rights != null && rights.Contains("WRITE"))
                {
                    addButton.Visibility = Visibility.Visible;
                }
                if (rights != null && rights.Contains("READ"))
                {
                    listviewData.Visibility = Visibility.Visible;
                }
                GetDataToListView();
            }
            else
            {
                string messageBoxText = "Неверные данные!\nПопробуйте еще раз.";
                string caption = "Ошибка";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }

        // Обработчик события для перехода к форме регистрации
        private void GetFormRegistr(object sender, RoutedEventArgs e)
        {
            Registration dlg = new Registration();
            dlg.Owner = this;
            dlg.ShowDialog();
        }

        // Обработчик события для выхода из аккаунта
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            SetVisibilityElements();
        }
    }
}
