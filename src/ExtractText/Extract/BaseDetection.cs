using Amazon;
using Amazon.Textract;
using Microsoft.Extensions.Configuration;

namespace ExtractText.Extract;

internal class BaseDetection
{
    public AmazonTextractClient textractClient;
    public string bucketName;
    public string documentName;

    public BaseDetection(IConfiguration configuration)
    {
        bucketName = configuration.GetSection("S3")["BucketName"];
        documentName = configuration.GetSection("S3")["DocumentName"];

        var awsAccessKey = configuration.GetSection("AWSCredentials")["AccessCoezzionKey"];
        var awsSecretKey = configuration.GetSection("AWSCredentials")["SecretCoezzionKey"];
        var awsRegion = RegionEndpoint.USEast1;

        textractClient = new AmazonTextractClient(awsAccessKey, awsSecretKey, awsRegion);
    }
}
