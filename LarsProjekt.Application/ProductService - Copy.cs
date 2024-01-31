//using LarsProjekt.Dto;
//using LarsProjekt.Dto.Mapping;
//using LarsProjekt.Domain;
//using System.Text.Json;
//using System;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace LarsProjekt.Application;

//internal class Service<T> : IService<T> where T : class
//{
//    //private readonly IConfiguration _configuration;
//    //public ProductService(IConfiguration configuration)
//    //{
//    //    _configuration = configuration;
//    //}
//    private HttpRequestMessage GetMessage(string uriMethod, string entity, HttpMethod httpMethod)
//    {        
//        var uri = $"https://localhost:7182/api/{entity}/{uriMethod}";
//        var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);
//        httpRequestMessage.Headers.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036");
//        return httpRequestMessage;
//    }
//    public async Task<T> GetAll()
//    {
//        var httpClient = new HttpClient();
//        var httpResponseMessage = await httpClient.SendAsync();

//        if (httpResponseMessage.IsSuccessStatusCode)
//        {
//            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            
//            var productDtos = JsonSerializer.Deserialize<List<ProductDto>>(content, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });

//            var products = new List<Product>();
//            foreach (var product in productDtos)
//            {                
//                products.Add(product.ToDomain());
//            }

//            return products;
//        }

//        throw new Exception();
//    }

//    public async Task<Product> GetById(long id)
//    {
//        var httpClient = new HttpClient();
//        var httpResponseMessage = await httpClient.SendAsync(GetMessage($"getbyid?id={id}", HttpMethod.Get));

//        if (httpResponseMessage.IsSuccessStatusCode)
//        {
//            var content = await httpResponseMessage.Content.ReadAsStringAsync();

//            var productDto = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });

//            return productDto.ToDomain();
//        }

//        throw new Exception();
//    }
//    public async Task<Product> Update(Product product)
//    {
//        var httpRequestMessage = GetMessage("update", HttpMethod.Post);
//        var requestContent = JsonSerializer.Serialize(product.ToDto());
//        httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

//        var httpClient = new HttpClient();
//        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

//        if (httpResponseMessage.IsSuccessStatusCode)
//        {
//            var content = await httpResponseMessage.Content.ReadAsStringAsync();

//            var result = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });

//            return result.ToDomain();
//        }

//        throw new Exception();
//    }
//    public async Task<Product> Create(Product product)
//    {       
//        var httpRequestMessage = GetMessage("create", HttpMethod.Post);
//        var requestContent = JsonSerializer.Serialize(product.ToDto());
//        httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

//        var httpClient = new HttpClient();
//        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

//        if (httpResponseMessage.IsSuccessStatusCode)
//        {
//            var content = await httpResponseMessage.Content.ReadAsStringAsync();

//            var result = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });

//            return result.ToDomain();
//        }

//        throw new Exception();
//    }
//    public async Task<string> Delete(long id)
//    {
//        var httpClient = new HttpClient();
//        var httpResponseMessage = await httpClient.SendAsync(GetMessage($"delete?id={id}", HttpMethod.Delete));

//        if (httpResponseMessage.IsSuccessStatusCode)
//        {
//            return await httpResponseMessage.Content.ReadAsStringAsync();
//        }
//        throw new Exception();
//    }
//}



////var uri = "https://localhost:7182/api/products/getall";
////var httpClient = new HttpClient();
////var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
////httpRequestMessage.Headers.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036"); // KeyValue Pair, aus config???