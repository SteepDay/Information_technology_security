using System.Text.Json;

namespace lab_3_client
{
    // Класс для настройки стратегии преобразования имени свойства в JSON
    internal class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        // Метод для преобразования имени свойства в нижний регистр
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
                return name;

            return name.ToLower();
        }
    }
}
