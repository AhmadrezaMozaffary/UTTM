using Microsoft.EntityFrameworkCore;
using UTTM.Business.Interfaces;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business
{
    public class EventBusiness : BusinessBase, IEventBusiness
    {
        public SocietyBusiness SocietyBiz { get; set; }
        public EventBusiness(UttmDbContext ctx, SocietyBusiness societyBiz) : base(ctx)
        {
            SocietyBiz = societyBiz;
        }

        public async Task<int> CreateEvent(EventViewModel e)
        {
            if (SocietyBiz.SocietyExists(e.SocietyId))
            {
                throw new Exception("انجمن برگزار کننده یافت نشد");
            }

            Event addedEvent = new Event()
            {
                Id = 0,
                Title = e.Title,
                Description = e.Description,
                Status = e.Status,
                Target = e.Target,
                Platform = e.Platform,
                TotalCapacity = e.TotalCapacity,
                AvailableCapacity = e.AvailableCapacity,
                CratedAt = DateTime.Now,
                StartAt = e.StartAt,
                EndAt = e.EndAt,
                SocietyId = e.SocietyId,
            };

            await ctx.Event.AddAsync(addedEvent);
            Save();

            return addedEvent.Id;
        }


        public async Task<List<Event>> GetAllEvents()
        {
            return await ctx.Event.ToListAsync();
        }

        public async Task<Event?> GetEventById(int id)
        {
            var events = await GetAllEvents();

            return events.FirstOrDefault(e => e.Id == id);
        }

        public async Task<int> RemoveEvent(int id)
        {
            var e = await GetEventById(id) ?? throw new Exception("رویدادی جهت حذف وجود ندارد");

            ctx.Event.Remove(e);
            Save();

            return e.Id;

        }
    }
}
