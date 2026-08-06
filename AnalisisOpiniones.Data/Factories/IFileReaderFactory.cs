using AnalisisOpiniones.Data.Interfaces;

namespace AnalisisOpiniones.Data.Factories;

public interface IFileReaderFactory
{
    IFileReaderRepository<T> CreateReader<T>() where T : class;
    IFileReaderRepository<T> CreateReader<T>(string filePath) where T : class;
}