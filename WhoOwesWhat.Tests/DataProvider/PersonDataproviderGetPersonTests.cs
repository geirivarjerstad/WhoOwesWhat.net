using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
    public class PersonDataproviderGetPersonTests
    {
        private static MoqMockingKernel _kernel;

        public PersonDataproviderGetPersonTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IPersonDataProvider>().To<PersonDataProvider>();
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

            //var mockContext = new Mock<WhoOwesWhatContext>();
            var contextMock = _kernel.GetMock<IWhoOwesWhatContext>();
            contextMock.Setup(m => m.UserCredentials).Returns(userCredentialAsQueryableSet.Object);
            contextMock.Setup(m => m.Persons).Returns(personsAsQueryableSet.Object);
        }

        [Test]
        public void When_getting_a_existing_person__it_should_succeed()
        {
            var dataProvider = _kernel.Get<IPersonDataProvider>();

            var personRepositoryTestMock = _kernel.GetMock<IPersonDataProviderLogic>();
            personRepositoryTestMock.Setup(mock => mock.MapToDomain(It.IsAny<DataProvider.Entity.Person>())).Returns(new Domain.DTO.Person());

            var result = dataProvider.GetPerson(new Guid("8B1B52A5-18A7-45CE-940C-8181B8EBC9E4"));
            result.Should().NotBeNull();
            personRepositoryTestMock.VerifyAll();
        }             
        
        [Test]
        public void When_getting_a_momexisting_person__it_should_return_null()
        {
            var dataProvider = _kernel.Get<IPersonDataProvider>();

            var personRepositoryTestMock = _kernel.GetMock<IPersonDataProviderLogic>();

            var result = dataProvider.GetPerson(new Guid("FC16C853-94F8-42CB-929F-58770C38B71E"));
            result.Should().BeNull();
            
            personRepositoryTestMock.VerifyAll();
        }          
        
        
    }
}

