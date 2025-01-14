﻿/****************************************************************** 
** File Name:IPScaner.cs 
** Copyright (c) 2004-2005 PPTech Studio(PPTech.Net) 
** Creater:Rexsp(MSN:yubo@x263.net) 
** Create Date:2004-12-27 20:10:28 
** Modifier: 
** Modify Date: 
** Description:to scan the ip location from qqwry.dat 
** Version: IPScaner 1.0.0 
******************************************************************/ 
using System; 
using System.IO; 
using System.Collections; 
using System.Text; 
using System.Text.RegularExpressions; 
namespace WebIPDemo.IPData
{ 
    /**//// <summary> 
    ///IP获取信息qqwry.dat
    /// </summary> 
    public class IPScaner1
    {
        #region  私有成员

        private string dataPath;
        private string ip;
        private string country;
        private string local;

        private long firstStartIp = 0;
        private long lastStartIp = 0;
        private FileStream objfs = null;
        private long startIp = 0;
        private long endIp = 0;
        private int countryFlag = 0;
        private long endIpOff = 0;
        private string errMsg = null;


        #endregion

        #region 公共属性 
        /// <summary>
        /// qqwry.dat数据存储路径（服务路径和物理路径都可以）
        /// </summary>
        public string DataPath 
        { 
            set{dataPath=value;} 
        } 
        /// <summary>
        /// IP
        /// </summary>
        public string IP 
        { 
            set{ip=value;} 
        } 
        /// <summary>
        /// 省市地区
        /// </summary>
        public string Country 
        { 
            get{return country;} 
        } 
        public string Local 
        { 
            get{return local;} 
        } 
        public string ErrMsg 
        { 
            get{return errMsg;} 
        }

        #endregion
        //
        /// <summary>
        ///搜索匹配数据 
        /// </summary>
        /// <returns></returns>
        private int QQwry() 
        { 
            string pattern = @"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))"; 
            Regex objRe = new Regex(pattern); 
            Match objMa = objRe.Match(ip); 
            if(!objMa.Success) 
            { 
                this.errMsg="IP格式错误"; 
                return 4; 
            } 

            long ip_Int = this.IpToInt(ip); 
            int nRet=0; 
            if(ip_Int>=IpToInt("127.0.0.0")&&ip_Int<=IpToInt("127.255.255.255")) 
            { 
                this.country="本机内部环回地址"; 
                this.local=""; 
                nRet=1; 
            } 
            else if((ip_Int>=IpToInt("0.0.0.0")&&ip_Int<=IpToInt("2.255.255.255"))||(ip_Int>=IpToInt("64.0.0.0")&&ip_Int<=IpToInt("126.255.255.255"))||(ip_Int>=IpToInt("58.0.0.0")&&ip_Int<=IpToInt("60.255.255.255"))) 
            { 
                this.country="网络保留地址"; 
                this.local=""; 
                nRet=1; 
            } 
            objfs = new FileStream(this.dataPath, FileMode.Open, FileAccess.Read); 
            try 
            { 
                //objfs.Seek(0,SeekOrigin.Begin); 
                objfs.Position=0; 
                byte[] buff = new Byte[8] ; 
                objfs.Read(buff,0,8); 
                firstStartIp=buff[0]+buff[1]*256+buff[2]*256*256+buff[3]*256*256*256; 
                lastStartIp=buff[4]*1+buff[5]*256+buff[6]*256*256+buff[7]*256*256*256; 
                long recordCount=Convert.ToInt64((lastStartIp-firstStartIp)/7.0); 
                if(recordCount<=1) 
                { 
                    country="FileDataError"; 
                    objfs.Close(); 
                    return 2; 
                } 
                long rangE=recordCount; 
                long rangB=0; 
                long recNO=0; 
                while(rangB<rangE-1) 
                { 
                    recNO=(rangE+rangB)/2; 
                    this.GetStartIp(recNO); 
                    if(ip_Int==this.startIp) 
                    { 
                        rangB = recNO; 
                        break; 
                    } 
                    if(ip_Int>this.startIp) 
                        rangB=recNO; 
                    else 
                        rangE=recNO; 
                } 
                this.GetStartIp(rangB); 
                this.GetEndIp(); 
                if(this.startIp<=ip_Int&&this.endIp>=ip_Int) 
                { 
                    this.GetCountry(); 
                    this.local=this.local.Replace("（我们一定要解放台湾！！！）",""); 
                } 
                else 
                { 
                    nRet=3; 
                    this.country="未知"; 
                    this.local=""; 
                } 
                objfs.Close(); 
                return nRet; 
            } 
            catch 
            { 
                return 1; 
            } 

        } 

       /// <summary>
        /// IP地址转换成Int数据 
       /// </summary>
       /// <param name="ip">IP地址</param>
       /// <returns>int类型ip</returns>
        private long IpToInt(string ip) 
        { 
            char[] dot = new char[]{'.'}; 
            string [] ipArr = ip.Split(dot); 
            if(ipArr.Length==3) 
                ip=ip+".0"; 
            ipArr=ip.Split(dot); 

            long ip_Int=0; 
            long p1=long.Parse(ipArr[0])*256*256*256; 
            long p2=long.Parse(ipArr[1])*256*256; 
            long p3=long.Parse(ipArr[2])*256; 
            long p4=long.Parse(ipArr[3]); 
            ip_Int=p1+p2+p3+p4; 
            return ip_Int; 
        } 

        /// <summary>
        /// int转换成IP
        /// </summary>
        /// <param name="ip_Int"></param>
        /// <returns></returns>
 
        private string IntToIP(long ip_Int) 
        { 
            long seg1=(ip_Int&0xff000000)>>24; 
            if(seg1<0) 
                seg1+=0x100; 
            long seg2=(ip_Int&0x00ff0000)>>16; 
            if(seg2<0) 
                seg2+=0x100; 
            long seg3=(ip_Int&0x0000ff00)>>8; 
            if(seg3<0) 
                seg3+=0x100; 
            long seg4=(ip_Int&0x000000ff); 
            if(seg4<0) 
                seg4+=0x100; 
            string ip=seg1.ToString()+"."+seg2.ToString()+"."+seg3.ToString()+"."+seg4.ToString(); 

            return ip; 
        } 
        /// <summary>
        /// 获取起始IP范围
        /// </summary>
        /// <param name="recNO"></param>
        /// <returns></returns>
        private long GetStartIp(long recNO) 
        { 
            long offSet = firstStartIp+recNO*7; 
            //objfs.Seek(offSet,SeekOrigin.Begin); 
            objfs.Position=offSet; 
            byte [] buff = new Byte[7]; 
            objfs.Read(buff,0,7); 

            endIpOff=Convert.ToInt64(buff[4].ToString())+Convert.ToInt64(buff[5].ToString())*256+Convert.ToInt64(buff[6].ToString())*256*256; 
            startIp=Convert.ToInt64(buff[0].ToString())+Convert.ToInt64(buff[1].ToString())*256+Convert.ToInt64(buff[2].ToString())*256*256+Convert.ToInt64(buff[3].ToString())*256*256*256; 
            return startIp; 
        } 
        /// <summary>
        /// 获取结束IP 
        /// </summary>
        /// <returns></returns>
        private long GetEndIp() 
        { 
            //objfs.Seek(endIpOff,SeekOrigin.Begin); 
            objfs.Position=endIpOff; 
            byte [] buff = new Byte[5]; 
            objfs.Read(buff,0,5); 
            this.endIp=Convert.ToInt64(buff[0].ToString())+Convert.ToInt64(buff[1].ToString())*256+Convert.ToInt64(buff[2].ToString())*256*256+Convert.ToInt64(buff[3].ToString())*256*256*256; 
            this.countryFlag=buff[4]; 
            return this.endIp; 
        } 
       /// <summary>
        /// 获取国家/区域偏移量
       /// </summary>
       /// <returns></returns>
        private string GetCountry() 
        { 
            switch(this.countryFlag) 
            { 
                case 1: 
                case 2: 
                    this.country=GetFlagStr(this.endIpOff+4); 
                    this.local=( 1 == this.countryFlag )?" ":this.GetFlagStr(this.endIpOff+8); 
                    break; 
                default: 
                    this.country=this.GetFlagStr(this.endIpOff+4); 
                    this.local=this.GetFlagStr(objfs.Position); 
                    break; 
            } 
            return " "; 
        } 
        /// <summary>
        /// 获取国家/区域字符串 
        /// </summary>
        /// <param name="offSet"></param>
        /// <returns></returns>
        private string GetFlagStr(long offSet) 
        { 
            int flag=0; 
            byte [] buff = new Byte[3]; 
            while(1==1) 
            { 
                //objfs.Seek(offSet,SeekOrigin.Begin); 
                objfs.Position=offSet; 
                flag = objfs.ReadByte(); 
                if(flag==1||flag==2) 
                { 
                    objfs.Read(buff,0,3); 
                    if(flag==2) 
                    { 
                        this.countryFlag=2; 
                        this.endIpOff=offSet-4; 
                    } 
                    offSet=Convert.ToInt64(buff[0].ToString())+Convert.ToInt64(buff[1].ToString())*256+Convert.ToInt64(buff[2].ToString())*256*256; 
                } 
                else 
                { 
                    break; 
                } 
            } 
            if(offSet<12) 
                return " "; 
            objfs.Position=offSet; 
            return GetStr(); 
        } 
       /// <summary>
        /// GetStr 
       /// </summary>
       /// <returns></returns>
        private string GetStr() 
        { 
            byte lowC=0; 
            byte upC=0; 
            string str=""; 
            byte[] buff = new byte[2]; 
            while(1==1) 
            { 
                lowC= (Byte)objfs.ReadByte(); 
                if(lowC==0) 
                    break; 
                if(lowC>127) 
                { 
                    upC=(byte)objfs.ReadByte(); 
                    buff[0]=lowC; 
                    buff[1]=upC; 
                    System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312"); 
                    str+=enc.GetString(buff); 
                } 
                else 
                { 
                    str+=(char)lowC; 
                } 
            } 
            return str; 
        }      
        /// <summary>
        /// 获取IP地址 
        /// </summary>
        /// <returns></returns>
 
        public string IPLocation() 
        { 
            this.QQwry(); 
            return this.country+this.local; 
        } 
        public string IPLocation(string dataPath,string ip) 
        { 
            this.dataPath=dataPath; 
            this.ip=ip; 
            this.QQwry(); 
            return this.country+this.local; 
        } 
    
    } 
} 
    
