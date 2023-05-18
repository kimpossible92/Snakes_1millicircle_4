
namespace Gameplay.Weapons
{
    public interface IDamageDealer
    {
        
        UnitBattleIdentity BattleIdentity { get; }

        float Damage { get; }

    }
}
