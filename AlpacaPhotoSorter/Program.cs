using MachineLearningML.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlpacaPhotoSorter.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Path to photo directory:");
            //var inputPath = Console.ReadLine();
            var inputPath = @"D:\MachineLearningData\SortData\Takeout\Google Foto_s";
            Console.WriteLine(inputPath);

            if (!Directory.Exists(inputPath))
            {
                Console.WriteLine("Directory does not exist");
            }

            var extensions = new List<string> { ".jpg", ".png" };
            string[] files = Directory.GetFiles(inputPath, "*.*", SearchOption.AllDirectories)
                .Where(f => extensions.IndexOf(Path.GetExtension(f).ToLower()) >= 0).ToArray();

            var outputPathAlpaca = @$"{inputPath}\_Output\Alpaca";
            var outputPathNoAlpaca = @$"{inputPath}\_Output\Notalpaca";

            Directory.CreateDirectory(outputPathAlpaca);
            Directory.CreateDirectory(outputPathNoAlpaca);

            Console.WriteLine($"Images: {files.Length}");
            var counter = 0;
            
            foreach (var file in files)
            {
                ModelInput sampleData = new ModelInput()
                {
                    ImageSource = file
                };
                // Make a single prediction on the sample data and print results
                var predictionResult = ConsumeModel.Predict(sampleData);

                var array = file.Split('\\');
                var name = array.Last();

                try
                {
                    File.Copy(file,
                        predictionResult.Prediction.Equals("YesAlpaca") ? @$"{outputPathAlpaca}\{name}" : @$"{outputPathNoAlpaca}\{name}", true);
                }
                catch (Exception e)
                {
                    // Continue
                }
                Console.WriteLine($"Image nummer {counter}");
                counter++;

            }
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }
    }
}
