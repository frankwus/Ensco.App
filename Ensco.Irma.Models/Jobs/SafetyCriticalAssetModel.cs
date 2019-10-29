using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Jobs
{
    public class SafetyCriticalAssetModel
    {
        [Column(Visible =false, Key =true)]
        public long Id { get; set; }
        [Column(Visible = false)]
        public long Parent { get; set; }
        public string Name { get; set; }
        public Guid Key { get; set; }
        public SafetyCriticalAssetModel()
        {            
        }
    }
}
