using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider
{
    public interface IUserCredentialDataProviderLogic
    {
        void UpdateEntity(Domain.DTO.UserCredential source, Entity.UserCredential target);
        UserCredential MapToDomain(Entity.UserCredential source);
    }

    public class UserCredentialDataProviderLogic : IUserCredentialDataProviderLogic
    {
        private readonly IPersonDataProviderLogic _personDataProviderLogic;

        public UserCredentialDataProviderLogic(IPersonDataProviderLogic personDataProviderLogic)
        {
            _personDataProviderLogic = personDataProviderLogic;
        }

        public void UpdateEntity(Domain.DTO.UserCredential source, Entity.UserCredential target)
        {

            Guard.NotNull(() => source, source);
            Guard.NotNull(() => target, target); 
            
            target.Email = source.Email;
            target.Username = source.Username;
            target.PasswordHash = source.PasswordHash;
            _personDataProviderLogic.UpdateEntity(source.Person, target.Person);

        }

        public UserCredential MapToDomain(Entity.UserCredential source)
        {
            var targetDomain = new Domain.DTO.UserCredential();
            targetDomain.Email = source.Email;
            targetDomain.Username = source.Username;
            targetDomain.PasswordHash = source.PasswordHash;
            targetDomain.Person = _personDataProviderLogic.MapToDomain(source.Person);
            return targetDomain;
        }
    }
}
