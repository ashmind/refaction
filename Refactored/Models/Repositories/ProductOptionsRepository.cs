using System;
using System.Data;

namespace Refactored.Models.Repositories
{
    public class ProductOptionsRepository : RepositoryBase<ProductOption>
    {
        public ProductOptionsRepository(Func<IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string UpdateSql => @"
            update productoption
            set name = @Name, description = @Description, productid = @ProductId
            where id = @Id
        ";

        protected override string InsertSql => @"
            insert into productoption (id, productid, name, description)
            values(@Id, @ProductId, @Name, @Description)
        ";
    }
}