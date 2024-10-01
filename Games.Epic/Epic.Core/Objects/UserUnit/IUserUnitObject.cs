using System;

namespace Epic.Core.Objects
{
    public interface IUserUnitObject
    {
        Guid Id { get; }
        IUnitTypeObject UnitType { get; }
        int Count { get; }
        Guid UserId { get; }
        bool IsAlive { get; }
    }
}