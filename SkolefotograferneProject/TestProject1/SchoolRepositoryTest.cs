using RazorPageApplication.Enums;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Models;
using RazorPageApplication.Services;

namespace TestProject1;

[TestClass]
[DoNotParallelize]
public sealed class SchoolRepositoryTest
{
    private SchoolRepositoryAsync _repository;
    private readonly List<School> _createdSchool = new();

    [TestInitialize]
    public void Setup()
    {
        _repository = new SchoolRepositoryAsync();
        _createdSchool.Clear();
    }
    [TestCleanup]
    public async Task Cleanup()
    {
        foreach (School school in _createdSchool)
        {
            await _repository.DeleteAsync(school);
        }
        _createdSchool.Clear();
    }

    [TestMethod]
    public async Task CreateSchoolAsyncTest()
    {
        // Arrange
        School testSubject = new School(0, "schoolNameTest", "TestAddresse", "2625");
        _createdSchool.Add(testSubject);
        // Act
        await _repository.CreateAsync(testSubject);
        // Assert
        Assert.IsNotNull(await _repository.GetAsync(testSubject.Id));
    }

    [TestMethod]
    public async Task GetAllAsyncTest()
    {
        // Arrange
        School t1 = new School(0, "schoolNameTest", "TestAddresse", "2625");
        School t2 = new School(0, "schoolNameTest", "TestAddresse", "2625");
        await _repository.CreateAsync(t1);
        await _repository.CreateAsync(t2);
        _createdSchool.Add(t1);
        _createdSchool.Add(t2);
        // Act
        List<School> list = await _repository.GetAllAsync();
        // Assert
        Assert.IsNotNull(list);
        Assert.IsTrue(list.Any(t => t.Id == t1.Id));
        Assert.IsTrue(list.Any(t => t.Id == t2.Id));
        Assert.IsTrue(list.Count >= 2);
    }
    [TestMethod]
    public async Task GetAsyncTest()
    {
        // Arrange
        School testSubject = new School(0, "schoolNameTest", "TestAddresse", "2625");
        await _repository.CreateAsync(testSubject);
        _createdSchool.Add(testSubject);
        // Act
        School? cached = await _repository.GetAsync(testSubject.Id);
        // Assert
        Assert.IsNotNull(cached);
        Assert.AreEqual(testSubject.Name, cached.Name);
        Assert.AreEqual(testSubject.Address, cached.Address);
        Assert.AreEqual(testSubject.PostalCode, cached.PostalCode);
        Assert.AreEqual(testSubject.Id, cached.Id);
    }
    [TestMethod]
    public async Task FitlerAsyncTest()
    {
        // Arrange
        School t1 = new School(0, "schoolNameTest", "TestAddresse", "2625Test");
        School t2 = new School(0, "schoolNameTest", "TestAddresse", "2625Test");
        School t3 = new School(0, "schoolNameTest", "TestAddresse", "2625Test");
        await _repository.CreateAsync(t1);
        await _repository.CreateAsync(t2);
        await _repository.CreateAsync(t3);
        _createdSchool.Add(t1);
        _createdSchool.Add(t2);
        _createdSchool.Add(t3);
        // Act
        List<School> list = await _repository.FilterAsync("schoolNameTest");
        // Assert
        Assert.IsNotNull(list);
        Assert.IsFalse(list.Any(t => t.Id == t1.Id));
        Assert.IsTrue(list.Any(t => t.Id == t2.Id));
        Assert.IsTrue(list.Any(t => t.Id == t3.Id));
        Assert.IsTrue(list.Count >= 2);
    }
    [TestMethod]
    public async Task DeleteAsyncTest()
    {
        // Arrange
        School testSubject = new School(0, "schoolNameTest", "TestAddresse", "2625");
        await _repository.CreateAsync(testSubject);
        // Act
        await _repository.DeleteAsync(testSubject);
        School? cached = await _repository.GetAsync(testSubject.Id);
        // Assert
        Assert.IsNull(cached);
    }
}

