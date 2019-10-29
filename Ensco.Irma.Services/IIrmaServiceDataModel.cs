using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Services
{
    public interface IIrmaServiceDataModel
    {
        Dictionary<string, string> ServerInfo();
        IQueryable GetQueryable(string sortColumn);

        IQueryable GetQueryable(string filter, string sort);

        List<dynamic> GetAllItems();

        List<dynamic> GetItems(Expression filter);
        List<dynamic> GetItems(string filter, string sort);
        dynamic GetItem(string filter, string sort);
        dynamic Add(dynamic model, bool commit = true);
        bool Update(dynamic model, bool commit = true);
        bool Delete(dynamic model, bool commit = true);
        dynamic Refresh(dynamic model);
    }
}
