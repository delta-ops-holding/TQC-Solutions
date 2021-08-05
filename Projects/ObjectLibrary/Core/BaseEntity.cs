namespace ObjectLibrary.Core
{
    public abstract class BaseEntity : IBaseEntity
    {
        private readonly int _id;

        public BaseEntity(int id)
        {
            _id = id;
        }

        public int Id { get { return _id; } }
    }
}
