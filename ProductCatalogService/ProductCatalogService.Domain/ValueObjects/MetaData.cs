using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Domain.ValueObjects
{
    public record MetaData 
    {
        public int TotalCount; 
        public int PageSize; 
        public int CurrentPage; 
        public int TotalPages; 
        public bool HasNext; 
        public bool HasPrevious; 
    }
}
