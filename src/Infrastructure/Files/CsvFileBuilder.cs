using System.Globalization;

using CsvHelper;

using Application.Common.Interfaces;
using Domain.Common;
using CsvHelper.Configuration;

namespace Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
    public byte[] BuildTodoItemsFile<T, TMapper>(IEnumerable<T> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Context.RegisterClassMap<ClassMap<TMapper>>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}
