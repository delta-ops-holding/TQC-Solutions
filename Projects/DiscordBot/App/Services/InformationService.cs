using DatabaseAccess.Repositories.Interfaces;
using DataClassLibrary.Models;
using DiscordBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class InformationService : IInformationService
    {
        //private List<ApplicationChannel> _applicationChannels;
        private MinionInformationModel _minionInformation;

        private readonly IBotInformationRepository _informationRepository;
        private readonly IClanPlatformRepository _platformRepository;

        public MinionInformationModel MinionInformation => _minionInformation;

        public InformationService(IBotInformationRepository informationRepository, IClanPlatformRepository platformRepository)
        {
            _informationRepository = informationRepository;
            _platformRepository = platformRepository;
            LoadData();
        }

        private void LoadData()
        {
            _ = Task.Run(async () =>
            {
                await LoadBotInformationAsync();
                await LoadPlatformApplicationChannelsAsync();
            });
        }

        private async Task LoadBotInformationAsync()
        {
            _minionInformation = await _informationRepository.GetBotInformationAsync();
        }

        private async Task LoadPlatformApplicationChannelsAsync()
        {
            throw new NotImplementedException();
            //_applicationChannels = await _platformRepository.GetAllAsync();
        }
    }
}
