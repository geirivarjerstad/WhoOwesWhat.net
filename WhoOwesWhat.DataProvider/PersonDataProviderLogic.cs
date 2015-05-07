using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using WhoOwesWhat.DataProvider.Interfaces;

namespace WhoOwesWhat.DataProvider
{
    public interface IPersonDataProviderLogic
    {
        void UpdateEntity(Domain.DTO.Person source, Entity.Person target);
        Domain.DTO.Person MapToDomain(DataProvider.Entity.Person source);
    }

    public class PersonDataProviderLogic : IPersonDataProviderLogic
    {

        public void UpdateEntity(Domain.DTO.Person source, Entity.Person target)
        {
            Guard.NotNull(() => source, source);
            Guard.NotNull(() => target, target);
            //target.PersonId = source.PersonId; Skal ikke endre på Id ved Update
            target.PersonGuid = source.PersonGuid;
            target.Displayname = source.Displayname;
            target.Mobile = source.Mobile;
            target.IsDeleted = source.IsDeleted;
        }


        public Domain.DTO.Person MapToDomain(DataProvider.Entity.Person source)
        {
            Guard.NotNull(() => source, source);

            var targetDomain = new Domain.DTO.Person();
            targetDomain.PersonGuid = source.PersonGuid;
            targetDomain.Displayname = source.Displayname;
            targetDomain.Mobile = source.Mobile;
            targetDomain.IsDeleted = source.IsDeleted;
            return targetDomain;
        }
    }
}
