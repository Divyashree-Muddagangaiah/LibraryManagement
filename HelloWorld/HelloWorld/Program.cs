using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    class CommandLine
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Hello, World!");
            Console.WriteLine("you have entered following {0} arguments:", args.Length);
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("{0}", args[i]);
                Console.WriteLine('a');
            }
            Console.ReadLine();
        }
    }
}
