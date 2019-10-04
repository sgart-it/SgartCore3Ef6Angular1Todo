using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SgartCore3Ef6Angular1Todo.Models;
using SgartCore3Ef6Angular1Todo.ServerApp;

namespace SgartCore3Ef6Angular1Todo.API
{
  [ApiController]
  [Route("api/todo")]
  public class TodoController : ControllerBase
  {
    private readonly IDataRepository _manager;

    // inject del manager
    public TodoController(IDataRepository manager)
    {
      _manager = manager;
    }

    [HttpGet]
    [Route("version")]
    public string Version()
    {
      return Constants.VERSION;
    }

    [Route("search")]
    public async Task<ServiceStatusListItem<MyTask>> MyTaskSearchAsync([FromQuery] FilterItem filter)
    {
      ServiceStatusListItem<MyTask> result = new ServiceStatusListItem<MyTask>();
      try
      {
        result.Data = _manager.TaskSearch(filter).ToList();
        if (result.Data?.Count > 0)
          result.AddSuccess($"Readed page: {filter.Page}, items: {result.Data.Count}");
        else
          result.AddSuccess("no data");
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddError(ex);
      }
      return result;
    }

    [Route("{id}")]
    public async Task<ServiceStatusItem<MyTask>> MyTaskReadAsync(int id)
    {
      ServiceStatusItem<MyTask> result = new ServiceStatusItem<MyTask>();
      try
      {
        result.Data = _manager.TaskGet(id);
        if (result.Data != null)
          result.AddSuccess($"Readed 1");
        else
          result.AddSuccess("no data");
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddError(ex);
      }
      return result;
    }

    [HttpPost]
    [Route("insert")]
    public async Task<ServiceStatusItem> MyTaskInsertAsync(MyTask item)
    {
      ServiceStatusItem<MyTask> result = new ServiceStatusItem<MyTask>();
      try
      {
        if (item.ID != 0)
          result.AddError("Ivalid `id` in INSERT");
        if (item.Date < new DateTime(1970, 1, 1))
          result.AddError("`date` required");
        if (string.IsNullOrWhiteSpace(item.Title))
          result.AddError("`title` required");
        if (item.Category == null || item.Category.ID < 1)
          result.AddError("`category` required");
        if (result.Messages.Count > 0)
        {
          return result;
        }
        result.ReturnValue = _manager.TaskAdd(item);
        if (result.ReturnValue > 0)
          result.AddSuccess($"Readed 1");
        else
          result.AddWarning("No change");
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddError(ex);
      }
      return result;
    }

    //[HttpPost]
    //[Route("update")]
    //public async Task<ServiceStatusItem> TodoUpdateAsync(TodoItem item)
    //{
    //  return await _manager.TodoUpdateAsync(item);
    //}

    //[HttpPost]
    //[Route("delete")]
    //public async Task<ServiceStatusItem> TodoDeleteAsync(FilterItem filter)
    //{
    //  return await _manager.TodoDeleteAsync(filter.ID);
    //}

    //[HttpPost]
    //[Route("toggle")]
    //public async Task<ServiceStatusItem<TodoItem>> TodoToggle(FilterItem filter)
    //{
    //  return await _manager.TodoToggleAsync(filter.ID);
    //}

    //[HttpPost]
    //[Route("category")]
    //public async Task<ServiceStatusItem> TodoCategoryAsync(FilterItem filter)
    //{
    //  return await _manager.TodoCategoryAsync(filter.ID, filter.IDCategory);
    //}

  }
}
