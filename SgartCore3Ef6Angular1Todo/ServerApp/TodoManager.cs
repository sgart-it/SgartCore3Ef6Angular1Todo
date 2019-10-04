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

    public IEnumerable<MyTask> TaskSearch(FilterItem filter)
    {
      var r = _ctx.MyTasks.AsEnumerable();

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
        r = r.Where(x => x.Title.Contains(filter.Text, StringComparison.InvariantCultureIgnoreCase) || x.Note.Contains(filter.Text, StringComparison.InvariantCultureIgnoreCase));

      if (filter.Page < 1) filter.Page = 1;
      if (filter.Page < 1) filter.Page = 10;
      var startIndex = (filter.Page - 1) * filter.Page;

      r = r.Skip(startIndex).Take(filter.Page);

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
      return _ctx.MyTasks.FirstOrDefault(x => x.ID == id);
    }

    public int TaskAdd(MyTask entity)
    {
      _ctx.MyTasks.Add(entity);
      return _ctx.SaveChanges();
    }

    public int TaskUpdate(MyTask dbEntity, MyTask entity)
    {
      dbEntity.Date = entity.Date;
      dbEntity.Title = entity.Title;
      dbEntity.Note = entity.Note;
      dbEntity.Completed = entity.Completed;
      //dbEntity.Category.CategoryID = entity.Category.CategoryID;
      return _ctx.SaveChanges();
    }

    public int TaskDelete(MyTask entity)
    {
      _ctx.MyTasks.Remove(entity);
      return _ctx.SaveChanges();
    }

    public IEnumerable<Category> CategoryGetAllAsync()
    {
      return _ctx.Categories;
    }

    public IEnumerable<StatisticItem> StatisticGetAll()
    {
      return null;
    }

  }
}
