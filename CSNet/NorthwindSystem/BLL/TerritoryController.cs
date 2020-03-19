using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using NorthwindSystem.DAL;
using NorthwindSystem.Entities;
#endregion

namespace NorthwindSystem.BLL
{
    public class TerritoryController
    {
        public List<Territory> Territories_List()
        {
            using (var context = new NorthwindSystemContext())
            {
                return context.Territories.ToList();
            }
        }
        public Territory Territories_FindByID(int territoryid)
        {
            using (var context = new NorthwindSystemContext())
            {
                return context.Territories.Find(territoryid);
            }
        }
    }
}
