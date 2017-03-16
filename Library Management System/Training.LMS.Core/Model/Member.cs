using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.LMS.Core.Model
{
    class Member
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<int> BooksTaken { get; set; }
    }
}
