using Microsoft.EntityFrameworkCore;
using MinimalAPI.Modal;

namespace MinimalAPI.Context
{
    public class TodoDb: DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
    }
}
