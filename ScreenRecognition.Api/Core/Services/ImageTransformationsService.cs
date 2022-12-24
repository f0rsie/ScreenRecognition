using Google.Protobuf.WellKnownTypes;
using ScreenRecognition.Api.Models;
using ScreenRecognition.ImagePreparation.Services;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml.Linq;

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

        private List<ImageSeparationThreadModel> _newResults;

        private byte[] _inputImage;

        public ImageTransformationsService()
        {
            _threads = new List<Thread>();
            _results = new List<byte[]>();

            _newResults = new List<ImageSeparationThreadModel>();

            _imagePreparationService = new ImagePreparationService();
        }

        // Новая работа с изображением
        public List<byte[]> PreparedImagesV2(byte[] inputImage)
        {
            _results = new();
            _threads = new();
            _newResults = new();

            _inputImage = inputImage;
            var bmp = ImagePreparationService.ByteToBitmap(inputImage);

            List<ImageSeparationThreadModel> models = new List<ImageSeparationThreadModel>();

            var imageParts = ImageSeparation(bmp, 10);

            int currentThreadsNumber = 0;

            for (int i = 0; i < imageParts.Count; i++)
            {
                for (int floodValue = 80; floodValue <= 260; floodValue += 30)
                {
                    var model = new ImageSeparationThreadModel(floodValue, imageParts[i], i, false);
                    models.Add(model);
                    _threads.Add(new Thread(PrepareV2));
                    _threads[currentThreadsNumber].Start(model);

                    currentThreadsNumber++;
                }
            }

            while (true)
            {
                if (_threads.Where(e => e.ThreadState == ThreadState.Running).Count() == 0)
                    break;
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

        // Склейка всех частей изображений в целое
        private List<byte[]> WholeImage()
        {
            int i = 0;
            var res = new List<List<Bitmap>>();
            var result = new List<Bitmap>();
            for (int floodValue = 80; floodValue < 260; floodValue += 30)
            {
                var r = _newResults.Where(e => e.FloodValue == floodValue).OrderBy(e => e.Number).ToList();

                List<Bitmap> currentBitmap = new();

                foreach (var value in r)
                {
                    currentBitmap.Add(ImagePreparationService.ByteToBitmap(value.ImagePart));
                }

                result.AddRange(currentBitmap);
                res.Add(currentBitmap);
                result = new List<Bitmap>();
            }

            foreach (var item in res)
            {
                Bitmap? resElement = null;

                if (item.Count >= 10)
                {
                    resElement = Draw(item.GetRange(0, 10), item[0].Width, item[0].Height);
                }
                else
                {
                    resElement = Draw(item.GetRange(0, item.Count), item[0].Width, item[0].Height);
                }

                resElement.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/convertedImage{i++}.png", ImageFormat.Png);
                _results.Add(ImagePreparationService.BitmapToByte(resElement));
            }

            return _results;
        }

        // Склейка нескольких изоюражений в одно
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

            ImageSeparationThreadModel result = new ImageSeparationThreadModel
            {
                BackgroundMoreThanText = sepModel.BackgroundMoreThanText,
                FloodValue = sepModel.FloodValue,
                ImagePart = image,
                Number = sepModel.Number,
            };

            _newResults.Add(result);
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