using Training.LMS.Core.Model;

namespace Training.LMS.Core
{
    public class Business
    {
        public Books Search(string title)
        {
            return Data.Instance.GetBooks(title);
        }

        public int Issue(int bookId, int memberId)
        {
            return Data.Instance.Issue(bookId, memberId);
        }

        public int Return(int issueId)
        {
            return Data.Instance.Return(issueId);
        }
    }
}
