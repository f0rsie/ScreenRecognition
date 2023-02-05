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

        private Color _startPixelColor;
        private int _imageSize;

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

        // Работа с изображением
        public List<byte[]> PreparedImages(byte[] inputImage)
        {
            List<byte[]>? result = new List<byte[]>();

            try
            {
                _results = new();
                _threads = new();
                _newResults = new();

                _inputImage = inputImage;
                var bmp = ImagePreparationService.ByteToBitmap(inputImage);
                //bmp.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/defaultImage.png", ImageFormat.Png);

                _startPixelColor = bmp.GetPixel(new Random().Next(0, bmp.Width), new Random().Next(0, bmp.Height));
                _imageSize = bmp.Width * bmp.Height;

                List<ImageSeparationThreadModel> models = new();

                int countParts = inputImage.Length / 200000;
                if (countParts <= 0)
                    countParts = 1;
                else if (inputImage.Length / 200000.0 > countParts)
                    countParts++;

                var imageParts = ImageSeparation(bmp, countParts);

                int currentThreadsNumber = 0;

                for (int i = 0; i < imageParts.Count; i++)
                {
                    var model = new ImageSeparationThreadModel(imageParts[i], i, "default");
                    models.Add(model);
                    _threads.Add(new Thread(Prepare));
                    _threads[currentThreadsNumber].Start(model);

                    currentThreadsNumber++;
                }

                while (true)
                {
                    if (_threads.Where(e => e.ThreadState == ThreadState.Running).Count() == 0)
                        break;
                }

                result = WholeImage(countParts);
            }
            catch { }

            return result;
        }

        // ImageSeparation через ImageWrapper
        private List<byte[]> ImageSeparation(Bitmap bmp, int countParts)
        {
            List<byte[]>? result = new List<byte[]>();

            try
            {
                _countParts = countParts;

                int currentWidth = bmp.Width / _countParts;
                int currentHeight = bmp.Height;

                int lastWidth = 0;
                for (int count = 0; count < countParts; count++)
                {
                    var resultBitmap = bmp.Clone(new Rectangle(lastWidth, 0, currentWidth - lastWidth, currentHeight), bmp.PixelFormat);

                    lastWidth = currentWidth;

                    currentWidth += bmp.Width / _countParts;

                    var element = ImagePreparationService.BitmapToByte(resultBitmap);

                    //resultBitmap.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/testImage{count}.png", ImageFormat.Png);

                    result.Add(element);
                }
            }
            catch { }

            return result;
        }

        private List<byte[]> WholeImage(int countParts)
        {
            try
            {
                int i = 0;
                var res = new List<List<Bitmap>>();
                var result = new List<Bitmap>();
                var r = _newResults.OrderBy(e => e.Number).ToList();

                List<Bitmap> currentBitmap = new();

                foreach (var value in r)
                {
                    currentBitmap.Add(ImagePreparationService.ByteToBitmap(value.ImagePart));
                }

                result.AddRange(currentBitmap);
                res.Add(currentBitmap);
                result = new List<Bitmap>();

                foreach (var item in res)
                {
                    Bitmap? resElement = null;

                    if (item.Count >= countParts)
                    {
                        resElement = Draw(item.GetRange(0, countParts), item[0].Width, item[0].Height);
                    }
                    else
                    {
                        resElement = Draw(item.GetRange(0, item.Count), item[0].Width, item[0].Height);
                    }

                    //resElement.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/convertedImage{i++}.png", ImageFormat.Png);
                    _results.Add(ImagePreparationService.BitmapToByte(resElement));
                }
            }
            catch { }

            return _results;
        }

        // Склейка нескольких изоюражений в одно
        public Bitmap? Draw(List<Bitmap> images, int width, int height)
        {
            try
            {
                int num = 0;
                int ListLength = images.Count * width;

                Bitmap bitmap = new Bitmap(ListLength, height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    foreach (var img in images)
                    {
                        Image tmp = img;
                        g.DrawImageUnscaled(tmp, width * num, 0);
                        num++;
                    }
                }

                return bitmap;

            }
            catch { }

            return null;
        }

        // Обработка одной части изображения
        private void Prepare(object? model)
        {
            try
            {
                var sepModel = model as ImageSeparationThreadModel;

                var bmp = ImagePreparationService.ByteToBitmap(sepModel.ImagePart);
                var image = _imagePreparationService.GetPreparedImage(bmp, Color.White, Color.Black, _startPixelColor, _imageSize);

                ImageSeparationThreadModel result = new ImageSeparationThreadModel
                {
                    ImagePart = image,
                    Number = sepModel.Number,
                    Type = "default",
                };

                _newResults.Add(result);
            }
            catch { }
        }
    }
}