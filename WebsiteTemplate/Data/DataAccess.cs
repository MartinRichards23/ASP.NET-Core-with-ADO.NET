

namespace WebsiteTemplate.Data
{
    public class DataAccess
    {
        public DataAccess(Database database)
        {
            Database = database;
        }

        public Database Database { get; }
    }
}
