using Microsoft.EntityFrameworkCore;
using SgartCore3Ef6Angular1Todo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SgartCore3Ef6Angular1Todo.ServerApp
{
    public class TodoManager : IDataRepository
    {
        readonly TodoContext _ctx;

        public TodoManager(TodoContext context)
        {
            _ctx = context;
        }

        #region Task

        public async Task<IEnumerable<MyTask>> TaskSearchAsync(FilterItem filter)
        {
            //.AsNoTracking() solo perchè è in read only, non mi serve modificarlo
            var r = _ctx.MyTasks.Include(x => x.Category).AsNoTracking();

            if (filter.IDCategory.HasValue && filter.IDCategory >= 0)
                r = r.Where(x => x.Category.ID == filter.IDCategory);
            if (filter.Status.HasValue && filter.Status > 0)
            {
                if (filter.Status == 1)  // completed
                    r = r.Where(x => x.Completed != null);
                else if (filter.Status == 2)
                    r = r.Where(x => x.Completed == null);
            }
            if (string.IsNullOrWhiteSpace(filter.Text) == false)
            {
                string text = filter.Text.ToLower();
                r = r.Where(x =>
                (x.Title != null && x.Title.ToLower().Contains(text))
                ||
                (x.Note != null && x.Note.ToLower().Contains(text))
             );
            }
            if (filter.Page < 1) filter.Page = 1;
            if (filter.Size < 1) filter.Size = 10;
            var startIndex = (filter.Page - 1) * filter.Size;

            r = r.Skip(startIndex).Take(filter.Size);

            if (string.IsNullOrWhiteSpace(filter.Sort) == false)
            {
                // normalizzo la stringa di sort
                string sort = filter.Sort.ToLower().Trim().Replace(" asc", "").Replace("  ", " ").Replace("  ", " ");
                switch (sort)
                {
                    case "id": r = r.OrderBy(x => x.ID); break;
                    case "id desc": r = r.OrderByDescending(x => x.ID); break;
                    case "title": r = r.OrderBy(x => x.Title); break;
                    case "title desc": r = r.OrderByDescending(x => x.Title); break;
                    case "date": r = r.OrderBy(x => x.Date); break;
                    case "date desc": r = r.OrderByDescending(x => x.Date); break;
                    case "category": r = r.OrderBy(x => x.Category.Name); break;
                    case "category desc": r = r.OrderByDescending(x => x.Category.Name); break;
                    case "modified": r = r.OrderBy(x => x.Date); break;
                    case "modified desc": r = r.OrderByDescending(x => x.Date); break;
                }
            }
            return await r.ToListAsync();
        }

        public async Task<MyTask> TaskGetAsync(int id)
        {
            return await _ctx.MyTasks.Include(x => x.Category).FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<int> TaskAddAsync(MyTask entity)
        {
            DateTime dt = DateTime.Now;
            entity.ID = 0;
            entity.Modified = dt;
            entity.Created = dt;
            entity.Category =await CategoryGetAsync(entity.Category.ID);
            _ctx.MyTasks.Add(entity);
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> TaskUpdateAsync(MyTask entity)
        {
            DateTime dt = DateTime.Now;

            MyTask dbEntity =await TaskGetAsync(entity.ID);
            dbEntity.Date = entity.Date;
            dbEntity.Title = entity.Title;
            dbEntity.Note = entity.Note;
            if (entity.Completed.HasValue)
                dbEntity.Completed = dt;
            else
                dbEntity.Completed = null;
            dbEntity.Category =await CategoryGetAsync(entity.Category.ID);
            dbEntity.Modified = dt;
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> TaskDeleteAsync(int id)
        {
            MyTask entity =await TaskGetAsync(id);
            _ctx.MyTasks.Remove(entity);
            return await _ctx.SaveChangesAsync();
        }

        public async Task<MyTask> TaskToggleAsync(int id)
        {
            DateTime dt = DateTime.Now;

            MyTask dbEntity =await TaskGetAsync(id);
            if (dbEntity.Completed.HasValue)
                dbEntity.Completed = null;
            else
                dbEntity.Completed = dt;
            dbEntity.Modified = dt;
            int row =await _ctx.SaveChangesAsync();
            if (row == 0)
                return null;
            return dbEntity;
        }

        public async Task<MyTask> TaskCategoryAsync(int id, int idCategory)
        {
            DateTime dt = DateTime.Now;

            MyTask dbEntity =await TaskGetAsync(id);
            dbEntity.Category =await CategoryGetAsync(idCategory);
            dbEntity.Modified = dt;
            int row =await _ctx.SaveChangesAsync();
            if (row == 0)
                return null;
            return dbEntity;
        }
        #endregion

        #region Category

        public async Task<IEnumerable<Category>> CategoryGetAllAsync()
        {
            return await _ctx.Categories.ToListAsync();
        }

        public async Task< Category> CategoryGetAsync(int id)
        {
            return await _ctx.Categories.FirstOrDefaultAsync(x => x.ID == id);
        }
        #endregion

        #region Statistic

        public async Task<IEnumerable<Statistic>> StatisticGetAllAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
