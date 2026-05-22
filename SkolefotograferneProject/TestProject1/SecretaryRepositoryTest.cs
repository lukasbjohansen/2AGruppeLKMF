//using Moq;
//using RazorPageApplication.Interfaces;
//using RazorPageApplication.Models;
//using RazorPageApplication.Services;
//using Xunit;

//namespace TestProject1;

//[TestClass]
//[DoNotParallelize]
//public sealed class SecretaryRepositoryTest
//{
//    private SecretaryRepositoryAsync _repository;
//    private readonly Mock<IRepositoryAsync<School>> _schools;
//    private readonly List<Secretary> _createdSecretary = new(); //Used for cleanup to avoid filling the databse with test values

//    public SecretaryRepositoryTest()
//    {
//        _schools = new Mock<IRepositoryAsync<School>>();
//        _repository = new SecretaryRepositoryAsync(_schools.Object);
//        _createdSecretary = new List<Secretary>();
//    }
//    //Mock-data initiasation set-up
//    [Fact]
//    public async Task GetAsync_ReturnsSecretary_WithResolveSchool()
//    {
//        School school = new School(1, "TestName", "addresseTest", "2625");
//        _schools.Setup(r => r.GetAsync(1)).ReturnsAsync(school);
//    }
//    [TestInitialize] //automatically called before each test
//    public void Setup()
//    {
//        _createdSecretary.Clear();
//    }
//    [TestCleanup]
//    public async Task Cleanup()
//    {
//        foreach (Secretary secretary in _createdSecretary)
//        {
//            await _repository.DeleteAsync(secretary);
//        }
//        _createdSecretary.Clear();
//    }

//    [TestMethod]
//    public async Task CreateSecretaryAsyncTest()
//    {
//        // Arrange
//        Secretary testSubject = new Secretary(2, "mail", "Password", "Name", "22345678", await _schools.Object.GetAsync(1));
//        _createdSecretary.Add(testSubject);
//        // Act
//        await _repository.CreateAsync(testSubject);
//        // Assert
//        Assert.IsNotNull(await _repository.GetAsync(testSubject.Id));
//    }
//}
