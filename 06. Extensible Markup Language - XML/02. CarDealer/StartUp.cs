using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            // 09. Import Suppliers
            //string inputXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, inputXml));

            // 10. Import Parts
            //string inputXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, inputXml));

            // 11. Import Cars
            //string inputXml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, inputXml));

            // 12. Import Customers
            //string inputXml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, inputXml));

            // 13. Import Sales
            //string inputXml = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, inputXml));

            // 14. Export Cars With Distance
            //Console.WriteLine(GetCarsWithDistance(context));

            // 15. Export Cars from Make BMW
            //Console.WriteLine(GetCarsFromMakeBmw(context));

            // 16. Export Local Suppliers
            //Console.WriteLine(GetLocalSuppliers(context));

            // 17. Export Cars With Their List Of Parts
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            // 18. Export Total Sales by Customer
            //Console.WriteLine(GetTotalSalesByCustomer(context));

            // 19. Export Sales With Applied Discount
            //Console.WriteLine(GetSalesWithAppliedDiscount(context));

        }

        private static IMapper CreateMapper()
        {
            MapperConfiguration configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<CarDealerProfile>();
            });

            IMapper mapper = configuration.CreateMapper();

            return mapper;
        }

        // 09. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            // Create xml serializer
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]), new XmlRootAttribute("Suppliers"));

            using StringReader reader = new StringReader(inputXml);

            ImportSupplierDto[] importSupplierDtos = (ImportSupplierDto[])xmlSerializer.Deserialize(reader);

            Supplier[] suppliers = mapper.Map<Supplier[]>(importSupplierDtos);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        // 10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]), new XmlRootAttribute("Parts"));

            using StringReader reader = new StringReader(inputXml);

            ImportPartDto[] importPartDtos = (ImportPartDto[])xmlSerializer.Deserialize(reader);

            var supplierIds = context.Suppliers
                .Select(s => s.Id)
                .ToArray();

            List<Part> parts = new List<Part>();

            foreach (var importPartDto in importPartDtos)
            {
                if (supplierIds.Contains(importPartDto.SupplierId))
                {
                    Part part = mapper.Map<Part>(importPartDto);
                    parts.Add(part);
                }
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        // 11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarDto[]), new XmlRootAttribute("Cars"));

            using StringReader reader = new StringReader(inputXml);

            ImportCarDto[] importCarDtos = (ImportCarDto[])xmlSerializer.Deserialize(reader);

            List<Car> cars = new List<Car>();

            var partIds = context.Parts
                .Select(p => p.Id)
                .ToArray();

            foreach (var importCarDto in importCarDtos)
            {
                Car car = mapper.Map<Car>(importCarDto);

                foreach (var part in importCarDto.Parts.DistinctBy(p => p.PartId))
                {
                    if (partIds.Contains(part.PartId))
                    {
                        car.PartsCars.Add(new PartCar
                        {
                            PartId = part.PartId
                        });
                    }
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        // 12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));

            using StringReader reader = new StringReader(inputXml);

            ImportCustomerDto[] importCustomerDtos = (ImportCustomerDto[])xmlSerializer.Deserialize(reader);

            Customer[] customers = mapper.Map<Customer[]>(importCustomerDtos);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }

        // 13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSaleDto[]), new XmlRootAttribute("Sales"));

            using StringReader reader = new StringReader(inputXml);

            ImportSaleDto[] importSaleDtos = (ImportSaleDto[])xmlSerializer.Deserialize(reader);

            List<Sale> sales = new List<Sale>();

            var carIds = context.Cars
                .Select(c => c.Id)
                .ToArray();

            foreach (var importSaleDto in importSaleDtos)
            {
                if (carIds.Contains(importSaleDto.CarId))
                {
                    Sale sale = mapper.Map<Sale>(importSaleDto);
                    sales.Add(sale);
                }
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        // 14. Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            var cars = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                    .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<ExportCarWithDistanceDto>(mapper.ConfigurationProvider)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarWithDistanceDto[]), new XmlRootAttribute("cars"));

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();

            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, cars, xmlSerializerNamespaces);
            }

            return sb.ToString().TrimEnd();
        }

        // 15. Export Cars from Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                    .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<ExportCarFromMakeBMWDto>(mapper.ConfigurationProvider)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarFromMakeBMWDto[]), new XmlRootAttribute("cars"));

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();

            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, cars, xmlSerializerNamespaces);
            }

            return sb.ToString().TrimEnd();
        }

        // 16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportLocalSupplierDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count()
                })
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportLocalSupplierDto[]), new XmlRootAttribute("suppliers"));

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();

            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, suppliers, xmlSerializerNamespaces);
            }

            return sb.ToString().TrimEnd();
        }

        // 17. Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new ExportCarWithTheirListOfPartDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    Parts = c.PartsCars
                        .Select(pc => new ExportPartDto()
                        {
                            Name = pc.Part.Name,
                            Price = pc.Part.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()

                })
                .OrderByDescending(c => c.TraveledDistance)
                    .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarWithTheirListOfPartDto[]), new XmlRootAttribute("cars"));

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();

            xmlSerializerNamespaces.Add (string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, cars, xmlSerializerNamespaces);
            }

            return sb.ToString().TrimEnd();
        }

        // 18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            XmlParser xmlParser = new XmlParser();

            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SalesInfo = c.Sales
                        .Select(s => new
                        {
                            Prices = c.IsYoungDriver
                                    ? s.Car.PartsCars.Sum(pc => Math.Round((double)pc.Part.Price * 0.95, 2))
                                    : s.Car.PartsCars.Sum(pc => (double)pc.Part.Price)
                        })
                        .ToArray()
                })
                .ToArray();

            var totalSales = customers
                .OrderByDescending(c => c.SalesInfo.Sum(s => s.Prices))
                .Select(c => new ExportTotalSaleByCustomerDto()
                {
                    FullName = c.FullName,
                    BoughtCars = c.BoughtCars,
                    SpentMoney = c.SalesInfo.Sum(s => s.Prices).ToString("f2")
                })
                .ToArray();

            return xmlParser.Serialize<ExportTotalSaleByCustomerDto[]>(totalSales, "customers");
        }

        // 19. Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            XmlParser xmlParser = new XmlParser();

            var sales = context.Sales
                .Select(s => new ExportSaleWithAppliedDiscountDto()
                {
                    CarInfo = new ExportCarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance,
                    },
                    Discount = (int)s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = Math.Round((double)(s.Car.PartsCars.Sum(pc => pc.Part.Price) * (1 - (s.Discount / 100))), 4)
                })
                .ToArray();

            return xmlParser.Serialize<ExportSaleWithAppliedDiscountDto[]>(sales, "sales");
        }
    }
}