using RazorPageApplication.Enums;
using RazorPageApplication.Models;
using RazorPageApplication.Services;
using System;
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
    }
}
