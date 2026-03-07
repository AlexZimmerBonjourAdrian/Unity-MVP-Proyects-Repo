namespace HorrorEngine
{
    /// <summary>
    /// Datos del evento de hit melee
    /// </summary>
    public class MeleeHitEventData
    {
        public string weaponName;
        public string targetName;
        public int damage;

        public MeleeHitEventData(string weapon, string target, int dmg)
        {
            weaponName = weapon;
            targetName = target;
            damage = dmg;
        }
    }
}
