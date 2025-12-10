using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using _7E_Server.Core;
using System.Net.Http.Headers;

namespace ImageEditAI.Helpers;

public static class ClipdropHelper
{
    public const string USER_ID = "1yNJAYV4aaTJ9KWrb0UCsRozEms2";
    public const string API_KEY = "73c90efcf634c57442cd522199b2108ddc8c829cac32fd083b169df5906799eadab3d04bde50c5490f656963af771068";
   public  static async Task<Result> UncropImage( IFormFile imageFile,  int extendLeft,  int extendDown)
        {
           
            try
            {
                using (var client = new HttpClient())
                using (var formData = new MultipartFormDataContent())
                {
                    // Добавляем заголовок с API ключом
                    client.DefaultRequestHeaders.Add("x-api-key", API_KEY);

                    // Добавляем файл изображения в запрос
                    var fileContent = new ByteArrayContent(await GetBytesFromFile(imageFile));
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    formData.Add(fileContent, "image_file", imageFile.FileName);

                    // Добавляем параметры для расширения
                    formData.Add(new StringContent(extendLeft.ToString()), "extend_left");
                    formData.Add(new StringContent(extendDown.ToString()), "extend_down");

                    // Отправляем POST запрос
                    var response = await client.PostAsync("https://clipdrop-api.co/uncrop/v1", formData);

                    if (response.IsSuccessStatusCode)
                    {
                        // Получаем результат как байты и отправляем его обратно клиенту
                        var resultContent  = await response.Content.ReadAsByteArrayAsync();
                           var filePath = AppDomain.CurrentDomain.BaseDirectory + "data/clipdrop.png";
                        await File.WriteAllBytesAsync(filePath, resultContent);
                        return Result.Success(filePath);
                    }
                    else
                    {
                        return Result.Error( $"Error: {response.ReasonPhrase}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Result.Error(ex);
            }
        }

        // Метод для конвертации IFormFile в массив байтов
        private static async Task<byte[]> GetBytesFromFile(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }

