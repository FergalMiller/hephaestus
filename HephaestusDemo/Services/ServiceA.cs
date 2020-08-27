using Forge.Attributes;
using HephaestusDemo.Database;

namespace HephaestusDemo.Services
{
    [Element]
    public class ServiceA
    {
        private DAO _dao;
        
        [ForgeConstruct]
        public ServiceA(DAO dao)
        {
            _dao = dao;
        }
    }
}