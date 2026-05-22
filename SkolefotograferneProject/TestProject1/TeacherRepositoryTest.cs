using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using RazorPageApplication.Enums;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Models;
using RazorPageApplication.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    [DoNotParallelize]
    public sealed class TeacherRepositoryTest
    {
        private TeacherRepositoryAsync _repository;
        private readonly List<Teacher> _createdTeachers = new();

        [TestInitialize]
        public void Setup()
        {
            _repository = new TeacherRepositoryAsync();
            _createdTeachers.Clear();
        }
        [TestCleanup]
        public async Task Cleanup()
        {
            foreach (Teacher teacher in _createdTeachers)
            {
                await _repository.DeleteAsync(teacher);
            }
            _createdTeachers.Clear();
        }
        [TestMethod]
        public async Task CreateAsyncTest()
        {
            // Arrange
            Teacher testSubject = new Teacher(0, "testc@email.com", "1234", "morten", "22334455");
            _createdTeachers.Add(testSubject);
            // Act
            await _repository.CreateAsync(testSubject);
            // Assert
            Assert.IsNotNull(await _repository.GetAsync(testSubject.Id));
        }
        [TestMethod]
        public async Task CreateAsyncInvalidPhoneTest()
        {
            // Arrange
            Teacher testSubject = new Teacher(0, "testc@email.com", "1234", "morten", "12334455");
            // Act & Assert
            await Assert.ThrowsExceptionAsync<RepositoryException>(
                () => _repository.CreateAsync(testSubject));
        }
        [TestMethod]
        public async Task GetAllAsyncTest()
        {
            // Arrange
            Teacher t1 = new Teacher(0, "getall1@email.com", "1234", "morten", "22334455");
            Teacher t2 = new Teacher(0, "getall2@email.com", "1234", "anne", "22334466");
            await _repository.CreateAsync(t1);
            await _repository.CreateAsync(t2);
            _createdTeachers.Add(t1);
            _createdTeachers.Add(t2);
            // Act
            List<Teacher> list = await _repository.GetAllAsync();
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
            Teacher testSubject = new Teacher(0, "get@email.com", "1234", "morten", "22334455");
            await _repository.CreateAsync(testSubject);
            _createdTeachers.Add(testSubject);
            // Act
            Teacher? cached = await _repository.GetAsync(testSubject.Id);
            // Assert
            Assert.IsNotNull(cached);
            Assert.AreEqual(testSubject.Name, cached.Name);
            Assert.AreEqual(testSubject.Mail, cached.Mail);
            Assert.AreEqual(testSubject.Password, cached.Password);
            Assert.AreEqual(testSubject.PhoneNumber, cached.PhoneNumber);
            Assert.AreEqual(testSubject.Id, cached.Id);
        }
        [TestMethod]
        public async Task GetAsyncNotExistingTest()
        {
            // Act
            Teacher? cached = await _repository.GetAsync(-1);
            // Assert
            Assert.IsNull(cached);
        }
        [TestMethod]
        public async Task FitlerAsyncTest()
        {
            // Arrange
            Teacher t1 = new Teacher(0, "filter1@email.com", "1234", "morten", "22334455");
            Teacher t2 = new Teacher(0, "filter2@email.com", "1234", "Jenspeter", "22334466");
            Teacher t3 = new Teacher(0, "filter3@email.com", "1234", "Peter", "22334477");
            await _repository.CreateAsync(t1);
            await _repository.CreateAsync(t2);
            await _repository.CreateAsync(t3);
            _createdTeachers.Add(t1);
            _createdTeachers.Add(t2);
            _createdTeachers.Add(t3);
            // Act
            List<Teacher> list = await _repository.FilterAsync("peter", TeacherFilterBy.TeacherName);
            // Assert
            Assert.IsNotNull(list);
            Assert.IsFalse(list.Any(t => t.Id == t1.Id));
            Assert.IsTrue(list.Any(t => t.Id == t2.Id));
            Assert.IsTrue(list.Any(t => t.Id == t3.Id));
            Assert.IsTrue(list.Count >= 2);
        }
        [TestMethod]
        public async Task UpdateAsyncTest()
        {
            // Arrange
            Teacher originalTeacher = new Teacher(0, "update@email.com", "1234", "morten", "22334455");
            Teacher newTeacher = new Teacher(0, "update@gmail.com", "12345", "morten hansen", "22334456");
            await _repository.CreateAsync(originalTeacher); // Sets the Id of the parameter
            newTeacher.Id = originalTeacher.Id;
            _createdTeachers.Add(originalTeacher);
            // Act
            await _repository.UpdateAsync(newTeacher);
            Teacher? updatedTeacher = await _repository.GetAsync(originalTeacher.Id);
            // Assert
            Assert.IsNotNull(updatedTeacher);
            Assert.AreEqual(originalTeacher.Id, newTeacher.Id);

            Assert.AreEqual(newTeacher.Name, updatedTeacher.Name);
            Assert.AreEqual(newTeacher.Mail, updatedTeacher.Mail);
            Assert.AreEqual(newTeacher.Password, updatedTeacher.Password);
            Assert.AreEqual(newTeacher.PhoneNumber, updatedTeacher.PhoneNumber);
            Assert.AreEqual(newTeacher.Id, updatedTeacher.Id);

            Assert.AreNotEqual(originalTeacher.Name, updatedTeacher.Name);
            Assert.AreNotEqual(originalTeacher.Mail, updatedTeacher.Mail);
            Assert.AreNotEqual(originalTeacher.Password, updatedTeacher.Password);
            Assert.AreNotEqual(originalTeacher.PhoneNumber, updatedTeacher.PhoneNumber);
        }
        [TestMethod]
        public async Task DeleteAsyncTest()
        {
            // Arrange
            Teacher testSubject = new Teacher(0, "delete@email.com", "1234", "morten", "22334455");
            await _repository.CreateAsync(testSubject);
            // Act
            await _repository.DeleteAsync(testSubject);
            Teacher? cached = await _repository.GetAsync(testSubject.Id);
            // Assert
            Assert.IsNull(cached);
        }
        [TestMethod]
        public async Task DeleteNotExistingTest()
        {
            // Arrange
            Teacher testSubject = new Teacher(0, "delete@email.com", "1234", "morten", "22334455");
            //await _repository.CreateAsync(testSubject); - not created
            // Act & Assert
            await Assert.ThrowsExceptionAsync<RepositoryException>(() => _repository.DeleteAsync(testSubject));
        }
    }
}
