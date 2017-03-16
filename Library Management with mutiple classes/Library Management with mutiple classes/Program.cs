using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_with_mutiple_classes
{
    public class Book
    {
        public string BookName { get; set; }
        //public string Author { get; set; }
    }

    class Library
    {
        static void Main(string[] args)
        {
            
            Book book = new Book();
            List<String> Books = new List<String>();
            {
                Books.Add("C# Programming Guide");
                Books.Add("Desert Solitaire");
                Books.Add("Disgrace");
                Books.Add("Gilead");
                Books.Add("Giovanni's Room");
            };
            int length = Books.Count();
            for (int i = 0; i < length; i++)
            {
                Console.WriteLine(Books[i]);
            }
            Console.ReadLine();
        }
    }
}
