using System.Collections.Generic;
using WhoOwesWhat.DataProvider.Debug;
using WhoOwesWhat.Service.Controller;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Service
{
    public partial class WhoOwesWhatService : ServiceStack.ServiceInterface.Service, IWhoOwesWhatService
    {
        private readonly IUserController _userController;
        private readonly IDataProviderDebug _dataProviderDebug;

        public WhoOwesWhatService(IUserController userController, IDataProviderDebug dataProviderDebug)
        {
            _userController = userController;
            _dataProviderDebug = dataProviderDebug;
        }

        public BasicResponse Any(AuthenticateUserRequest request)
        {
            return _userController.AuthenticateUser(request);
        }

        public BasicResponse Any(CreateUserRequest request)
        {
            return _userController.CreateUser(request);
        }

    }
}