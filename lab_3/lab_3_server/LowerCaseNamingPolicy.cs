using System.Text.Json;

namespace lab_3_server
{
    // Класс для настройки стратегии преобразования имени свойства в JSON
    public class LowerCaseNamingPolicy : JsonNamingPolicy
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
