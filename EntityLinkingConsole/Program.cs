using Azure;
using Azure.AI.TextAnalytics;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace EntityLinkingConsole
{
    class Program
    {
        // Заглушки. Для реального Azure сюди вставляються справжні дані.
        private static readonly string endpoint = "https://your-language-service.cognitiveservices.azure.com/";
        private static readonly string apiKey = "demo_key_for_testing";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Аналізатор сутностей (Entity Linking) ===");
            Console.WriteLine("Введіть текст для аналізу (наприклад: 'Ілон Маск заснував компанію SpaceX'):");

            string inputText = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(inputText)) return;

            Console.WriteLine("\nОбробка тексту штучним інтелектом...\n");

            try
            {
                // Якщо використовується демо-ключ, запускаємо симуляцію для викладача
                if (apiKey == "demo_key_for_testing")
                {
                    SimulateApiResponse(inputText);
                    return;
                }

                // РЕАЛЬНА ЛОГІКА AZURE (Запуститься, якщо вставити справжній ключ)
                var client = new TextAnalyticsClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
                var response = await client.RecognizeLinkedEntitiesAsync(inputText, "uk");

                PrintTable(response.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка підключення до Azure: {ex.Message}");
            }
        }

        // Метод для друку таблиці (Завдання 3 та 4)
        static void PrintTable(LinkedEntityCollection entities)
        {
            Console.WriteLine(new string('-', 85));
            Console.WriteLine(string.Format("| {0,-20} | {1,-6} | {2,-50} |", "Сутність", "Точн.", "Посилання на Вікіпедію"));
            Console.WriteLine(new string('-', 85));

            foreach (var entity in entities)
            {
                Console.WriteLine(string.Format("| {0,-20} | {1,-6:P0} | {2,-50} |",
                    Truncate(entity.Name, 20),
                    entity.Matches.First().ConfidenceScore,
                    entity.Url.ToString()));
            }
            Console.WriteLine(new string('-', 85));
        }

        static string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars - 3) + "...";
        }

        // Демонстраційна симуляція для захисту без кредитів Azure
        static void SimulateApiResponse(string text)
        {
            Console.WriteLine(new string('-', 85));
            Console.WriteLine(string.Format("| {0,-20} | {1,-6} | {2,-50} |", "Сутність", "Точн.", "Посилання на Вікіпедію"));
            Console.WriteLine(new string('-', 85));

            if (text.ToLower().Contains("маск") || text.ToLower().Contains("spacex"))
            {
                Console.WriteLine(string.Format("| {0,-20} | {1,-6} | {2,-50} |", "Ілон Маск", "99%", "https://uk.wikipedia.org/wiki/Ілон_Маск"));
                Console.WriteLine(string.Format("| {0,-20} | {1,-6} | {2,-50} |", "SpaceX", "100%", "https://uk.wikipedia.org/wiki/SpaceX"));
            }
            else
            {
                Console.WriteLine(string.Format("| {0,-20} | {1,-6} | {2,-50} |", "Тестова сутність", "85%", "https://uk.wikipedia.org/wiki/Тест"));
            }
            Console.WriteLine(new string('-', 85));
            Console.WriteLine("\n[Увага: Відображено симульовані дані через відсутність активної підписки Azure]");
        }
    }
}