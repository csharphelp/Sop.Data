using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sop.Data.Tests.Entities;
using Sop.Data.Tests.Helper;
using Sop.Data.Tests.Repositories;
using Sop.Data.Tests.Utlity;
using Xunit;

namespace Sop.Data.Tests
{
    public class DapperTest
    {
        #region DapperTest
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISchoolRepository _schoolRepository;

        public DapperTest()
        {
            var services = new ServiceCollection();
            services.AddSopData(opt =>
            {
                opt.UseMySql(DBConfig.MYSQL);
            });

            var sp = services.BuildServiceProvider();
            _unitOfWork = sp.GetRequiredService<IUnitOfWork>();
            _schoolRepository = sp.GetRequiredService<ISchoolRepository>();
        } 
        #endregion

        [Fact]
        public async Task QueryAsync()
        {
            var schools = await _unitOfWork.QueryAsync<School>("select * from school");
            Assert.True(schools.Any());
        }

        [Fact]
        public async Task QueryPageListAsync()
        {
            var schools = await _unitOfWork.QueryPageListAsync<School>(1, 10, "select * from school order by id");
            //Assert.True(schools.Item.Any());
        }

        [Fact]
        public async Task Student_QueryPageListAsync()
        {
            var students = await _unitOfWork.QueryPageListAsync<Student>(1, 10, "SELECT *FROM school\r\nJOIN student\r\nON student.SchoolId=school.SchoolId", "order by id DESC");
            //Assert.True(schools.Item.Any());
        }

        [Fact]
        public async Task SchoolAndStudent_QueryPageListAsync()
        {
            var students = await _unitOfWork.QueryPageListAsync<SchoolAndStudent>(1, 10, "SELECT school.SchoolId,school.Name as SchoolName,student.id,Body,DateCreated\r\nFROM school\r\nJOIN student\r\nON student.SchoolId=school.SchoolId", "order by id DESC");
            Assert.True(students.Any());
        }




        [Fact]
        public async Task ExecuteAsync()
        {
            var school = new School
            {
                SchoolId = Guid.NewGuid().ToString("N"),
                Name = "school"
            };

            await _unitOfWork.ExecuteAsync("insert into school(id,name,SchoolId) values(@Id,@Name,@SchoolId)",
                school);

            var newSchool = await _unitOfWork.QueryAsync<School>("select * from school where SchoolId =@SchoolId",
                new { SchoolId = school.SchoolId });

            Assert.True(school.Name == newSchool.First().Name);
        }

        [Fact]
        public async Task GetConnection()
        {
            //可以直接使用dapper
            var schools = await _unitOfWork.GetConnection().QueryAsync("select * from school");
            Assert.True(schools.Any());
        }

        [Fact]
        public async Task TransactionUseSaveChange()
        {
            var school1 = new School
            {
                Name = "school1",
                SchoolId = Guid.NewGuid().ToString("N"),
            };

            var school2 = new School
            {
                SchoolId = Guid.NewGuid().ToString("N"),
                Name = "school2"
            };

            await _schoolRepository.InsertAsync(school1);
            await _schoolRepository.InsertAsync(school2);
            await _unitOfWork.SaveChangesAsync();

            var newSchool1 = await _unitOfWork.QueryAsync<School>("select * from school where SchoolId =@Id",
                new { SchoolId = school1.SchoolId });
            var newSchool2 = await _unitOfWork.QueryAsync<School>("select * from school where SchoolId =@Id",
                new { SchoolId = school2.SchoolId });
            Assert.False(newSchool1.Any() && newSchool2.Any());

        }

        [Fact]
        public async Task Transaction()
        {
            var school1 = new School
            {
                SchoolId = Guid.NewGuid().ToString("N"),
                Name = "school1"
            };

            var school2 = new School
            {
                SchoolId = Guid.NewGuid().ToString("N"),
                Name = "school2"
            };

            using (var tran = _unitOfWork.BeginTransaction())
            {
                try
                {
                    await _unitOfWork.ExecuteAsync("insert school(id,name) values(@Id,@Name)",
                        school1, tran);
                    await _unitOfWork.ExecuteAsync("insert school(id,name) values(@Id,@Name)",
                        school2, tran);
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                }
            }
            var newSchool1 = await _unitOfWork.QueryAsync<School>("select * from school where id =@Id",
                new { Id = school1.Id });
            var newSchool2 = await _unitOfWork.QueryAsync<School>("select * from school where id =@Id",
                new { Id = school2.Id });
            Assert.False(newSchool1.Any() || newSchool2.Any());

        }

        [Fact]
        public async Task Transaction_Rollback()
        {
            var school1 = new School
            {

                Name = "school1"
            };

            var school2 = new School
            {

                Name = "school2"
            };

            using (var tran = _unitOfWork.BeginTransaction())
            {
                try
                {
                    await _unitOfWork.ExecuteAsync("insert school(id,name) values(@Id,@Name)",
                        school1, tran);
                    await _unitOfWork.ExecuteAsync("insert school(id,name) values(@Id,@Name)",
                        school2, tran);
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                }
            }
            var newSchool1 = await _unitOfWork.QueryAsync<School>("select * from school where id =@Id",
                new { Id = school1.Id });
            var newSchool2 = await _unitOfWork.QueryAsync<School>("select * from school where id =@Id",
                new { Id = school2.Id });
            Assert.False(newSchool1.Any() || newSchool2.Any());

        }



        [Fact]
        public async Task HybridTransaction()
        {
            var school1 = new School
            {
                Name = "school1",
                SchoolId = Guid.NewGuid().ToString("N"),

            };

            var school2 = new School
            {
                Name = "school2",
                SchoolId = Guid.NewGuid().ToString("N")
            };
            using (var tran = _unitOfWork.BeginTransaction())
            {
                try
                {
                    await _schoolRepository.InsertAsync(school1);
                    await _unitOfWork.SaveChangesAsync();

                    await _unitOfWork.ExecuteAsync("insert school(name,SchoolId) values(@Name,@SchoolId)",
                        school2, tran);

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                }
            }
            var newSchool1 = await _unitOfWork.QueryAsync<School>("select * from school where SchoolId =@SchoolId",
                new { SchoolId = school1.SchoolId });
            var newSchool2 = await _unitOfWork.QueryAsync<School>("select * from school where SchoolId =@SchoolId",
                new { SchoolId = school2.SchoolId });
            Assert.True(newSchool1.Any() && newSchool2.Any());

            var school3 = new School
            {
                Name = "school3",
                SchoolId = Guid.NewGuid().ToString("N")
            };
            var school4 = new School
            {
                Name = "school4",
                SchoolId = StringHelper.GetRandomString(1000)
            };
            using (var tran = _unitOfWork.BeginTransaction())
            {
                try
                {

                    await _schoolRepository.InsertAsync(school3);
                    await _unitOfWork.SaveChangesAsync();

                    await _unitOfWork.ExecuteAsync("insert school(name,SchoolId) values(@Name,@SchoolId)",
                        school4, tran);

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();

                }
            }
            var newSchool3 = await _unitOfWork.QueryAsync<School>("select * from school where SchoolId =@SchoolId",
                new { SchoolId = school4.SchoolId });
            Assert.False(newSchool3.Any());
        }

    }
}
