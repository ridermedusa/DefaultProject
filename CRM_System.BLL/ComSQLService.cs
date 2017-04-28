using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM_System.DAL;
using System.Data;
using System.Collections;

namespace CRM_System.BLL
{
    public class ComSQLService
    {
        ComSQLRepository repository;

        public ComSQLService(ComSQLRepository repository)
        {
            this.repository = repository;
        }
        public ComSQLService()
        {
            this.repository = new ComSQLRepository();
        }
        public void DataTableToSQLServer(DataTable dt, string TableName)
        {
            repository.DataTableToSQLServer(dt, TableName);
        }
        public DataSet Query(string sql)
        {
            return repository.Query(sql);
        }

        public object GetSingle(string sql)
        {
            return repository.GetSingle(sql);
        }

        public int  ExecuteSql(string sql)
        {
            return repository.ExecuteSql(sql);
        }


        static ComSQLRepository rep = new ComSQLRepository();

        public static DataSet SQuery(string sql)
        {
            return rep.Query(sql);
        }

        public static object SGetSingle(string sql)
        {
            return rep.GetSingle(sql);
        }

        public static int SExecuteSql(string sql)
        {
            return rep.ExecuteSql(sql);
        }

        public static void SExecuteSqlTran(ArrayList SQLStringList)
        {
            rep.ExecuteSqlTran(SQLStringList);
        }


    }
}
