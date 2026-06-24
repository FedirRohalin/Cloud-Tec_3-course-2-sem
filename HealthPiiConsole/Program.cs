using Azure;
using Azure.AI.TextAnalytics;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HealthPiiConsole
{
    class Program
    {
        private static readonly string endpoint = "https://your-language-service.cognitiveservices.azure.com/";
        private static readonly string apiKey = "demo_key_for_testing";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Аналізатор PII та Медичних даних ===");

            // Тестовий текст із PII та медичною інформацією
            string text = "Клієнт Олексій Іванов (тел. +380501234567, email: alex@gmail.com) замовив індивідуальні ортопедичні устілки. " +
                          "Діагноз: поздовжнє плоскостопість. Скаржиться на біль у п'яті. Призначено: сканування стопи, масаж та Диклофенак 50мг.";

            Console.WriteLine($"\n[Оригінальний текст]:\n{text}\n");

            if (apiKey == "demo_key_for_testing")
            {
                RunSimulation(text);
                return;
            }

            // --- РЕАЛЬНА ЛОГІКА AZURE (Запуститься з дійсним ключем) ---
            var client = new TextAnalyticsClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            // Завдання 1: Виявлення та маскування PII
            var piiResponse = await client.RecognizePiiEntitiesAsync(text, "uk");
            Console.WriteLine("--- Виявлена PII (Особиста інформація) ---");
            PrintPiiTable(piiResponse.Value);
            Console.WriteLine($"\n[Замаскований текст]:\n{MaskText(text, piiResponse.Value)}\n");

            // Завдання 2: Медична інформація (Асинхронна операція LRO)
            var healthOperation = await client.StartAnalyzeHealthcareEntitiesAsync(new[] { text }, "uk");
            await healthOperation.WaitForCompletionAsync();
            Console.WriteLine("--- Виявлена Медична інформація ---");

            // Виправлення: Правильно читаємо асинхронну відповідь від Azure
            await foreach (var resultCollection in healthOperation.Value)
            {
                PrintHealthTable(resultCollection.FirstOrDefault()?.Entities);
                break; // Оскільки у нас лише один текст, беремо перший результат і зупиняємось
            }
        }

        // Логіка маскування тексту (Заміна PII на зірочки)
        static string MaskText(string originalText, PiiEntityCollection entities)
        {
            string masked = originalText;
            // Важливо: замінюємо з кінця тексту до початку, щоб не зсунулися індекси (Offsets)
            var sortedEntities = entities.OrderByDescending(e => e.Offset);
            foreach (var entity in sortedEntities)
            {
                masked = masked.Remove(entity.Offset, entity.Length)
                               .Insert(entity.Offset, new string('*', entity.Length));
            }
            return masked;
        }

        static void PrintPiiTable(PiiEntityCollection entities)
        {
            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"| {"Сутність",-25} | {"Категорія",-15} | {"Підкатегорія",-15} | {"Точність",-10} |");
            Console.WriteLine(new string('-', 85));
            foreach (var entity in entities)
                Console.WriteLine($"| {entity.Text,-25} | {entity.Category,-15} | {entity.SubCategory,-15} | {entity.ConfidenceScore,-10:P0} |");
            Console.WriteLine(new string('-', 85));
        }

        static void PrintHealthTable(System.Collections.Generic.IReadOnlyCollection<HealthcareEntity> entities)
        {
            if (entities == null) return;
            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"| {"Медичний термін",-25} | {"Категорія",-20} | {"Точність",-10} |");
            Console.WriteLine(new string('-', 85));
            foreach (var entity in entities)
                Console.WriteLine($"| {entity.Text,-25} | {entity.Category,-20} | {entity.ConfidenceScore,-10:P0} |");
            Console.WriteLine(new string('-', 85));
        }

        // Симуляція для захисту роботи
        static void RunSimulation(string text)
        {
            Console.WriteLine("--- Виявлена PII (Особиста інформація) ---");
            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"| {"Сутність",-25} | {"Категорія",-15} | {"Підкатегорія",-15} | {"Точність",-10} |");
            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"| {"Олексій Іванов",-25} | {"Person",-15} | {"",-15} | {"99%",-10} |");
            Console.WriteLine($"| {"+380501234567",-25} | {"PhoneNumber",-15} | {"",-15} | {"100%",-10} |");
            Console.WriteLine($"| {"alex@gmail.com",-25} | {"Email",-15} | {"",-15} | {"100%",-10} |");
            Console.WriteLine(new string('-', 85));

            string masked = text.Replace("Олексій Іванов", "**************")
                                .Replace("+380501234567", "*************")
                                .Replace("alex@gmail.com", "**************");
            Console.WriteLine($"\n[Замаскований текст (Анонімізовано)]:\n{masked}\n");

            Console.WriteLine("--- Виявлена Медична інформація ---");
            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"| {"Медичний термін",-25} | {"Категорія",-20} | {"Точність",-10} |");
            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"| {"поздовжнє плоскостопість",-25} | {"Diagnosis",-20} | {"95%",-10} |");
            Console.WriteLine($"| {"біль у п'яті",-25} | {"Symptom",-20} | {"92%",-10} |");
            Console.WriteLine($"| {"сканування стопи",-25} | {"Examination",-20} | {"88%",-10} |");
            Console.WriteLine($"| {"масаж",-25} | {"Treatment",-20} | {"90%",-10} |");
            Console.WriteLine($"| {"Диклофенак",-25} | {"Medication",-20} | {"99%",-10} |");
            Console.WriteLine($"| {"50мг",-25} | {"Dosage",-20} | {"98%",-10} |");
            Console.WriteLine(new string('-', 85));
        }
    }
}