using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MissingWhat
{
    class Program
    {
        static void Main(string[] args)
        {
            string projectPath = @"C:\Users\bbdnet1963\source\repos\TestConsole[735]\TestConsole\";


            //Console.WriteLine(  );

            IEnumerable<string> compareList = FilesFromRef(projectPath).Except(FilesFromProj(projectPath));
            foreach (var item in compareList)
            {
                string[] projectFiles = Directory.GetFiles(projectPath, "*.csproj", SearchOption.AllDirectories);
                List<string> firstList = new List<string>();

                foreach (string file in projectFiles)
                {
                    XDocument doc = XDocument.Load(file);
                    var references = doc.Descendants().Elements().Attributes().ToList();
                    try
                    {
                        foreach (var line in references)
                        {
                            string lineValues = line.Value;

                            if (lineValues.Contains(".cs") && !lineValues.Contains("AssemblyInfo") && lineValues == item)
                            {
                                Console.WriteLine(line);
                                line.Remove();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //string savePath = new DirectoryInfo(file).Name;
                    doc.Save(file);
                    //Console.WriteLine(savePath);
                }
                Console.ReadKey();
            }
            //foreach (string classFile in classFiles)
            //{
            //    string justName = new DirectoryInfo(classFile).Name;

            //    List<string> secondList = justName.Split('\n').ToList();
            //    List<string> firstList = lineValues.Split('\n').ToList();



            //    IEnumerable<string> differenceList = firstList.Except(secondList);
            //    foreach (string item in differenceList)
            //    {

            //        //if (firstList.Contains(item) == false)
            //        //{
            //        Console.WriteLine(item);
            //        //        result.Add(item.ToString());
            //        //}
            //    }
            //}

            // line.RemoveAll();

        }

        public static List<string> FilesFromProj(string projectPath)
        {
            string[] classFiles = Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories);
            List<string> listOfClass = new List<string>(classFiles);
            List<string> firstList = new List<string>();
            string justName = null;

            foreach (var displayList in listOfClass)
            {
                if (!displayList.Contains("Temporary") && !displayList.Contains("AssemblyInfo"))
                {
                    justName = new DirectoryInfo(displayList).Name;
                    firstList.Add(justName);
                }
            }
            return firstList;
        }

        public static List<string> FilesFromRef(string projectPath)
        {
            string[] projectFiles = Directory.GetFiles(projectPath, "*.csproj", SearchOption.AllDirectories);
            List<string> firstList = new List<string>();

            foreach (string file in projectFiles)
            {
                try
                {
                    XDocument doc = XDocument.Load(file);
                    var references = doc.Descendants().Elements().Attributes().ToList();
                    foreach (var line in references)
                    {
                        string lineValues = line.Value;

                        if (lineValues.Contains(".cs") && !lineValues.Contains("AssemblyInfo"))
                            firstList.Add(lineValues);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return firstList;
        }
    }
}

