using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

namespace IDCardNumber
{
    /// <summary>
    /// ���֤�������������
    /// </summary>
    public class IDCardNumber
    {
        #region ���֤��Ϣ����
        private string _province;
        /// <summary>
        /// ����ʡ����Ϣ
        /// </summary>
        public string Province
        {
            get { return _province; }
            set { _province = value; }
        }
        private string _area;
        /// <summary>
        /// ���ڵ�����Ϣ
        /// </summary>
        public string Area
        {
            get { return _area; }
            set { _area = value; }
        }
        private string _city;
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
        private DateTime _age;
        /// <summary>
        /// ����
        /// </summary>
        public DateTime Age
        {
            get { return _age; }
            set { _age = value; }
        }
        private int _sex;
        /// <summary>
        /// �Ա�0ΪŮ��1Ϊ��
        /// </summary>
        public int Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
        private string _cardnumber;
        /// <summary>
        /// ���֤����
        /// </summary>
        public string CardNumber
        {
            get { return _cardnumber; }
            set { _cardnumber = value; }
        }
        private string _json;
        /// <summary>
        /// ����Javascript����
        /// </summary>
        public string Json
        {
            get { return _json; }
            set { _json = value; }
        }

        #endregion

        #region ��̬����
        /// <summary>
        /// 
        /// </summary>
        private static readonly List<string[]> Areas = new List<string[]>();
        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        private static void FillAreas()
        {
            XmlDocument docXml = new XmlDocument();
            string fileXML = AppDomain.CurrentDomain.BaseDirectory + "/Config/AreaCodeInfo.xml";
            docXml.Load(fileXML);
            XmlNodeList nodelist = docXml.GetElementsByTagName("area");
            foreach (XmlNode node in nodelist)
            {
                string code = node.Attributes["code"].Value;
                string name = node.Attributes["name"].Value;
                IDCardNumber.Areas.Add(new string[] { code, name });
            }
        }
        /// <summary>
        /// �������֤��Ϣ
        /// </summary>
        /// <param name="idCardNumber"></param>
        /// <example>
        ///  IDCardNumber card = IDCardNumber.Get(code);
        /// </example>
        /// <returns>IDCardNumber</returns>
        public static IDCardNumber Get(string idCardNumber)
        {
            if (IDCardNumber.Areas.Count < 1)
                IDCardNumber.FillAreas();
            if (!IDCardNumber.CheckIDCardNumber(idCardNumber))
                throw new Exception("�Ƿ������֤����");
            //
            IDCardNumber cardInfo = new IDCardNumber(idCardNumber);
            return cardInfo;
        }
        /// <summary>
        /// У�����֤�����Ƿ�Ϸ�
        /// </summary>
        /// <param name="idCardNumber"></param>
        /// <returns></returns>
        public static bool CheckIDCardNumber(string idCardNumber)
        {
            //������֤
            Regex rg = new Regex(@"^\d{17}(\d|X)$");
            Match mc = rg.Match(idCardNumber);
            if (!mc.Success) return false;
            //��Ȩ��
            string code = idCardNumber.Substring(17, 1);
            double sum = 0;
            string checkCode = null;
            for (int i = 2; i <= 18; i++)
            {
                sum += int.Parse(idCardNumber[18 - i].ToString(), NumberStyles.HexNumber) * (Math.Pow(2, i - 1) % 11);
            }
            string[] checkCodes ={ "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            checkCode = checkCodes[(int)sum % 11];
            if (checkCode != code) return false;
            //
            return true;
        }
        /// <summary>
        /// �������һ�����֤��
        /// </summary>
        /// <returns></returns>
        public static IDCardNumber Radom()
        {
            long tick = DateTime.Now.Ticks;
            return new IDCardNumber(_radomCardNumber((int)tick));
        }
        /// <summary>
        /// �����������֤
        /// </summary>
        /// <param name="count"></param>
        /// <example> 
        /// List<IDCardNumber> list = IDCardNumber.Radom(number);
        /// </example>
        /// <returns></returns>
        public static List<IDCardNumber> Radom(int count)
        {
            List<IDCardNumber> list = new List<IDCardNumber>();
            string cardNumber;
            bool isExits;
            for (int i = 0; i < count; i++)
            {
                do
                {
                    isExits = false;
                    int tick = (int)DateTime.Now.Ticks;
                    cardNumber = IDCardNumber._radomCardNumber(tick * (i + 1));
                    foreach (IDCardNumber c in list)
                    {
                        if (c.CardNumber == cardNumber)
                        {
                            isExits = true;
                            break;
                        }
                    }

                } while (isExits);
                list.Add(new IDCardNumber(cardNumber));
            }
            return list;
        }
        /// <summary>
        /// ���������֤��
        /// </summary>
        /// <param name="seed">���������</param>
        /// <returns></returns>
        private static string _radomCardNumber(int seed)
        {
            if (IDCardNumber.Areas.Count < 1)
                IDCardNumber.FillAreas();
            System.Random rd = new System.Random(seed);
            //������ɷ�֤��
            string area = "";
            do
            {
                area = IDCardNumber.Areas[rd.Next(0, IDCardNumber.Areas.Count - 1)][0];
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
            string[] checkCodes ={ "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            checkCode = checkCodes[(int)sum % 11];
            codeNumber = codeNumber.Substring(0, 17) + checkCode;
            //
            return codeNumber;
        }
        #endregion

        #region ���֤��������

        private IDCardNumber(string idCardNumber)
        {
            this._cardnumber = idCardNumber;
            _analysis();
        }
        /// <summary>
        /// �������֤
        /// </summary>
        private void _analysis()
        {
            //ȡʡ�ݣ�����������
            string provCode = _cardnumber.Substring(0, 2).PadRight(6, '0');
            string areaCode = _cardnumber.Substring(0, 4).PadRight(6, '0');
            string cityCode = _cardnumber.Substring(0, 6).PadRight(6, '0');
            for (int i = 0; i < IDCardNumber.Areas.Count; i++)
            {
                if (provCode == IDCardNumber.Areas[i][0])
                    this._province = IDCardNumber.Areas[i][1];
                if (areaCode == IDCardNumber.Areas[i][0])
                    this._area = IDCardNumber.Areas[i][1];
                if (cityCode == IDCardNumber.Areas[i][0])
                    this._city = IDCardNumber.Areas[i][1];
                if (_province != null && _area != null && _city != null) break;
            }
            //ȡ����
            string ageCode = _cardnumber.Substring(6, 8);
            try
            {
                int year = Convert.ToInt16(ageCode.Substring(0, 4));
                int month = Convert.ToInt16(ageCode.Substring(4, 2));
                int day = Convert.ToInt16(ageCode.Substring(6, 2));
                _age = new DateTime(year, month, day);
            }
            catch
            {
                throw new Exception("�Ƿ��ĳ�������");
            }
            //ȡ�Ա�
            string orderCode = _cardnumber.Substring(14, 3);
            this._sex = Convert.ToInt16(orderCode) % 2 == 0 ? 0 : 1;
            //����Json����
            _json = @"prov:'{0}',area:'{1}',city:'{2}',year:'{3}',month:'{4}',day:'{5}',sex:'{6}',number:'{7}'";
            _json = string.Format(_json, _province, _area, _city, _age.Year, _age.Month, _age.Day, (_sex == 1 ? "boy" : "gril"), _cardnumber);
            _json = "{" + _json + "}";
        }
        #endregion
    }

}
