public interface IHittable<T>
{
    void ReceiveDamage(T damageTaken);
}