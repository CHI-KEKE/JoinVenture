using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.S3;

namespace API.Service.IService
{
    public interface IStorageService
    {
        Task<S3ResponseDto> UploadFileAsync(s3Obj s3obj, AWSCredentials aWSCredentials);

    }
}