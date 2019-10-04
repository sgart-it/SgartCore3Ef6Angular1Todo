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
  public class StatisticsController : ControllerBase
  {
    private readonly IDataRepository _manager;

    // inject del manager
    public StatisticsController(IDataRepository manager)
    {
      _manager = manager;
    }

    [HttpGet]
    [Route("api/statistics")]
    public async Task<ServiceStatusListItem<StatisticItem>> Get()
    {
      ServiceStatusListItem<StatisticItem> result = new ServiceStatusListItem<StatisticItem>();
      try
      {
        result.Data = _manager.StatisticGetAll().ToList();
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