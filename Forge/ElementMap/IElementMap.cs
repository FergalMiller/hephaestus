using System;

namespace Forge.ElementMap
{
    /**
     * An <see cref="IElementMap"/> object acts as an IoC container. It maps
     * all types to their instantiated element, and gives helpful methods to access.
     */
    public interface IElementMap
    {
        void AddElement(Type type, object element);
        object GetElement(Type type);
        bool ContainsElementForType(Type type);
    }
}