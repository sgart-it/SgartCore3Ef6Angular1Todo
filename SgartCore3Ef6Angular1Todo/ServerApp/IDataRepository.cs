using SgartCore3Ef6Angular1Todo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SgartCore3Ef6Angular1Todo.ServerApp
{
    public interface IDataRepository
    {
        Task<IEnumerable<MyTask>> TaskSearchAsync(FilterItem filter);
        Task<MyTask> TaskGetAsync(int id);
        Task<int> TaskAddAsync(MyTask entity);
        Task<int> TaskUpdateAsync(MyTask entity);
        Task<int> TaskDeleteAsync(int id);
        Task<MyTask> TaskToggleAsync(int id);
        Task<MyTask> TaskCategoryAsync(int id, int idCategory);

        Task<IEnumerable<Category>> CategoryGetAllAsync();
        Task<Category> CategoryGetAsync(int id);

        Task<IEnumerable<Statistic>> StatisticGetAllAsync();
    }
}
