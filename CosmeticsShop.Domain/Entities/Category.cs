using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsShop.Domain.Entities
{
    public sealed class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }

        private Category() { } // requis par EF Core

        public Category(string name, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        public void Rename(string newName) => Name = newName;
    }
}
