using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Net;
using WebStore.Database;
using WebStore.Database.DTO;
using WebStore.Database.Entities;
using WebStore.Domain.Core;
using WebStore.Infrastructure.DTO;
using WebStore.Infrastructure.Entities;

namespace WebStore.API
{
    public static class WebStoreApi
    {
        public static void AddServices(this IHostApplicationBuilder builder)
        {
            var services = builder.Services;

            services
                .AddLogging()
                .AddScoped<WebStoreService, WebStoreService>()
                .AddDbContext<WebStoreContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sqlString"), x => x.MigrationsAssembly("WebStore.Infrastructure")));
        }

        public static IEndpointRouteBuilder MapWebStore(this IEndpointRouteBuilder app)
        {
            //phones
            app.MapGet("/items/phones/{id:int}", GetPhone);
            app.MapGet("/items/phones", GetAllPhones);
            app.MapPost("/items/phones", PostPhone);
            app.MapDelete("/items/phones/{id:int}", DeletePhone);
            app.MapPut("/items/phones/", PutPhone);

            //laptops
            app.MapGet("/items/laptops/{id:int}", GetLaptop);
            app.MapGet("/items/laptops", GetAllLaptops);
            app.MapPost("/items/laptops", PostLaptop);
            app.MapDelete("/items/laptops/{id:int}", DeleteLaptop);
            app.MapPut("/items/laptops/", PutLaptop);

            return app;
        }

        #region Phones
        public async static Task<HttpResponseMessage> PostPhone([AsParameters] WebStoreService service, PhoneDto item) => await AddOrUpdatePhoneCore(service, item);
        public async static Task<HttpResponseMessage> PutPhone([AsParameters] WebStoreService service, PhoneDto item) => await AddOrUpdatePhoneCore(service, item, false);
        public async static Task<HttpResponseMessage> GetPhone([AsParameters] WebStoreService service, int id)
        {
            var entity = await service.Context.Phones.FindAsync(id);

            if (entity is null)
            {
                return CreateResponseMessage(HttpStatusCode.NotFound);
            }

            var dto = entity.Adapt<PhoneDto>();

            return CreateResponseMessage(HttpStatusCode.Found, JsonConvert.SerializeObject(dto));
        }

        public async static Task<HttpResponseMessage> GetAllPhones([AsParameters] WebStoreService service)
        {
            var entities = await service.Context.Phones.ToListAsync();

            var collection = new List<PhoneDto>();

            entities.ForEach(x =>
            {
                collection.Add(x.Adapt<PhoneDto>());
            });

            return CreateResponseMessage(HttpStatusCode.OK, JsonConvert.SerializeObject(collection));
        }

        public async static Task<HttpResponseMessage> DeletePhone([AsParameters] WebStoreService service, int id)
        {
            var entity = await service.Context.Phones.FindAsync(id);

            if (entity is null)
            {
                return CreateResponseMessage(HttpStatusCode.NotFound);
            }

            var result = service.Context.Phones.Remove(entity);

            if (result.State is EntityState.Deleted)
            {
                return CreateResponseMessage(HttpStatusCode.OK);
            }

            return CreateResponseMessage(HttpStatusCode.InternalServerError, $"Cannot remove {entity.Name}");
        }

        private static async Task<HttpResponseMessage> AddOrUpdatePhoneCore(WebStoreService service, PhoneDto item, bool isAdding = true)
        {
            var entity = GetEntity<PhoneEntity, PhoneDto>(item);

            if (entity is null)
            {
                return CreateResponseMessage(HttpStatusCode.BadRequest);
            }

            EntityState entityState;
            string action;
            EntityEntry<PhoneEntity> result;

            if (isAdding)
            {
                result = await service.Context.Phones.AddAsync(entity);
                entityState = EntityState.Added;
                action = "add";
            }
            else
            {
                result = service.Context.Phones.Update(entity);
                entityState = EntityState.Modified;
                action = "update";
            }


            if (result.State.Equals(entityState))
            {
                await service.Context.SaveChangesAsync();

                return CreateResponseMessage(HttpStatusCode.Created, $"{item.Name} has been {action}ed.");
            }

            return CreateResponseMessage(HttpStatusCode.InternalServerError, $"Cannot {action} {item.Name}.");
        }
        #endregion

        #region Laptops

        public async static Task<HttpResponseMessage> PostLaptop([AsParameters] WebStoreService service, LaptopDto item) => await AddOrUpdateLaptopCore(service, item);
        public async static Task<HttpResponseMessage> PutLaptop([AsParameters] WebStoreService service, LaptopDto item) => await AddOrUpdateLaptopCore(service, item);
        public async static Task<HttpResponseMessage> GetLaptop([AsParameters] WebStoreService service, int id)
        {
            var entity = await service.Context.Laptops.FindAsync(id);

            if (entity is null)
            {
                return CreateResponseMessage(HttpStatusCode.NotFound);
            }

            return CreateResponseMessage(HttpStatusCode.Found, JsonConvert.SerializeObject(entity));
        }
        public async static Task<HttpResponseMessage> GetAllLaptops([AsParameters] WebStoreService service)
        {
            var entities = await service.Context.Laptops.ToListAsync();

            var collection = new List<LaptopDto>();

            entities.ForEach(x =>
            {
                collection.Add(x.Adapt<LaptopDto>());
            });

            return CreateResponseMessage(HttpStatusCode.OK, JsonConvert.SerializeObject(collection));
        }

        public async static Task<HttpResponseMessage> DeleteLaptop([AsParameters] WebStoreService service, int id)
        {
            var entity = await service.Context.Laptops.FindAsync(id);

            if (entity is null)
            {
                return CreateResponseMessage(HttpStatusCode.NotFound);
            }

            var result = service.Context.Laptops.Remove(entity);

            if (result.State is EntityState.Deleted)
            {
                return CreateResponseMessage(HttpStatusCode.OK);
            }

            return CreateResponseMessage(HttpStatusCode.InternalServerError, $"Cannot remove {entity.Name}");
        }

        private async static Task<HttpResponseMessage> AddOrUpdateLaptopCore(WebStoreService service, LaptopDto item, bool isAdding = true)
        {
            var entity = GetEntity<LaptopEntity, LaptopDto>(item);

            if (entity is null)
            {
                return CreateResponseMessage(HttpStatusCode.BadRequest);
            }

            EntityState entityState;
            string action;
            EntityEntry<LaptopEntity> result;

            if (isAdding)
            {
                result = await service.Context.Laptops.AddAsync(entity);
                entityState = EntityState.Added;
                action = "add";
            }
            else
            {
                result = service.Context.Laptops.Update(entity);
                entityState = EntityState.Modified;
                action = "update";
            }


            if (result.State.Equals(entityState))
            {
                await service.Context.SaveChangesAsync();

                return CreateResponseMessage(HttpStatusCode.Created, $"{item.Name} has been {action}ed.");
            }

            return CreateResponseMessage(HttpStatusCode.InternalServerError, $"Cannot {action} {item.Name}.");
        }

        #endregion

        public static TEntity? GetEntity<TEntity, TDto>(TDto dto) where TDto : ItemDto where TEntity : class
        {
            if (dto is null)
            {
                return null;
            }

            try
            {
                return dto.Adapt<TEntity>();
            }
            catch
            {
                return null;
            }
        }

        public static HttpResponseMessage CreateResponseMessage(HttpStatusCode statusCode, string message) => new(statusCode) { Content = new StringContent(message) };
        public static HttpResponseMessage CreateResponseMessage(HttpStatusCode statusCode) => CreateResponseMessage(statusCode, string.Empty);
    }
}