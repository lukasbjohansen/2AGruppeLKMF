using RazorPageApplication.Models;
using RazorPageApplication.Services;

namespace TestProject1;

[TestClass]
[DoNotParallelize]
public class PhotographerRepositoryTest
{
    private readonly PhotographerRepositoryAsync _repo;
    private readonly List<Photographer> _createdPhotographers = new();

    public PhotographerRepositoryTest()
    {
        _repo = new PhotographerRepositoryAsync();
    }

    [TestInitialize]
    public void Setup()
    {
        _createdPhotographers.Clear();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        foreach (Photographer p in _createdPhotographers)
        {
            await _repo.DeleteAsync(p);
        }
        _createdPhotographers.Clear();
    }

    [TestMethod]
    public async Task TestCreate()
    {
        // Arrange
        Photographer testSubject = new(0, "pede", "testphotographer@mail.com", "4321", "44556677", "010101");
        // Act
        await _repo.CreateAsync(testSubject);
        _createdPhotographers.Add(testSubject);
        // Assert
        Assert.IsNotNull(await _repo.GetAsync(testSubject.Id));
    }

    [TestMethod]
    public async Task TestDelete()
    {
        // Arrange
        Photographer testSubject = new(0, "mark", "mark@marksen.dk", "1111", "43218888", "02020202");
        await _repo.CreateAsync(testSubject);
        _createdPhotographers.Add(testSubject);
        // Act
        await _repo.DeleteAsync(testSubject);
        Photographer deleted = await _repo.GetAsync(testSubject.Id);
        // Assert
        Assert.IsNull(deleted);
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        // Arrange
        Photographer originalPhotographer = new(0, "hans", "update@email.com", "1234", "22334455", "010203");
        Photographer newPhotographer = new(0, "bob", "update@gmail.com", "12345", "22334456", "020304");
        await _repo.CreateAsync(originalPhotographer);
        newPhotographer.Id = originalPhotographer.Id;
        _createdPhotographers.Add(originalPhotographer);
        // Act
        await _repo.UpdateAsync(newPhotographer);
        Photographer? updatedPhotographer = await _repo.GetAsync(originalPhotographer.Id);
        // Assert
        Assert.IsNotNull(updatedPhotographer);
        Assert.AreEqual(originalPhotographer.Id, newPhotographer.Id);
        Assert.AreEqual(newPhotographer.Name, updatedPhotographer.Name);
        Assert.AreEqual(newPhotographer.Mail, updatedPhotographer.Mail);
        Assert.AreEqual(newPhotographer.Password, updatedPhotographer.Password);
        Assert.AreEqual(newPhotographer.PhoneNumber, updatedPhotographer.PhoneNumber);
        Assert.AreEqual(newPhotographer.Id, updatedPhotographer.Id);
        Assert.AreNotEqual(originalPhotographer.Name, updatedPhotographer.Name);
        Assert.AreNotEqual(originalPhotographer.Mail, updatedPhotographer.Mail);
        Assert.AreNotEqual(originalPhotographer.Password, updatedPhotographer.Password);
        Assert.AreNotEqual(originalPhotographer.PhoneNumber, updatedPhotographer.PhoneNumber);
    }

    [TestMethod]
    public async Task TestGet()
    {
        // Arrange
        Photographer photographer = new(0, "per", "per@persen.dk", "12345", "22334455", "020304");
        await _repo.CreateAsync(photographer);
        // Act
        Photographer retrievedPhotographer = await _repo.GetAsync(photographer.Id);
        // Assert
        Assert.IsNotNull(retrievedPhotographer);
    }

    [TestMethod]
    public async Task TestGetAll()
    {
        // Arrange
        Photographer p1 = new(0, "poul", "poul@poulsen.dk", "123456", "22554455", "020304");
        Photographer p2 = new(0, "didrik", "didrik@didriksen.dk", "123457", "22664455", "020308");
        await _repo.CreateAsync(p1);
        await _repo.CreateAsync(p2);
        _createdPhotographers.Add(p1);
        _createdPhotographers.Add(p2);
        // Act
        List<Photographer> photographers = await _repo.GetAllAsync();
        // Assert
        Assert.IsNotNull(photographers);
        Assert.IsTrue(photographers.Any(t => t.Id == p1.Id));
        Assert.IsTrue(photographers.Any(t => t.Id == p2.Id));
        Assert.IsTrue(photographers.Count >= 2);
    }

    [TestMethod]
    public async Task TestFilter()
    {
        // Arrange
        Photographer p1 = new(0, "henrik", "henrik@henriksen.dk", "123451", "33554455", "020304");
        Photographer p2 = new(0, "mark", "mark@marksen.dk", "123459", "44664455", "020308");
        Photographer p3 = new(0, "marko", "marko@markosen.dk", "123460", "44664460", "020310");
        _createdPhotographers.Add(p1);
        _createdPhotographers.Add(p2);
        _createdPhotographers.Add(p3);
        await _repo.CreateAsync(p1);
        await _repo.CreateAsync(p2);
        await _repo.CreateAsync(p3);
        // Act
        List<Photographer> filtered = await _repo.FilterAsync("mark");
        // Assert
        Assert.IsTrue(filtered.Count >= 2);
    }
}
