using System.Collections.Generic;
using Forge.Attributes;
using HephaestusDemo.Services;

namespace HephaestusDemo.Providers
{
    [Element]
    public class FooServiceProvider
    {
        private readonly List<IFooService> _fooServices;
        
        [ForgeConstruct]
        public FooServiceProvider(List<IFooService> fooServices)
        {
            _fooServices = fooServices;
        }

        public List<IFooService> GetFooServices()
        {
            return _fooServices;
        }
    }
}