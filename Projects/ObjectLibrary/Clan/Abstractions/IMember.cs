using ObjectLibrary.Common;
using ObjectLibrary.Common.Abstractions;

namespace ObjectLibrary.Clan.Abstractions
{
    public interface IMember : IBaseEntity
    {
        string UserName { get; }
        AuthorityType AuthorityType { get; }
    }
}