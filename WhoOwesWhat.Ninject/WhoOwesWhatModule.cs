//using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Collections.Generic;
using Funq;
using log4net;
using log4net.Config;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using ServiceStack.Configuration;
using WhoOwesWhat.DataProvider;
using WhoOwesWhat.DataProvider.Debug;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Service.Controller;

namespace WhoOwesWhat.Ninject
{
    public class WhoOwesWhatModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWhoOwesWhatContext>().To<WhoOwesWhatContext>().InRequestScope();

            Bind<IUserController>().To<UserController>().InRequestScope();

            Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            Bind<IUserRepositoryLogic>().To<UserRepositoryLogic>().InRequestScope();
            Bind<IPersonRepositoryLogic>().To<PersonRepositoryLogic>().InRequestScope();

            Bind<IUserCredentialContext>().To<UserCredentialContext>().InRequestScope();
            Bind<IUserCredentialDataProvider>().To<UserCredentialDataProvider>().InRequestScope();
            Bind<IUserCredentialDataProviderLogic>().To<UserCredentialDataProviderLogic>().InRequestScope();
            Bind<IPersonRepository>().To<PersonRepository>().InRequestScope();
            Bind<IPersonContext>().To<PersonContext>().InRequestScope();
            Bind<IPersonDataProvider>().To<PersonDataProvider>().InRequestScope();
            Bind<IPersonDataProviderLogic>().To<PersonDataProviderLogic>().InRequestScope();

            Bind<IHashUtils>().To<HashUtils>().InRequestScope();

            Bind<IDataProviderDebug>().To<DataProviderDebug>().InRequestScope();
            
            Bind<ILog>().ToMethod(context => LogManager.GetLogger(context.Request.Target.Member.ReflectedType));

        }
    }

    // TODO: Fungerer ikke på Entity Framework
    //public static class FunqModule
    //{
    //    public static void Load(Container container)
    //    {
    //        container.RegisterAutoWiredAs<WhatToBuyContext, WhatToBuyContext>().ReusedWithin(ReuseScope.Request);
    //        container.RegisterAutoWiredAs<HandlelisteDataRepository, IHandlelisteDataRepository>().ReusedWithin(ReuseScope.Request);
    //        container.RegisterAutoWiredAs<RequestResponseRepository, IRequestResponseRepository>().ReusedWithin(ReuseScope.Request);
    //    }
    //}

    public static class NinjectKernel
    {

        public static void Load(Container container)
        {
            var kernel = CreateKernel();
            container.Adapter = new NinjectIocAdapter(kernel);
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                var config = XmlConfigurator.Configure();

                var modules = new List<INinjectModule>
                {
                    new WhoOwesWhatModule()
                };
                kernel.Load(modules);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }
    }

    public class NinjectIocAdapter : IContainerAdapter
    {
        private readonly IKernel kernel;

        public NinjectIocAdapter(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T Resolve<T>()
        {
            return this.kernel.Get<T>();
        }

        public T TryResolve<T>()
        {
            return this.kernel.TryGet<T>();
        }
    }
}
