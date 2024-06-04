public interface IHealth
{
    public void TakeDamage(int damage);

    //This would be use to test the function
    protected void BasicDamage();

    //This would be use to test the function or cheat
    protected void TakeTotalDamage();

    public void Dead();
}