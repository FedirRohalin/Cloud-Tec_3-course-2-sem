using Azure.AI.TextAnalytics;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBasedDocumentClassifier
{
    public class LangBasedDocumentClassifier
    {
        private readonly ILogger<LangBasedDocumentClassifier> _logger;
        private readonly TextAnalyticsClient _textAnalyticsClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobSourceContainerClient;
        private readonly BlobContainerClient _blobDestinationContainerClient;

        // Конструктор: отримуємо підключені клієнти з Program.cs
        public LangBasedDocumentClassifier(
            ILogger<LangBasedDocumentClassifier> logger,
            TextAnalyticsClient textAnalyticsClient,
            BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _textAnalyticsClient = textAnalyticsClient;
            _blobServiceClient = blobServiceClient;

            _blobSourceContainerClient = _blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("sourceContainerName"));
            _blobDestinationContainerClient = _blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("targetContainerName"));
        }

        // Тригер спрацьовує при появі файлу в контейнері source
        [Function(nameof(LangBasedDocumentClassifier))]
        public async Task Run([BlobTrigger("rohalinsource/{name}", Connection = "blobConn")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"Оброблено файл: {name}");

            try
            {
                // 1. Звернення до ШІ для визначення мови
                var detectedLanguage = await _textAnalyticsClient.DetectLanguageAsync(content);
                var languageName = detectedLanguage.Value.Name;
                _logger.LogInformation($"Визначена мова: {languageName}");

                // 2. Формування шляху для збереження (наприклад, "Ukrainian/Sample_1.txt")
                string targetBlobName = $"{languageName}/{name}";
                BlobClient blobClient = _blobDestinationContainerClient.GetBlobClient(targetBlobName);

                // 3. Завантаження файлу в destination
                byte[] byteArray = Encoding.UTF8.GetBytes(content);
                await blobClient.UploadAsync(new MemoryStream(byteArray));
                _logger.LogInformation($"Файл завантажено у {targetBlobName}");

                // 4. Видалення файлу з source
                await _blobSourceContainerClient.DeleteBlobIfExistsAsync(name);
                _logger.LogInformation($"Оригінал {name} видалено.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка ШІ (очікувано без дійсного ключа): {ex.Message}");
            }
        }
    }
}