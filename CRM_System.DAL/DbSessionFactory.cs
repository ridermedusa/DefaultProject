using CRM_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CRM_System.DAL
{
    public class DbSessionFactory
    {
        /// <summary>
        /// 保证了线程内DbSession实例唯一
        /// </summary>
        /// <returns></returns>
        /// 
        public static ComSQLRepository GetCurrentDbSession()
        {
            ComSQLRepository _dbSession = CallContext.GetData("DbSession") as ComSQLRepository;
            if (_dbSession == null)
            {
                _dbSession = new ComSQLRepository();
                CallContext.SetData("DbSession", _dbSession);
            }
           
            return _dbSession;
        }
    }
}
