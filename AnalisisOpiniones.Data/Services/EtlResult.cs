using System.Collections.Generic;

namespace AnalisisOpiniones.Data.Services;

public class EtlResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ProcessedCount { get; set; }
    public int InsertedCount { get; set; }
    public int UpdatedCount { get; set; }
    public int ErrorCount { get; set; }
    public List<string> Errors { get; set; } = new List<string>();

    public void AddError(string error)
    {
        Errors.Add(error);
        ErrorCount++;
    }
}