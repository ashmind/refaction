using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Refactored.Models;
using Refactored.Models.Repositories;

namespace Refactored.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IRepository<Product> _repository;
        
        public ProductsController(IRepository<Product> repository)
        {
            _repository = repository;
        }

        [Route]
        [HttpGet]
        public ListModel<Product> GetAll()
        {
            return new ListModel<Product>(_repository.GetAll());
        }

        [Route]
        [HttpGet]
        public ListModel<Product> GetByName(string name)
        {
            return new ListModel<Product>(_repository.GetAllBy(nameof(Product.Name), name));
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetById(Guid id)
        {
            var product = _repository.GetAllBy(nameof(Product.Id), id).FirstOrDefault();
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return product;
        }

        [Route]
        [HttpPost]
        public void Post(Product product)
        {
            _repository.SaveNew(product);
        }

        [Route("{id}")]
        [HttpPut]
        public void Put(Guid id, Product product)
        {
            product.Id = id;
            _repository.Save(product);
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }
    }
}
