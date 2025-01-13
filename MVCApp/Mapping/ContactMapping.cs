using MVCApp.Models;
using TinyCsvParser.Mapping;

namespace MVCApp.Mapping
{
    public class ContactMapping: CsvMapping<Contact>
    {
        public ContactMapping(): base() { 
            MapProperty(0, x => x.FirstName);
            MapProperty(1, x => x.LastName);
            MapProperty(2, x => x.Address);
            MapProperty(3, x => x.PhoneNumber);
        }
    }
}
