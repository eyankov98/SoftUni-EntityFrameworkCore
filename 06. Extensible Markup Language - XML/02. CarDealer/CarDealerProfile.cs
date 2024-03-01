using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Import Dtos
            CreateMap<ImportSupplierDto, Supplier>();

            CreateMap<ImportPartDto, Part>();

            CreateMap<ImportCarDto, Car>();

            CreateMap<ImportCustomerDto, Customer>();

            CreateMap<ImportSaleDto, Sale>();

            //Export Dtos
            CreateMap<Car, ExportCarWithDistanceDto>();

            CreateMap<Car, ExportCarFromMakeBMWDto>();

            CreateMap<Supplier, ExportLocalSupplierDto>();

            CreateMap<Car, ExportCarWithTheirListOfPartDto>();
        }
    }
}
