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
using UserCredential = WhoOwesWhat.Domain.DTO.UserCredential;

namespace WhoOwesWhat.DataProvider
{
    public class UserCredentialDataProvider : IUserCredentialDataProvider
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private readonly IUserCredentialDataProviderLogic _userCredentialDataProviderLogic;
        private readonly IUserCredentialContext _userCredentialContext;
        private readonly IPersonContext _personContext;

        public UserCredentialDataProvider(IWhoOwesWhatContext whoOwesWhatContext, IUserCredentialDataProviderLogic userCredentialDataProviderLogic, IUserCredentialContext userCredentialContext, IPersonContext personContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _userCredentialDataProviderLogic = userCredentialDataProviderLogic;
            _userCredentialContext = userCredentialContext;
            _personContext = personContext;
        }

        public List<UserCredential> GetUserCredentials()
        {
            var result = _whoOwesWhatContext.UserCredentials.ToList().Select(a => _userCredentialDataProviderLogic.MapToDomain(a)).ToList();
            return result;
        }

        public UserCredential GetUserCredential(string username)
        {
            var userCredential = _userCredentialContext.GetUserCredential(username);
            if (userCredential == null)
            {
                return null;
            }

            userCredential.Person = _personContext.GetPerson(userCredential.PersonId);
            return _userCredentialDataProviderLogic.MapToDomain(userCredential);
        }

        public Result SaveUserCredential(Domain.DTO.UserCredential userCredential)
        {
            var userDb = _whoOwesWhatContext.UserCredentials.SingleOrDefault(a => a.Username == userCredential.Username);
            if (userDb == null)
            {
                userDb = new Entity.UserCredential();
                
                var personDb = _whoOwesWhatContext.Persons.SingleOrDefault(a => a.PersonGuid == userCredential.Person.PersonGuid);
                if (personDb == null)
                {
                    throw new UserCredentialDataProviderException("PersonGuid not found. Can not add UserCredential.");
                }
                userDb.Person = personDb;

                _whoOwesWhatContext.UserCredentials.Add(userDb);
            }

            _userCredentialDataProviderLogic.UpdateEntity(userCredential, userDb);

            var result = new Result();
            result.IsSuccess = _whoOwesWhatContext.SaveChanges() > 0;

            return result;
        }

        public bool IsUniqueEmail(string email)
        {
            return _whoOwesWhatContext.UserCredentials.FirstOrDefault(a => a.Email == email) == null;
        }

        public bool IsUniqueUsername(string username)
        {
            return _whoOwesWhatContext.UserCredentials.FirstOrDefault(a => a.Username == username) == null;
        }

        public bool IsNotInUsePersonGuid(Guid personGuid)
        {
            return _whoOwesWhatContext.UserCredentials.All(a => a.Person.PersonGuid != personGuid);
        }
    }

    public class UserCredentialDataProviderException : Exception
    {
        public UserCredentialDataProviderException(string message)
            : base(message)
        {
        }
    }
}
