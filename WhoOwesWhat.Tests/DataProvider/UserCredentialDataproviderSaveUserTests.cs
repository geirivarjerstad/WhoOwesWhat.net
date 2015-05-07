using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using FluentAssertions;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.Interfaces;

namespace WhoOwesWhat.Tests
{

    [TestFixture]
    public class UserCredentialDataproviderSaveUserTests
    {
        private static MoqMockingKernel _kernel;
        private Mock<IWhoOwesWhatContext> _contextMock;

        public UserCredentialDataproviderSaveUserTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IUserCredentialDataProvider>().To<UserCredentialDataProvider>();
        }

        
        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();

            var person1 = new Person()
            {
                PersonId = 1,
                PersonGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7"),
                Displayname = "Geir1"
            };

            var person2 = new Person()
            {
                PersonId = 2,
                PersonGuid = new Guid("8B1B52A5-18A7-45CE-940C-8181B8EBC9E4"),
                Displayname = "Geir2"
            };

            var person3 = new Person()
            {
                PersonId = 3,
                PersonGuid = new Guid("8035804C-496C-4BAD-A568-098FA53C3416"),
                Displayname = "Geir3"
            };


            var persons = new List<Person>();
            persons.Add(person1);
            persons.Add(person2);
            persons.Add(person3);

            IQueryable<Person> personsAsQueryable = persons.AsQueryable();
            Mock<DbSet<Person>> personsAsQueryableSet = new Mock<DbSet<Person>>();
            personsAsQueryableSet.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(personsAsQueryable.Provider);
            personsAsQueryableSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(personsAsQueryable.Expression);
            personsAsQueryableSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(personsAsQueryable.ElementType);
            personsAsQueryableSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(personsAsQueryable.GetEnumerator());


            IQueryable<UserCredential> userCredentialAsQueryable = new List<UserCredential> 
            { 
                new UserCredential { PersonId = 1, Username = "BBB", Email = "asd1@hotmail.com", PasswordHash = "asdasdasdasdasd", Person = person1}, 
                new UserCredential { PersonId = 2, Username = "FFF", Email = "asd2@hotmail.com", PasswordHash = "asdasdasdasdasd", Person = person2}, 
                new UserCredential { PersonId = 3, Username = "XXX", Email = "asd3@hotmail.com", PasswordHash = "asdasdasdasdasd", Person = person3}, 
            }.AsQueryable();

            Mock<DbSet<UserCredential>> userCredentialAsQueryableSet = new Mock<DbSet<UserCredential>>();
            userCredentialAsQueryableSet.As<IQueryable<UserCredential>>().Setup(m => m.Provider).Returns(userCredentialAsQueryable.Provider);
            userCredentialAsQueryableSet.As<IQueryable<UserCredential>>().Setup(m => m.Expression).Returns(userCredentialAsQueryable.Expression);
            userCredentialAsQueryableSet.As<IQueryable<UserCredential>>().Setup(m => m.ElementType).Returns(userCredentialAsQueryable.ElementType);
            userCredentialAsQueryableSet.As<IQueryable<UserCredential>>().Setup(m => m.GetEnumerator()).Returns(userCredentialAsQueryable.GetEnumerator());

            _contextMock = _kernel.GetMock<IWhoOwesWhatContext>();
            _contextMock.Setup(m => m.UserCredentials).Returns(userCredentialAsQueryableSet.Object);
            //_contextMock.SetupProperty(m => m.UserCredentials, userCredentialAsQueryableSet.Object);
            _contextMock.Setup(m => m.Persons).Returns(personsAsQueryableSet.Object);
        }
        
        [Test]
        public void When_saving_an_existing_User__it_should_succeed()
        {
            Guid personGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7");
            const string displayname = "Garg1337";
            const string username = "BBB"; // existing
            const string email = "test@test.com";
            const string mobile = "80015000";
            const string password = "asdasd";
            const string passwordHash = "A8F5F167F44F4964E6C998DEE827110C"; // asdasd

            var person = new Domain.DTO.Person()
            {
                PersonGuid = personGuid,
                Displayname = displayname,
                Mobile = mobile,
                IsDeleted = false,
            };

            var userDomain = new Domain.DTO.UserCredential()
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash,
                Person = person
            };

            _contextMock.Setup(m => m.SaveChanges()).Returns(1); // true, one row affected

            var logicMock = _kernel.GetMock<IUserCredentialDataProviderLogic>();
            logicMock.Setup(m => m.UpdateEntity(It.IsAny<Domain.DTO.UserCredential>(), It.IsAny<UserCredential>()));

            var dataProvider = _kernel.Get<IUserCredentialDataProvider>();

            var result = dataProvider.SaveUserCredential(userDomain);

            result.IsSuccess.Should().BeTrue();
            _contextMock.Verify(m => m.UserCredentials);
            logicMock.VerifyAll();

        }                
        
        
        [Test]
        public void When_creating_new_User_with_an_existing_Person__it_should_succeed()
        {
            Guid personGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7");

            const string username = "test@test.com";
            const string email = "test@test.com";
            const string password = "asdasd";
            const string passwordHash = "A8F5F167F44F4964E6C998DEE827110C"; // asdasd

            var person = new Domain.DTO.Person()
            {
                PersonGuid = personGuid,
            };

            var userDomain = new Domain.DTO.UserCredential()
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash,
                Person = person
            };

            // Callback == spyOn
            //UserCredential actual;
            //_contextMock.Setup(a => a.UserCredentials.Add(It.IsAny<UserCredential>())).Callback<UserCredential>(x => actual = x);
            _contextMock.Setup(m => m.SaveChanges()).Returns(1); // one row affected

            var logicMock = _kernel.GetMock<IUserCredentialDataProviderLogic>();
            logicMock.Setup(m => m.UpdateEntity(It.IsAny<Domain.DTO.UserCredential>(), It.IsAny<UserCredential>()));

            var dataProvider = _kernel.Get<IUserCredentialDataProvider>();

            var result = dataProvider.SaveUserCredential(userDomain);

            result.IsSuccess.Should().BeTrue();

            //_contextMock.Verify(a => a.UserCredentials.Add(It.IsAny<UserCredential>()), Times.Once);
            // expect(UserCredentials.Add).toBeCalledWith(user);
        }


        [Test]
        public void When_creating_new_User_with_new_Person__it_should_fail() // because it should have been created before adding a Credential
        {
            Guid personGuid = new Guid("4987921B-5B15-45E9-8177-119927A32AC9");
            const string displayname = "JollyGood";
            const string username = "goof@troop.com";
            const string email = "goof@troop.com";
            const string mobile = "55555555";
            const string password = "asdasd";
            const string passwordHash = "A8F5F167F44F4964E6C998DEE827110C"; // asdasd

            var person = new Domain.DTO.Person()
            {
                PersonGuid = personGuid,
                Displayname = displayname,
                Mobile = mobile,
                IsDeleted = false,
            };

            var userDomain = new Domain.DTO.UserCredential()
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash,
                Person = person
            };

            var contextMock = _kernel.GetMock<IWhoOwesWhatContext>();
            contextMock.Setup(m => m.SaveChanges()).Returns(1); // true, one row affected

            var logicMock = _kernel.GetMock<IUserCredentialDataProviderLogic>();
            logicMock.Setup(m => m.UpdateEntity(It.IsAny<Domain.DTO.UserCredential>(), It.IsAny<UserCredential>()));

            var dataProvider = _kernel.Get<IUserCredentialDataProvider>();

            Action a = () =>
            {
                var result = dataProvider.SaveUserCredential(userDomain);
            };

            a.ShouldThrow<UserCredentialDataProviderException>();
        }    

    }
}

