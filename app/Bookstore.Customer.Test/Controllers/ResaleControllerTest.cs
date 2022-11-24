using Amazon.Extensions.CognitoAuthentication;
using AutoMapper;
using CustomerSite.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Bookstore.Domain.Customers;
using Bookstore.Data.Repository.Interface;
using Bookstore.Domain.Offers;

namespace CustomerSite.Test.Controllers
{
    public class ResaleControllerTest
    {
        private readonly Mock<IGenericRepository<Offer>> mockResaleRepository = new Mock<IGenericRepository<Offer>>();
        private readonly Mock<IGenericRepository<Customer>> mockCustomerRepository = new Mock<IGenericRepository<Customer>>();
        private readonly Mock<IInventory> mockInventory = new Mock<IInventory>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();

       
       

        private List<Offer> GetSampleTestData()
        {
            var sampleData = new List<Offer>
            {
                new Offer
                    {
                        Id = 29, Author = "Test Author", ISBN = "122365543", BookName = "Test Book", GenreName = "Test Genre",  Customer = new Customer(), ConditionName = "Test Condition"
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
            //var controller = new ResaleController(mockMapper.Object, mockInventory.Object, mockResaleStatusRepository.Object, mockResaleRepository.Object, mockCustomerRepository.Object, signInManagerMock.Object, userManagerMock.Object);

            //var signinResult = SignInResult.Success;
           
            //var result = await controller.Index();
            //var viewResult = Assert.IsType<ViewResult>(result);
            //var model = Assert.IsType<Resale>(viewResult.ViewData.Model);
        }
            }

        }
    

