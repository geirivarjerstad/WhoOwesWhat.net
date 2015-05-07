using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Service.Controller
{
    public interface IUserController
    {
        BasicResponse AuthenticateUser(AuthenticateUserRequest request);
        BasicResponse CreateUser(CreateUserRequest createUserRequest);
    }

    public class UserController : IUserController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public BasicResponse AuthenticateUser(AuthenticateUserRequest request)
        {
            var isAuthenticated = _userRepository.AuthenticateUser(request.username, request.password);

            return new BasicResponse()
            {
                isSuccess = isAuthenticated
            };
        }

        public BasicResponse CreateUser(CreateUserRequest request)
        {
            var result = _userRepository.CreateUser(request.personGuid, request.displayname, request.username, request.email, request.mobil, request.password);
            return new BasicResponse() { isSuccess = result.IsSuccess };
        }
    }
    
}