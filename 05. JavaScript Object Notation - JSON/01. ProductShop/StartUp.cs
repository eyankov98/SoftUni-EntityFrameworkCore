using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            // 01. Import Users
            //string usersJson = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, usersJson));

            // 02. Import Products
            //string productsJson = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, productsJson));

            // 03. Import Categories
            //string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, categoriesJson));

            // 04. Import Categories and Products
            //string categoriesProductsJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsJson));

            // 05. Export Products in Range
            //Console.WriteLine(GetProductsInRange(context));

            //06. Export Sold Products
            //Console.WriteLine(GetSoldProducts(context));

            // 07. Export Categories by Products Count
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            // 08. Export Users and Products
            Console.WriteLine(GetUsersWithProducts(context));
        }

        // Without Mapper

        // 01. Import Users
        //public static string ImportUsers(ProductShopContext context, string inputJson)
        //{
        //    var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

        //    context.Users.AddRange(users);
        //    context.SaveChanges();

        //    return $"Successfully imported {users.Count}";
        //}

        // 02. Import Products
        //public static string ImportProducts(ProductShopContext context, string inputJson)
        //{
        //    var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

        //    context.Products.AddRange(products);
        //    context.SaveChanges();

        //    return $"Successfully imported {products.Count}";
        //}

        // 03. Import Categories
        //public static string ImportCategories(ProductShopContext context, string inputJson)
        //{
        //    var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);

        //    var categoriesWithoutNullNames = categories
        //        .Where(c => c.Name != null)
        //        .ToList();

        //    context.Categories.AddRange(categoriesWithoutNullNames);
        //    context.SaveChanges();

        //    return $"Successfully imported {categoriesWithoutNullNames.Count}";
        //}

        // 04. Import Categories and Products
        //public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        //{
        //    var categoriesProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

        //    context.CategoriesProducts.AddRange(categoriesProducts);
        //    context.SaveChanges();

        //    return $"Successfully imported {categoriesProducts.Count}";
        //}

        // 05. Export Products in Range
        //public static string GetProductsInRange(ProductShopContext context)
        //{
        //    var products = context.Products
        //        .Where(p => p.Price >= 500 && p.Price <= 1000)
        //        .Select(p => new
        //        {
        //            name = p.Name,
        //            price = p.Price,
        //            seller = p.Seller.FirstName + " " + p.Seller.LastName
        //        })
        //        .OrderBy(p => p.price)
        //        .ToList();

        //    string json = JsonConvert.SerializeObject(products, Formatting.Indented);

        //    return json;
        //}

        // 06. Export Sold Products
        //public static string GetSoldProducts(ProductShopContext context)
        //{
        //    var users = context.Users
        //        .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
        //        .OrderBy(u => u.LastName)
        //            .ThenBy(u => u.FirstName)
        //        .Select(u => new
        //        {
        //            firstName = u.FirstName,
        //            lastName = u.LastName,
        //            soldProducts = u.ProductsSold
        //                .Where(p => p.Buyer != null)
        //                .Select(p => new
        //                {
        //                    name = p.Name,
        //                    price = p.Price,
        //                    buyerFirstName = p.Buyer!.FirstName,
        //                    buyerLastName = p.Buyer.LastName
        //                })
        //                .ToList()
        //        })
        //        .ToList();

        //    string json = JsonConvert.SerializeObject(users, Formatting.Indented);

        //    return json;
        //}

        // 07. Export Categories by Products Count
        //public static string GetCategoriesByProductsCount(ProductShopContext context)
        //{
        //    var categories = context.Categories
        //        .OrderByDescending(c => c.CategoriesProducts.Count)
        //        .Select(c => new
        //        {
        //            category = c.Name,
        //            productsCount = c.CategoriesProducts.Count(),
        //            averagePrice = c.CategoriesProducts.Average(cp => cp.Product.Price).ToString("f2"),
        //            totalRevenue = c.CategoriesProducts.Sum(cp => cp.Product.Price).ToString("f2")
        //        })
        //        .ToList();

        //    string json = JsonConvert.SerializeObject(categories, Formatting.Indented);

        //    return json;
        //}

        // 08. Export Users and Products
        //public static string GetUsersWithProducts(ProductShopContext context)
        //{
        //    var users = context.Users
        //        .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
        //        .Select(u => new
        //        {
        //            firstName = u.FirstName,
        //            lastName = u.LastName,
        //            age = u.Age,
        //            soldProducts = u.ProductsSold
        //                .Where(p => p.BuyerId != null)
        //                .Select(p => new
        //                {
        //                    name = p.Name,
        //                    price = p.Price
        //                })
        //                .ToList()
        //        })
        //        .OrderByDescending(u => u.soldProducts.Count)
        //        .ToList();

        //    var output = new
        //    {
        //        usersCount = users.Count,
        //        users = users.Select(u => new
        //        {
        //            u.firstName,
        //            u.lastName,
        //            u.age,
        //            soldProducts = new
        //            {
        //                count = u.soldProducts.Count,
        //                products = u.soldProducts
        //            }
        //        })
        //    };

        //    string json = JsonConvert.SerializeObject(output, new JsonSerializerSettings
        //    {
        //        Formatting = Formatting.Indented,
        //        NullValueHandling = NullValueHandling.Ignore
        //    });

        //    return json;
        //}

        // With Mappper

        public static IMapper CreateMapper()
        {
            MapperConfiguration configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = configuration.CreateMapper();

            return mapper;
        }

        // 01. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportUserDto[] importUserDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);
            User[] users = mapper.Map<User[]>(importUserDtos);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        // 02. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportProductDto[] importProductDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);
            Product[] products = mapper.Map<Product[]>(importProductDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        // 03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCategoryDto[] importCategoryDtos = JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);
            ImportCategoryDto[] categoriesWithoutNullNames = importCategoryDtos
                    .Where(c => c.Name != null)
                    .ToArray();
            Category[] categories = mapper.Map<Category[]>(categoriesWithoutNullNames);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        // 04. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCategoryProductDto[] importCategoryProductDtos = JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson);
            CategoryProduct[] categoryProducts = mapper.Map<CategoryProduct[]>(importCategoryProductDtos);

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }

        // 05. Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToList();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }

        // 06. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
           var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.BuyerId != null))
                .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer!.FirstName,
                            buyerLastName = p.Buyer.LastName
                        })
                        .ToList()
                })
                .ToList();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }

        // 07. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count)
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count,
                    averagePrice = c.CategoriesProducts.Average(p => p.Product.Price).ToString("f2"),
                    totalRevenue = c.CategoriesProducts.Sum(p => p.Product.Price).ToString("f2")
                })
                .ToList();

            return JsonConvert.SerializeObject(categories, Formatting.Indented);
        }

        // 08. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                        })
                        .ToList()
                })
                .OrderByDescending(u => u.soldProducts.Count)
                .ToList();

            var resultUsers = new
            {
                usersCount = users.Count,
                users = users
                    .Select(u => new
                    {
                        u.firstName,
                        u.lastName,
                        u.age,
                        soldProducts = new
                        {
                            count = u.soldProducts.Count,
                            products = u.soldProducts
                        }
                    })
            };

            return JsonConvert.SerializeObject(resultUsers, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            });
        }
    }
}