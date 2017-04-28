using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Configuration;
using System.IO;
using CRM_System.Model;
using CRM_System.DAL;
using System.Net;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.Web.Security;
using System.Drawing.Drawing2D;

namespace CRM_System.Tools
{
    public class Tools
    {
        /// <summary>
        /// 获取Json字符串某节点的值
        /// </summary>
        public static string GetJsonValue(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }
            return result;
        }
        private static ComSQLRepository sqlRep = new ComSQLRepository();
        /// <summary> 
        /// 中英文字符串截取
        /// </summary> 
        /// <param name="stringToSub">要转换的字符串</param>  
        /// <param name="length">长度</param>  
        /// <returns>中英文字符串截取</returns> 
        public static string GetFirstString(string stringToSub, int length)
        {
            stringToSub = NoHTML(stringToSub);
            length = length - 2;//-2
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = stringToSub.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;
            bool isCut = false;
            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength > length)
                {
                    isCut = true;
                    break;
                }
            }
            if (isCut)
                return sb.ToString() + "..";
            else
                return sb.ToString();
        }

        /// <summary> 
        /// Request参数过滤
        /// </summary> 
        /// <param name="str">要转换的字符串</param>  
        /// <returns>参数过滤</returns> 
        public static string FunStr(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("'", "''");
            str = str.Replace("*", "");
            str = str.Replace("\n", "<br/>");
            str = str.Replace("\r\n", "<br/>");
            //str   =   str.Replace("?","");   
            str = str.Replace("select", "");
            str = str.Replace("insert", "");
            str = str.Replace("update", "");
            str = str.Replace("delete", "");
            str = str.Replace("create", "");
            str = str.Replace("drop", "");
            str = str.Replace("delcare", "");
            str = str.Replace("   ", "&nbsp;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Trim();
            if (str.Trim().ToString() == "")
                str = "";//\u65e0
            return str;
        }

        /// <summary> 
        /// Request参数过滤
        /// </summary> 
        /// <param name="str">要转换的字符串</param>  
        /// <returns>参数过滤</returns> 
        public static string RevStr(string str)
        {
            str = str.Replace("&amp;", "&");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Trim();
            if (str.Trim().ToString() == "")
                str = "";
            return str;
        }

        /// <summary> 
        /// MD5加密
        /// </summary> 
        /// <param name="str">要转换的字符串</param>  
        /// <param name="i">16/32位</param>
        /// <returns>16/32位MD5</returns> 
        public static string ToMD5(string str)
        {
            string md5tmp = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");

            return md5tmp.ToLower();//.Substring(8, 32);
        }

        /// <summary> 
        /// 汉字转拼音缩写 
        /// </summary> 
        /// <param name="str">要转换的汉字字符串</param>  
        /// <returns>拼音缩写</returns> 
        public static string GetPYString(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {//字母和符号原样保留 
                    tempStr += c.ToString();
                }
                else
                {//累加拼音声母 
                    tempStr += GetPYChar(c.ToString());
                }
            }
            return tempStr;
        }

        /// <summary> 
        /// 取单个字符的拼音声母 
        /// Code By MuseStudio@hotmail.com 
        /// 2004-11-30 
        /// </summary> 
        /// <param name="c">要转换的单个汉字</param> 
        /// <returns>拼音声母</returns> 
        public static string GetPYChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));

            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "g";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";

            return "*";
        }

        /// <summary>
        /// 去除HMTL标签
        /// </summary>
        /// <param name="Htmlstring">原字符串</param>
        /// <returns>去除HMLT标签后的字符串</returns>
        public static string NoHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring) == true)
            {
                return string.Empty;
            }

            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<style[^>]*?>.*?</style>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<[^>]*>", "", RegexOptions.Compiled);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", " ", RegexOptions.Compiled);
            return Htmlstring.Replace("&nbsp;", " ");
        }
        /// <summary>
        /// 获取类似url传参字符串中 字符串的格式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string getUrlParam(string url, string param)
        {
            Regex reg = new Regex("(^|&)" + param + "=([^&]*)(&|$)");
            Match cc = reg.Match(url);
            if (!string.IsNullOrEmpty(cc.Value))
            {


                string val = cc.Value.Replace("&", "").Trim();
                int start = 1 + param.Length;
                int end = val.Length - 1 - param.Length;
                return val.Substring(start, end);
            }

            return "";
        }

        #region 获取天气信息
        /// <summary>
        /// 读取天气预报接口获取天气信息json字符串
        /// </summary>
        /// <param name="citycode"></param>
        /// <returns>读取天气预报接口获取天气信息json字符串</returns>
        public static LitJson.JsonData ReadWeatherJosnByCode(string citycode)
        {
            try
            {
                byte[] cityJson = null;
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    cityJson = wc.DownloadData(string.Format("http://m.weather.com.cn/data/{0}.html", citycode));
                }
                Encoding enc = Encoding.GetEncoding("utf-8");
                string jsonStr = enc.GetString(cityJson);

                LitJson.JsonData jd = LitJson.JsonMapper.ToObject(jsonStr);

                return jd;

            }
            catch (Exception ex)
            {

                throw ex;

            }

        }

        

        #endregion

        #region 检测字符串类型

        public static bool IsNum(string num)
        {
            return new Regex("^[0-9]*$").IsMatch(num);
        }

        #endregion

        #region dataTable/dataset转换成Json格式
        /// <summary>      
        /// dataTable转换成Json格式      
        /// </summary>      
        /// <param name="dt"></param>      
        /// <returns></returns>      
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(dt.TableName.ToString());
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        /// <summary>      
        /// DataSet转换成Json格式      
        /// </summary>      
        /// <param name="ds">DataSet</param>      
        /// <returns></returns>      
        public static string ToJson(DataSet ds)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"");
                json.Append(dt.TableName);
                json.Append("\":");
                json.Append(ToJson(dt));
                json.Append("}");
            }
            return json.ToString();
        }
        #endregion

        #region 获取当前IP信息
        /// <summary> 
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址 
        /// by flower.b 
        /// </summary> 
        public static string IPAddress
        {
            get
            {
                string result = String.Empty;
                try
                {


                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (result != null && result != String.Empty)
                    {
                        //可能有代理 
                        if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式 
                            result = null;
                        else
                        {
                            if (result.IndexOf(",") != -1)
                            {
                                //有“,”，估计多个代理。取第一个不是内网的IP。 
                                result = result.Replace(" ", "").Replace("'", "");
                                string[] temparyip = result.Split(",;".ToCharArray());
                                for (int i = 0; i < temparyip.Length; i++)
                                {
                                    if (IsIPAddress(temparyip[i])
                                        && temparyip[i].Substring(0, 3) != "10."
                                        && temparyip[i].Substring(0, 7) != "192.168"
                                        && temparyip[i].Substring(0, 7) != "172.16.")
                                    {
                                        return temparyip[i];    //找到不是内网的地址 
                                    }
                                }
                            }
                            else if (IsIPAddress(result)) //代理即是IP格式 
                                return result;
                            else
                                result = null;    //代理中的内容 非IP，取IP 
                        }

                    }

                    string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];



                    if (null == result || result == String.Empty)
                        result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                    if (result == null || result == String.Empty)
                        result = HttpContext.Current.Request.UserHostAddress;

                }
                catch (Exception ex)
                {
                    result = "";
                }
                return result;
            }
        }

        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }
        #endregion

        #region 页面跳转

        public static string ToPage(string mes, string url)
        {
            return string.Format("<script>alert('{0}');window.location.href='{1}'</script>", mes, url);
        }


        public static string ToTopPage(string mes, string url)
        {
            return string.Format("<script>alert('{0}');window.top.location.href='{1}'</script>", mes, url);
        }

        /// <summary>
        /// 判断是否为手机访问 true 为是
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static bool IsMobileDevice(System.Web.HttpRequest Request)
        {

            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino|ucweb|mqqbrowser", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (b.IsMatch(u))
            {
                return true;
            }
            return v.IsMatch(u.Substring(0, 4));
        }


        #endregion

        public static string GetWebUrl()
        {
            return "";
        }

        public static long GenerateIntID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        //流水号10+19形式生成
        public static string GetLS()
        {
            return string.Format("{0}{1}", DateTime.Now.ToString("yyMMddHHmm"), GenerateIntID());
        }
        /// <summary>
        /// 生成17为订单编号
        /// </summary>
        /// <returns>订单编号</returns>
        public static string GetOrderID() {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        public class CheckType
        {
            public static bool IsNum(string num)
            {
                return new Regex("^[0-9]*$").IsMatch(num);
            }
        }

        #region 判断是否是合法
        /// <summary>  
        /// 判断输入的字符串是否是一个合法的Email地址  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsEmail(string input)
        {
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
        #endregion

        #region 判断是否是合法手机号
        /// <summary>
        /// 判断输入的字符串是否是一个合法的手机号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(string input)
        {
            Regex regex = new Regex("^[1][3-8]\\d{9}$");
            return regex.IsMatch(input);

        }
        #endregion
         
        #region 邮箱显示格式
        public static string GetEmileFromat(string emile)
        {
            Regex reg = new Regex(".");
            int replacelLen = emile.IndexOf('@') - 3;
            emile = reg.Replace(emile, "*", replacelLen > 0 ? replacelLen : 0, 3);
            return emile;
        }
        #endregion

        #region 手机号显示格式
        public static string GetphoneFromat(string phone)
        {
            Regex reg = new Regex(".");
            phone = reg.Replace(phone, "*", 4, 3);
            return phone;
        }
        #endregion

        #region 性别
        public static string GetSexfromat(int sex)
        {
            if (sex == 0)
            {
                return "男";
            }
            else
            {
                return "女";
            }
        }
        #endregion

        #region 身份证显示格式
        public static string GetCardFromat(string cardid)
        {
            Regex reg = new Regex(".");
            cardid = reg.Replace(cardid, "*", 9, 6);
            return cardid;
        }
        #endregion

        #region 出生日期
        public static string GetCardByDate(string Card)
        {
            string strCard = "";
            strCard = Card.Substring(0, 4) + "-" + Card.Substring(4, 2) + "-" + Card.Substring(6, 2);
            return strCard;
        }
        #endregion

        

        public static string GetImgurl(string img)
        {
            if (string.IsNullOrEmpty(img))
            {
                return "";
            }
            string[] shuzu = img.Split(',');
            string strimg = "";
            for (int i = 0; i < shuzu.Length; i++)
            {
                strimg += "<a href=\"javascript:;\"><img src=\"" + shuzu[i] + "\" /></a>";
            }
            return strimg;
        }
        public static string GetImgurl2(string img, string rel)
        {
            if (string.IsNullOrEmpty(img))
            {
                return "";
            }
            string[] shuzu = img.Split(',');
            string strimg = "";
            for (int i = 0; i < shuzu.Length; i++)
            {
                strimg += "<a href=\"" + shuzu[i] + "\" rel=\"[gall" + rel + "]\"><img src=\"" + shuzu[i] + "\" /></a>";
            }
            return strimg;
        }
        public static string Getsfwc(string varture, string name, string valname)
        {
            string strval = "";
            if (varture == "true" && name == valname)
            {
                strval += "<img src=\"/Content/Web/images/shangb.jpg\" />";
            }
            else
            {
                strval += "<img src=\"/Content/Web/images/xiab.jpg\" />";
            }
            return strval;
        }

        public static string GetAreaPrice(string id, string area)
        {
            string sql = string.Format("select Price from  sys_product_price where PID='{0}' and AreaName='{1}'", id, area);
            object obj = sqlRep.GetSingle(sql);
            if (obj == null)
            {
                return "";
            }
            return obj.ToString();
        }
        public static string GetCoinfo(string id, string area)
        {
            string sql = string.Format("select top 1 * from  sys_product_price where PID='{0}' and AreaName='{1}' order by addtime desc", id, area);
            string info = "";
            DataTable dt = sqlRep.Query(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                info += dt.Rows[i]["GroupId"].ToString() + ":" + dt.Rows[i]["Price"].ToString() + ",";
            }

            return info.TrimEnd(',');
        }


        public static string GetModelValue(string FieldName, object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                object o = Ts.GetProperty(FieldName).GetValue(obj, null);
                string Value = Convert.ToString(o);
                if (string.IsNullOrEmpty(Value)) return null;
                return Value;
            }
            catch
            {
                return null;
            }
        }

        public static string GetQujian(string params1, string params2, string fuhao)
        {

            if (params1 == params2 && params2 == "0.00")
            {
                return "--";
            }

            string strval = "0";
            if (params1 != "0.00")
            {
                strval = params1;
            }
            if (params2 != "0.00")
            {
                strval += fuhao + params2;
            }
            return strval;
        }

        public static string GetCountBN(int shur)
        {
            string strval = "";
            int shengyu = 8 - shur.ToString().Length;
            if (shengyu != 0)
            {
                for (int i = 0; i < shengyu; i++)
                {
                    strval = strval + "0";
                }
                return strval + shur.ToString();
            }
            return shur.ToString();
        }

        public static string GetHtmlTheme(string strfile)
        {
            
            string strout;
            strout = "";

            string weburl = string.Format("http://{0}/", ConfigurationManager.AppSettings["domain"]);

            if (!System.IO.File.Exists(strfile))
            {
            }
            else
            {
                StreamReader sr = new StreamReader(strfile, System.Text.Encoding.UTF8);
                String input = sr.ReadToEnd();
                sr.Close();
                strout = input;
            }
            return strout.Replace("{domail}", weburl);
        }
        public static int[] GetRandomTen(int minValue, int maxValue, int count)
        {

            Random rnd = new Random();
            int length = maxValue - minValue + 1;
            byte[] keys = new byte[length];
            rnd.NextBytes(keys);
            int[] items = new int[length];
            for (int i = 0; i < length; i++)
            {
                items[i] = i + minValue;
            }
            Array.Sort(keys, items);
            int[] result = new int[count];
            Array.Copy(items, result, count);
            return result;

        }
        public static string GettenNum()
        {
            string chars = "0123456789";

            Random randrom = new Random((int)DateTime.Now.Ticks);

            string str = "";
            for (int i = 0; i < 6; i++)
            {
                str += chars[randrom.Next(chars.Length)];
            }
            return str;
        }
        ////////////////////////////////////////
        private static string[] strs = new string[]
                                 {
                                  "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                                  "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
                                 };
        private static string[] strsInt = new string[]
                                 {
                                  "1","2","3","4","5","6","7","8","9","0"
                                 };
        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <returns></returns>
        public static string CreatenNonce_str()
        {
            Random r = new Random();
            var sb = new StringBuilder();
            var length = strs.Length;
            for (int i = 0; i < 15; i++)
            {
                sb.Append(strs[r.Next(length - 1)]);
            }
            return sb.ToString();
        }
        /// <summary> 
        /// 创建随机字符串 可选择数量
        /// </summary>
        /// <returns></returns>
        public static string CreatenNonce_str(int Num)
        {
            Random r = new Random();
            var sb = new StringBuilder();
            var length = strs.Length;
            for (int i = 0; i < Num; i++)
            {
                sb.Append(strs[r.Next(length - 1)]);
            }
            return sb.ToString();
        }
        /// <summary> 
        /// 创建纯数字字符串 可选择数量
        /// </summary>
        /// <returns></returns>
        public static string CreatenNonce_strInt(int Num)
        {
            Random r = new Random();
            var sb = new StringBuilder();
            var length = strsInt.Length;
            for (int i = 0; i < Num; i++)
            {
                sb.Append(strsInt[r.Next(length - 1)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 创建时间戳  
        /// </summary>
        /// <returns></returns>
        public static long CreatenTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
        /// </summary>
        /// 返回signature值
        /// <param name="timestamp"></param>
        /// <param name="nonceStr"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetSignature(string jsapi_ticket, long timestamp, string nonceStr, string url)
        {
            try
            {
                //这里是获取jsapi_ticket  方法就不粘贴了
                //对所有待签名参数按照字段名的ASCII 码从小到大排序
                var string1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapi_ticket, nonceStr, timestamp, url);
                // 对string1进行sha1签名，得到signature
                var signature = FormsAuthentication.HashPasswordForStoringInConfigFile(string1, "SHA1");
                return signature;
            }
            catch
            {

                return string.Empty;
            }
        }
        /// <summary>
        /// 根据链接获取二维码
        /// </summary>
        /// <param name="link">链接</param>
        /// <returns>返回二维码图片</returns>
        public static Bitmap GetDimensionalCode(string link)
        {
            Bitmap bmp = null;
            try
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 4;
                qrCodeEncoder.QRCodeVersion = 0;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                bmp = qrCodeEncoder.Encode(link, Encoding.UTF8);
                int width = bmp.Width / 10;
                int dwidth = width * 2;
                Bitmap bmp2 = new Bitmap(bmp.Width + dwidth, bmp.Height + dwidth);
                Graphics g = Graphics.FromImage(bmp2);
                System.Drawing.Color c = System.Drawing.Color.White;
                g.FillRectangle(new SolidBrush(c), 0, 0, bmp.Width + dwidth, bmp.Height + dwidth);
                g.DrawImage(bmp, width, width);
                g.Dispose();
                return bmp2;
            }
            catch (Exception ex)
            {

            }
            return bmp;
        }
        /// <summary>  
        /// 调用此函数后使此两种图片合并，类似相册，有个  
        /// 背景图，中间贴自己的目标图片  
        /// </summary>  
        /// <param name="sourceImg">粘贴的源图片</param>  
        /// <param name="destImg">粘贴的目标图片</param>  
        public static System.Drawing.Image CombinImage(System.Drawing.Image imgBack, string destImg)
        {

            System.Drawing.Image img = System.Drawing.Image.FromFile(destImg);        //照片图片    
            if (img.Height != 50 || img.Width != 50)
            {
                img = KiResizeImage(img, 50, 50, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);      //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);   

            g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1, 1, 1);//相片四周刷一层黑色边框  

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }
        /// <summary>  
        /// Resize图片  
        /// </summary>  
        /// <param name="bmp">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <param name="Mode">保留着，暂时未用</param>  
        /// <returns>处理以后的图片</returns>  
        public static System.Drawing.Image KiResizeImage(System.Drawing.Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                System.Drawing.Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量  
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }
        #region URL中去除指定参数
        /// <summary> 
        /// 中去除指定参数 
        /// </summary> 
        /// <param name="url">地址</param> 
        /// <param name="param">参数</param> 
        /// <returns></returns> 
        public static string buildurl(string url, string param)
        {
            string url1 = url;
            if (url.IndexOf(param) > 0)
            {
                if (url.IndexOf("&", url.IndexOf(param) + param.Length) > 0)
                {
                    url1 = url.Substring(0, url.IndexOf(param) - 1) + url.Substring(url.IndexOf("&", url.IndexOf(param) + param.Length) + 1);
                }
                else
                {
                    url1 = url.Substring(0, url.IndexOf(param) - 1);
                }
                return url1;
            }
            else
            {
                return url1;
            }
        }
        public enum enumWebrequestContextType
        {
            [Description("application/x-www-form-urlencoded")]
            form,
            [Description("application/json")]
            json,
            [Description("application/xml")]
            xml
        }
        /// <summary>
        /// 让枚举中使用特殊字符。
        /// 原理：让枚举中的值都拥有Attribute属性，然后通过在属性里设置特殊字符。从而返回属性的ToString()
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetEnumName(Enum en)
        {

            Type temType = en.GetType();
            MemberInfo[] memberInfos = temType.GetMember(en.ToString());
            if (memberInfos != null && memberInfos.Length > 0)
            {
                object[] objs = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs != null && objs.Length > 0)
                {
                    return ((DescriptionAttribute)objs[0]).Description;
                }
            }
            return en.ToString();
        }
        public static string PostWebRequest(string postUrl, string paramData)
        {
            return PostWebRequest(postUrl, paramData, Encoding.UTF8);
        }

        public static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }
        #endregion 

        #region 入参结束时间返回当前时间和结束时间之间的天数
        public  static int  GetEndDay(DateTime EndTime)
        {
            double Dayinfo = (EndTime - DateTime.Now).TotalDays;
            int GetDay = (EndTime - DateTime.Now).Days;
            if (Dayinfo > GetDay)
            {
                return (GetDay + 1);
            }
            else {
                return GetDay;
            }
        }
        #endregion
        #region 返回当前星期几
        public static string GetDateTimeWeek(DateTime InDate)
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = Day[Convert.ToInt32(InDate.DayOfWeek.ToString("d"))].ToString();
            return week;
        }
        #endregion
        #region  数字转换为字符串
        public static string GetABC(int Number)
        {
            if (1 <= Number && 36 >= Number)
            {
                int num = Number + 64;
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] btNumber = new byte[] { (byte)num };
                return asciiEncoding.GetString(btNumber);
            }
            else {
                return "";
            }
        }
        #endregion


        /// <summary>

        /// 根据身份证号获取出生日期和性别

        /// </summary>

        /// <param name="identityCard">输入的身份证号码</param>

        /// <returns>出身日期</returns>

        public static string GetBirthdayAndSex(string identityCard, ref int outsex, ref string birthday) 
        {
            string sex = "";
            if (string.IsNullOrEmpty(identityCard)) 
            { 
                //MessageBox.Show("身份证号码不能为空！");//身份证号码不能为空，如果为空返回 
                
                return "身份证号码不能为空";

            } 
            else 
            { 
                if (identityCard.Length != 15 && identityCard.Length != 18)//身份证号码只能为15位或18位其它不合法 
                { 
                    //MessageBox.Show("身份证号码为15位或18位，请检查！"); 
                    return "身份证号码为15位或18位，请检查"; 
                } 
            } 
            if (identityCard.Length == 18)//处理18位的身份证号码从号码中得到生日和性别代码 
            { 
                birthday = identityCard.Substring(6, 4) + "-" + identityCard.Substring(10, 2) + "-" + identityCard.Substring(12, 2); 
                sex = identityCard.Substring(14, 3); 
            } 
            if (identityCard.Length == 15) 
            { 
                birthday = "19" + identityCard.Substring(6, 2) + "-" + identityCard.Substring(8, 2) + "-" + identityCard.Substring(10, 2); 
                sex = identityCard.Substring(12, 3);

            } 
            if (int.Parse(sex) % 2 == 0)//性别代码为偶数是女性奇数为男性 
            {
                outsex = 0; 
            } 
            else 
            {
                outsex = 1; 
            }
            return "success";
        }
    }
}
