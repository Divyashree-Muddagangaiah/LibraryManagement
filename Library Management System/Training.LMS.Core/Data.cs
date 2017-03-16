using System;
using System.Configuration;
using System.Data;
using Training.LMS.Core.Model;

namespace Training.LMS.Core
{
    class Data
    {
        private Db db;

        private Data()
        {
            db = new Db(ConnectionString);
        }

        private static Data instance;

        internal int Issue(int bookId, int memberId)
        {
            // Check if the specified book is in stock.
            String sql = "SELECT Top 1(Id) FROM [dbo].[Catalog] WHERE [bookId] = " + bookId + " AND [Available] = 'True'";
            int catalogId = db.ExecuteScalar<int>(sql);

            // No book available. Done!!!
            if (catalogId == 0)
                return -1;

            // Have it in register.
            sql = "INSERT INTO [Register] ([CatalogId], [MemberId],  [IssueDate]) OUTPUT INSERTED.IssueId VALUES (" +
                catalogId + "," +
                memberId + "," +
                "'" + DateTime.Now.Date + "')";
            int issueId = db.ExecuteScalar<int>(sql);

            // Failed to add to register. Done!!!
            if (issueId == 0)
                return issueId;

            // All steps success. Change book status to 'unavailable'
            sql = "UPDATE [dbo].[Catalog] SET [Available] = 'False' WHERE [Id] = " + catalogId;
            db.ExecuteNonQuery(sql);

            return issueId;
        }

        internal int Return(int issueId)
        {
            // Check if the specified book is in stock.
            String sql = "UPDATE [Register] SET [ReturnedDate] = '" + DateTime.Now + "' " +
                         "OUTPUT INSERTED.* WHERE [IssueId] = " + issueId;
            DataTable dt= db.GetDT(sql);

            // Not found. Done!!!
            if (dt.Rows.Count == 0)
                return -1;

            Register entry = ToRegister(dt.Rows[0]);
            
            // Change book's status back to 'Available'
            sql = "UPDATE [dbo].[Catalog] SET [Available] = 'True' WHERE [Id] = " + entry.CatalogId;

            db.ExecuteNonQuery(sql);

            return entry.FineAmount;
        }

        private Register ToRegister(DataRow row)
        {
            Register reg = new Register()
            {
                IssueId = Convert.ToInt32(row["IssueId"]),
                CatalogId = Convert.ToInt32(row["CatalogId"]),
                MemberId = Convert.ToInt32(row["MemberId"]),
                IssueDate = Convert.ToDateTime(row["IssueDate"]),
                ReturnedDate = Convert.ToDateTime(row["ReturnedDate"]),
            };

            int lateBy = (int)(reg.ReturnedDate - reg.IssueDate).TotalDays - 15;
            reg.FineAmount = lateBy >  0 ? lateBy * 2 : 0;

            return reg;
        }

        public static Data Instance
        {
            get
            {
                if (instance == null)
                    instance = new Data();
                return instance;
            }
        }

        internal Books GetBooks(string title)
        {
            String sql = "SELECT Book.Id AS Id, [Title], [Author], Count([Available]) AS InStock " +
                         "FROM [LMS].[dbo].[Book], [dbo].[Catalog] " +
                         "GROUP BY [BookId], Book.Id, [Title], [Author], [Available] " +
                         " HAVING Book.Id = Catalog.BookId AND Available = 'True' AND Title LIKE '%" + title + "%'";

            DataTable dt = db.GetDT(sql);
            return ToBooks(dt);
        }

        private Books ToBooks(DataTable dt)
        {
            Books books = new Books();

            foreach (DataRow row in dt.Rows)
            {
                books.Add(new Book()
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Title = Convert.ToString(row["Title"]),
                    Author = Convert.ToString(row["Author"]),
                    Available = Convert.ToInt32(row["InStock"]) > 0 ? "Yes" : "No"
                });
            }

            return books;
        }

        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["LMSConnection"].ConnectionString; }
        }
    }
}
