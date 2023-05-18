

namespace Gameplay.Weapons
{
    public interface IDamagable
    {
    
        UnitBattleIdentity BattleIdentity { get; }

        void ApplyDamage(IDamageDealer damageDealer);

    }


    public enum UnitBattleIdentity
    {
        Neutral,
        Ally,
        Enemy
    }
}


