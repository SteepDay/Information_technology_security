using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab_1_client
{
    /// Класс, представляющий основное окно приложения
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            beforeStartGetData();
        }

        // Обработчик добавления данных
        private async void AddData(object sender, RoutedEventArgs e)
        {
            PostDataWindow dlg = new PostDataWindow();
            dlg.Owner = this;
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                DataValue data = new DataValue(dlg.valueTextBox.Text);
                var response = await client.PostAsJsonAsync("http://localhost:5000/data/write", data);
                if (response.IsSuccessStatusCode)
                {
                    beforeStartGetData();
                }
            }
        }

        // Метод для загрузки данных перед началом получения
        private async void beforeStartGetData()
        {
            var response = await client.GetFromJsonAsync<List<Data>>("http://localhost:5000/data/read");
            listviewData.ItemsSource = response;
        }
    }
}
