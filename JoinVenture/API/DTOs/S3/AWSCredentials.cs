using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.S3
{
    public class AWSCredentials
    {
        public string AwsKey { get; set; } ="";
        public string AwsSecretKey { get; set; } ="";
    }
}