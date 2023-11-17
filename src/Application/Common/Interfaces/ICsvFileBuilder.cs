using Domain.Common;

namespace Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile<T, TMapper>(IEnumerable<T> records);
}
