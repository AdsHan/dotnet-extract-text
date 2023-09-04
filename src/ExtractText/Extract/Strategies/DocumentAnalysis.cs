using Amazon.Textract.Model;
using Microsoft.Extensions.Configuration;

namespace ExtractText.Extract.Strategies;

internal class DocumentAnalysis : BaseDetection, IExtractText<AnalyzeDocumentResponse>
{
    public DocumentAnalysis(IConfiguration configuration) : base(configuration)
    {

    }

    public async Task<AnalyzeDocumentResponse> GetResults()
    {
        byte[] fileData = File.ReadAllBytes("./files/bill.pdf");

        var request = new AnalyzeDocumentRequest()
        {
            Document = new Document { Bytes = new MemoryStream(fileData) },
            FeatureTypes = new List<string> { "FORMS", "TABLES" }
        };

        return await textractClient.AnalyzeDocumentAsync(request);
    }
}

