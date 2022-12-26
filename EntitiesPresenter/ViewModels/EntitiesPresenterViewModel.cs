using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTOs;
using EntitiesPresenter.Interfaces;
using MessagePipe;

namespace EntitiesPresenter.ViewModels
{
    public class EntitiesPresenterViewModel: IEntitiesPresenterViewModel
    {
        private readonly IDistributedSubscriber<string, EntityDetailsDto> _subscriber;

        public EntitiesPresenterViewModel(IDistributedSubscriber<string, EntityDetailsDto> subscriber)
        {
            _subscriber = subscriber;
            SubscribeAsync();
        }

        public async void SubscribeAsync()
        {
            await _subscriber.SubscribeAsync("entityDetails", x =>
            {
                Console.WriteLine(x.Name);
            });
        }
    }
}
