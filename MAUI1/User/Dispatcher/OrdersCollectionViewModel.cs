using MAUI1.User.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User.Dispatcher
{
    public class OrdersCollectionViewModel : ViewModel
    {
        private TaxiDispatcherViewModel _taxiDispatcherViewModel;
        public TaxiDispatcherViewModel TaxiDispatcherViewModel
        {
            get => _taxiDispatcherViewModel;
            set
            {
                _taxiDispatcherViewModel = value;
                OnPropertyChanged(nameof(TaxiDispatcherViewModel));
            }
        }
        public ICommand SelectOrderCommand { get; set; }
        public OrdersCollectionViewModel()
        {
            SelectOrderCommand = new Command(obj => 
            {
                OrderViewModel order = obj as OrderViewModel;
                TaxiDispatcherViewModel.SelectedOrderVM = order;
                (Application.Current.MainPage.Navigation.NavigationStack.Last() as NavigationPage).PopAsync();
            });
        }
    }
}
