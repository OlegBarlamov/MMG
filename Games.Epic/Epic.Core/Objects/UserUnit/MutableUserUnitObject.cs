using System;

namespace Epic.Core.Objects
{
    internal class MutableUserUnitObject : IUserUnitObject
    {
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public int Count { get; set; }
        public Guid UserId { get; set; }
        public bool IsAlive { get; set; }
    }
}