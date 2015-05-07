// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace WhoOwesWhat.Service.DTO
{
    [Route("/authenticateUser")]
    public class AuthenticateUserRequest
    {
        // username or email. Todo: Email confirmation on account creation
        public string username { get; set; }
        public string password { get; set; }
    }

    [Route("/user/new")]
    public class CreateUserRequest
    {
        public Guid personGuid { get; set; }
        public string displayname { get; set; }
        public string email { get; set; }
        public string mobil { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }    
    
    [Route("/debug/resetdatabase")]
    public class ResetDatabaseRequest
    {
    }

    public class BasicResponse
    {
        public bool isSuccess { get; set; }
    }
}

// ReSharper restore InconsistentNaming
