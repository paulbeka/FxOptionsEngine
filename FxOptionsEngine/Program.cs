using DotNetEnv;

namespace FxOptionsEngine
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Env.Load(Path.Combine(AppContext.BaseDirectory, ".env"));

            var app = new FxOptionsEngineApp();
            app.Run();
        }
    }
}