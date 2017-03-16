using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_Management
{
    public class Library
    {
        public static void Main(string[] args)
        {
            bool showMenu = true;
            int Choice;
            LibraryOperations LOp = new LibraryOperations();
            using (var connection = new SqlConnection("data source=DIVYASHREE\\SQLEXPRESS;initial catalog=LibraryManagement;Integrated Security=SSPI"))
            {
                connection.Open();
                SqlDataReader rdr;
                String query = @"SELECT [BookName],[BookNumber] FROM [LibraryManagement].[dbo].[Books] where [Avialable]=1";
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = connection;
                    comm.CommandText = query;

                    try
                    {
                        comm.ExecuteNonQuery();
                        rdr = comm.ExecuteReader();
                        if (rdr.HasRows == true)
                        {
                            DataTable datatable = new DataTable();
                            datatable.Load(rdr);
                            Console.WriteLine("list of books in library");
                            foreach (DataRow item in datatable.Rows)
                            {
                                Console.WriteLine(item.Field<string>(0), ": ", item.Field<int>(1));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                connection.Close();
            }

            while (showMenu)
            {
                Console.WriteLine("\n1.add new book\n2.Search\n3.issue\n4.submit\n5.exit\n please enter your choice");
                try
                {
                    Choice = int.Parse(Console.ReadLine());

                    switch (Choice)
                    {

                        case 1: LOp.addnewBook();
                            break;

                        case 2: LOp.serach();
                            break;

                        case 3: LOp.Issue();
                            break;

                        case 4: LOp.Submit();
                            break;

                        case 5: showMenu = false;
                            break;

                        default: Console.WriteLine("Please enter valid input");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("please enter integer between 1-5");
                }
            }
        }
    }
}
