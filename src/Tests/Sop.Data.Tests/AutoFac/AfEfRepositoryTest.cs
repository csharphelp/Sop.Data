using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sop.Data.Repository;
using Sop.Data.Tests.Entities;
using Sop.Data.Tests.Repositories;
using Sop.Data.Tests.Utlity;
using Xunit;

namespace Sop.Data.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class AfEfRepositoryTest
    {

        #region AfEfRepositoryTest
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISchoolRepository _schoolRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly int _count = 10;

        public AfEfRepositoryTest()
        {
            var builder = new Autofac.ContainerBuilder();

            builder.AddSopData(opt =>
            {
                opt.UseMySql("server =127.0.0.1;database=soptestdb;uid=root;password=123456;");
            });
            var container = builder.Build();
            _unitOfWork = container.Resolve<IUnitOfWork>();
            _schoolRepository = container.Resolve<ISchoolRepository>();
            _studentRepository = container.Resolve<IStudentRepository>();
        } 
        #endregion
        


        #region School Insert
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public School School_Insert()
        {
            var school = new School
            {
                SchoolId = Guid.NewGuid().ToString("N"),
                Name = "希望小学" + StringHelper.GetRandomString(50),
            };
            _schoolRepository.Insert(school);
            _unitOfWork.SaveChanges();
            return school;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public List<School> School_Inserts()
        {

            var schools = new List<School>();
            for (int i = 0; i < this._count; i++)
            {
                var school = new School
                {
                    SchoolId = Guid.NewGuid().ToString("N"),
                    Name = "希望小学" + StringHelper.GetRandomString(50),
                };
                schools.Add(school);
            }
            _schoolRepository.Insert(schools);
            _unitOfWork.SaveChanges();
            var count = _schoolRepository.TableNoTracking.Count();
            Assert.True(count > this._count);
            foreach (var item in schools)
            {
                var school = _schoolRepository.GetById(item.Id);
                if (school != null)
                {
                    Assert.True(true);
                }
            }
            return schools;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task School_InsertAsync()
        {
            var school = new School
            {
                SchoolId = Guid.NewGuid().ToString("N"),
                Name = "希望小学" + StringHelper.GetRandomString(50),
            };
            await _schoolRepository.InsertAsync(school);
            await _unitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task School_InsertsAsync()
        {
            var schools = new List<School>();
            for (int i = 0; i < this._count; i++)
            {
                var school = new School
                {
                    SchoolId = Guid.NewGuid().ToString("N"),
                    Name = "希望小学" + StringHelper.GetRandomString(50),
                };
                schools.Add(school);
            }
            await _schoolRepository.InsertAsync(schools);
            await _unitOfWork.SaveChangesAsync();
            var count = _schoolRepository.TableNoTracking.Count();
            Assert.True(count > this._count);
            foreach (var item in schools)
            {
                var school = _schoolRepository.GetById(item.Id);
                if (school != null)
                {
                    Assert.True(true);
                }
            }
        }

        #endregion

        #region School Update
        [Fact]
        public void School_Update()
        {
            var school = School_Insert();
            school.Name = "希望小学Update" + StringHelper.GetRandomString(50);

            _schoolRepository.Update(school);
            _unitOfWork.SaveChanges();
        }
        [Fact]
        public void School_Updates()
        {
            var schools = School_Inserts();

            schools.ForEach(x => x.Name = "希望小学Update" + StringHelper.GetRandomString(50));
            _schoolRepository.Update(schools);
            _unitOfWork.SaveChanges();

            foreach (var item in schools)
            {
                var school = _schoolRepository.GetById(item.Id);
                if (school.Name != item.Name)
                {
                    Assert.True(false);
                }
            }
        }
        [Fact]
        public void School_UpdateNoSelect()
        {
            var data = School_Insert();
            var school = new School
            {
                Id = data.Id,
                SchoolId = Guid.NewGuid().ToString("N"),
                Name = "希望小学UpdateNoSelect" + StringHelper.GetRandomString(32),
            };
            _schoolRepository.Update(school, x => x.Name);
            _unitOfWork.SaveChanges();

            var dsdsd = _schoolRepository.GetById(data.Id);

            var newSchool = _schoolRepository.TableNoTracking.First(x => x.Id == data.Id);
            Assert.True(newSchool.Name == school.Name);
        }

        #endregion

        #region School Delete
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void School_Delete()
        {
            var school = School_Insert();

            _schoolRepository.Delete(school);
            _unitOfWork.SaveChanges();

            var newSchool = _schoolRepository.Table.FirstOrDefault(x => x.Id == school.Id);
            Assert.True(newSchool == null);
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void School_Deletes()
        {
            var schools = new List<School>();
            for (int i = 0; i < _count; i++)
            {
                var school = new School
                {
                    Name = Guid.NewGuid().ToString()
                };
                schools.Add(school);
            }
            _schoolRepository.Insert(schools);
            _unitOfWork.SaveChanges();

            _schoolRepository.Delete(schools);
            _unitOfWork.SaveChanges();

            foreach (var item in schools)
            {
                var school = _schoolRepository.GetById(item.Id);
                if (school != null)
                {
                    Assert.True(false);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void School_DeleteByLambda()
        {
            var data = School_Insert();

            _schoolRepository.Delete(x => x.Id == data.Id);
            _unitOfWork.SaveChanges();

            var newSchool = _schoolRepository.TableNoTracking.FirstOrDefault(x => x.Id == data.Id);
            Assert.True(newSchool == null);
        }
        #endregion

        #region School Get 
        [Fact]
        public void School_GetById()
        {
            var newSchool = School_Insert();
            var school = _schoolRepository.GetById(newSchool.Id);
            Assert.True(school != null);
        }

        [Fact]
        public void School_PagedList()
        {
            var schools = _schoolRepository.TableNoTracking.ToPagedList(1, 10);
            Assert.True(schools != null);
        }

        [Fact]
        public async Task School_PagedListAsync()
        {
            var schools = await _schoolRepository.TableNoTracking.ToPagedListAsync(1, 10);
            Assert.True(schools != null);
        }

        [Fact]
        public async Task School_GetByIdAsync()
        {
            var newSchool = School_Insert();

            var school = await _schoolRepository.GetByIdAsync(newSchool.Id);
            Assert.True(school != null);
        }
        #endregion

        #region Student Insert
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public Student Student_Insert()
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine("SELECT \"_ss_dapper_1_\".*, ROWNUM RNUM FROM (");
            sb.Append("sql");
            sb.AppendLine(") \"_ss_dapper_1_\"");
            sb.AppendLine("WHERE ROWNUM <= :topLimit) \"_ss_dapper_2_\" ");
            sb.AppendLine("WHERE \"_ss_dapper_2_\".RNUM > :toSkip");


            var newSchool = School_Insert();
            var student = new Student
            {
                Type = 1,
                IsDel = false,
                Status = Status.NotActive,
                LongValue = 123321312L,
                FloatValue = 2222222f,
                DecimalValue = 213123123m,
                Body = "希望小学" + StringHelper.GetRandomString(500),
                DateCreated = DateTime.Now,
                SchoolId = newSchool.SchoolId,
            };
            _studentRepository.Insert(student);
            _unitOfWork.SaveChanges();
            return student;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Student_InsertAsync()
        {
            var newSchool = School_Insert();
            var student = new Student
            {
                Type = 2,
                IsDel = false,
                Status = Status.NotActive,
                LongValue = 123321312L,
                FloatValue = 2222222f,
                DecimalValue = 213123123m,
                Body = "希望小学" + StringHelper.GetRandomString(500),
                DateCreated = DateTime.Now,
                SchoolId = newSchool.SchoolId,
            };
            await _studentRepository.InsertAsync(student);
            await _unitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public List<Student> Student_Inserts()
        {
            var newSchool = School_Insert();
            var students = new List<Student>();
            for (int i = 0; i < this._count; i++)
            {
                var student = new Student
                {
                    Type = 1,
                    IsDel = false,
                    Status = Status.NotActive,
                    LongValue = long.Parse($"1{i}1{i}3{i}{i}"),
                    FloatValue = float.Parse($"1233{i}21{i}.2{i}2"),
                    DecimalValue = decimal.Parse($"1233{i}21{i}.2{i}2"),
                    Body = "希望小学" + StringHelper.GetRandomString(500),
                    DateCreated = DateTime.Now.AddHours(-i),
                    SchoolId = newSchool.SchoolId,
                };
                students.Add(student);
            }
            _studentRepository.Insert(students);
            _unitOfWork.SaveChanges();
            var count1 = _studentRepository.TableNoTracking.Count();
            Assert.True(count1 > this._count);
            foreach (var item in students)
            {
                var school = _studentRepository.GetById(item.Id);
                if (school != null)
                {
                    Assert.True(true);
                }
            }
            return students;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Student_InsertsAsync()
        {
            var newSchool = School_Insert();
            var students = new List<Student>();
            for (int i = 0; i < this._count; i++)
            {
                var student = new Student
                {
                    Type = 1,
                    IsDel = false,
                    Status = Status.NotActive,
                    LongValue = long.Parse($"10000{i}"),
                    FloatValue = float.Parse($"12{i}1{i}.2{i}2"),
                    DecimalValue = decimal.Parse($"1233{i}21{i}.2{i}2"),
                    Body = "希望小学" + StringHelper.GetRandomString(500),
                    DateCreated = DateTime.Now.AddMilliseconds(i),
                    SchoolId = newSchool.SchoolId,
                };
                students.Add(student);
            }
            await _studentRepository.InsertAsync(students);
            await _unitOfWork.SaveChangesAsync();

            var count1 = _studentRepository.TableNoTracking.Count();
            Assert.True(count1 > this._count);


        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public Student Student_Insert_BeginTransaction()
        {

            var newSchool = School_Insert();
            var student = new Student
            {
                Type = 1,
                IsDel = false,
                Status = Status.NotActive,
                LongValue = 123321312L,
                FloatValue = 2222222f,
                DecimalValue = 213123123m,
                Body = "希望小学-BeginTransaction" + StringHelper.GetRandomString(400),
                DateCreated = DateTime.Now,
                SchoolId = newSchool.SchoolId,
            };
            _studentRepository.Insert(student);
            student.Body = "希望小学-BeginTransaction" + StringHelper.GetRandomString(400);
            _studentRepository.Insert(student);
            _unitOfWork.SaveChanges();
            return student;
        }

    }
}




