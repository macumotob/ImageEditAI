using System;
using System.Net.Http;
using System.Threading.Tasks;
using _7E_Server.Core;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;



namespace ImageEditAI.Helpers;

public class SnapHelper
{
    public const string API_KEY = "778a6edc7e056fcce46220d485702a801307b972b92a6637e36b6e5f4fa68517";
    private const string ApiUrl = "https://platform.snapedit.app/api/background_removal/v1/erase"; // URL API

    public static async Task<Result> AutoSuggest()
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("X-API-KEY", API_KEY);

            try
            {
                var response = await client.GetAsync("https://platform.snapedit.app/api/object_removal/v1/auto_suggest");

                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadAsStringAsync();

                return Result.Success(data);
            }
            catch (HttpRequestException e)
            {
                return Result.Error($"Request error: {e.Message}");
            }
        }
    }


    public static async Task<Result> RemoveBackground([FromForm] IFormFile inputImage)
    {
        using (var client = new HttpClient())
        {
            var form = new MultipartFormDataContent();

            // Создаем поток для файла
            var fileStream = inputImage.OpenReadStream();
            var fileContent = new StreamContent(fileStream);

            // Устанавливаем тип контента для файла
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            // Добавляем файл в форму
            form.Add(fileContent, "input_image", inputImage.FileName);

            // Добавляем заголовок с API-ключом
            client.DefaultRequestHeaders.Add("X-API-KEY", API_KEY);

            // Отправляем POST запрос
            var response = await client.PostAsync(ApiUrl, form);

            // Проверяем успешность запроса
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Result.Success(data); // Возвращаем ответ от API
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return Result.Error(error); 
            }
        }
    }
}




