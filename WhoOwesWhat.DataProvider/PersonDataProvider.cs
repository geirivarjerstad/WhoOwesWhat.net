using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using Person = WhoOwesWhat.Domain.DTO.Person;

namespace WhoOwesWhat.DataProvider
{
    public class PersonDataProvider : IPersonDataProvider
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private ILog _log;
        private readonly IPersonDataProviderLogic _personDataProviderLogic;

        public PersonDataProvider(IWhoOwesWhatContext whoOwesWhatContext, ILog log, IPersonDataProviderLogic personDataProviderLogic)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _log = log;
            _personDataProviderLogic = personDataProviderLogic;
        }

        public bool IsUniquePersonGuid(Guid personGuid)
        {
            return _whoOwesWhatContext.Persons.FirstOrDefault(a => a.PersonGuid == personGuid) == null;
        }

        public Domain.DTO.Person GetPerson(Guid personGuid)
        {
            var personDb = _whoOwesWhatContext.Persons.SingleOrDefault(a => a.PersonGuid == personGuid);
            return personDb == null ? null : _personDataProviderLogic.MapToDomain(personDb);
        }

        public Domain.DTO.Result SavePerson(Domain.DTO.Person person)
        {
            var personDb = _whoOwesWhatContext.Persons.SingleOrDefault(a => a.PersonGuid == person.PersonGuid);
            if (personDb == null)
            {
                personDb = new Entity.Person();

                _whoOwesWhatContext.Persons.Add(personDb);
            }

            _personDataProviderLogic.UpdateEntity(person, personDb);

            var result = new Result();
            result.IsSuccess = _whoOwesWhatContext.SaveChanges() > 0;

            return result;
        }
    }
}
