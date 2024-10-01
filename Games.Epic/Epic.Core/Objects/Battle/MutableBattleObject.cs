using System;
using System.Collections.Generic;
using Epic.Core.Objects.BattleUnit;

namespace Epic.Core.Objects.Battle
{
    public class MutableBattleObject : IBattleObject
    {
        public Guid Id { get; set; }
        public int TurnIndex { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsActive { get; set; }
        public IReadOnlyCollection<IBattleUnitObject> Units { get; set; }
    }
}