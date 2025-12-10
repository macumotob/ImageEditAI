using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageEditAI.Helpers;


public static class FileHelpers
{
    public static Image Base64ToImage(string base64String)
    {
        byte[] imageBytes = Convert.FromBase64String(base64String);
        using (var ms = new MemoryStream(imageBytes))
        {
            return Image.FromStream(ms);
        }
    }
    public static async Task<byte[]> GetFileBytes(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public static async Task<string> ConvertIFormFileToBase64(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null;

        using (var memoryStream = new MemoryStream())
        {
            // Копируем содержимое файла в поток памяти
            await file.CopyToAsync(memoryStream);

            // Получаем байты из потока
            byte[] fileBytes = memoryStream.ToArray();

            // Преобразуем байты в строку Base64
            string base64String = Convert.ToBase64String(fileBytes);

            return base64String;
        }
    }

    // Метод для наложения маски на изображение
    public static Image OverlayImages(Image background, Image mask)
    {
        // Убедимся, что оба изображения имеют одинаковые размеры
        if (background.Width != mask.Width || background.Height != mask.Height)
        {
            throw new ArgumentException("Images must have the same dimensions.");
        }

        // Создаем новое изображение для результата наложения
        Bitmap result = new Bitmap(background.Width, background.Height);

        // Проходим по всем пикселям изображений
        for (int x = 0; x < background.Width; x++)
        {
            for (int y = 0; y < background.Height; y++)
            {
                // Получаем пиксель фона
                Color backgroundColor = ((Bitmap)background).GetPixel(x, y);

                // Получаем пиксель маски
                Color maskColor = ((Bitmap)mask).GetPixel(x, y);

                // Если пиксель маски белый (или близкий к белому), то используем пиксель фона
                // в качестве наложенного пикселя, иначе оставляем пиксель фона
                if (maskColor.R == 255 && maskColor.G == 255 && maskColor.B == 255) // Белый
                {
                    result.SetPixel(x, y, backgroundColor);
                }
                else
                {
                    // Черный или любой другой цвет на маске - оставляем оригинальный фон
                    result.SetPixel(x, y, backgroundColor);
                }
            }
        }

        return result;
    }

    // Метод для конвертации изображения в base64 строку
    public static string ImageToBase64(Image image, ImageFormat format)
    {
        using (var ms = new MemoryStream())
        {
            image.Save(ms, format);
            byte[] imageBytes = ms.ToArray();
            return Convert.ToBase64String(imageBytes);
        }
    }

    public static void SaveImageToFile(Image image, string filePath)
    {
        image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg); // або PNG, Bmp тощо
    }


    public static Image RemoveBackground(Image originalImage, Image maskImage)
    {
        // Перетворюємо Image на Bitmap для обробки
        Bitmap originalBitmap = new Bitmap(originalImage);
        Bitmap maskBitmap = new Bitmap(maskImage);

        // Перевірка розмірів зображень
        if (originalBitmap.Width != maskBitmap.Width || originalBitmap.Height != maskBitmap.Height)
            throw new ArgumentException("Зображення повинні мати однакові розміри.");

        Bitmap result = new Bitmap(originalBitmap.Width, originalBitmap.Height, PixelFormat.Format32bppArgb);

        for (int x = 0; x < originalBitmap.Width; x++)
        {
            for (int y = 0; y < originalBitmap.Height; y++)
            {
                Color originalPixel = originalBitmap.GetPixel(x, y);
                Color maskPixel = maskBitmap.GetPixel(x, y);

                // Якщо піксель маски чорний (фон), робимо його прозорим
                if (maskPixel.R < 128 && maskPixel.G < 128 && maskPixel.B < 128)
                {
                    result.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                }
                else
                {
                    // Якщо піксель маски білий (фігура), залишаємо його без змін
                    result.SetPixel(x, y, originalPixel);
                }
            }
        }

        return result;
    }

    public static void Mai122n()
    {
        // Пример base64 строк
        string base64Image1 = "<BASE64_STRING_IMAGE_1>"; // Первая картинка с фигурой на фоне
        string base64Image2 = "<BASE64_STRING_IMAGE_2>"; // Вторая картинка с белой фигурой на черном фоне

        // Декодируем base64 строки в изображения
        Image backgroundImage = Base64ToImage(base64Image1);
        Image maskImage = Base64ToImage(base64Image2);

        // Наложение изображения маски на фоновое изображение
        Image resultImage = OverlayImages(backgroundImage, maskImage);

        // Преобразуем результат в base64 строку (если нужно)
        string resultBase64 = ImageToBase64(resultImage, ImageFormat.Png);

        // Выводим результат
        Console.WriteLine(resultBase64);
    }
}


