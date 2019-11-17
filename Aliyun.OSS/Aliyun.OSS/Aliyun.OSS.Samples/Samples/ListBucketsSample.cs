/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using Aliyun.OSS.Common;

namespace Aliyun.OSS.Samples
{
    /// <summary>
    /// Sample for listing buckets.
    /// </summary>
    public static class ListBucketsSample
    {
        static string accessKeyId = Config.AccessKeyId;
        static string accessKeySecret = Config.AccessKeySecret;
        static string endpoint = Config.Endpoint;
        static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);

        /// <summary>
        /// 列出账户下所有的存储空间信息
        /// </summary>
        public static void ListBuckets()
        {
            ListAllBuckets();
            ListBucketsByConditions();
        }

        /// <summary>
        /// 1. Try to list all buckets. 
        /// </summary>
        public static void ListAllBuckets()
        {
            try
            {
                var buckets = client.ListBuckets();

                Console.WriteLine("所有的存储空间信息: ");
                foreach (var bucket in buckets)
                {
                    Console.WriteLine(" \n\r存储空间:{0}", bucket.Name);
                }
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
        }

        /// <summary>
        /// 列表存储空间由指定的条件,例如前缀/标志/ max-keys。
        /// </summary>
        public static void ListBucketsByConditions()
        {
            try
            {
                var req = new ListBucketsRequest {Prefix = "test", MaxKeys = 3, Marker = "test2"};
                var result = client.ListBuckets(req);
                var buckets = result.Buckets;
                Console.WriteLine("List buckets by page: ");
                Console.WriteLine("Prefix: {0}, MaxKeys: {1},  Marker: {2}, NextMarker:{3}", 
                                   result.Prefix, result.MaxKeys, result.Marker, result.NextMaker);
                foreach (var bucket in buckets)
                {
                    Console.WriteLine("Name:{0}, Location:{1}, Owner:{2}", bucket.Name, bucket.Location, bucket.Owner);
                }
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
        }
    }
}
