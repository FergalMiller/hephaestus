using System;
using System.Collections.Generic;

namespace Forge.ElementMap
{
    public class ElementMap : IElementMap
    {
        private readonly Dictionary<Type, object> _elementMap;

        public ElementMap()
        {
            _elementMap = new Dictionary<Type, object>();
        }

        public void AddElement(Type type, object element)
        {
            if (_elementMap.ContainsKey(type))
                throw new Exception($"Dependency already created for element \"{type}\"");
            
            _elementMap.Add(type, element);
        }

        public object GetElement(Type type)
        {
            if (_elementMap.TryGetValue(type, out var obj))
            {
                return obj;
            }
            throw new Exception($"No such element exists for type {type}");
        }

        public bool ContainsElementForType(Type type)
        {
            return _elementMap.ContainsKey(type);
        }
    }
}