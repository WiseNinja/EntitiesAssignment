using System;
using System.Windows.Input;
using Common.DTOs;
using EntitiesCreator.Interfaces;
using EntitiesCreator.Models;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using XDMessaging;

namespace EntitiesCreator.ViewModels
{
    public class EntitiesCreatorViewModel : IEntitiesCreatorViewModel
    {
        private IXDBroadcaster _broadcaster;

        public ICommand ButtonCommand { get; private set; }

        public EntityModel EntityModel { get; set; }

        public EntitiesCreatorViewModel(XDMessagingClient client)
        {
            _broadcaster = client.Broadcasters
                .GetBroadcasterForMode(XDTransportMode.HighPerformanceUI);

            EntityModel = new EntityModel();
            ButtonCommand = new RelayCommand(Publish);
        }

        public void Publish()
        {
           
            try
            {
                EntityDetailsDto entityDetailsDto = new EntityDetailsDto
                {
                    Name = EntityModel.Name,
                    X = EntityModel.X,
                    Y = EntityModel.Y
                };
                string entityDetailsDtoSerialized = JsonConvert.SerializeObject(entityDetailsDto);
                _broadcaster.SendToChannel("entityDetails", entityDetailsDtoSerialized);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred during entity dispatch, details: {ex}");
            }
        }
    }
}
