using System.Reflection;
using Forge.ElementMap;

namespace Forge
{
    public interface IBlacksmith
    {
        void WeldAssembly(Assembly assembly);
        IElementMap GetElementMap();
    }
}