using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Common.DTOs;
using EntitiesPresenter.Interfaces;
using EntitiesPresenter.Models;
using MessagePipe;

namespace EntitiesPresenter.ViewModels
{
    public class EntitiesPresenterViewModel: IEntitiesPresenterViewModel, INotifyPropertyChanged
    {
        private readonly IDistributedSubscriber<string, EntityDetailsDto> _subscriber;
        public ObservableCollection<EntityModel> EntitiesToShowInCanvas { get; set; }

        public EntitiesPresenterViewModel(IDistributedSubscriber<string, EntityDetailsDto> subscriber)
        {
            EntitiesToShowInCanvas = new ObservableCollection<EntityModel>();
            _subscriber = subscriber;
            SubscribeAsync();
        }

        public async void SubscribeAsync()
        {
            await _subscriber.SubscribeAsync("entityDetails", x =>
            {
                EntityModel entityModel = new EntityModel
                {
                    Name = x.Name,
                    X = x.X,
                    Y = x.Y
                };
                Application.Current.Dispatcher.Invoke(() => EntitiesToShowInCanvas.Add(entityModel));
              
                OnPropertyChanged(nameof(EntitiesToShowInCanvas));

            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
