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

namespace WhoOwesWhat.DataProvider
{
    public interface IUserCredentialContext
    {
        List<Entity.UserCredential> GetUserCredentials();
        Entity.UserCredential GetUserCredential(string username);
    }

    public class UserCredentialContext : IUserCredentialContext
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;

        public UserCredentialContext(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public List<Entity.UserCredential> GetUserCredentials()
        {
            var result = _whoOwesWhatContext.UserCredentials.ToList();
            return result;
        }

        public Entity.UserCredential GetUserCredential(string username)
        {
            var result = _whoOwesWhatContext.UserCredentials.SingleOrDefault(a => a.Username == username);
            return result;
        }
    }


}
