using Dtos;
using System.Collections;

namespace DataAccess
{
    public class TodoRepo
    {
        public async Task<IEnumerable> GetTodos()
        {
            var todos = new List<Todo> {
                new Todo { Title = "Hi Nova, how u doing?" },
                new Todo { Title = "Hi Dave, how u doing?" },
            }.AsEnumerable();

            return todos;
        }
    }
}
