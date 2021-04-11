namespace Todo.API.Data.Seeds
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class TodoDbContextSeed
    {
        public async Task SeedAsync(TodoDbContext context, ILogger<TodoDbContextSeed> logger,  int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                if (!context.Todos.Any())
                {
                    context.Todos.AddRange(new List<Todo> 
                    { 
                        new Todo{ Name = "Test name 1", Content ="Test content 1"},
                        new Todo{ Name = "Test name 2", Content ="Test content 2"},
                        new Todo{ Name = "Test name 3", Content ="Test content 3"},
                        new Todo{ Name = "Test name 4", Content ="Test content 4"},
                    });
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 5)
                {
                    retryForAvaiability++;
                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(TodoDbContext));
                    await SeedAsync(context, logger, retryForAvaiability);
                }
            }
        }
    }
}
