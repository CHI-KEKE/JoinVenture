using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.S3;
using API.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace API.Service
{
    public class SaveUploadedFileService
    {
        private readonly IStorageService _storageService;

        public SaveUploadedFileService(IStorageService storageService)
        {
            _storageService = storageService;
            
        }
        public async Task<string> SaveUploadedFileMethod(IFormFile image)
        {
            S3ResponseDto result = new S3ResponseDto();


                await using var memoryStr = new MemoryStream();
                await image.CopyToAsync(memoryStr);

                var fileExt = Path.GetExtension(image.Name);
                var originalFileName = Path.GetFileNameWithoutExtension(image.FileName);

                var objName = $"JoinVenture/{originalFileName}.{fileExt}";


                var s3Obj = new s3Obj(){
                    BucketName = "thefirstbucket001",
                    InputStream = memoryStr,
                    Name = objName,
                };
                
                var cred = new AWSCredentials()
                {
                    AwsKey = Environment.GetEnvironmentVariable("AwsKey"),
                    AwsSecretKey = Environment.GetEnvironmentVariable("AwsSecretKey")
                };


                result = await _storageService.UploadFileAsync(s3Obj,cred);

            if(result.StatusCode == 200)
            {
                return $"https://d1pjwdyi3jyxcs.cloudfront.net/{objName}";
            }

            return "Fail to Upload Image";
        }
    }
}