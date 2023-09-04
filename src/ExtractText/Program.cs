using Amazon.Textract.Model;
using ExtractText.Extensions;
using ExtractText.Extract;
using ExtractText.Extract.Strategies;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
         .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
         .AddJsonFile("appsettings.json")
         .Build();

/* 
 * Usado para extrair texto de documentos PDF que estão no S3. 
 * Ela é adequada quando você deseja extrair o texto contido no PDF, 
 * mas não necessariamente deseja fazer uma análise mais profunda do conteúdo do documento, 
 * como detecção de tabelas, formas ou entidades.
 */
//var extract = new ExtractText<GetDocumentTextDetectionResponse>(new DocumentStartTextDetection(configuration));

/* 
 * Usado para realizar uma análise completa do documento PDF que estão no S3, 
 * incluindo detecção de tabelas, formas, palavras-chave, entidades, etc. 
 * É mais adequada quando você precisa extrair informações estruturadas de documentos PDF, 
 * além do texto simples.
 */
//var extract = new ExtractText<GetDocumentAnalysisResponse>(new DocumentStartAnalysis(configuration));

/* 
 * Usado para realizar uma análise completa em um PDF local
*/
var extract = new ExtractText<AnalyzeDocumentResponse>(new DocumentAnalysis(configuration));

var response = await extract.GetResults();

foreach (var kvp in response.GetKeyValue())
{
    Console.WriteLine($"Key: {kvp.Key} - {kvp.Value}");
}