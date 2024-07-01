using System;

public interface IHealth
{
    public void TakeDamage(int damage);

    //This would be use to test the function
    protected void BasicDamage();

    //This would be use to test the function or cheat
    protected void TakeTotalDamage();

    public void Dead();

    public void SuscribeAction(Action action);

    public void Unsuscribe(Action action);
}

public interface IHealth<T> : IHealth
{
    public void SuscribeAction(Action<T> action);

    public void Unsuscribe(Action<T> action);
}