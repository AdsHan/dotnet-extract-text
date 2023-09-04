using Amazon.Textract;
using Amazon.Textract.Model;
using Microsoft.Extensions.Configuration;

namespace ExtractText.Extract.Strategies;

internal class DocumentStartTextDetection : BaseDetection, IExtractText<GetDocumentTextDetectionResponse>
{
    public DocumentStartTextDetection(IConfiguration configuration) : base(configuration)
    {

    }

    public async Task<GetDocumentTextDetectionResponse> GetResults()
    {
        var request = new StartDocumentTextDetectionRequest
        {
            DocumentLocation = new DocumentLocation
            {
                S3Object = new S3Object
                {
                    Bucket = bucketName,
                    Name = documentName
                }
            }
        };

        var response = await textractClient.StartDocumentTextDetectionAsync(request);

        string jobId = response.JobId;

        await WaitJobCompletion(textractClient, jobId);

        return await GetTextractResults(textractClient, jobId);
    }

    public async Task WaitJobCompletion(IAmazonTextract textractClient, string jobId)
    {
        var status = "IN_PROGRESS";
        while (status == "IN_PROGRESS")
        {
            var response = await textractClient.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
            {
                JobId = jobId
            });
            status = response.JobStatus;
            Console.WriteLine($"Status do trabalho: {status}");

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }

    public async Task<GetDocumentTextDetectionResponse> GetTextractResults(IAmazonTextract textractClient, string jobId)
    {
        var response = await textractClient.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
        {
            JobId = jobId
        });

        return response;
    }
}
