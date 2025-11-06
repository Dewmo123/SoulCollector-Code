namespace Work.KimMin._01_Code.PlayerSkill
{
    public interface IDamageIncreasable
    {
        int Level { get; }
        float BaseDamageMultiplier { get; }
        float DamageIncreasePerLevel { get; }
        float DamageMultiplier => BaseDamageMultiplier + DamageIncreasePerLevel * Level;
    }
}