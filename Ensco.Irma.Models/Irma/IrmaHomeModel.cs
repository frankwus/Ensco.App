using Ensco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class IrmaHomeModel
    {
        public IrmaHomePassportModel UserInfo { get; set; }
        public DataTableModel PobHistory { get; set; }
        public DataTableModel TasksCapa { get; set; }
        public DataTableModel TasksJob { get; set; }
        public DataTableModel TasksTask { get; set; }
        public DataTableModel TasksIsolation { get; set; }
        public DataTableModel TasksCap { get; set; }
        public DataTableModel TasksTlc{ get; set; }
        public int Id { get; set; } // PassporId
        public IrmaHomeModel()
        {
            UserInfo = new IrmaHomePassportModel();
        }
    }
}
