using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM_System.Model;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace CRM_System.DAL
{
    public class ComSQLRepository : RepositoryBase<ComSQL>
    {

        //基于SqlBulkCopy进行批量添加(表结构相同)
        public void DataTableToSQLServer(DataTable dt, string TableName)
        {
            context = new CRM_SystemEntities();
            using (SqlConnection destinationConnection = (SqlConnection)context.Database.Connection)
            {
                destinationConnection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    try
                    {
                        bulkCopy.DestinationTableName = TableName;//要插入的表的表明  
                        foreach (DataColumn Columninfo in dt.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(Columninfo.ColumnName, Columninfo.ColumnName);//映射字段名 DataTable列名 ,数据库 对应的列名  
                        }
                        bulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        // Close the SqlDataReader. The SqlBulkCopy  
                        // object is automatically closed at the end  
                        // of the using block.  

                    }
                }


            }

        }  
        //执行DataTable查询
        public int ExecuteSql(string SQLString)
        {
            context = new CRM_SystemEntities();
            using (SqlConnection connection = (SqlConnection)context.Database.Connection)
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        connection.Dispose();
                        throw e;
                    }
                }
            }
        }

        //执行DataTable查询
        public DataSet Query(string SQLString)
        {
            context = new CRM_SystemEntities();
            using (SqlConnection connection = (SqlConnection)context.Database.Connection)
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString)
        {
            context = new CRM_SystemEntities();
            using (SqlConnection connection = (SqlConnection)context.Database.Connection)
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {

                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            cmd.Dispose();
                            connection.Close();
                            connection.Dispose();
                            return null;
                        }
                        else
                        {
                            cmd.Dispose();
                            connection.Close();
                            connection.Dispose();
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        cmd.Dispose();
                        connection.Close();
                        connection.Dispose();
                        throw e;
                    }
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public void ExecuteSqlTran(ArrayList SQLStringList)
        {

            context = new CRM_SystemEntities();// EFContextFactory.GetCurrentDbContext();
            using (SqlConnection connection = (SqlConnection)context.Database.Connection)
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;

                SqlTransaction tx = connection.BeginTransaction();

                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (Exception E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }


    }

}
