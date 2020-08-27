using Forge.Attributes;
using HephaestusDemo.Services;

namespace HephaestusDemo.Controllers
{
    [Element]
    public class ControllerA
    {
        private readonly ServiceA _serviceA;
        
        [ForgeConstruct]
        public ControllerA(ServiceA serviceA)
        {
            _serviceA = serviceA;
        }
    }
}