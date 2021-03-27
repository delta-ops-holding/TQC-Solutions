namespace RestAPI.Data.Objects
{
    public class ClanAuthority : BaseEntity
    {
        private string _userName;
        private bool _isFounder;

        public string UserName { get => _userName; set => _userName = value; }
        public bool IsFounder { get => _isFounder; set => _isFounder = value; }
    }
}
