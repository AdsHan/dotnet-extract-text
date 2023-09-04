using Amazon.Textract;
using Amazon.Textract.Model;

namespace ExtractText.Extensions;

public static class AnalyzeDocumentResponseExtension
{
    public static Dictionary<string, string> GetKeyValue(this AnalyzeDocumentResponse response)
    {
        var dictionary = new Dictionary<string, string>();

        var keyBlocks = response.Blocks.Where(b => b.BlockType == BlockType.KEY_VALUE_SET && b.Relationships.Count == 2).ToList();

        foreach (var block in keyBlocks)
        {
            var key = GetKey(block, response.Blocks);
            var value = GetValue(block, response.Blocks);

            dictionary.TryAdd($"{key}", value);
        }

        return dictionary;

        string GetKey(Block block, List<Block> blocks)
        {
            var ids = block.Relationships.ElementAtOrDefault(1)?.Ids ?? new List<string>();

            var value = string.Join(" ", blocks.Where(b => ids.Contains(b.Id)).Select(b => b.Text));

            return value;
        }

        string GetValue(Block block, List<Block> blocks)
        {
            var id = block.Relationships.ElementAtOrDefault(0)?.Ids.FirstOrDefault() ?? string.Empty;

            var ids = blocks.FirstOrDefault(b => b.Id == id)?.Relationships.ElementAtOrDefault(0)?.Ids ?? new List<string>();

            var value = string.Join(" ", blocks.Where(b => ids.Contains(b.Id)).Select(b => b.Text));

            return value;
        }
    }
}