using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Human human = new Human("viet anh nguyen", 12, "pr");
            human.Add(1);
            human.Add(2);
            human.Add(3);
            human.Add(4);
            human.Add(5);
            string dpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(dpath, "va1.xml");
            DTO tmp = new DTO(human);
            XmlSerializer xmlSerializer = new XmlSerializer(tmp.GetType());
            using(StreamWriter writer = new StreamWriter(path))
            {
                xmlSerializer.Serialize(writer, tmp);
            }
            Console.WriteLine(File.ReadAllText(path));
            Console.WriteLine();
            DTO newtmp = null;
            using(StreamReader fs=new StreamReader(path))
            {
                newtmp = xmlSerializer.Deserialize(fs) as DTO;
            }
            Human newhuman = new Human(newtmp.Name, newtmp.Age, newtmp.Job);
            for(int i = 0; i < human.Days.Length; i++)
            {
                newhuman.Add(human.Days[i]);
            }
            newhuman.Print();
            Console.WriteLine();
            Console.WriteLine(Check(human,newhuman));
        }
        public static bool Check(Human obj1, Human obj2)
        {
            if (obj1.Name != obj2.Name) return false;
            if (obj1.Age != obj2.Age) return false;
            if (obj1.Job != obj2.Job) return false;
            if (obj1.Days.Length != obj2.Days.Length) return false;
            for(int i = 0; i < obj1.Days.Length; i++)
            {
                if (obj1.Days[i] != obj2.Days[i]) return false;
            }
            return true;
        }
    }
    public class Human
    {
        private int[] _days;
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Job { get; private set; }
        public int[] Days
        {
            get
            {
                if (_days == null) return null;
                int[] days = new int[_days.Length];
                for(int i = 0; i < days.Length; i++)
                {
                    days[i] = _days[i];
                }
                return days;
            }
        }
        public Human(string name, int age, string job)
        {
            Name = name;
            Age = age;
            Job = job;
            _days = new int[0];
        }
        public void Add(int day)
        {
            Array.Resize(ref _days, _days.Length + 1);
            _days[_days.Length - 1] = day;
        }
        public void Print()
        {
            Console.WriteLine($"Name: {Name}{Environment.NewLine}" +
                $"Age: {Age}{Environment.NewLine}" +
                $"Job: {Job}{Environment.NewLine}" +
                $"Days: {string.Join(" ",Days)}");
        }
    }
    public class DTO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Job { get; set; }
        public int[] Days { get; set; }
        public DTO(string name, int age, string job, int[] days)
        {
            Name = name;
            Age = age;
            Job = job;
            Days = days;
        }
        public DTO()
        {
            Name = null;
            Age = default(int);
            Job = null;
            Days = null;
        }
        public DTO(Human obj)
        {
            Name = obj.Name;
            Age = obj.Age;
            Job = obj.Job;
            Days = obj.Days;
        }
    }
}
