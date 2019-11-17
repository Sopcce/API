using System;
using System.Configuration;
using Aliyun.OSS.Common;

namespace Aliyun.OSS.Samples
{
    /// <summary>
    /// 
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 
        /// </summary>
        public static string productVersion = ConfigurationManager.AppSettings["ProductVersion"];
        /// <summary>
        /// 
        /// </summary>
        public static string bucketName = ConfigurationManager.AppSettings["AliyunOSS.BucketName"];


        /// <summary>
        /// 
        /// </summary>
        public static string Endpoint = ConfigurationManager.AppSettings["AliyunOSS.Endpoint.API"];

        /// <summary>
        ///  Access Key ID 
        ///  https://ak-console.aliyun.com/#/accesskey
        /// </summary>
        public static string AccessKeyId = ConfigurationManager.AppSettings["AliyunOSS.AccessKeyID"];

        /// <summary>
        /// Access Key Secret
        /// https://ak-console.aliyun.com/#/accesskey
        /// </summary>
        public static string AccessKeySecret = ConfigurationManager.AppSettings["AliyunOSS.AccessKeySecret"];
        /// <summary>
        /// 
        /// </summary>
 
        public static string DirToDownload = Endpoint+ "/DirToDownload";

        public static string FileToUpload = Endpoint+ "/FileToUpload";

        public static string BigFileToUpload = Endpoint + "/BigFileToUpload";
    }
}