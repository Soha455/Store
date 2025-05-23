﻿using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Specifications;
using Services.Specificeations;
using ServicesAbstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork , IMapper mapper) : IProductService
    {
        public async Task<PaginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParameters specParams)
        {
            var spec = new ProductWithBrandsAndTypesSpecsifications(specParams);

            // Get All Products Through ProductRepository
            var products = await unitOfWork.GetRepository<Product,int>().GetAllAsync(spec);

            var specCount = new ProductWithCountSpecifications(specParams);

            var count = await unitOfWork.GetRepository<Product, int>().CountAsync(specCount);

            // Map IEnumerable<Product> to IEnumerable<ProductResultDto> : AutoMapper
            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);

            return new PaginationResponse<ProductResultDto>(specParams.PageIndex, specParams.PageSize,count, result);
        }

        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandsAndTypesSpecsifications(id);

            var product = await unitOfWork.GetRepository<Product,int>().GetAsync(spec);
            if (product is null) return null;

            var result = mapper.Map<ProductResultDto>(product);
            return result;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypessAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;
        }

        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return result;
        }

    }
}
