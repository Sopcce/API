using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace IDCard
{
    /// <summary>
    /// ���֤�������������
    /// </summary>
    public class IdCardInfo
    {
        public IdCardInfo(AreasType areasType)
        {
            AreasType = areasType;
        }
        public IdCardInfo()
        {
            AreasType = AreasType.XML;
        }

        #region MyRegion
        /// <summary>
        /// ����ʡ����Ϣ
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// ���ڵ�����Ϣ
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public DateTime Age { get; set; }
        /// <summary>
        /// �Ա�0ΪŮ��1Ϊ��
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// ���֤����
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// ����Javascript����
        /// </summary>
        public string Json { get; set; }

        public AreasType AreasType { get; set; } = AreasType.XML;

        private List<string[]> areas = new List<string[]>();

        /// <summary>
        /// 
        /// </summary>
        public List<string[]> GetAreas()
        {
            if (AreasType.Json == AreasType)
            {
                string fileJson = AppDomain.CurrentDomain.BaseDirectory + "/Config/AreaCodeInfo.json";
                string json = File.ReadAllText(fileJson, System.Text.Encoding.UTF8);
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<Area>(json, new Newtonsoft.Json.JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });




            }
            switch (AreasType)
            {
                case AreasType.Json:




                    break;
                case AreasType.XML:
                    System.Xml.XmlDocument docXml = new System.Xml.XmlDocument();
                    string fileXML = AppDomain.CurrentDomain.BaseDirectory + "/Config/AreaCodeInfo.xml";
                    docXml.Load(fileXML);
                    System.Xml.XmlNodeList nodelist = docXml.GetElementsByTagName("area");
                    foreach (System.Xml.XmlNode node in nodelist)
                    {
                        string code = node.Attributes["code"].Value;
                        string name = node.Attributes["name"].Value;
                        areas.Add(new string[] { code, name });
                    }
                    break;
                case AreasType.Input:
                    break;
                default:
                    break;
            }

            return areas;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetAreas(List<string[]> value)
        {
            areas = value;
        }
        #endregion




        #region ��̬����

        /// <summary>
        /// �������֤��Ϣ
        /// </summary>
        /// <param name="idCardNumber"></param>
        /// <example>
        ///  IDCardNumber card = IDCardNumber.Get(code);
        /// </example>
        /// <returns>IDCardNumber</returns>
        public IdCardInfo GetIdCardInfo(string idCardNumber)
        {
            if (!CheckIDCardNumber(idCardNumber))
                throw new Exception("�Ƿ������֤����");

            this.AreasType = AreasType.Json;
            if (this.GetAreas().Count < 1)
                throw new Exception("������Ϣ������");
            IdCardInfo cardInfo = new IdCardInfo(idCardNumber);
            return cardInfo;
        }
        /// <summary>
        /// �������һ�����֤��
        /// </summary>
        /// <returns></returns>
        public IdCardInfo GetRadomIdCard()
        {
            long tick = DateTime.Now.Ticks;
            return new IdCardInfo(_radomCardNumber((int)tick));
        }
        /// <summary>
        /// У�����֤�����Ƿ�Ϸ�
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public bool CheckIDCardNumber(string idCard)
        {
            //������֤
            Regex rg = new Regex(@"^\d{17}(\d|X)$");
            Match mc = rg.Match(idCard);
            if (!mc.Success) return false;
            //��Ȩ��
            string code = idCard.Substring(17, 1);
            double sum = 0;
            string checkCode = null;
            for (int i = 2; i <= 18; i++)
            {
                sum += int.Parse(idCard[18 - i].ToString(), NumberStyles.HexNumber) * (Math.Pow(2, i - 1) % 11);
            }
            string[] checkCodes = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            checkCode = checkCodes[(int)sum % 11];
            if (checkCode != code) return false;
            //
            return true;
        }

        /// <summary>
        /// �����������֤
        /// </summary>
        /// <param name="count"></param>
        /// <example> 
        /// List<IDCardNumber> list = IDCardNumber.Radom(number);
        /// </example>
        /// <returns></returns>
        public List<IdCardInfo> GetRadomIdCardList(int count)
        {
            List<IdCardInfo> list = new List<IdCardInfo>();
            string cardNumber;
            bool isExits;
            for (int i = 0; i < count; i++)
            {
                do
                {
                    isExits = false;
                    int tick = (int)DateTime.Now.Ticks;
                    cardNumber = _radomCardNumber(tick * (i + 1));
                    foreach (IdCardInfo c in list)
                    {
                        if (c.CardNumber == cardNumber)
                        {
                            isExits = true;
                            break;
                        }
                    }

                } while (isExits);
                list.Add(new IdCardInfo(cardNumber));
            }
            return list;
        }

        #endregion

        #region private
        /// <summary>
        /// ���������֤��
        /// </summary>
        /// <param name="seed">���������</param>
        /// <returns></returns>
        private string _radomCardNumber(int seed)
        {
            System.Random rd = new System.Random(seed);
            //������ɷ�֤��
            string area = "";
            do
            {
                area = GetAreas()[rd.Next(0, GetAreas().Count - 1)][0];
            } while (area.Substring(4, 2) == "00");
            //�����������
            DateTime birthday = DateTime.Now;
            birthday = birthday.AddYears(-rd.Next(16, 60));
            birthday = birthday.AddMonths(-rd.Next(0, 12));
            birthday = birthday.AddDays(-rd.Next(0, 31));
            //�����
            string code = rd.Next(1000, 9999).ToString("####");
            //�����������֤��
            string codeNumber = area + birthday.ToString("yyyyMMdd") + code;
            double sum = 0;
            string checkCode = null;
            for (int i = 2; i <= 18; i++)
            {
                sum += int.Parse(codeNumber[18 - i].ToString(), NumberStyles.HexNumber) * (Math.Pow(2, i - 1) % 11);
            }
            string[] checkCodes = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            checkCode = checkCodes[(int)sum % 11];
            codeNumber = codeNumber.Substring(0, 17) + checkCode;
            //
            return codeNumber;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCardNumber"></param>
        private IdCardInfo(string idCardNumber)
        {
            this.CardNumber = idCardNumber;
            _analysis();
        }



        /// <summary>
        /// �������֤
        /// </summary>
        private void _analysis()
        {
            //ȡʡ�ݣ�����������
            string provCode = CardNumber.Substring(0, 2).PadRight(6, '0');
            string areaCode = CardNumber.Substring(0, 4).PadRight(6, '0');
            string cityCode = CardNumber.Substring(0, 6).PadRight(6, '0');
            for (int i = 0; i < GetAreas().Count; i++)
            {
                if (provCode == GetAreas()[i][0])
                    this.Province = GetAreas()[i][1];
                if (areaCode == GetAreas()[i][0])
                    this.Area = GetAreas()[i][1];
                if (cityCode == GetAreas()[i][0])
                    this.City = GetAreas()[i][1];
                if (Province != null && Area != null && City != null) break;
            }
            //ȡ����
            string ageCode = CardNumber.Substring(6, 8);
            try
            {
                int year = Convert.ToInt16(ageCode.Substring(0, 4));
                int month = Convert.ToInt16(ageCode.Substring(4, 2));
                int day = Convert.ToInt16(ageCode.Substring(6, 2));
                Age = new DateTime(year, month, day);
            }
            catch
            {
                throw new Exception("�Ƿ��ĳ�������");
            }
            //ȡ�Ա�
            string orderCode = CardNumber.Substring(14, 3);
            this.Sex = Convert.ToInt16(orderCode) % 2 == 0 ? 0 : 1;
            //����Json����
            Json = @"prov:'{0}',area:'{1}',city:'{2}',year:'{3}',month:'{4}',day:'{5}',sex:'{6}',number:'{7}'";
            Json = string.Format(Json, Province, Area, City, Age.Year, Age.Month, Age.Day, (Sex == 1 ? "boy" : "gril"), CardNumber);
            Json = "{" + Json + "}";
        }
        #endregion
    }

}
