using System;

namespace Training.LMS.Core.Model
{
    class Register
    {
        public int IssueId { get; set; }

        public int CatalogId { get; set; }

        public int MemberId { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ReturnedDate { get; set; }

        public int FineAmount { get; set; }
    }
}
