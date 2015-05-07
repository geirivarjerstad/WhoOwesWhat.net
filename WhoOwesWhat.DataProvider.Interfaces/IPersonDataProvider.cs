using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IPersonDataProvider
    {
        bool IsUniquePersonGuid(Guid personGuid);
        Domain.DTO.Person GetPerson(Guid personGuid);
        Result SavePerson(Person person);
    }

}
