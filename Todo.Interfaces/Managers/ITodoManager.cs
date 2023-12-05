using System.Collections;

namespace Interfaces.Managers
{
    public interface ITodoManager
    {
        Task<IEnumerable> GetTodos();
    }
}
