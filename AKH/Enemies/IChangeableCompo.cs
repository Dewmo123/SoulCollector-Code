namespace Scripts.Enemies
{
    public interface IChangeableCompo<T>
    {
        void Change(T before,T current);
    }
}
