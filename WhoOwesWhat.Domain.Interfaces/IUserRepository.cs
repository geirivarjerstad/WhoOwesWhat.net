using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.Domain.Interfaces
{
    public interface IUserRepository
    {
        bool AuthenticateUser(string username, string password);
        Result CreateUser(Guid personGuid, string displayname, string username, string email, string mobile, string password);
    }    
    
    public interface IUserRepositoryLogic
    {
        UserCredential MapToUserCredential(Person person, string username, string email, string passwordHash);
    }

}
