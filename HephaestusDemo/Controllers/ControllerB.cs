using Forge.Attributes;
using HephaestusDemo.Services;

namespace HephaestusDemo.Controllers
{
    [Element]
    public class ControllerB
    {
        private ServiceA _serviceA;
        private ServiceB _serviceB;
        
        [ForgeConstruct]
        public ControllerB(ServiceA serviceA, ServiceB serviceB)
        {
            _serviceA = serviceA;
            _serviceB = serviceB;
        }
    }
}