using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace awsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3BucketController : ControllerBase
    {
        IAmazonS3 _S3Client { get; set; }
        private readonly TransferUtility _transferUtility;
        public S3BucketController(IAmazonS3 s3Client)
        {
            this._S3Client = s3Client;
            _transferUtility = new TransferUtility(_S3Client);
        }
    }
}
