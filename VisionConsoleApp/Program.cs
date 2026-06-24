using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System;
using System.Threading.Tasks;

namespace VisionConsoleApp
{
    class Program
    {
        // Заглушки для ключів. Якщо тут "demo_key", спрацює симуляція.
        static string endpoint = "https://your-vision-service.cognitiveservices.azure.com/";
        static string key = "demo_key_for_testing";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Комп'ютерний зір Azure AI ===");
            string imageUrl = "https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png";
            Console.WriteLine($"Аналіз зображення за посиланням: {imageUrl}\n");

            if (key == "demo_key_for_testing")
            {
                RunSimulation();
                return;
            }

            // --- РЕАЛЬНА ЛОГІКА AZURE ---
            var client = new ImageAnalysisClient(new Uri(endpoint), new AzureKeyCredential(key));

            // Завдання 1: OCR
            Console.WriteLine("--- 1. Розпізнавання тексту (OCR) ---");
            var ocrResult = await client.AnalyzeAsync(new Uri(imageUrl), VisualFeatures.Read);
            // Додаємо .Value для доступу до даних
            foreach (var line in ocrResult.Value.Read.Blocks[0].Lines)
                Console.WriteLine($"Знайдено текст: '{line.Text}'");

            // Завдання 2: Аналіз зображення
            Console.WriteLine("\n--- 2. Аналіз зображення (Image Analysis) ---");
            var analysisResult = await client.AnalyzeAsync(new Uri(imageUrl), VisualFeatures.Caption | VisualFeatures.Tags);
            // Додаємо .Value
            Console.WriteLine($"Опис: {analysisResult.Value.Caption.Text} (Точність: {analysisResult.Value.Caption.Confidence:P0})");
            Console.WriteLine("Теги:");
            foreach (var tag in analysisResult.Value.Tags.Values)
                Console.WriteLine($"- {tag.Name} ({tag.Confidence:P0})");

            // Завдання 3: Face Service
            Console.WriteLine("\n--- 3. Розпізнавання облич (Face) ---");
            var faceResult = await client.AnalyzeAsync(new Uri(imageUrl), VisualFeatures.People);
            // Додаємо .Value
            foreach (var person in faceResult.Value.People.Values)
                Console.WriteLine($"Знайдено людину. Координати: X={person.BoundingBox.X}, Y={person.BoundingBox.Y}");
        }

        static void RunSimulation()
        {
            Console.WriteLine("--- 1. Розпізнавання тексту (OCR) ---");
            Console.WriteLine("Знайдено текст: 'Contoso Strategy 2026'");
            Console.WriteLine("Знайдено текст: 'Quarterly Revenue Growth'");

            Console.WriteLine("\n--- 2. Аналіз зображення (Image Analysis) ---");
            Console.WriteLine("Опис: Чоловік у діловому костюмі стоїть біля дошки з графіками (Точність: 95%)");
            Console.WriteLine("Теги:");
            Console.WriteLine("- person (99%)");
            Console.WriteLine("- whiteboard (98%)");
            Console.WriteLine("- business (90%)");

            Console.WriteLine("\n--- 3. Розпізнавання облич (Face) ---");
            Console.WriteLine("Знайдено обличчя: 1");
            Console.WriteLine("Вік: 35, Емоція: Впевненість/Нейтральна");
            Console.WriteLine("Координати квадрата обличчя (Bounding Box): X=120, Y=45, W=80, H=100");
            Console.WriteLine("\n[Симуляція успішно завершена]");
        }
    }
}