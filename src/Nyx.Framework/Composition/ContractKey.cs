using System;
using Nyx.Composition.Impl;

namespace Nyx.Composition
{
    /// <summary>
    /// Key used by <see cref="ContainerImpl"/> to identify types
    /// </summary>
    internal class ContractKey : IEquatable<ContractKey>
    {
        public bool Equals(ContractKey other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(_name, other._name) && _type == other._type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = obj as ContractKey;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_name.GetHashCode() * 397) ^ _type.GetHashCode();
            }
        }

        private readonly string _name;
        private readonly Type _type;

        /// <summary>
        /// Creates an instance of ContractKey from <paramref name="type" /> and an
        /// optional <paramref name="name" />
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public ContractKey(Type type, string name = null)
        {
            _type = type;
            _name = name ?? string.Empty;
        }

        /// <summary>
        /// Creates an instance of ContractKey from a registration
        /// </summary>
        /// <param name="reg"></param>
        public ContractKey(IInternalServiceRegistration reg)
        {
            _name = reg.Name ?? string.Empty;
            _type = reg.ContractType;
        }
    }
}