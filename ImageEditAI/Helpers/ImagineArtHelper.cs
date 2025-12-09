using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using _7E_Server.Core;


public class ImagineArtHelper
{
    private static readonly string ApiUrl = "https://api.vyro.ai/v2/image/generations";
    //vk-w23CFH3kLzDnbBDPbi6236Xa1zA2k2hJvPoOFLBs462gFJrMI
    private static readonly string ApiKey = "vk-w23CFH3kLzDnbBDPbi6236Xa1zA2k2hJvPoOFLBs462gFJrMI";
    public static void DecodeBase64ToImage(string base64String, string outputFilePath)
    {
        byte[] imageBytes = Convert.FromBase64String(base64String);
        File.WriteAllBytes(outputFilePath, imageBytes);
    }
    public static async Task<Result> GenerateImageAsync(string prompt)
    {
        using (var client = new HttpClient())
        {
            // Добавляем Authorization заголовок
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            // Создаем form-data с параметрами запроса
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(prompt), "prompt");
            formData.Add(new StringContent("realistic"), "style");
            formData.Add(new StringContent("1:1"), "aspect_ratio");
            formData.Add(new StringContent("5"), "seed");

            // Выполняем запрос POST
            var response = await client.PostAsync(ApiUrl, formData);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                //var base64 = Convert.ToBase64String(content);
                var fileName = Guid.NewGuid().ToString() + ".JPEG";
                var outputFilePath = AppDomain.CurrentDomain.BaseDirectory + Path.Combine("data", fileName);
                 File.WriteAllBytes(outputFilePath, content);
                return Result.Success(outputFilePath);
            }
            else
            {
                return Result.Error($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }
}
