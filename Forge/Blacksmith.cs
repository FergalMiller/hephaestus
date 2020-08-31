using System;
using System.Collections;
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

        public static IBlacksmith Instance
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

        private readonly IElementMap _elementMap;

        private Type[] _elementTypes;

        public void WeldAssembly(Assembly assembly)
        {
            _elementTypes = GetElementTypes(assembly);
            InitialiseElementTypes();
        }

        public IElementMap GetElementMap()
        {
            return _elementMap;
        }

        private Type[] GetElementTypes(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(type => type.GetCustomAttributes(typeof(Element), true).Length > 0)
                .ToArray();
        }

        private void InitialiseElementTypes()
        {
            foreach (var elementType in _elementTypes)
            {
                if (_elementMap.ContainsElementForType(elementType)) continue;
                InitialiseElementType(elementType);
            }
        }

        private void InitialiseElementType(Type elementType)
        {
            if (_elementMap.ContainsElementForType(elementType)) throw new Exception($"Element already exists for type {elementType}");
            if (!IsElement(elementType)) throw new Exception($"Type does not have Element attribute {elementType}");
            var elementInstance = ForgeNewInstance(elementType);
            _elementMap.AddElement(elementType, elementInstance);
        }

        private bool ElementHasForgeConstructor(Type elementType, out ConstructorInfo constructorInfo)
        {
            foreach (var constructor in elementType.GetConstructors())
            {
                if (IsForgeConstructor(constructor))
                {
                    constructorInfo = constructor;
                    return true;
                }
            }

            //If not, set constructorInfo to a default constructor
            constructorInfo = elementType.GetConstructors()[0];
            return false;
        }

        private object ForgeNewInstance(Type elementType)
        {
            if (ElementHasForgeConstructor(elementType, out var constructorInfo))
            {
                var constructorParameters = constructorInfo.GetParameters();
                var extractedArguments = GetInitialisedConstructorArguments(constructorParameters);
                return Activator.CreateInstance(elementType, extractedArguments);
            }
            
            return Activator.CreateInstance(elementType);
        }

        private object[] GetInitialisedConstructorArguments(ParameterInfo[] constructorParameters)
        {
            var elementArguments = new object[constructorParameters.Length];

            var i = 0;
            foreach (var parameter in constructorParameters)
            {
                var parameterType = parameter.ParameterType;
                if (TypeImplementsInterface(parameterType, typeof(ICollection)))
                {
                    elementArguments[i] = GenerateCollectionArgument(parameterType);
                }
                else
                {
                    elementArguments[i] = GetOrInitiateAndGetElement(parameterType);
                }
                
                i++;
            }

            return elementArguments;
        }

        private ICollection GenerateCollectionArgument(Type type)
        {
            if (TypeImplementsInterface(type, typeof(IList)))
            {
                return GenerateListArgument(type);
            }
            
            throw new Exception($"Cannot create injectable dependency for collection type {type}");
        }

        private IList GenerateListArgument(Type listType)
        {
            var parameterType = listType.GenericTypeArguments[0];
            
            var elements = _elementTypes
                .Where(type => parameterType.IsAssignableFrom(type))
                .Select(GetOrInitiateAndGetElement)
                .ToList();

            //Invokes default constructor for parameterised list type and populates with relevant elements
            var argumentListConstructor = listType.GetConstructors()[0];
            var list = (IList) argumentListConstructor.Invoke(new object[] { });
            elements.ForEach(e => list.Add(e));
            
            return list;
        }

        private object GetOrInitiateAndGetElement(Type elementType)
        {
            if (_elementMap.ContainsElementForType(elementType))
            {
                return _elementMap.GetElement(elementType);
            }
            
            InitialiseElementType(elementType);
            
            return _elementMap.GetElement(elementType);
        }
        
        private bool IsElement(Type elementType)
        {
            return elementType.GetCustomAttributes(typeof(Element), true).Length > 0;
        }
        
        private bool IsForgeConstructor(ConstructorInfo constructor)
        {
            return constructor.GetCustomAttributes(typeof(ForgeConstruct), true).Length > 0;
        }

        private bool TypeImplementsInterface(Type type, Type @interface)
        {
            return type.GetInterfaces().Contains(@interface);
        }
    }
}