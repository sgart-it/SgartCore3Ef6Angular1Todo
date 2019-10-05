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

        public IEnumerable<MyTask> TaskSearch(FilterItem filter)
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
            return r;
        }

        public MyTask TaskGet(int id)
        {
            return _ctx.MyTasks.Include(x => x.Category).FirstOrDefault(x => x.ID == id);
        }

        public int TaskAdd(MyTask entity)
        {
            DateTime dt = DateTime.Now;
            entity.ID = 0;
            entity.Modified = dt;
            entity.Created = dt;
            entity.Category = CategoryGet(entity.Category.ID);
            _ctx.MyTasks.Add(entity);
            return _ctx.SaveChanges();
        }

        public int TaskUpdate(MyTask entity)
        {
            DateTime dt = DateTime.Now;

            MyTask dbEntity = TaskGet(entity.ID);
            dbEntity.Date = entity.Date;
            dbEntity.Title = entity.Title;
            dbEntity.Note = entity.Note;
            if (entity.Completed.HasValue)
                dbEntity.Completed = dt;
            else
                dbEntity.Completed = null;
            dbEntity.Category = CategoryGet(entity.Category.ID);
            dbEntity.Modified = dt;
            return _ctx.SaveChanges();
        }

        public int TaskDelete(int id)
        {
            MyTask entity = TaskGet(id);
            _ctx.MyTasks.Remove(entity);
            return _ctx.SaveChanges();
        }

        public MyTask TaskToggle(int id)
        {
            DateTime dt = DateTime.Now;

            MyTask dbEntity = TaskGet(id);
            if (dbEntity.Completed.HasValue)
                dbEntity.Completed = null;
            else
                dbEntity.Completed = dt;
            dbEntity.Modified = dt;
            int row = _ctx.SaveChanges();
            if (row == 0)
                return null;
            return dbEntity;
        }

        public MyTask TaskCategory(int id, int idCategory)
        {
            DateTime dt = DateTime.Now;

            MyTask dbEntity = TaskGet(id);
            dbEntity.Category = CategoryGet(idCategory);
            dbEntity.Modified = dt;
            int row = _ctx.SaveChanges();
            if (row == 0)
                return null;
            return dbEntity;
        }
        #endregion

        #region Category

        public IEnumerable<Category> CategoryGetAllAsync()
        {
            return _ctx.Categories;
        }

        public Category CategoryGet(int id)
        {
            return _ctx.Categories.FirstOrDefault(x => x.ID == id);
        }
        #endregion

        #region Statistic

        public IEnumerable<Statistic> StatisticGetAll()
        {
            return null;
        }
        #endregion
    }
}
