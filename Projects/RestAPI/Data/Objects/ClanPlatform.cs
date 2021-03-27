namespace RestAPI.Data.Objects
{
    public class ClanPlatform : BaseEntity
    {
        private string _name;
        private string _platformImageURL;

        public string Name { get => _name; set => _name = value; }
        public string PlatformImageURL { get => _platformImageURL; set => _platformImageURL = value; }
    }
}
