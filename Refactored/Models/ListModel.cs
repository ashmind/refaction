using System.Collections.Generic;

namespace Refactored.Models
{
    // assuming that we might add some other common things later on,
    // e.g. pagination
    public class ListModel<T>
    {
        public ListModel(IReadOnlyCollection<T> items)
        {
            Items = Argument.NotNull("items", items);
        }

        public IReadOnlyCollection<T> Items { get; }
    }
}