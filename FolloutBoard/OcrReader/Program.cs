using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\repos\FolloutBoard\FolloutBoard\Assets\вещи, перки, влияние, герои\vlijanie\";
            var directories = Directory.GetDirectories(path);

            var cards = new List<Card>();
            foreach (var dirc in directories)
            {
                foreach (var filename in Directory.GetFiles(dirc))
                {
                    var ocrtext = string.Empty;
                    using (var engine = new TesseractEngine(@"D:\repos\FolloutBoard\Utils\OCr\tessdata-3.04.00\", "rus", EngineMode.TesseractAndLstm))
                    {
                        using (var img = PixConverter.ToPix(new Bitmap(filename)))
                        {
                            using (var page = engine.Process(img))
                            {
                                ocrtext = page.GetText();
                            }
                        }
                    }
                    var lines = ocrtext.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

                    Card card = new Card();
                    card.Class = "influence";
                    card.FileName = filename;
                    card.Score = new DirectoryInfo(filename).Parent.Name;
                    card.Lines = lines;
                    cards.Add(card);

                }

            }
            XmlSerializer serialiser = new XmlSerializer(typeof(List<Card>));

            // Create the TextWriter for the serialiser to use
            TextWriter filestream = new StreamWriter(path + "data.xml");

            //write to the file
            serialiser.Serialize(filestream, cards);

            // Close the file
            filestream.Close();

        }
    }
}
