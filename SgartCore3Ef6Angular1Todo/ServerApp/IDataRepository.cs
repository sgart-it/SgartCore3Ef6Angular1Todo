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
    int TaskUpdate(MyTask dbEntity, MyTask entity);
    int TaskDelete(MyTask entity);

    IEnumerable<Category> CategoryGetAllAsync();

    IEnumerable<StatisticItem> StatisticGetAll();
  }
}
