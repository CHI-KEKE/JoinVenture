using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using API.DTOs.S3;
using API.Service.IService;

namespace API.Service
{
    public class StorageService:IStorageService
    {
       public async Task<S3ResponseDto> UploadFileAsync(s3Obj s3obj, DTOs.S3.AWSCredentials aWSCredentials)
        {

            //adding AWS credentials
            var credentials = new BasicAWSCredentials(aWSCredentials.AwsKey,aWSCredentials.AwsSecretKey);

            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.APSoutheast2
            };

            var response = new S3ResponseDto();


            try
            {
                var UploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = s3obj.InputStream,
                    Key = s3obj.Name,
                    BucketName = s3obj.BucketName,
                    CannedACL = S3CannedACL.NoACL
                };

            //Create an S3 Client
            using var client = new AmazonS3Client(credentials, config);

            //Upload Utility to S3
            var transferUtility = new TransferUtility(client);

            //We are actually upload the file to S3
            await transferUtility.UploadAsync(UploadRequest);

            response.StatusCode = 200;
            response.Message = $"{s3obj.Name} has been uploaded successfully";

            }
            catch(AmazonS3Exception ex)
            {
                response.StatusCode = (int)ex.StatusCode;
                response.Message = ex.Message;
            }
            catch(Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}