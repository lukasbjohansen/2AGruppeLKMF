using RazorPageApplication.Enums;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Models;
using RazorPageApplication.Services;

namespace TestProject1;

[TestClass]
[DoNotParallelize]
public sealed class ParentRepositoryTest
{
    private ParentRepositoryAsync _repository;
    private readonly List<Parent> _createdParents = new(); //Used for cleanup to avoid filling the database with test values

    [TestInitialize]
    //Automatically called before each test
    public void Setup()
    {
        _repository = new ParentRepositoryAsync();
        _createdParents.Clear();
    }

    [TestCleanup]
    //Loops through all the added values and deletes them
    public async Task Cleanup()
    {
        foreach (Parent parent in _createdParents)
        {
            await _repository.DeleteAsync(parent);
        }
        _createdParents.Clear();
    }

    [TestMethod]
    public async Task CreateAsyncTest()
    {
        // Arrange
        Parent testSubject = new Parent(0, "Hans@hotmail.com", "123", "Hans", "22345678", "Roskildevej 2", "4000");
        _createdParents.Add(testSubject);
        // Act
        await _repository.CreateAsync(testSubject);
        // Assert
        Assert.IsNotNull(await _repository.GetAsync(testSubject.Id));
    }

    [TestMethod]
    public async Task GetAllAsyncTest()
    {
        // Arrange
        Parent p1 = new Parent(0, "Hans@hotmail.com", "123", "Hans", "22345678", "Roskildevej 2", "4000");
        Parent p2 = new Parent(0, "Jens@hotmail.com", "123", "Jens", "22334455", "Roskildevej 5", "8000");
        await _repository.CreateAsync(p1);
        await _repository.CreateAsync(p2);
        _createdParents.Add(p1);
        _createdParents.Add(p2);
        // Act
        List<Parent> list = await _repository.GetAllAsync();
        // Assert
        Assert.IsNotNull(list);
        Assert.IsTrue(list.Any(p => p.Id == p1.Id));
        Assert.IsTrue(list.Any(p => p.Id == p2.Id));
        Assert.IsTrue(list.Count >= 2);
    }

    [TestMethod]
    public async Task GetAsyncTest()
    {
        // Arrange
        Parent testSubject = new Parent(0, "Hans@hotmail.com", "123", "Hans", "22345678", "Roskildevej 2", "4000");
        await _repository.CreateAsync(testSubject);
        _createdParents.Add(testSubject);
        // Act
        Parent? cached = await _repository.GetAsync(testSubject.Id);
        // Assert
        Assert.IsNotNull(cached);
        Assert.AreEqual(testSubject.Name, cached.Name);
        Assert.AreEqual(testSubject.Mail, cached.Mail);
        Assert.AreEqual(testSubject.Password, cached.Password);
        Assert.AreEqual(testSubject.PhoneNumber, cached.PhoneNumber);
        Assert.AreEqual(testSubject.Address, cached.Address);
        Assert.AreEqual(testSubject.PostalCode, cached.PostalCode);
        Assert.AreEqual(testSubject.Id, cached.Id);
    }

    [TestMethod]
    public async Task FilterAsyncTest()
    {
        // Arrange
        Parent p1 = new Parent(0, "Hans@hotmail.com", "123", "Hans", "22345678", "Roskildevej 2", "4000");
        Parent p2 = new Parent(0, "Jens@hotmail.com", "123", "Jens", "22334455", "Hellerupvej 2", "2900");
        Parent p3 = new Parent(0, "Poul@hotmail.com", "123", "Poul", "11223344", "Glostrupvej 2", "2600");
        await _repository.CreateAsync(p1);
        await _repository.CreateAsync(p2);
        await _repository.CreateAsync(p3);
        _createdParents.Add(p1);
        _createdParents.Add(p2);
        _createdParents.Add(p3);
        // Act
        List<Parent> list = await _repository.FilterAsync("peter");
        // Assert
        Assert.IsNotNull(list);
        Assert.IsFalse(list.Any(p => p.Id == p1.Id));
        Assert.IsTrue(list.Any(p => p.Id == p2.Id));
        Assert.IsTrue(list.Any(p => p.Id == p3.Id));
        Assert.IsTrue(list.Count >= 2);
    }
}