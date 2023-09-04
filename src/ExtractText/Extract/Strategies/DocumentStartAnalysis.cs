using Amazon.Textract;
using Amazon.Textract.Model;
using Microsoft.Extensions.Configuration;

namespace ExtractText.Extract.Strategies;

internal class DocumentStartAnalysis : BaseDetection, IExtractText<GetDocumentAnalysisResponse>
{
    public DocumentStartAnalysis(IConfiguration configuration) : base(configuration)
    {

    }

    public async Task<GetDocumentAnalysisResponse> GetResults()
    {
        var request = new StartDocumentAnalysisRequest
        {
            DocumentLocation = new DocumentLocation
            {
                S3Object = new S3Object
                {
                    Bucket = bucketName,
                    Name = documentName
                }
            },
            FeatureTypes = new List<string> { "FORMS", "TABLES" }
        };

        var response = await textractClient.StartDocumentAnalysisAsync(request);

        string jobId = response.JobId;

        await WaitJobCompletion(textractClient, jobId);

        return await GetTextractResults(textractClient, jobId);
    }

    public async Task WaitJobCompletion(IAmazonTextract textractClient, string jobId)
    {
        var status = "IN_PROGRESS";
        while (status == "IN_PROGRESS")
        {
            var response = await textractClient.GetDocumentAnalysisAsync(new GetDocumentAnalysisRequest
            {
                JobId = jobId
            });
            status = response.JobStatus;
            Console.WriteLine($"Status do trabalho: {status}");

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }

    public async Task<GetDocumentAnalysisResponse> GetTextractResults(IAmazonTextract textractClient, string jobId)
    {
        var response = await textractClient.GetDocumentAnalysisAsync(new GetDocumentAnalysisRequest
        {
            JobId = jobId
        });

        return response;
    }
}
