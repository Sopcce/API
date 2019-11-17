using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using WebIPDemo.IPData;

namespace IP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //string a=Server.MapPath("./Config/qqwry.dat");
            //IPScanner qqWry = new IPScanner(a);

            //IPLocation ip = qqWry.Query("182.40.42.141");
            //ip.ToString();

            //获得本地IP
            //string ip = Request.UserHostAddress.ToString();


            IPScaner1 objScan = new IPScaner1(); 
            objScan.DataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\ip.dat");
            objScan.IP = "111.201.148.10";
            string addre = objScan.IPLocation();
            //省市地区
            string Country = objScan.Country;
            //服务商
            string Local = objScan.Local;
            string text = string.Format("<br />国家:{0},{1},{2}", addre, Country, Local);
            Console.WriteLine(text);


            string url = "http://apis.baidu.com/apistore/iplookupservice/iplookup";
            string param = "ip=111.201.148.10";
            string result = request(url, param);

            //Hashtable HTFirst = (Hashtable)JSON.JsonDecode(result);
            //Hashtable HTsecond = (Hashtable)HTFirst["retData"];


            //string country = Convert.ToString(HTsecond["country"]);
            //string province = Convert.ToString(HTsecond["province"]);
            //string city = Convert.ToString(HTsecond["city"]);  //如果需要其他数据，依次读取即可
            //string district = Convert.ToString(HTsecond["district"]);
            //string carrier = Convert.ToString(HTsecond["carrier"]);

            //string txt = string.Format("<br />国家:{0},{1},{2},{3},{4}", country, province, city, district, carrier);



        }
        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        public string GetAddressIP()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        /// <returns>请求结果</returns>
        public static string request(string url, string param)
        {
            string strURL = url + '?' + param;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "GET";
            // 添加header
            request.Headers.Add("apikey", "6c8db4a9f254871b1056202803ea8fbc");
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;
        }
    }
   
}
