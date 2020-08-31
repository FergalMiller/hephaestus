using System;
using System.Reflection;
using Forge;
using HephaestusDemo.Providers;

namespace HephaestusDemo
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Application
    {
        public static void Main(string[] args)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var blacksmith = Blacksmith.Instance;
            
            blacksmith.WeldAssembly(currentAssembly);
            var elementMap = blacksmith.GetElementMap();
            var fooServiceProvider = (FooServiceProvider) elementMap.GetElement(typeof(FooServiceProvider));
            var services = fooServiceProvider.GetFooServices();
            
            services.ForEach(i => Console.WriteLine(i.GetType().ToString()));
        }
    }
}