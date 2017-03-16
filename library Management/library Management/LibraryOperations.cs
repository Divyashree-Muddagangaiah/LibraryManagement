using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
namespace library_Management
{
    class LibraryOperations
    {
        public String RequiredBook;
        public String BookName;
        //public varchar Name=null;
        public int BookNumber;
        public String BorrowerName;
        public String BookBorrowing;
        public Guid BookID;
        public bool bookavail = false;
        public bool Bookreturn = false;

        public void addnewBook()
        {
            using (var connection = new SqlConnection("data source=DIVYASHREE\\SQLEXPRESS;initial catalog=LibraryManagement;Integrated Security=SSPI"))
            {
                connection.Open();
                Console.WriteLine("enter New Book details to add");
                Console.WriteLine("BookName:");
                BookName = Console.ReadLine();
                Console.WriteLine("BookNumber");
                BookNumber = int.Parse(Console.ReadLine());
                var Checkquery = @" select * from [LibraryManagement].[dbo].[Books]
                                                                where BookName like'%" + BookName + "%' and [BookNumber]=" + BookNumber + "";
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = connection;
                    comm.CommandText = Checkquery;
                    SqlDataReader rdr;
                    try
                    {
                        comm.ExecuteNonQuery();
                        rdr = comm.ExecuteReader();
                        if (rdr.HasRows == true)
                        {
                            DataTable datatable = new DataTable();
                            datatable.Load(rdr);
                            foreach (DataRow item in datatable.Rows)
                            {
                                Console.WriteLine("book{0,10} and Number {1,-10}", item.Field<string>(1), item.Field<int>(2));
                                Console.WriteLine(" with this combination already available");
                                bookavail = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                if (bookavail == false)
                {
                    //connection.Close();
                    //connection.Open();
                    var query = @"INSERT INTO [LibraryManagement].[dbo].[Books]
                                                ([BookId],[BookName],[BookNumber],[Avialable])
                                                VALUES
                                                ('" + Guid.NewGuid() + @"','" + BookName + @"','" + BookNumber + @"','1')"
                                        ;

                    String additem = string.Format("{0}-{1}", BookName, BookNumber);
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = connection;
                        comm.CommandText = query;
                        try
                        {
                            comm.ExecuteNonQuery();
                            Console.WriteLine("Book added to library");
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                        connection.Close();
                    }
                }
            }
        }

        public void serach()
        {
            using (var connection = new SqlConnection("data source=DIVYASHREE\\SQLEXPRESS;initial catalog=LibraryManagement;Integrated Security=SSPI"))
            {
                Console.WriteLine("Please enter the book you are looking for");
                RequiredBook = Console.ReadLine().Replace("'", "''");
                Console.WriteLine("Searching a book");
                connection.Open();
                SqlDataReader rdr;
                var query = @"select * from [LibraryManagement].[dbo].[Books] where [BookName] like '%" + RequiredBook + "%' and [Avialable]=1 ";
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
                            foreach (DataRow item in datatable.Rows)
                            {
                                Console.WriteLine("{0,10} | {1,-20} ", item.Field<string>(1), item.Field<int>(2));
                                Console.WriteLine("book is available");
                            }
                        }

                        else
                            Console.WriteLine("No such books available in library");
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                connection.Close();
            }
        }

        public void Issue()
        {
            try
            {
                using (var connection = new SqlConnection("data source=DIVYASHREE\\SQLEXPRESS;initial catalog=LibraryManagement;Integrated Security=SSPI"))
                {
                    Console.WriteLine("please enter the borrower information");
                    Console.WriteLine("Borrower Name:");
                    BorrowerName = Console.ReadLine();
                    Console.WriteLine("BookName");
                    BookName = Console.ReadLine().Replace("'", "''");
                    connection.Open();
                    SqlDataReader rdr;
                    var query = @" select Top 1 * from [LibraryManagement].[dbo].[Books]
                                                                where BookName like'%" + BookName + "%' and [Avialable]=1 ";
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
                                foreach (DataRow item in datatable.Rows)
                                {
                                    Console.WriteLine("issued book");
                                    Console.WriteLine("{0,10} | {1,-10} |{2,-10}", item.Field<string>(1), item.Field<int>(2), BorrowerName);
                                    BookID = item.Field<Guid>(0);
                                    //Console.ReadLine();
                                }
                            }
                            else
                                Console.WriteLine("book is not avaible to Issue");
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                    connection.Close();

                    connection.Open();
                    var UpdateBook = @"update [LibraryManagement].[dbo].[Books]
                                                    set Avialable=0
                                                    where BookName like'%" + BookName + "%' and [BookId]='" + BookID + "' ";
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = connection;
                        comm.CommandText = UpdateBook;
                        try
                        {
                            comm.ExecuteNonQuery();
                            // Console.WriteLine("issued, Borrower:" + BorrowerName);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                    connection.Close();

                    connection.Open();
                    var queryBorrower = @"INSERT INTO [LibraryManagement].[dbo].[borrower]
                                                            ([borrowerId],[borrowerName],[BookName],[BookId],[Due])
                                                            VALUES
                                                            ('" + Guid.NewGuid() + @"','" + BorrowerName + @"','" + BookName + @"','" + BookID + @"','1')";

                    String additemBorrower = string.Format("{0}-{1}", BorrowerName, BookName);
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = connection;
                        comm.CommandText = queryBorrower;
                        try
                        {
                            comm.ExecuteNonQuery();
                            // Console.WriteLine("issued, Borrower:" + BorrowerName);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                // Console.WriteLine("Please enter"); 
                throw e;
            }
        }

        public void Submit()
        {

            using (var connection = new SqlConnection("data source=DIVYASHREE\\SQLEXPRESS;initial catalog=LibraryManagement;Integrated Security=SSPI"))
            {
                // Console.WriteLine("retun book to library");
                Console.WriteLine("Borrower Name:");
                BorrowerName = Console.ReadLine();
                Console.WriteLine("BookName");
                BookName = Console.ReadLine();

                SqlDataReader rdr;
                connection.Open();
                var checkbook = @"select top 1 * from [LibraryManagement].[dbo].[borrower] Br
	                                        inner join [LibraryManagement].[dbo].[Books] b on b.BookId=Br.BookId
	                                        where Br.borrowerName like '%" + BorrowerName + "%' and B.BookName like '%" + BookName + "%' and Avialable=0 and due=1";
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = connection;
                    comm.CommandText = checkbook;

                    try
                    {
                        comm.ExecuteNonQuery();
                        rdr = comm.ExecuteReader();
                        if (rdr.HasRows == true)
                        {
                            DataTable datatable = new DataTable();
                            datatable.Load(rdr);
                            foreach (DataRow item in datatable.Rows)
                            {
                                // Console.WriteLine("Book returned", item.Field<string>(1));
                                Bookreturn = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                connection.Close();
                if (Bookreturn == true)
                {
                    connection.Open();
                    var query = @" update [LibraryManagement].[dbo].[Books] set [Avialable]=1 where BookName like '%" + BookName + "%' ";
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = connection;
                        comm.CommandText = query;

                        try
                        {
                            comm.ExecuteNonQuery();
                            Console.WriteLine("Book returned");
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                    connection.Close();
                    connection.Open();
                    var queryReturn = @" update [LibraryManagement].[dbo].[borrower]
                                            set [Due]=0
                                            where borrowerName like'%" + BorrowerName + "%' ";
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = connection;
                        comm.CommandText = queryReturn;
                        try
                        {
                            comm.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                    connection.Close();
                }
                else
                    Console.WriteLine("this book is not from our library");
            }
        }
    }
}
