using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson01
{
    internal class FileManager<T>
    {
        static string path = Directory.GetCurrentDirectory() + $"\\{typeof(T)}Data";
        public static void Save(T file)
        {

            using FileStream fs = new(path, FileMode.OpenOrCreate);
                using StreamWriter sw = new(fs);
            
            string json = JsonConvert.SerializeObject(file);

            sw.WriteLine(json);
        }
        public static T Load()
        {
            string json ="";
            try
            {
                using FileStream fs = new(path, FileMode.Open);
                using StreamReader sr = new(fs);

                json = sr.ReadLine();
            }
            catch(Exception e)
            {
                Console.WriteLine("File not exist , file be create");
            }

            T file = JsonConvert.DeserializeObject<T>(json);

            return file;
        }
    }
}
