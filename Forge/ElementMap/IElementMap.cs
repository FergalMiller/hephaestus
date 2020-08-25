using System;

namespace Forge.ElementMap
{
    public interface IElementMap
    {
        void AddElement(Type type, object element);
        object GetElement(Type type);
        bool ContainsElementForType(Type type);
    }
}