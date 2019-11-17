using System;
using Aliyun.OSS.Common;
using System.Configuration;
using System.Reflection;
using System.IO;
using Aliyun.OSS;
using Microsoft.Win32;

namespace Aliyun.OSS.Samples
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {

        private static OssClient ossClient;
      

        /// <summary>
        /// SDK的示例程序
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("Aliyun 案例");

            ossClient = new OssClient(Config.Endpoint, Config.AccessKeyId, Config.AccessKeySecret);
            var homePath = Assembly.GetExecutingAssembly().Location;
            homePath = homePath.Substring(0, homePath.LastIndexOf('\\'));
            UploadDirectory(new DirectoryInfo(homePath), Config.productVersion);
            ObjectListing str = ossClient.ListObjects(Config.bucketName);
            
            #region MyRegion


            try
            {
                #region 管理Bucket
                //创建一个新的存储空间（Bucket）
                //CreateBucketSample.CreateBucket(bucketName);
                //列出账户下所有的存储空间信息
                //ListBucketsSample.ListBuckets();
                //判断存储空间是否存在
                //DoesBucketExistSample.DoesBucketExist(bucketName);

                //SetBucketCorsSample.SetBucketCors(bucketName);
                #endregion






                // GetBucketCorsSample.GetBucketCors(bucketName);

                //DeleteBucketCorsSample.DeleteBucketCors(bucketName);

                //SetBucketLoggingSample.SetBucketLogging(bucketName);

                //GetBucketLoggingSample.GetBucketLogging(bucketName);

                //DeleteBucketLoggingSample.DeleteBucketLogging(bucketName);

                //SetBucketAclSample.SetBucketAcl(bucketName);

                //GetBucketAclSample.GetBucketAcl(bucketName);

                //SetBucketWebsiteSample.SetBucketWebsite(bucketName);

                //GetBucketWebsiteSample.GetBucketWebsite(bucketName);

                //DeleteBucketWebsiteSample.DeleteBucketWebsite(bucketName);

                //SetBucketRefererSample.SetBucketReferer(bucketName);

                //GetBucketRefererSample.GetBucketReferer(bucketName);

                //SetBucketLifecycleSample.SetBucketLifecycle(bucketName);

                //GetBucketLifecycleSample.GetBucketLifecycle(bucketName);
                //
                //

                //PutObjectSample.PutObject(bucketName);

                //ResumbaleSample.ResumableUploadObject(bucketName);

                //CreateEmptyFolderSample.CreateEmptyFolder(bucketName);

                //AppendObjectSample.AppendObject(bucketName);

                // ListObjectsSample.ListObjects(bucketName);

                //UrlSignatureSample.UrlSignature(bucketName);

                //GetObjectSample.GetObjects(bucketName);
                //GetObjectByRangeSample.GetObjectPartly(bucketName);

                //DeleteObjectsSample.DeleteObject(bucketName);
                //DeleteObjectsSample.DeleteObjects(bucketName);

                //const string sourceBucket = bucketName;
                //const string sourceKey = "ResumableUploadObject";
                //const string targetBucket = bucketName;
                //const string targetKey = "ResumableUploadObject2";
                //CopyObjectSample.CopyObject(sourceBucket, sourceKey, targetBucket, targetKey);
                //CopyObjectSample.AsyncCopyObject(sourceBucket, sourceKey, targetBucket, targetKey);

                //ResumbaleSample.ResumableCopyObject(sourceBucket, sourceKey, targetBucket, targetKey);

                //ModifyObjectMetaSample.ModifyObjectMeta(bucketName);

                //DoesObjectExistSample.DoesObjectExist(bucketName);

                //MultipartUploadSample.UploadMultipart(bucketName);
                //MultipartUploadSample.AsyncUploadMultipart(bucketName);

                //MultipartUploadSample.UploadMultipartCopy(targetBucket, targetKey, sourceBucket, sourceKey);

                //MultipartUploadSample.AsyncUploadMultipartCopy(targetBucket, targetKey, sourceBucket, sourceKey);

                //MultipartUploadSample.ListMultipartUploads(bucketName);

                ////CNameSample.CNameOperation(bucketName);

                //PostPolicySample.GenPostPolicy(bucketName);

                //DeleteBucketSample.DeleteNoEmptyBucket(bucketName);

            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                                 ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }

            Console.WriteLine("Press any key to continue . . . ");
            Console.ReadKey(true); 
            #endregion
        }



        /// <summary>
        /// 上传队列
        /// </summary>
        /// <param name="directoryInfo">文件目录</param>
        /// <param name="productVersion">产品版本</param>
        private static void UploadDirectory(DirectoryInfo directoryInfo, string productVersion = null)
        {

            if (!directoryInfo.Exists)
                return;

            foreach (var fileInfo in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
            {
                string key = fileInfo.FullName.Replace(directoryInfo.FullName, directoryInfo.Name).Replace('\\', '/');

                if (!string.IsNullOrEmpty(productVersion))
                {
                    key = string.Format("Sop/{0}/{1}/{2}", productVersion,DateTime.Now.ToString("yyyyMM/dd"), key);
                }

                UploadFile(fileInfo, key);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="key"></param>
        private static void UploadFile(FileInfo fileInfo, string key)
        {
            
            ObjectMetadata meta = new ObjectMetadata();
            meta.ContentType = MimeTypes.MimeTypeMap.GetMimeType(fileInfo.FullName);
            
            
            //上传文档至OSS
            var result = ossClient.PutObject(Config.bucketName, key, fileInfo.FullName, meta);
            Console.WriteLine("Uploading：" + fileInfo.DirectoryName + "," + result.ETag);

        }
     
     
    }
}