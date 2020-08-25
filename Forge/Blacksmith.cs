using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Forge.Attributes;
using Forge.ElementMap;

namespace Forge
{
    /**
     * The <see cref="Blacksmith"/> class creates all of the dependencies 
     */
    public class Blacksmith : IBlacksmith
    {
        private static Blacksmith _blacksmith;

        private Blacksmith()
        {
            _elementMap = new ElementMap.ElementMap();
        }

        public static Blacksmith Instance
        {
            get
            {
                if (_blacksmith != null)
                {
                    return _blacksmith;
                }

                _blacksmith = new Blacksmith();
                return _blacksmith;
            }
        }

        private IElementMap _elementMap;

        public void WeldAssembly(Assembly assembly)
        {
            var elementTypes = GetElementTypes(assembly);
            InitialiseElementTypes(elementTypes);
        }

        public object GetElement(Type elementType)
        {
            return _elementMap.GetElement(elementType);
        }

        private IEnumerable<Type> GetElementTypes(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(type => type.GetCustomAttributes(typeof(Element), true).Length > 0)
                .ToArray();
        }

        private void InitialiseElementTypes(IEnumerable<Type> elementTypes)
        {
            foreach (var elementType in elementTypes)
            {
                if (_elementMap.ContainsElementForType(elementType)) continue;
                InitialiseElementType(elementType);
            }
        }

        private void InitialiseElementType(Type elementType)
        {
            if (_elementMap.ContainsElementForType(elementType)) throw new Exception($"Element already exists for type {elementType}");
            if (elementType.GetCustomAttributes(typeof(Element), true).Length == 0) throw new Exception($"Type does not have Element attribute {elementType}");
            var elementInstance = ForgeNewInstance(elementType);
            _elementMap.AddElement(elementType, elementInstance);
        }

        private bool ElementHasForgeConstructor(Type elementType, out ConstructorInfo constructorInfo)
        {
            foreach (var constructor in elementType.GetConstructors())
            {
                if (constructor.GetCustomAttributes(typeof(ForgeConstruct), true).Length > 0)
                {
                    constructorInfo = constructor;
                    return true;
                }
            }

            constructorInfo = default;
            return false;
        }

        private object ForgeNewInstance(Type elementType)
        {
            if (ElementHasForgeConstructor(elementType, out var constructorInfo))
            {
                var constructorParameters = constructorInfo.GetParameters();
                var elementArguments = new object[constructorParameters.Length];

                var i = 0;
                foreach (var parameter in constructorParameters)
                {
                    var parameterType = parameter.ParameterType;
                    if (!_elementMap.ContainsElementForType(parameterType))
                    {
                        InitialiseElementType(parameterType);
                    }

                    elementArguments[i] = _elementMap.GetElement(parameterType);
                    i++;
                }

                return Activator.CreateInstance(elementType, elementArguments);
            }
            
            return Activator.CreateInstance(elementType);
        }
    }
}