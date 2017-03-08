using System;
using System.Data;

namespace Refactored.Models.Repositories
{
    public class ProductRepository : RepositoryBase<Product>
    {
        public ProductRepository(Func<IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string UpdateSql => @"
            update product
            set name = @Name, description = @Description, price = @Price, deliveryprice = @DeliveryPrice
            where id = @Id
        ";

        protected override string InsertSql => @"
            insert into product(id, name, description, price, deliveryprice)
            values(@Id, @Name, @Description, @Price, @DeliveryPrice)
        ";
    }
}