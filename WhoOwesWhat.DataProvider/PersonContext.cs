using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.Interfaces;

namespace WhoOwesWhat.DataProvider
{
    public interface IPersonContext
    {
        Entity.Person GetPerson(int personId);
    }

    public class PersonContext : IPersonContext
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;

        public PersonContext(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public Person GetPerson(int personId)
        {
            return _whoOwesWhatContext.Persons.SingleOrDefault(a => a.PersonId == personId);
        }
    }
}
