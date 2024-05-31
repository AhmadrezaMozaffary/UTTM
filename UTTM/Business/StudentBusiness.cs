using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UTTM.Business.Interfaces;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business
{
    public class StudentBusiness : BusinessBase, IStudentBusiness
    {
        ISocietyBusiness SocietyBiz { get; set; }
        IUserBusiness UserBiz { get; set; }

        public StudentBusiness(UttmDbContext ctx, SocietyBusiness societyBiz, UserBusiness userBiz) : base(ctx)
        {
            SocietyBiz = societyBiz;
            UserBiz = userBiz;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            return await ctx.Student.ToListAsync();
        }

        public bool CurrentStudentExists(int userId)
        {
            return ctx.Student.Where(s => s.UserId == userId).Any();
        }

        public async Task<Student?> GetStudent(int id)
        {
            Student _std = await ctx.Student.FirstOrDefaultAsync(s => s.Id == id) ?? throw new Exception("دانشجو مد نظر یافت نشد");

            return _std;
        }

        public async Task<int> AddNewStudent(StudentViewModel req)
        {
            if (!UserBiz.UserExists(req.UserId)) throw new Exception("کاربر یافت نشد");

            if (!SocietyBiz.SocietyExists(req.SocietyId)) throw new Exception("انجمن یافت نشد");

            if (CurrentStudentExists(req.UserId)) throw new Exception("برای کاربر انتخاب شده قبلا دانشجو اضافه شده است");

            if (!UserBiz.UserCanBeTypeOf(UserRole.Student, req.UserId)) throw new Exception("کابر از نوع دانشجو نیست");

            Student _newStd = new()
            {
                Id = 0,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Avatar = req.Avatar,
                UserId = req.UserId,
                SocietyId = req.SocietyId
            };

            await ctx.Student.AddAsync(_newStd);
            Save();

            return _newStd.Id;
        }

        public async Task<Student> EditStudent(StudentEditViewModel req)
        {
            Student? _existingStd = await GetStudent(req.Id);

            if (_existingStd == null) throw new Exception("دانشجو جهت ویرایش یافت نشد");

            _existingStd.FirstName = req.FirstName;
            _existingStd.LastName = req.LastName;
            _existingStd.Avatar = req.Avatar;

            Save();

            return _existingStd;
        }

        public async Task<int> DeleteStudent(int id)
        {
            Student? _std = await GetStudent(id);

            if (_std == null) throw new Exception("دانشجو مد نظر یافت نشد");

            ctx.Student.Remove(_std);
            Save();

            return _std.Id;

        }
    }
}
