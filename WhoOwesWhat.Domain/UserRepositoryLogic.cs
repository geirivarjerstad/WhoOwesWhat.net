using System;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain
{
    public class UserRepositoryLogic : IUserRepositoryLogic
    {

        public UserCredential MapToUserCredential(Person person, string username, string email, string passwordHash)
        {
            var user = new Domain.DTO.UserCredential()
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash,
                Person = person
            };

            return user;
        }

    }
}
