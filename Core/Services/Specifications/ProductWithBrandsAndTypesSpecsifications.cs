using Domain.Models;
using Services.Specificeations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithBrandsAndTypesSpecsifications : BaseSpecifications<Product, int>
    {
        public ProductWithBrandsAndTypesSpecsifications(int? brandId, int? typeId, string? sort)    // Get All Products
            : base(
                    P => 
                    (!brandId.HasValue || P.BrandId == brandId ) &&  (!typeId.HasValue || P.TypeId == typeId)
                  )
        {
            AppluIncludes();

            ApplySorting(sort);
        }

        public ProductWithBrandsAndTypesSpecsifications(int id) : base(P => P.Id ==id)  // Get Product
        {
            AppluIncludes();
        }

        private void AppluIncludes()
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDescending(P => P.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }
        }
    }
}
