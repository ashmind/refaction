using System;
using Newtonsoft.Json;

namespace Refactored.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        [JsonIgnore] // according to docs this should not be served
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}