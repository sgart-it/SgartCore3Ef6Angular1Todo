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
  public class CategoryController: ControllerBase
  {
    private readonly IDataRepository _manager;

    // inject del manager
    public CategoryController(IDataRepository manager)
    {
      _manager = manager;
    }

    [HttpGet]
    [Route("api/categories")]
    public async Task<ServiceStatusListItem<Category>> Get()
    {
      ServiceStatusListItem<Category> result = new ServiceStatusListItem<Category>();
      try
      {
        result.Data = _manager.CategoryGetAllAsync().ToList();
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
