using System;
using FluentAssertions;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Service.Controller;
using WhoOwesWhat.Service.DTO;


namespace WhoOwesWhat.Tests
{

    [TestFixture]
    public class UserRepositoryCreateUserTests
    {
        private static MoqMockingKernel _kernel;

        public UserRepositoryCreateUserTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IUserRepository>().To<UserRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();
        }

        [Test]
        public void When_creating_a_user__it_should_invalidate_an_empty_PersonGuid()
        {
            Guid personGuid = Guid.Empty;
            const string displayname = "Garg1337";
            const string username = "test@test.com";
            const string email = "test@test.com";
            const string mobile = "80015000";
            const string password = "myPassword";

            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserCredentialDataProvider>();
            userCredentialRepositoryMock.Setup(mock => mock.IsUniqueEmail(It.IsAny<string>())).Returns(true);
            userCredentialRepositoryMock.Setup(mock => mock.IsUniqueUsername(It.IsAny<string>())).Returns(true);

            var personRepositoryMock = _kernel.GetMock<IPersonDataProvider>();
            personRepositoryMock.Setup(mock => mock.IsUniquePersonGuid(It.IsAny<Guid>())).Returns(true);

            var userRepository = _kernel.Get<IUserRepository>(); // this will inject the mocked IBar into our normal MyFoo implementation

            Action a = () =>
            {
                var result = userRepository.CreateUser(personGuid, displayname, username, email, mobile, password);
            };

            a.ShouldThrow<ArgumentException>();

            // expect(_personRepository.IsUniquePersonGuid).tobecalledwith(user.PersonGuid);
        }

        [Test]
        public void When_creating_an_user_with_new_user__it_should_validate_successfully()
        {
            Guid personGuid = new Guid("E113B60A-3A4B-4074-978C-7CF29FF18352");
            const string displayname = "Garg1337";
            const string username = "test@test.com";
            const string email = "test@test.com";
            const string mobile = "80015000";
            const string password = "asdasd";
            const string passwordHash = "A8F5F167F44F4964E6C998DEE827110C"; // asdasd

            var person = new Domain.DTO.Person()
            {
                PersonGuid = personGuid,
                Displayname = displayname,
                Mobile = mobile,
                IsDeleted = false
            };

            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserCredentialDataProvider>();
            userCredentialRepositoryMock.Setup(mock => mock.IsUniqueEmail(email)).Returns(true);
            userCredentialRepositoryMock.Setup(mock => mock.IsUniqueUsername(username)).Returns(true);
            userCredentialRepositoryMock.Setup(mock => mock.IsNotInUsePersonGuid(personGuid)).Returns(true);

            var personDataProviderMock = _kernel.GetMock<IPersonDataProvider>();
            personDataProviderMock.Setup(mock => mock.GetPerson(personGuid)).Returns(null as Person);
            personDataProviderMock.Setup(mock => mock.SavePerson(person)).Returns(new Result(){IsSuccess = true});

            var personRepositoryTestMock = _kernel.GetMock<IPersonRepositoryLogic>();
            personRepositoryTestMock.Setup(mock => mock.MapToPerson(personGuid, displayname, mobile)).Returns(person);

            var hashUtilsMock = _kernel.GetMock<IHashUtils>();
            hashUtilsMock.Setup(mock => mock.GetHashString(password)).Returns(passwordHash); 

            var userRepositoryTestMock = _kernel.GetMock<IUserRepositoryLogic>();
            var userRepositoryTestMockUserCredential = new UserCredential();
            userRepositoryTestMock.Setup(mock => mock.MapToUserCredential(person, username, email, passwordHash)).Returns(userRepositoryTestMockUserCredential);

            var userDataProviderMock = _kernel.GetMock<IUserCredentialDataProvider>();
            userDataProviderMock.Setup(mock => mock.SaveUserCredential(userRepositoryTestMockUserCredential)).Returns(new Result() { IsSuccess = true });

            var userRepository = _kernel.Get<IUserRepository>(); // this will inject the mocked IBar into our normal MyFoo implementation

            var result = userRepository.CreateUser(personGuid, displayname, username, email, mobile, password);

            result.IsSuccess.Should().BeTrue();

            userCredentialRepositoryMock.VerifyAll();
            personDataProviderMock.VerifyAll();
            hashUtilsMock.VerifyAll();
            userRepositoryTestMock.VerifyAll();
            personRepositoryTestMock.VerifyAll();
        }

        [Test]
        public void When_creating_an_user_with_existing_user__it_should_succeed()
        {
            Guid personGuid = new Guid("E113B60A-3A4B-4074-978C-7CF29FF18352");
            const string displayname = "Garg1337";
            const string username = "test@test.com";
            const string email = "test@test.com";
            const string mobile = "80015000";
            const string password = "asdasd";
            const string passwordHash = "A8F5F167F44F4964E6C998DEE827110C"; // asdasd

            var personDb = new DataProvider.Entity.Person()
            {
                PersonId = 1337,
                PersonGuid = personGuid,
                Displayname = displayname
            };

            var person = new Domain.DTO.Person()
            {
                PersonGuid = personGuid,
                Displayname = displayname,
                Mobile = mobile
            };

            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserCredentialDataProvider>();
            userCredentialRepositoryMock.Setup(mock => mock.IsUniqueEmail(email)).Returns(true);
            userCredentialRepositoryMock.Setup(mock => mock.IsUniqueUsername(username)).Returns(true);
            userCredentialRepositoryMock.Setup(mock => mock.IsNotInUsePersonGuid(personGuid)).Returns(true);

            var personDataProviderMock = _kernel.GetMock<IPersonDataProvider>();
            personDataProviderMock.Setup(mock => mock.GetPerson(personGuid)).Returns(person);

            var hashUtilsMock = _kernel.GetMock<IHashUtils>();
            hashUtilsMock.Setup(mock => mock.GetHashString(password)).Returns(passwordHash);

            var userRepositoryTestMock = _kernel.GetMock<IUserRepositoryLogic>();
            userRepositoryTestMock.Setup(mock => mock.MapToUserCredential(person, username, email, passwordHash)).Returns(new UserCredential());

            var userDataProviderMock = _kernel.GetMock<IUserCredentialDataProvider>();
            userDataProviderMock.Setup(mock => mock.SaveUserCredential(It.IsAny<UserCredential>())).Returns(new Result() { IsSuccess = true });

            var userRepository = _kernel.Get<IUserRepository>(); // this will inject the mocked IBar into our normal MyFoo implementation

            var result = userRepository.CreateUser(personGuid, displayname, username, email, mobile, password);

            result.IsSuccess.Should().BeTrue();

            userCredentialRepositoryMock.VerifyAll();
            personDataProviderMock.VerifyAll();
            hashUtilsMock.VerifyAll();
            userRepositoryTestMock.VerifyAll();
 
        }

    }
}

