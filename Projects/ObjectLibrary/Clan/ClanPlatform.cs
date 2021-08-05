using ObjectLibrary.Clan.Interfaces;
using ObjectLibrary.Core;

namespace ObjectLibrary.Clan
{
    public class ClanPlatform : BaseEntity, IPlatform
    {
        private readonly string _name;
        private readonly string _imagePath;
        private readonly IApplicationChannel _applicationChannel;

        public ClanPlatform(int id, string name, string imagePath, IApplicationChannel applicationChannel) : base(id)
        {
            _name = name;
            _imagePath = imagePath;
            _applicationChannel = applicationChannel;
        }

        public string Name { get { return _name; } }
        public string ImagePath { get { return _imagePath; } }
        public IApplicationChannel ApplicationChannel { get { return _applicationChannel; } }
    }
}
