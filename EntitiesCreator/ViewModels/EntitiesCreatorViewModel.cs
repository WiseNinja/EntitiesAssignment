using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using Common.DTOs;
using EntitiesCreator.Interfaces;
using EntitiesCreator.Models;
using GalaSoft.MvvmLight.Command;
using MessagePipe;

namespace EntitiesCreator.ViewModels
{
    public class EntitiesCreatorViewModel : IEntitiesCreatorViewModel
    {
        private readonly IDistributedPublisher<string, EntityDetailsDto> _publisher;
        public ICommand ButtonCommand { get; private set; }

        public EntityModel EntityModel { get; set; }

        public EntitiesCreatorViewModel(IDistributedPublisher<string, EntityDetailsDto> publisher)
        {
            _publisher = publisher;
            EntityModel = new EntityModel();
            ButtonCommand = new RelayCommand(PublishAsync);
        }

        public async void PublishAsync()
        {
            try
            {
                EntityDetailsDto entityDetailsDto = new EntityDetailsDto
                {
                    Name = EntityModel.Name,
                    X = EntityModel.X,
                    Y = EntityModel.Y
                };
                await _publisher.PublishAsync("entityDetails", entityDetailsDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred during entity dispatch, details: {ex}");
            }
        }
    }
}
