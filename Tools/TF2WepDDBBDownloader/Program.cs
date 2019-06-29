using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TF2WepDDBBDownloader
{
    class Program
    {
        public class Attribute
        {
            public string Name { get; set; }
            public int ID { get; set; }
        }

        private static readonly string ATTRIBUTESDEFURL = "https://wiki.teamfortress.com/wiki/List_of_item_attributes";
        private static readonly string ATTRIBUTESDEFAULTPATH = Directory.GetCurrentDirectory() + "/attributes.json";

        private static readonly string ERRORJSONTEXT = "ERROR";

        static int Main(string[] args)
        {
            string outputFilePath = ATTRIBUTESDEFAULTPATH;

            for(int i = 0; i < args.Length; i++)
            {
                if(args[i] == "-save" || args[i] == "-s")
                {
                    if (i + 1 < args.Length)
                    {
                        outputFilePath = args[i + 1];

                        if (!Uri.IsWellFormedUriString(outputFilePath, UriKind.RelativeOrAbsolute))
                        {
                            Console.WriteLine("ERROR: Please, insert a valid path.");
                            return -1;
                        }
                        if (Uri.IsWellFormedUriString(outputFilePath, UriKind.Relative))
                            outputFilePath = Directory.GetCurrentDirectory() + "/" + outputFilePath;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Please, insert the output file Path.");
                        return -1;
                    }
                    i++;
                }
            }

            string outputJson = DownloadAttributesDefinition();

            if (string.IsNullOrWhiteSpace(outputJson) || outputJson == ERRORJSONTEXT)
                return -2;

            using (StreamWriter outputWriter = File.CreateText(outputFilePath))
            {
                outputWriter.Write(outputJson);
            }

            return 0;
        }

        private static string DownloadAttributesDefinition()
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(ATTRIBUTESDEFURL, "GET");

                HtmlNode tBody = htmlDoc.DocumentNode.Descendants("table").FirstOrDefault();
                if(tBody == null)
                {
                    Console.WriteLine("ERROR: table not found.");
                    return ERRORJSONTEXT;
                }
                List<Attribute> attrList = new List<Attribute>();
                foreach(HtmlNode trNode in tBody.ChildNodes)
                {
                    if (trNode.ChildNodes.Count < 2)
                        continue;

                    Attribute attr = new Attribute();
                    HtmlNode idTdNode = trNode.ChildNodes[1];
                    if (int.TryParse(idTdNode.InnerText, out int id))
                        attr.ID = id;
                    else
                        continue;

                    HtmlNode nameTdNode = trNode.ChildNodes[3];
                    attr.Name = nameTdNode.InnerText.Replace("\n", "").Trim();
                    if (string.IsNullOrWhiteSpace(attr.Name))
                        continue;

                    Console.WriteLine(string.Format("Attribute found ({0}: {1})", attr.ID, attr.Name));
                    attrList.Add(attr);
                }

                if(attrList.Count == 0)
                {
                    Console.WriteLine("ERROR: No attribute found.");
                    return ERRORJSONTEXT;
                }

                return Newtonsoft.Json.JsonConvert.SerializeObject(attrList, Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
                return ERRORJSONTEXT;
            }
        }
    }
}
