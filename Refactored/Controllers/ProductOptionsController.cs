using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Refactored.Models;
using Refactored.Models.Repositories;

namespace Refactored.Controllers
{
    [RoutePrefix("products/{productId}/options")]
    public class ProductOptionsController : ApiController
    {
        private readonly IRepository<ProductOption> _repository;

        public ProductOptionsController(IRepository<ProductOption> repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public ListModel<ProductOption> GetByProductId(Guid productId)
        {
            // This is a balancing act. In perfect REST API, I would have thrown
            // 404 if product does not exist, as compared to empty options.
            // However this would be an extra DB request, so might not be worth it.
            return new ListModel<ProductOption>(_repository.GetAllBy(nameof(ProductOption.ProductId), productId));
        }

        [Route("{id}")]
        [HttpGet]
        public ProductOption GetById(Guid productId, Guid id) {
            var option = _repository.GetAllBy(nameof(ProductOption.Id), id).FirstOrDefault();
            if (option == null || option.ProductId != productId)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return option;
        }

        [Route("")]
        [HttpPost]
        public void Post(Guid productId, ProductOption option)
        {
            // I think this one should throw 404 if product does not exist,
            // however the most reliable way would be to react to a foreign key
            // failure and I'll leave that complexity out for now.
            option.ProductId = productId;
            _repository.SaveNew(option);
        }

        [Route("{id}")]
        [HttpPut]
        public void Put(Guid productId, Guid id, ProductOption option)
        {
            option.Id = id;
            option.ProductId = productId;
            _repository.Save(option);
        }

        [Route("{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            _repository.Delete(id);
        }
    }
}