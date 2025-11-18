using AutoMapper;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Customer mappings
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();

            // Order mappings
            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>();

            // Product mappings
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.UnitsInStock, opt => opt.MapFrom(src => src.UnitsInStock.HasValue ? (short?)src.UnitsInStock.Value : null))
                .ForMember(dest => dest.UnitsOnOrder, opt => opt.MapFrom(src => src.UnitsOnOrder.HasValue ? (short?)src.UnitsOnOrder.Value : null))
                .ForMember(dest => dest.ReorderLevel, opt => opt.MapFrom(src => src.ReorderLevel.HasValue ? (short?)src.ReorderLevel.Value : null));
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.UnitsInStock, opt => opt.MapFrom(src => src.UnitsInStock.HasValue ? (short?)src.UnitsInStock.Value : null))
                .ForMember(dest => dest.UnitsOnOrder, opt => opt.MapFrom(src => src.UnitsOnOrder.HasValue ? (short?)src.UnitsOnOrder.Value : null))
                .ForMember(dest => dest.ReorderLevel, opt => opt.MapFrom(src => src.ReorderLevel.HasValue ? (short?)src.ReorderLevel.Value : null));

            // Employee mappings
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();

            // Category mappings
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            // Supplier mappings
            CreateMap<Supplier, SupplierDto>();
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<UpdateSupplierDto, Supplier>();

            // Shipper mappings
            CreateMap<Shipper, ShipperDto>();
            CreateMap<CreateShipperDto, Shipper>();
            CreateMap<UpdateShipperDto, Shipper>();
        }
    }
} 