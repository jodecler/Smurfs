using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SmurfApp
{
    class Program
    {
        private static int version = 2;
        private static string outputPath = "C:/Users/sigfr/Desktop/Output";

        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now);
            Console.WriteLine("Welcome to SmurfVillage. Papa smurf will greet you. Let me find him for you");

            // Read directory
            var fileDirectory = Directory.GetFiles("Pictures");

            if (version == 1)
            {
                RunVersion1(fileDirectory);
            }
            else 
            {
                RunVersion2(fileDirectory);
            }
            
            Console.Write(DateTime.Now);

            Console.Read();
        }

        private static void RunVersion1(string[] fileDirectory)
        {

            foreach (var filePath in fileDirectory)
            {
                var image = Image.FromFile(filePath);
                var bitmap = new Bitmap(image);

                ConvertRedColorToPurpleColor(bitmap);

                SavePicture(filePath, bitmap);
            }
            Console.WriteLine("");
            Console.WriteLine("I found Papa smurf.");
            Console.WriteLine("Gargamel has put a potion over Papa. His clothes are now purple!");
        }
        private static void ConvertRedColorToPurpleColor(Bitmap bitmap)
        {
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color pixelColor = bitmap.GetPixel(j, i);
                    if (ColorCloseToRed(pixelColor))
                    {
                        if (ColorCloseToSmurfBlue(bitmap.GetPixel(j, i)))
                        {
                            bitmap.SetPixel(j, i, Color.Purple);
                        }
                    }
                }
            }
        }

        private static void SavePicture(string filePath, Bitmap bitmap)
        {
            var fp = filePath.Replace("Pictures", outputPath);
            bitmap.Save(fp, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private static void RunVersion2(string[] fileDirectory)
        {
            foreach (var filePath in fileDirectory)
            {
                var image = Image.FromFile(filePath);
                var bitmap = new Bitmap(image);

                var redUpperY = -1;
                var redLowerY = -1;
                var width = -1;
                var timesRedBlueWhiteBlueRed = 0;

                for (int j = 0; j < bitmap.Width; j++)
                {
                    var red = false;
                    var redBlue = false;
                    var redBlueWhite = false;
                    var redBlueWhiteBlue = false;
                    var redBlueWhiteBlueRed = false;
                    redUpperY = -1;
                    redLowerY = -1;
                    for (int i = 0; i < bitmap.Height; i++)
                    {

                        var colorPixel = bitmap.GetPixel(j, i);
                        if (ColorCloseToRed(colorPixel) && redUpperY == -1)
                        {
                            red = true;
                            redUpperY = i;
                        }

                        if (red && ColorCloseToSmurfBlue(colorPixel)) // && redUpperY + 20 < i)
                        {
                            redBlue = true;
                        }

                        if (redBlue && ColorCloseToWhite(colorPixel)) //&& redUpperY + 40 < i)
                        {
                            redBlueWhite = true;
                        }

                        if (redBlueWhite && ColorCloseToSmurfBlue(colorPixel)) //&& redUpperY + 60 < i)
                        {
                            redBlueWhiteBlue = true;
                        }

                        if (redBlueWhiteBlue && ColorCloseToRed(colorPixel)) //)&& redUpperY + 80 < i)
                        {
                            redBlueWhiteBlueRed = true;
                            redLowerY = i;
                            timesRedBlueWhiteBlueRed += 1;
                            break;
                        }
                    }

                    if (redBlueWhiteBlueRed && timesRedBlueWhiteBlueRed > 30)
                    {
                        if (!GreenInbetweenReds(bitmap, redUpperY, redLowerY, j))
                        {
                            width = j;
                            break;
                        }
                    }
                }
                
                PrintImage(filePath, bitmap, width, redUpperY, redLowerY);
            }

            Console.WriteLine("");
            Console.WriteLine("I found Papa smurf. He is marked with a black square.");
        }

        private static bool GreenInbetweenReds(Bitmap bitmap, int redUpperY, int redLowerY, int width)
        {
            for (int i = redUpperY; i < redLowerY; i++)
            {
                var pixel = bitmap.GetPixel(width, i);
                if (pixel.R > 160 && pixel.R < 200 && pixel.G > 180 && pixel.B > 55)
                {
                    return true;
                }
            }
            return false;
        }

        private static void PrintImage(string filePath, Bitmap bitmap, int width, int redUpperY, int redLowerY)
        {
            var maxWidth = bitmap.Width;
            var maxHeight = bitmap.Height;


            var y = width - (maxWidth * 5 / 100) > 0 ? width - (maxWidth * 5 / 100) : 0;
            var a = width + (maxWidth * 10 / 100) < bitmap.Width ? width + (maxWidth * 10 / 100) : bitmap.Width;
            var x = redUpperY - (maxHeight * 5 / 100) > 0 ? redUpperY - (maxHeight * 5 / 100) : 0;
            var z = redLowerY + (maxHeight * 10 / 100) < bitmap.Height ? redLowerY + (maxHeight * 10 / 100) : bitmap.Height;

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    if (y < j && j < a && (i == x || i == z))
                    {
                        bitmap.SetPixel(j, i, Color.Black);
                    }

                    if (x < i && i < z && (j == y || j == a))
                    {
                        bitmap.SetPixel(j, i, Color.Black);
                    }
                }
            }

            var fp = filePath.Replace("Pictures", "C:/Users/sigfr/Desktop/Output");
            bitmap.Save(fp, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        
        private static bool ColorCloseToRed(Color pixelColor)
        {
            if (pixelColor.R > 155 && pixelColor.G < 50 && pixelColor.B < 50)
            {
                return true;
            }

            return false;
        }

        private static bool ColorCloseToSmurfBlue(Color pixelColor)
        {
            if (pixelColor.R < 150 && pixelColor.G > 150 && pixelColor.G < 200 && pixelColor.B > 180)
            {
                return true;
            }
            return false;
        }

        private static bool ColorCloseToWhite(Color pixelColor)
        {
            if (pixelColor.R > 230 && pixelColor.B > 230 && pixelColor.G > 230)
            {
                return true;
            }

            return false;
        }
    }
}
