using System;
using Datalayer.Database;
using Datalayer.Database.Tables;

namespace Console
{
    public class DataInserter
    {
        private MadWorldContext _context;

        public DataInserter(MadWorldContext context)
        {
            _context = context;
        }

        public void Insert()
        {
            AddNewResume();
        }

        private void AddNewResume()
        {
            _context.Resumes.Add(new Resume {
                FullName = "Oscar Veldman",
                Created = DateTime.Now
            });

            _context.SaveChanges();
        }
    }
}
