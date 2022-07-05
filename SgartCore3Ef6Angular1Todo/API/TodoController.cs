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
    [ApiController, Route("api/todo")]
    public class TodoController : ControllerBase
    {
        private readonly IDataRepository _manager;

        // inject del manager
        public TodoController(IDataRepository manager)
        {
            _manager = manager;
        }

        [HttpGet, Route("version")]
        public string Version()
        {
            return Constants.VERSION;
        }

        [HttpGet, Route("search")]
        public async Task<ServiceStatusListItem<MyTask>> MyTaskSearchAsync([FromQuery] FilterItem filter)
        {
            ServiceStatusListItem<MyTask> result = new ServiceStatusListItem<MyTask>();
            try
            {
                var items = await _manager.TaskSearchAsync(filter);
                if (items != null)
                {
                    result.Data = items.ToList();
                }
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

        [HttpGet, Route("{id}")]
        public async Task<ServiceStatusItem<MyTask>> MyTaskReadAsync(int id)
        {
            ServiceStatusItem<MyTask> result = new ServiceStatusItem<MyTask>();
            try
            {
                result.Data = await _manager.TaskGetAsync(id);
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

        [HttpPost, Route("insert")]
        public async Task<ServiceStatus> MyTaskInsertAsync(MyTaskInput inputData)
        {
            ServiceStatus result = new ServiceStatus();
            try
            {
                if (inputData.ID.HasValue == true)
                    result.AddError("Invalid `id` in INSERT");
                if (inputData.Date.HasValue == false || inputData.Date < new DateTime(1970, 1, 1))
                    result.AddError("`date` required");
                if (string.IsNullOrWhiteSpace(inputData.Title))
                    result.AddError("`title` required");
                if (inputData.IDCategory == null || inputData.IDCategory < 1)
                    result.AddError("`category` required");
                if (result.Messages.Count > 0)
                {
                    return result;
                }

                DateTime dtNow = DateTime.Now;
                MyTask item = new MyTask
                {
                    Date = inputData.Date.Value,
                    Title = inputData.Title,
                    Note = string.IsNullOrWhiteSpace(inputData.Note) ? null : inputData.Note,
                    Category = new Category
                    {
                        ID = inputData.IDCategory.Value
                    }
                };
                result.ReturnValue = await _manager.TaskAddAsync(item);
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

        [HttpPost, Route("update")]
        public async Task<ServiceStatus> MyTaskUpdateAsync(MyTaskInput inputData)
        {
            ServiceStatus result = new ServiceStatus();
            try
            {
                if (inputData.ID.HasValue == false || inputData.ID == 0)
                    result.AddError("`id` required");
                if (inputData.Date.HasValue == false || inputData.Date < new DateTime(1970, 1, 1))
                    result.AddError("`date` required");
                if (string.IsNullOrWhiteSpace(inputData.Title))
                    result.AddError("`title` required");
                if (inputData.IDCategory == null || inputData.IDCategory < 1)
                    result.AddError("`category` required");
                if (result.Messages.Count > 0)
                {
                    return result;
                }

                DateTime dtNow = DateTime.Now;
                MyTask item = new MyTask
                {
                    ID = inputData.ID.Value,
                    Date = inputData.Date.Value,
                    Title = inputData.Title,
                    Note = string.IsNullOrWhiteSpace(inputData.Note) ? null : inputData.Note,
                    Category = new Category
                    {
                        ID = inputData.IDCategory.Value
                    },
                    Completed = inputData.Completed,
                    Created = dtNow,
                    Modified = dtNow
                };
                result.ReturnValue = await _manager.TaskUpdateAsync(item);
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

        [HttpPost]
        [Route("delete")]
        public async Task<ServiceStatus> MyTaskDeleteAsync(MyTaskInput inputData)
        {
            ServiceStatus result = new ServiceStatus();
            try
            {
                if (inputData.ID.HasValue == false || inputData.ID == 0)
                    result.AddError("`id` required");
                if (result.Messages.Count > 0)
                {
                    return result;
                }

                result.ReturnValue = await _manager.TaskDeleteAsync(inputData.ID.Value);
                if (result.ReturnValue > 0)
                    result.AddSuccess($"Deleted");
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

        [HttpPost]
        [Route("toggle")]
        public async Task<ServiceStatusItem<MyTask>> TodoToggleAsync(MyTaskInput inputData)
        {
            ServiceStatusItem<MyTask> result = new ServiceStatusItem<MyTask>();
            try
            {
                if (inputData.ID.HasValue == false || inputData.ID == 0)
                    result.AddError("`id` required");
                if (result.Messages.Count > 0)
                {
                    return result;
                }

                result.Data = await _manager.TaskToggleAsync(inputData.ID.Value);
                if (result.Data != null)
                    result.AddSuccess("Updated");
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

        [HttpPost]
        [Route("category")]
        public async Task<ServiceStatusItem<MyTask>> TodoCategoryAsync(MyTaskInput inputData)
        {
            ServiceStatusItem<MyTask> result = new ServiceStatusItem<MyTask>();
            try
            {
                if (inputData.ID.HasValue == false || inputData.ID == 0)
                    result.AddError("`id` required");
                if (inputData.IDCategory.HasValue == false || inputData.IDCategory == 0)
                    result.AddError("`idCategory` required");
                if (result.Messages.Count > 0)
                {
                    return result;
                }

                result.Data = await _manager.TaskCategoryAsync(inputData.ID.Value, inputData.IDCategory.Value);
                if (result.Data != null)
                    result.AddSuccess("Updated");
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

    }
}
