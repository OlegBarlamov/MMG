using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Console.Core.Commands.Types
{
    public static class ConsoleCommandTypeDescriptionAssociator
    {
        private static readonly Dictionary<Type, IConsoleCommandTypeDescription> AssociatedTypes = new Dictionary<Type, IConsoleCommandTypeDescription>
        {
            {typeof(int), new IntTypeDescription()},
            {typeof(string), new StringTypeDescription()},
            {typeof(float), new FloatTypeDescription()},
            {typeof(bool), new BoolTypeDescription()},
        };
        
        public static void Associate([NotNull] Type netType, [NotNull] IConsoleCommandTypeDescription description)
        {
            if (netType == null) throw new ArgumentNullException(nameof(netType));
            if (description == null) throw new ArgumentNullException(nameof(description));
            AssociatedTypes.Add(netType, description);
        }
        
        public static IConsoleCommandTypeDescription GetConsoleCommandTypeDescription(Type netType)
        {
            if (netType.IsEnum && !AssociatedTypes.ContainsKey(netType))
                return new EnumTypeDescription(netType);
            
            return AssociatedTypes[netType];
        }
    }
}