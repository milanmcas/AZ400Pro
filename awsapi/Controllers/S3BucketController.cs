using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
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
        private readonly string? _bucketName;
        private readonly IConfiguration _config;
        public S3BucketController(IAmazonS3 s3Client, IConfiguration config)
        {
            this._S3Client = s3Client;
            _transferUtility = new TransferUtility(_S3Client);
            _config=config;
            _bucketName = _config["AWS1:BucketName"];
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateBucketAsync(string bucketName)
        {
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_S3Client, bucketName); 
            if (bucketExists) return BadRequest($"Bucket {bucketName} already exists.");
            await _S3Client.PutBucketAsync(bucketName);
            return Ok($"Bucket {bucketName} created.");
        }
        [HttpPost("set-versioning")]
        public async Task<IActionResult> SetVersioning([FromQuery] bool enable)
        {
            try
            {
                await SetVersioningAsync(enable);
                return Ok(enable ? "Versioning enabled successfully." : "Versioning disabled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [NonAction]
        public async Task SetVersioningAsync(bool enableVersioning)
        {
            var versioningConfig = new S3BucketVersioningConfig
            {
                Status = enableVersioning ? VersionStatus.Enabled : VersionStatus.Suspended
            };

            var request = new PutBucketVersioningRequest
            {
                BucketName = _bucketName,
                VersioningConfig = versioningConfig
            };

            try
            {
                await _S3Client.PutBucketVersioningAsync(request);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while setting versioning: {ex.Message}");
            }
        }
        [NonAction]
        public async Task<List<S3ObjectVersion>> ListObjectVersionsAsync(string key)
        {
            var versions = new List<S3ObjectVersion>();

            var request = new ListVersionsRequest
            {
                BucketName = _bucketName,
                Prefix = key
            };

            ListVersionsResponse? response=null;
            do
            {
                response = await _S3Client.ListVersionsAsync(request);
                versions.AddRange(response.Versions);
                request.KeyMarker = response.NextKeyMarker;
                request.VersionIdMarker = response.NextVersionIdMarker;
            }
            while (response.IsTruncated.Value);

            return versions;
        }
    }
}
