using System;
using System.Data.Common;
using System.Data.SqlClient;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserRepositoryLogic _userRepositoryLogic;
        private readonly IUserCredentialDataProvider _userCredentialDataProvider;
        private readonly IPersonDataProvider _personDataProvider;
        private readonly IPersonRepositoryLogic _personRepositoryLogic;
        private readonly IHashUtils _hashUtils;

        public UserRepository(IUserRepositoryLogic userRepositoryLogic, 
            IUserCredentialDataProvider userCredentialDataProvider, 
            IPersonDataProvider personDataProvider, 
            IPersonRepositoryLogic personRepositoryLogic,
            IHashUtils hashUtils)
        {
            _userRepositoryLogic = userRepositoryLogic;
            _userCredentialDataProvider = userCredentialDataProvider;
            _personDataProvider = personDataProvider;
            _personRepositoryLogic = personRepositoryLogic;
            _hashUtils = hashUtils;
        }

        public bool AuthenticateUser(string username, string password)
        {
            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNullOrEmpty(() => password, password);

            UserCredential userCredential = _userCredentialDataProvider.GetUserCredential(username);
            if (userCredential == null)
            {
                return false;
            }
            var passwordHash = _hashUtils.GetHashString(password);
            var isAuthenticated = userCredential.PasswordHash == passwordHash;
            return isAuthenticated;
        }

        /// <summary>
        /// Create new Online UserCredential. (Not Online or Offline Friend)
        /// </summary>
        public Result CreateUser(Guid personGuid, string displayname, string username, string email, string mobile, string password)
        {
            Guard.NotNull(() => personGuid, personGuid);
            Guard.NotNullOrEmpty(() => displayname, displayname);
            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNullOrEmpty(() => email, email);
            Guard.NotNullOrEmpty(() => password, password);

            Guard.IsValid(() => personGuid, personGuid, ValidatePersonGuid, "PersonGuid can not be empty");
            Guard.IsValid(() => personGuid, personGuid, _userCredentialDataProvider.IsNotInUsePersonGuid, "UserCredential is already created for this personGuid");
            Guard.IsValid(() => email, email, _userCredentialDataProvider.IsUniqueEmail, "email was not unique");
            Guard.IsValid(() => username, username, _userCredentialDataProvider.IsUniqueUsername, "username was not unique");

            var person = _personDataProvider.GetPerson(personGuid);
            if (person == null)
            {
                person = _personRepositoryLogic.MapToPerson(personGuid, displayname, mobile);
                Result resultPerson = _personDataProvider.SavePerson(person);
                if (!resultPerson.IsSuccess)
                {
                    return resultPerson;
                }                
            }

            var passwordHash = _hashUtils.GetHashString(password);
            UserCredential user = _userRepositoryLogic.MapToUserCredential(person, username, email, passwordHash);

            Result result = _userCredentialDataProvider.SaveUserCredential(user);
            return result;

        }

        private bool ValidatePersonGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }
    }



}
