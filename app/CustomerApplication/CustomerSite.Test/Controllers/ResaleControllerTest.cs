using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using AutoMapper;
using BobCustomerSite.Controllers;
using DataAccess.Data;
using DataAccess.Repository.Interface;
using DataAccess.Repository.Interface.InventoryInterface;
using DataAccess.Repository.Interface.SearchImplementations;
using DataModels.Books;
using DataModels.Carts;
using DataModels.Customers;
using CustomerSite.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CustomerSite.Test.Controllers
{
    public class ResaleControllerTest
    {
        private readonly Mock<IGenericRepository<ResaleStatus>> mockResaleStatusRepository = new Mock<IGenericRepository<ResaleStatus>>();
        private readonly Mock<IGenericRepository<Resale>> mockResaleRepository = new Mock<IGenericRepository<Resale>>();
        private readonly Mock<IGenericRepository<Customer>> mockCustomerRepository = new Mock<IGenericRepository<Customer>>();
        private readonly Mock<IInventory> mockInventory = new Mock<IInventory>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();

       
       

        private List<Resale> GetSampleTestData()
        {
            var sampleData = new List<Resale>
            {
                new Resale
                    {
                        Resale_Id = 29, Author = "Test Author", ISBN = "122365543", BookName = "Test Book", GenreName = "Test Genre",  Customer = new Customer(), ConditionName = "Test Condition"
                }
                    };

            return sampleData;
        }
        [Fact]
        public async void TestIndexView()
        {
            var userManagerMock = new Mock<UserManager<CognitoUser>>(
    /* IUserStore<TUser> store */Mock.Of<IUserStore<CognitoUser>>(),
    /* IOptions<IdentityOptions> optionsAccessor */null,
    /* IPasswordHasher<TUser> passwordHasher */null,
    /* IEnumerable<IUserValidator<TUser>> userValidators */null,
    /* IEnumerable<IPasswordValidator<TUser>> passwordValidators */null,
    /* ILookupNormalizer keyNormalizer */null,
    /* IdentityErrorDescriber errors */null,
    /* IServiceProvider services */null,
    /* ILogger<UserManager<TUser>> logger */null);

            var signInManagerMock = new Mock<SignInManager<CognitoUser>>(
                userManagerMock.Object,
                /* IHttpContextAccessor contextAccessor */Mock.Of<IHttpContextAccessor>(),
                /* IUserClaimsPrincipalFactory<TUser> claimsFactory */Mock.Of<IUserClaimsPrincipalFactory<CognitoUser>>(),
                /* IOptions<IdentityOptions> optionsAccessor */null,
                /* ILogger<SignInManager<TUser>> logger */null,
                /* IAuthenticationSchemeProvider schemes */null,
                /* IUserConfirmation<TUser> confirmation */null);
            var controller = new ResaleController(mockMapper.Object, mockInventory.Object, mockResaleStatusRepository.Object, mockResaleRepository.Object, mockCustomerRepository.Object, signInManagerMock.Object, userManagerMock.Object);

            var signinResult = SignInResult.Success;
           
            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Resale>(
        viewResult.ViewData.Model);
        }
            }

        }
    

