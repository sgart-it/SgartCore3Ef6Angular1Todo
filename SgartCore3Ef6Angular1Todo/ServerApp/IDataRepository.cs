using SgartCore3Ef6Angular1Todo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SgartCore3Ef6Angular1Todo.ServerApp
{
    public interface IDataRepository
    {
        IEnumerable<MyTask> TaskSearch(FilterItem filter);
        MyTask TaskGet(int id);
        int TaskAdd(MyTask entity);
        int TaskUpdate(MyTask entity);
        int TaskDelete(int id);
        MyTask TaskToggle(int id);
        MyTask TaskCategory(int id, int idCategory);

        IEnumerable<Category> CategoryGetAllAsync();
        Category CategoryGet(int id);

        IEnumerable<Statistic> StatisticGetAll();
    }
}
