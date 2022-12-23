using Google.Protobuf.WellKnownTypes;
using ScreenRecognition.Api.Models;
using ScreenRecognition.ImagePreparation.Services;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace ScreenRecognition.Api.Core.Services
{
    /// <summary>
    /// В этом классе какая-то проблема, но я не пойму какая
    /// </summary>
    public class ImageTransformationsService
    {
        private ImagePreparationService _imagePreparationService;

        private List<Thread> _threads;
        private List<byte[]> _results;

        private int _countParts = 0;

        // Первый Int - Номер картинки, второй Int - FloodValue, третий массив - Картинка
        private static List<(int, int, byte[])> _resultsNew;

        private byte[] _inputImage;

        public ImageTransformationsService()
        {
            _threads = new List<Thread>();
            _results = new List<byte[]>();

            _resultsNew = new();

            _imagePreparationService = new ImagePreparationService();
        }

        // Новая работа с изображением
        public List<byte[]> PreparedImagesV2(byte[] inputImage)
        {
            _inputImage = inputImage;
            var bmp = ImagePreparationService.ByteToBitmap(inputImage);

            List<ImageSeparationThreadModel> models = new List<ImageSeparationThreadModel>();

            var imageParts = ImageSeparation(bmp, 10);

            int currentThreadsNumber = 0;

            for (int floodValue = 80; floodValue <= 260; floodValue += 30)
            {
                for (int i = 1; i <= imageParts.Count; i++)
                {
                    var model = new ImageSeparationThreadModel(floodValue, imageParts[i - 1].ToArray(), i - 1, false);
                    models.Add(model);
                    _threads.Add(new Thread(PrepareV2));
                    _threads[currentThreadsNumber].Start(model);

                    currentThreadsNumber++;
                }
            }

            while (_threads.Where(e => e.ThreadState == ThreadState.Running).Count() > 0)
            {

            }

            var result = WholeImage();

            return result;
        }

        public List<byte[]> PreparedImages(byte[] inputImage)
        {
            _inputImage = inputImage;

            int currentThreadNumber = 0;

            for (int i = 80; i <= 250; i += 10)
            {
                _threads.Add(new Thread(Prepare));
                _threads[currentThreadNumber].Start(i);

                currentThreadNumber++;
            }
            _threads.Add(new Thread(Prepare));
            _threads[currentThreadNumber].Start(null);

            while (_threads.Where(e => e.ThreadState == ThreadState.Running).Count() > 0) { }

            return _results;
        }

        // Деление массива байтов изображения на части
        private List<byte[]> ImageSeparation(Bitmap bmp, int countParts)
        {
            List<byte[]> result = new List<byte[]>();

            _countParts = countParts;

            int currentWidth = bmp.Width / _countParts;
            int currentHeight = bmp.Height;

            int lastWidth = 0;
            int lastHeight = 0;

            for (int count = 0; count < countParts; count++)
            {
                Bitmap bitmap = new Bitmap(currentWidth - lastWidth, currentHeight);

                try
                {

                    for (int i = lastWidth; i < currentWidth; i++)
                    {
                        for (int j = 0; j < currentHeight; j++)
                        {
                            Color p = Color.FromArgb(bmp.GetPixel(i, j).ToArgb());

                            bitmap.SetPixel(i - lastWidth, j, p);
                        }
                    }
                }
                catch
                {

                }

                lastWidth = currentWidth;
                lastHeight = currentHeight;

                currentWidth += bmp.Width / _countParts;

                var element = ImagePreparationService.BitmapToByte(bitmap);

                bitmap.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/testImage{count}.png", ImageFormat.Png);

                result.Add(element);
            }

            return result;
        }

        private List<byte[]> WholeImage()
        {
            var result = new List<Bitmap>();
            for (int floodValue = 80; floodValue <= 260; floodValue += 30)
            {
                var r = _resultsNew.Where(e => e.Item2 == floodValue).ToList();

                List<byte> currentValue = new();
                List<Bitmap> currentBitmap = new();
                r.Sort();
                foreach (var value in r)
                {
                    currentValue.AddRange(value.Item3);
                    currentBitmap.Add(ImagePreparationService.ByteToBitmap(value.Item3));
                }

                result.AddRange(currentBitmap);
            }

            var preResult = result;

            for (int i = 0; i <= 100; i += 10)
            {
                Bitmap? resElement = null;
                if (preResult.Count > 10)
                {
                    resElement = Draw(preResult.GetRange(0, 10), result[0].Width, result[0].Height);
                    preResult.RemoveRange(0, 10);

                    resElement.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/convertedImage{i / 10}.png", ImageFormat.Png);

                    _results.Add(ImagePreparationService.BitmapToByte(resElement));
                }
                else
                {
                    resElement = Draw(preResult.GetRange(0, preResult.Count), result[0].Width, result[0].Height);
                    preResult.RemoveRange(0, preResult.Count);

                    resElement.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/convertedImage{i / 10}.png", ImageFormat.Png);

                    _results.Add(ImagePreparationService.BitmapToByte(resElement));
                    break;
                }
            }

            return _results;
        }

        public Bitmap? Draw(List<Bitmap> images, int Width, int Height)
        {
            try
            {
                int num = 0;
                int ListLength = images.Count * Width;

                Bitmap bitmap = new Bitmap(ListLength, Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    foreach (var img in images)
                    {
                        Image tmp = img;
                        g.DrawImageUnscaled(tmp, Width * num, 0);
                        num++;
                    }
                }

                return bitmap;

            }
            catch { }

            return null;
        }

        // Новая работа с изображением
        private void PrepareV2(object? model)
        {
            var sepModel = model as ImageSeparationThreadModel;

            var bmp = ImagePreparationService.ByteToBitmap(sepModel.ImagePart);
            var image = _imagePreparationService.GetPreparedImage(bmp, Color.White, Color.Black, sepModel.BackgroundMoreThanText, int.Parse(sepModel.FloodValue.ToString()));

            _resultsNew.Add((sepModel.Number, sepModel.FloodValue, image));
        }

        private void Prepare(object? floodValue)
        {
            if (floodValue == null)
            {
                _results.Add(_inputImage);

                return;
            }

            var bmp = ImagePreparationService.ByteToBitmap(_inputImage);
            var image = _imagePreparationService.GetPreparedImage(bmp, Color.Black, Color.White, true, int.Parse(floodValue.ToString()));

            _results.Add(image);
        }
    }
}