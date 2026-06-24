using Azure;
using Azure.AI.Translation.Text;
using System;
using System.Threading.Tasks;

namespace TranslatorConsoleApp
{
    class Program
    {
        // Заглушки для автентифікації. Якщо залишити "demo_key", спрацює симуляція для захисту.
        private static readonly string key = "demo_key_for_testing";
        private static readonly string region = "eastus";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Azure AI Translator Console ===");

            Console.Write("Введіть текст для перекладу: ");
            string text = Console.ReadLine()!;

            Console.Write("Введіть код цільової мови (наприклад, 'en' для англійської, 'uk' для української, 'de' для німецької): ");
            string targetLanguage = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(targetLanguage))
            {
                Console.WriteLine("Помилка: Текст або код мови не можуть бути порожніми.");
                return;
            }

            if (key == "demo_key_for_testing")
            {
                RunSimulation(text, targetLanguage);
                return;
            }

            // --- РЕАЛЬНА ЛОГІКА AZURE ---
            try
            {
                var client = new TextTranslationClient(new AzureKeyCredential(key), region);

                // Звертаємося до ШІ для перекладу
                var response = await client.TranslateAsync(targetLanguage, text);
                var translationResult = response.Value[0];

                Console.WriteLine("\n--- Результат від Azure ---");
                Console.WriteLine($"Автоматично визначена мова оригіналу: {translationResult.DetectedLanguage.Language} (Точність: {translationResult.DetectedLanguage.Score:P0})");
                Console.WriteLine($"Переклад [{targetLanguage}]: {translationResult.Translations[0].Text}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка доступу до Azure: {ex.Message}");
            }
        }

        static void RunSimulation(string text, string targetLanguage)
        {
            Console.WriteLine("\n--- Результат аналізу (Симуляція) ---");
            Console.WriteLine("Автоматично визначена мова оригіналу: uk (Точність: 100%)");

            string translatedText = targetLanguage.ToLower() switch
            {
                "en" => "[English Translation]: We received your request.",
                "de" => "[Deutsche Übersetzung]: Wir haben Ihre Anfrage erhalten.",
                _ => $"[Переклад на {targetLanguage}]: (Тестовий переклад виконано успішно)"
            };

            Console.WriteLine($"Переклад [{targetLanguage}]: {translatedText}");
            Console.WriteLine("\n[Увага: Відображено симульовані дані через використання демо-ключа]");
        }
    }
}