using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.LMS.Core;
using Training.LMS.Core.Model;

namespace Nalashaa.Training.LMS
{
    static class Menu
    {
        static Business instance;
        static Business Instance
        {
            get
            {
                if (instance == null)
                    instance = new Business();
                return instance;
            }
        }

        public static void Show()
        {
            Console.WriteLine("Library Management System");
            Console.WriteLine("*************************");

            do
            {
                Console.WriteLine("1. Search");
                Console.WriteLine("2. Issue");
                Console.WriteLine("3. Return");
                Console.WriteLine("4. Exit");
                Console.WriteLine("Enter your option");

                int option = Convert.ToInt32(Console.ReadLine());

                ProcessOption(option);
                Console.WriteLine("--------------------------------------------------------------------------");
            } while (true);
        }

        private static void ProcessOption(int option)
        {
         switch(option)
            {
                case 1: DoSearch(); break;
                case 2: IssueBook(); break;
                case 3: ProcessReturn(); break;
                case 4: Environment.Exit(0); break;
                default:
                    Console.WriteLine("Enter a valid option");
                    break;
            }
        }
        
        private static void DoSearch()
        {
            Console.WriteLine("Enter book title (or a part)");
            string title = Console.ReadLine();
            Books books = Instance.Search(title);
            Console.WriteLine("BookId" + "\t\t"+ "Title" + "\t\t" + "Author" + "\t\t" + "Available");
            foreach (Book book in books)
            {
                Console.WriteLine(book.Id + "\t\t" + book.Title + "\t" + book.Author + "\t" + book.Available);
            }
        }

        private static void IssueBook()
        {
            int bookId;
            if (ReadNumericInput("book id", out bookId) == false)
                return;

            int memberId;
            if (ReadNumericInput("member id", out memberId) == false)
                return;

            int issueId = Instance.Issue(bookId, memberId);
            if (issueId == -1)
                Console.WriteLine("We are sorry. This book is not available right now.");
            else if (issueId == 0)
                Console.WriteLine("We are sorry. Could not complete issue process. Please try after some time.");
            else
                Console.WriteLine("Issued book with issue id " + issueId + ". Please note it down as would be asked when return.");
        }

        private static void ProcessReturn()
        {
            int issueId;
            if (ReadNumericInput("issue id", out issueId) == false)
                return;

            int result = Instance.Return(issueId);
            if (result == -1)
                Console.WriteLine("We are sorry. Could not complete return process. Please try after some time.");
            else if (result == 0)
                Console.WriteLine("Return completed.");
            else
                Console.WriteLine("Return completed. Please collect a fine amount of " + result + " rupees calculated for the delay.");
        }

        private static bool ReadNumericInput(string key, out int value)
        {
            Console.WriteLine("Enter " + key + ".");

            if (int.TryParse(Console.ReadLine(), out value))
                return true;
            else
            {
                Console.WriteLine("Please enter a valid " + key + ".");
                return false;
            }
        }
    }
}
