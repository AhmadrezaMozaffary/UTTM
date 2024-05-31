using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UTTM.Business.Interfaces;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business
{
    public class ProfessorBusiness : BusinessBase, IProfessorBusiness
    {
        public UserBusiness UserBiz { get; set; }
        public SocietyBusiness SocietyBiz { get; set; }

        public ProfessorBusiness(UttmDbContext ctx, UserBusiness userBiz, SocietyBusiness societyBiz) : base(ctx)
        {
            UserBiz = userBiz;
            SocietyBiz = societyBiz;
        }

        public async Task<List<Professor>> GetAllProfessors()
        {
            return await ctx.Professor.ToListAsync();
        }

        public bool CurrentProfessorExists(int userId)
        {
            return ctx.Professor.Where(s => s.UserId == userId).Any();
        }

        public async Task<Professor?> GetProfessor(int id)
        {
            Professor? p = await ctx.Professor.FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) throw new Exception("استاد مد نظر یافت نشد");

            return p;
        }

        public async Task<int> AddNewProfessor(ProfessorViewModel req)
        {
            if (!UserBiz.UserExists(req.UserId)) throw new Exception("کاربر یافت نشد");

            if (!SocietyBiz.SocietyExists(req.SocietyId)) throw new Exception("انجمن یافت نشد");

            if (CurrentProfessorExists(req.UserId)) throw new Exception("برای کاربر انتخاب شده قبلا استاد اضافه شده است");

            if (!UserBiz.UserCanBeTypeOf(UserRole.Professor, req.UserId)) throw new Exception("کابر از نوع استاد نیست");

            Professor _newProfessor = new()
            {
                Id = 0,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Avatar = req.Avatar,
                Discription = req.Discription,
                Rate = req.Rate,
                EmailAddress = req.EmailAddress,
                UserId = req.UserId,
                SocietyId = req.SocietyId
            };

            await ctx.Professor.AddAsync(_newProfessor);
            Save();

            return _newProfessor.Id;
        }

        public async Task<Professor> EditProfessor(ProfessorEditViewModel req)
        {
            Professor? p = await GetProfessor(req.Id);

            p.FirstName = req.FirstName;
            p.LastName = req.LastName;
            p.Avatar = req.Avatar;
            p.Discription = req.Discription;
            p.Rate = req.Rate;
            p.EmailAddress = req.EmailAddress;

            Save();

            return p;
        }

        public async Task<int> DeleteProfessor(int id)
        {
            Professor? p = await GetProfessor(id);

            ctx.Professor.Remove(p);
            Save();

            return p.Id;
        }
    }
}
