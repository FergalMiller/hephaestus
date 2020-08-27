using System.Reflection;
using Forge;
using HephaestusDemo.Controllers;

namespace HephaestusDemo
{
    public class Application
    {
        public static void Main(string[] args)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var blacksmith = Blacksmith.Instance;
            
            blacksmith.WeldAssembly(currentAssembly);
            var controller = blacksmith.GetElement(typeof(ControllerA));
        }
    }
}