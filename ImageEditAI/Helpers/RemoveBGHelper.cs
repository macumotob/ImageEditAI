using _7E_Server.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageEditAI.Helpers;

public class RemoveBGHelper
{
    //https://www.remove.bg/api
    public const string API_KEY = "5MrRCBZgVS8V454dT22pCvxo";

    private const string RemoveBgUrl = "https://api.remove.bg/v1.0/removebg";

    public static async Task<Result> RemoveBackground(IFormFile file)
    {

        try
        {
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                // Add API key header
                formData.Headers.Add("X-Api-Key", API_KEY);

                // Read the uploaded file and add it to the form data
                var fileContent = new ByteArrayContent(await FileHelpers.GetFileBytes(file));
                fileContent.Headers.Add("Content-Type", "image/jpeg"); // Adjust content type if necessary
                formData.Add(fileContent, "image_file", file.FileName);

                // Add the size parameter (auto)
                formData.Add(new StringContent("auto"), "size");

                // Make the API call
                var response = await client.PostAsync(RemoveBgUrl, formData);

                if (response.IsSuccessStatusCode)
                {
                    // Get the image content from response
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();

                    // Save the file locally (or you can return it as a stream)
                    var outputFilePath = AppDomain.CurrentDomain.BaseDirectory + Path.Combine("data", "no-bg.png");
                    await System.IO.File.WriteAllBytesAsync(outputFilePath, imageBytes);

                    return Result.Success($"Background removed successfully : {outputFilePath}");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Result.Error(errorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            return Result.Error(ex);
        }
    }
}

