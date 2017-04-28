using CRM_System.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_System.DAL
{
    public partial class Mpr_Admin_MenuRepository : RepositoryBase<Mpr_Admin_Menu>
    {
        public object GetPage(string Contextinfo, int pageIndex, int pageSize)
        {
            var q = from o in context.Mpr_Admin_Menu
                    select o;
            if (!string.IsNullOrEmpty(Contextinfo))
            {
                q = q.Where(s => s.RightName.Contains(Contextinfo));
            }
            q = q.OrderByDescending(s => s.AddTime).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return q.ToList();
        }
    } 
     

}
