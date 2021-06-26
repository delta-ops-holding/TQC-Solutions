using ObjectLibrary.Common;
using ObjectLibrary.Common.Abstractions;
using System.Collections.Generic;

namespace ObjectLibrary.Clan.Abstractions
{
    public interface IClan : IBaseEntity
    {
        string About { get; }
        ClanFounder Founder { get; }
        List<ClanAdmin> Admins { get; }
        string Name { get; }
        Platform Platform { get; }

        void AddClanMember(ClanAdmin member);

        void AddClanFounder(ClanFounder founder);
    }
}