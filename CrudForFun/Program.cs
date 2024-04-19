using System.Text.Json.Serialization;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);
            var app = builder.Build();

            app.Run();
        }
    }

}
