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
    [ApiController, Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IDataRepository _manager;

        // inject del manager
        public StatisticsController(IDataRepository manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<ServiceStatusListItem<Statistic>> GetAsync()
        {
            ServiceStatusListItem<Statistic> result = new ServiceStatusListItem<Statistic>();
            try
            {
                result.Data = (await _manager.StatisticGetAllAsync()).ToList();
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