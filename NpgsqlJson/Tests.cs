using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace NpgsqlJson
{
    public class Tests
    {
        [Fact]
        public async Task Should_save_customer()
        {
            // arrange
            using var dbContext = new AppDbContext();
            await dbContext.Database.EnsureCreatedAsync();
            var customer =
                new Customer
                {
                    Names =
                    {
                        "John",
                        "Charlie"
                    },
                    Phones =
                    {
                        new Phone
                        {
                            Place = "Home",
                            Number = "123-4567-8910"
                        },
                        new Phone
                        {
                            Place = "Work",
                            Number = "123-4567-8910"
                        }
                    }
                };
            dbContext.Customers.Add(customer);

            // act
            var result = await dbContext.SaveChangesAsync();
            dbContext.ChangeTracker.Clear();


            /* The following line throws
             * System.InvalidCastException : Unable to cast object of type 'System.Collections.Generic.List`1[System.String]' to type 'System.Collections.Generic.HashSet`1[System.String]'.
             * 
             * StackTrace:
             * lambda_method(Closure , QueryContext , DbDataReader , ResultContext , SingleQueryResultCoordinator )
             *    AsyncEnumerator.MoveNextAsync()
             *    ShapedQueryCompilingExpressionVisitor.SingleOrDefaultAsync[TSource](IAsyncEnumerable`1 asyncEnumerable, CancellationToken cancellationToken)
             *    ShapedQueryCompilingExpressionVisitor.SingleOrDefaultAsync[TSource](IAsyncEnumerable`1 asyncEnumerable, CancellationToken cancellationToken)
             */

            var retrieved = await dbContext.Customers.SingleOrDefaultAsync(cust => cust.Id == customer.Id);

            // assert
            Assert.Equal(1, result);
            Assert.Equal(retrieved.Id, customer.Id);
        }
    }
}