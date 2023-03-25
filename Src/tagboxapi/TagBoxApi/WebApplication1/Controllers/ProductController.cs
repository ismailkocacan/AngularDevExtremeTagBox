using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagBoxApi.Controllers
{


    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DataSourceLoadOptions :
       DataSourceLoadOptionsBase
    {
        public IDictionary<string, string> UserData { get; set; }

        public DataSourceLoadOptions()
        {
            this.PrimaryKey = new string[] { "Id" };
        }
    }


    public static class DevExQueryableExtensions
    {
        public static LoadResult DevExLoad<T>(this IQueryable<T> iQueryable,
                                                      DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(iQueryable, loadOptions);
        }
    }

    public static class DataSourceLoadOptionsParserEx
    {
        public const string KEY_USERDATA = "userData";
        public static void Parse(DataSourceLoadOptions loadOptions, Func<string, string> valueSource)
        {
            DataSourceLoadOptionsParser.Parse(loadOptions, valueSource);
            var userData = valueSource(KEY_USERDATA);
            if (String.IsNullOrEmpty(userData)) return;
            loadOptions.UserData = JsonConvert.DeserializeObject<Dictionary<string, string>>(userData, new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None
            });
        }
    }

    public class ModelBinderDataSourceLoadOptions : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var loadOptions = new DataSourceLoadOptions();
            DataSourceLoadOptionsParserEx.Parse(loadOptions, key => bindingContext.ValueProvider.
                                                                  GetValue(key).FirstOrDefault());
            bindingContext.Result = ModelBindingResult.Success(loadOptions);
            return Task.CompletedTask;
        }
    }

    public class DataSourceLoadOptionsBinderProvier : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(DataSourceLoadOptions))
                return new ModelBinderDataSourceLoadOptions();
            return null;
        }
    }



    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        List<Product> products = new List<Product>();

        public ProductController()
        {
            for (int i = 1; i <= 1000; i++)
            {
                products.Add(new Product() { Id = i, Name = "Product " + i.ToString() });
            }
        }


        [HttpGet]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            return products.AsQueryable().DevExLoad(loadOptions);
        }

    }
}
