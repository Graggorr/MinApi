namespace WebStore.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);
            builder.AddServices();

            var app = builder.Build();

            app.MapGroup("/api/v1/webstore").WithTags("WebStore API").MapWebStore();
            app.Run();
        }
    }

}
