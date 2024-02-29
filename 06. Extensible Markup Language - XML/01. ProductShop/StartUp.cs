using AutoMapper;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;
using System.Net;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            // 01. Import Users
            //string inputXml = File.ReadAllText("../../../Datasets/users.xml");
            //Console.WriteLine(ImportUsers(context, inputXml));

            // 02. Import Products
            //string inputXml = File.ReadAllText("../../../Datasets/products.xml");
            //Console.WriteLine(ImportProducts(context, inputXml));

            // 03 Import Categories
            //string inputXml = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context, inputXml));

            // 04. Import Categories and Products
            //string inputXml = File.ReadAllText("../../../Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(context, inputXml));

            // 05. Export Products In Range
            //Console.WriteLine(GetProductsInRange(context));

            // 06. Export Sold Products
            //Console.WriteLine(GetSoldProducts(context));

            // 07. Export Categories By Products Count
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            // 08 Export Users and Products
            //Console.WriteLine(GetUsersWithProducts(context));
        }

        private static IMapper CreateMapper()
        {
            MapperConfiguration configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = configuration.CreateMapper();

            return mapper;
        }

        // 01. Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlParser xmlParser = new XmlParser();

            ImportUserDto[] importUserDtos = xmlParser.Deserialize<ImportUserDto[]>(inputXml, "Users");

            User[] users = mapper.Map<User[]>(importUserDtos);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        // 02. Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlParser xmlParser = new XmlParser();

            ImportProductDto[] importProductDtos = xmlParser.Deserialize<ImportProductDto[]>(inputXml, "Products");

            Product[] products = mapper.Map<Product[]>(importProductDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        // 03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlParser xmlParser = new XmlParser();

            ImportCategoryDto[] importCategoryDtos = xmlParser.Deserialize<ImportCategoryDto[]>(inputXml, "Categories");

            List<Category> categories = new List<Category>();

            foreach (var importCategoryDto in importCategoryDtos)
            {
                if (importCategoryDto.Name != null)
                {
                    Category category = mapper.Map<Category>(importCategoryDto);

                    categories.Add(category);
                }
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        // 04. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlParser xmlParser = new XmlParser();

            ImportCategoryProductDto[] importCategoryProductDtos = xmlParser.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            var categoryIds = context.Categories
                .Select(c => c.Id)
                .ToArray();

            var productIds = context.Products
                .Select(c => c.Id)
                .ToArray();

            foreach (var importCategoryProductDto in importCategoryProductDtos)
            {
                if (categoryIds.Contains(importCategoryProductDto.CategoryId) && productIds.Contains(importCategoryProductDto.ProductId))
                {
                    CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(importCategoryProductDto);

                    categoryProducts.Add(categoryProduct);
                }
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        // 05. Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            XmlParser xmlParser = new XmlParser();

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new ExportProductDto()
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerName = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .Take(10)
                .ToArray();

            return xmlParser.Serialize<ExportProductDto[]>(products, "Products");
        }

        // 06. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            XmlParser xmlParser = new XmlParser();

            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                .Select(u => new ExportSoldProductsDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                        .Select(ps => new ExportProductDto()
                        {
                            Name = ps.Name,
                            Price = ps.Price
                        })
                        .ToArray()
                })
                .Take(5)
                .ToArray();

            return xmlParser.Serialize(users, "Users");
        }

        // 07. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            XmlParser xmlParser = new XmlParser();

            var categories = context.Categories
                .Select(c => new ExportCategoryDto()
                {
                    Name = c.Name,
                    NumberOfProducts = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(c => c.NumberOfProducts)
                    .ThenBy(c => c.TotalRevenue)
                .ToArray();

            return xmlParser.Serialize(categories, "Categories");
        }

        // 08. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            XmlParser xmlParser = new XmlParser();

            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(u => u.ProductsSold.Count)
                .Select(u => new UserInfo()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductCount()
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold
                            .Select(p => new SoldProduct()
                            {
                                Name= p.Name,
                                Price = p.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                    }
                })
                .Take(10)
                .ToArray();

            ExportUserCountDto exportUserCountDto = new ExportUserCountDto()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                Users = users
            };

            return xmlParser.Serialize(exportUserCountDto, "Users");
        }
    }
}