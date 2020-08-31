using Forge.Attributes;
using HephaestusDemo.Database;

namespace HephaestusDemo.Services
{
    [Element]
    public class ServiceB : IFooService
    {
        private DAO _dao;
        
        [ForgeConstruct]
        public ServiceB(DAO dao)
        {
            _dao = dao;
        }
    }
}