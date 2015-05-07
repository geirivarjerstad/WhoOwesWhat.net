using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IUserCredentialDataProvider
    {
        List<UserCredential> GetUserCredentials();
        UserCredential GetUserCredential(string username);
        Result SaveUserCredential(UserCredential userCredential);
        bool IsUniqueEmail(string email);
        bool IsUniqueUsername(string username);
        bool IsNotInUsePersonGuid(Guid personGuid);
    }
}
