using _7E_Server.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageEditAI.Helpers
{
    public class StabilityHelper
    {
        public const string API_KEY = "sk-ZjP9QXHQOpWrjY4r6TKmt5hFRrEvQOkOxVoADwq22cToANCE";

        public static async Task<Result> EditImage(string prompt, string search_prompt, IFormFile image)
        {
            try
            {
                var client = new HttpClient();
                // Устанавливаем заголовки запроса
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/*"));

                // Подготавливаем данные формы
                using (var formData = new MultipartFormDataContent())
                {
                    // Конвертируем изображение в массив байтов
                    var imageBytes = await ConvertToByteArray(image);
                    var fileContent = new ByteArrayContent(imageBytes);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"image\"",
                        FileName = $"\"{image.FileName}\""
                    };
                    formData.Add(fileContent);

                    // Добавляем текстовые параметры
                    var promptContent = new StringContent(prompt);
                    promptContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"prompt\""
                    };
                    formData.Add(promptContent);

                    var searchPromptContent = new StringContent(search_prompt);
                    searchPromptContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"search_prompt\""
                    };
                    formData.Add(searchPromptContent);

                    var outputFormatContent = new StringContent("webp");
                    outputFormatContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"output_format\""
                    };
                    formData.Add(outputFormatContent);

                    // Отправляем запрос к API
                    var response = await client.PostAsync(
                        "https://api.stability.ai/v2beta/stable-image/edit/search-and-replace",
                        formData
                    );

                    if (response.IsSuccessStatusCode)
                    {
                        // Сохраняем результат
                        var resultImageBytes = await response.Content.ReadAsByteArrayAsync();
                        var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        var filePath = Path.Combine(directoryPath, "stability.webp");
                        await File.WriteAllBytesAsync(filePath, resultImageBytes);
                        return Result.Success(filePath);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return Result.Error($"API call failed with status code: {response.StatusCode}, {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                return Result.Error($"An error occurred: {ex.Message}");
            }
        }

        private static async Task<byte[]> ConvertToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
