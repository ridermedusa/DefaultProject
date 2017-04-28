using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Cells;
using System.Data;
using System.IO;

namespace CRM_System.Tools
{
    public class Words
    {

        /// <summary>
        /// 将表格保存到Excle
        /// </summary>
        /// <param name="pstrFileName"></param>
        /// <returns></returns>
        public static byte[] SaveDataTableToExcel(DataTable pdtData)
        {
            if (pdtData == null)
            {
                throw new Exception("数据表中无数据");
            }
            Workbook book = new Workbook();
            Worksheet sheet = book.Worksheets[0];
            try
            {
                //列名的处理
                for (int i = 0; i < pdtData.Columns.Count; i++)
                {
                    sheet.Cells[0, i].PutValue(pdtData.Columns[i].ColumnName);
                    //列名加粗显示
                    //sheet.Cells[0, i].SetStyle.Font.IsBold = true;
                    // 列名居中显示
                    //sheet.Cells[0, i].Style.HorizontalAlignment = TextAlignmentType.Center;
                }
                // 数据行的处理
                for (int i = 0; i < pdtData.Rows.Count; i++)
                {
                    for (int j = 0; j < pdtData.Columns.Count; j++)
                    {
                        sheet.Cells[i + 1, j].PutValue(pdtData.Rows[i][j].ToString());
                    }
                }
                // 所有数据列自动调整宽度
                sheet.AutoFitColumns();
                MemoryStream stream = new MemoryStream();

                book.Save(stream, SaveFormat.Xlsx);// (pstrFileName);
                return stream.ToArray();
            }
            catch
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }
        }

        public static DataTable CreateNewDataTable(List<string> colums, DataTable dt)
        {
            DataTable dtData = new DataTable();
            foreach (var item in colums)
            {
                //DataColumn dc = new DataColumn();
                //dc.ColumnName = item;
                dtData.Columns.Add(item, typeof(decimal));
            }
            foreach (DataRow dw in dt.Rows)
            {
                DataRow dr = dtData.NewRow();

                foreach (var item in colums)
                {
                    dr[item] = Convert.ToDecimal(dw[item]);
                }
                dtData.Rows.Add(dr);
            }
            return dtData;
        }

        /// <summary>
        /// 将Excel文件读取到表格中
        /// </summary>
        /// <param name="pstrFileName"></param>
        /// <returns></returns>
        public static DataTable ReadExcelToDataTable(string pstrFileName)
        {
            try
            {
                //LoadLicense();
                // 从文件中读取Excel
                //Application excel = new Application();
                Workbook book = new Workbook();
                book.Open(pstrFileName);
                Worksheet sheet = book.Worksheets[0];

                // 转化成DataTable
                DataTable dtData = new DataTable();
                if (sheet != null && sheet.Cells.Rows.Count > 1)
                {
                    //sheet.Cells.FirstCell.

                    for (int i = 0; i < sheet.Cells.Rows.Count; i++)
                    {
                        DataRow dr = null;
                        for (int j = 0; j < sheet.Cells.MaxColumn + 1; j++)
                        {
                            string strCellValue = Convert.ToString(sheet.Cells[i, j].Value);
                            if (i == 0)
                            {
                                // 第1行为列头
                                DataColumn dc = new DataColumn();
                                dc.ColumnName = strCellValue;
                                dtData.Columns.Add(dc);
                            }
                            else
                            {
                                // 从第2行开始为数据行
                                if (dr == null)
                                {
                                    dr = dtData.NewRow();
                                }
                                dr[j] = strCellValue;
                            }
                        }
                        if (dr != null)
                        {
                            // 将创建的数据行添加到表格中
                            dtData.Rows.Add(dr);
                        }
                    }
                }
                return dtData;
            }
            finally
            {
                GC.Collect();
            }
        }


        /// <summary>
        /// 将Excel文件读取到表格中
        /// </summary>
        /// <param name="pstrFileName"></param>
        /// <returns></returns>
        public static DataTable ReadExcelToDataTableByQuestion(string pstrFileName)
        {
            try
            {
                //LoadLicense();
                // 从文件中读取Excel
                Workbook book = new Workbook();
                book.Open(pstrFileName);
                Worksheet sheet = book.Worksheets[0];
                // 转化成DataTable
                DataTable dtData = new DataTable();
                dtData.Columns.Add("data");
                if (sheet != null && sheet.Cells.Rows.Count > 1)
                {
                    for (int i = 0; i < sheet.Cells.Rows.Count; i++)
                    {
                        DataRow dr = null;
                        string strCellValue = Convert.ToString(sheet.Cells[i, 0].Value);
                        // 从第2行开始为数据行
                        if (dr == null)
                        {
                            dr = dtData.NewRow();
                        }
                        dr["data"] = strCellValue;
                        if (dr != null)
                        {
                            // 将创建的数据行添加到表格中
                            dtData.Rows.Add(dr);
                        }
                    }
                }
                return dtData;
            }
            finally
            {
                GC.Collect();
            }
        }


        /// <summary> 
        /// 导出数据到本地 
        /// </summary> 
        /// <param name="dt">要导出的数据</param> 
        /// <param name="tableName">表格标题</param> 
        /// <param name="path">保存路径</param> 
        public static string OutFileToDisk(DataTable dt, string tableName, string path)
        {


            Workbook workbook = new Workbook(); //工作簿 
            Worksheet sheet = workbook.Worksheets[0]; //工作表 
            Cells cells = sheet.Cells;//单元格 

            ////为标题设置样式     
            Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式 
            styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            styleTitle.Font.Name = "宋体";//文字字体 
            styleTitle.Font.Size = 18;//文字大小 
            styleTitle.Font.IsBold = true;//粗体 

            ////样式2 
            //Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            //style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            //style2.Font.Name = "宋体";//文字字体 
            //style2.Font.Size = 14;//文字大小 
            //style2.Font.IsBold = true;//粗体 
            //style2.IsTextWrapped = false;//单元格内容自动换行

            //style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            //style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            //style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            //style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            ////样式3 
            //Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            //style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            //style3.Font.Name = "宋体";//文字字体 
            //style3.Font.Size = 12;//文字大小 
            //style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            //style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            //style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            //style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            int Colnum = dt.Columns.Count;//表格列数 
            int Rownum = dt.Rows.Count;//表格行数

            for (int k = 0; k < Colnum; k++)
            {
                cells.SetColumnWidth(k, 20);
            }

            //生成行1 标题行    
            cells.Merge(0, 0, 1, Colnum);//合并单元格 
            cells[0, 0].PutValue(tableName);//填写内容 
            cells[0, 0].SetStyle(styleTitle);
            cells.SetRowHeight(0, 38);


            //生成行2 列名行 
            for (int i = 0; i < Colnum; i++)
            {
                cells[1, i].PutValue(dt.Columns[i].ColumnName);
                //cells[0, i].SetStyle(style2);
                cells.SetRowHeight(0, 25);


            }
            //生成数据行 
            for (int i = 0; i < Rownum; i++)
            {
                for (int k = 0; k < Colnum; k++)
                {
                    cells[2 + i, k].PutValue(dt.Rows[i][k].ToString());
                    //cells[1 + i, k].SetStyle(style3);
                }
                cells.SetRowHeight(2 + i, 24);
            }
            workbook.Save(path, SaveFormat.Xlsx);
            return "/Second/TempExcel/" + tableName + ".xlsx";
        }

        /// <summary>
        /// 读取Excel第一行数据直接进行储存
        /// </summary>
        /// <param name="pstrFileName"></param>
        /// <returns></returns>
        public static DataTable ReaderFirstPhone(string pstrFileName)
        {
            try
            {
                //LoadLicense();
                // 从文件中读取Excel
                //Application excel = new Application();
                Workbook book = new Workbook();
                book.Open(pstrFileName);
                Worksheet sheet = book.Worksheets[0]; 
                // 转化成DataTable
                DataTable dtData = new DataTable(); 
                //创建表结构
                DataColumn dc = new DataColumn();
                dc.ColumnName = "Phone";
                dtData.Columns.Add(dc);
                if (sheet != null && sheet.Cells.Rows.Count > 1)
                {
                    //sheet.Cells.FirstCell.

                    for (int i = 0; i < sheet.Cells.Rows.Count; i++)
                    {
                        DataRow dr = null;
                        for (int j = 0; j < sheet.Cells.MaxColumn + 1; j++)
                        {
                            string strCellValue = Convert.ToString(sheet.Cells[i, j].Value); 
                            // 从第1行开始为数据行
                            if (dr == null)
                            {
                                dr = dtData.NewRow();
                            }
                            dr[j] = strCellValue;

                        }
                        if (dr != null)
                        {
                            // 将创建的数据行添加到表格中
                            dtData.Rows.Add(dr);
                        }
                    }
                }
                return dtData;
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}
