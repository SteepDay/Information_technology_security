using System;

namespace lab_1_client
{
    // Класс, представляющий данные
    internal class Data
    {
        public int Id { get; set; }
        public string Value { get; set; } = "";
    }

    // Класс, представляющий значение данных
    internal class DataValue
    {
        public DataValue(string value)
        {
            Value = value;
        }

        public string Value { get; set; } = "";
    }
}
