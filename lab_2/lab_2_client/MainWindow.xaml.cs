using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab_2_client
{
    /// <summary>
    /// Главное окно приложения (MainWindow.xaml)
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

        /// <summary>
        /// Обработчик события нажатия кнопки "Добавить данные"
        /// </summary>
        private async void AddData(object sender, RoutedEventArgs e)
        {
            PostDataWindow dlg = new PostDataWindow();
            dlg.Owner = this;
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                DataValue data = new DataValue(dlg.valueTextBox.Text);
                var response = await client.PostAsJsonAsync("http://localhost:5000/data/write", data);
                if (response.IsSuccessStatusCode == true)
                {
                    GetDataToListView();
                }
            }
        }

        /// <summary>
        /// Загрузка данных в ListView
        /// </summary>
        private async void GetDataToListView()
        {
            var response = await client.GetFromJsonAsync<List<Data>>("http://localhost:5000/data/read");
            listviewData.ItemsSource = response;
        }

        /// <summary>
        /// Установка видимости элементов
        /// </summary>
        private void SetVisibilityElements()
        {
            authForm.Visibility = Visibility.Visible;
            exitButton.Visibility = Visibility.Collapsed;
            addButton.Visibility = Visibility.Collapsed;
            listviewData.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Вход"
        /// </summary>
        private async void SignIn(object sender, RoutedEventArgs e)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{textBoxLogin.Text}:{textBoxPassword.Password}")));
            var response = await client.GetAsync("http://localhost:5000/login");
            if (response.IsSuccessStatusCode == true)
            {
                var rights = response.Content.ReadFromJsonAsync<List<string>>().Result;
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

        /// <summary>
        /// Обработчик события нажатия кнопки "Регистрация"
        /// </summary>
        private void GetFormRegistr(object sender, RoutedEventArgs e)
        {
            Registration dlg = new Registration();
            dlg.Owner = this;
            dlg.ShowDialog();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Выход"
        /// </summary>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            SetVisibilityElements();
        }
    }
}
