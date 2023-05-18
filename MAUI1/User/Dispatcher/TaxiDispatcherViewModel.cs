using Mapsui.UI.Maui;
using MAUI1.User.Client;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Driver;
using MAUI1.User.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User.Dispatcher
{
    internal class TaxiDispatcherViewModel : UserVM 
    {
        public Color DefaultClientColor { get { return Colors.Red; } }
        public Color DefaultSelectedClientColor => Colors.DeepPink;
        public Color DefaultDriverColor { get { return Colors.SkyBlue; } }
        public ObservableCollection<OrderViewModel> Collection { get; private set; } = new ObservableCollection<OrderViewModel>();
        private bool _isShownAllOrders = true;
        public bool IsShownAllOrders 
        {
            get => _isShownAllOrders;
            set
            {
                _isShownAllOrders = value;
                OnPropertyChanged("IsShownAllOrders");
            }
        }
        private bool _isDriversShownOnTheMap = true;
        public bool IsDriversShownOnTheMap
        {
            get => _isDriversShownOnTheMap;
            set
            {
                _isDriversShownOnTheMap = value;
                OnPropertyChanged("IsDriversShownOnTheMap");
            }
        }
        private bool _isAllClientsShownOnTheMap = false;
        public bool IsAllClientsShownOnTheMap
        {
            get => _isAllClientsShownOnTheMap;
            set
            {
                _isAllClientsShownOnTheMap = value;
                if(value && SelectedClientVM != null)
                {
                    selectedClientPin.Color = DefaultSelectedClientColor;
                }
                else if(SelectedClientVM != null)
                {
                    selectedClientPin.Color = DefaultClientColor;
                }
                OnPropertyChanged("IsAllClientsShownOnTheMap");
            }
        }
        private Mapsui.UI.Maui.Pin selectedClientPin;
        private DriverViewModel _selectedDriverViewModel;
        public DriverViewModel SelectedDriverVM 
        {
            get => _selectedDriverViewModel;
            set
            {
                if(value != null)
                {
                    _selectedDriverViewModel = value;
                    OnPropertyChanged("SelectedDriverVM");
                }
            }
        }
        private ClientViewModel _selectedClientVM;
        public ClientViewModel SelectedClientVM 
        { 
            get { return _selectedClientVM; } 
            set 
            { 
                if (value != null) 
                { 
                    _selectedClientVM = value;
                    selectedClientPin = this.DispatcherMapController?.UserPinDataCollection?.Where(item => item.User == SelectedClientVM).FirstOrDefault()?.Pin;
                    if (IsAllClientsShownOnTheMap)
                    {
                        selectedClientPin.Color = DefaultSelectedClientColor;
                    }
                    else
                    {
                        selectedClientPin.Color = DefaultClientColor;
                    }
                    OnPropertyChanged("SelectedClientVM"); 
                }
            } 
        }
        private DispatcherMapController _dispatcherMapController;
        public DispatcherMapController DispatcherMapController
        {
            get => _dispatcherMapController;
            set
            {
                if(value != null)
                {
                    _dispatcherMapController = value;
                    _dispatcherMapController.IsMapClickable = false;
                }
            }
        }
        
        public ICommand ShowClientPinCommand { get; set; }
        public ICommand ClientOrderSelectedCommand { get; set; }
        public ICommand DriversOnTheMapShowCheckedChangedCommand { get; set; }
        public ICommand AllClientOnTheMapCheckedChangedCommand { get; set; }
        public TaxiDispatcherViewModel(MapView mapview, List<UserPinData> pins)
        {
            DispatcherMapController = new(mapview, pins);
            DispatcherVMInit();
        }
        public TaxiDispatcherViewModel()
        {
            DispatcherVMInit();
            
        }
        private void DispatcherVMInit()
        {
            ShowClientPinCommand = new Command(obj =>
            {
                ShowClientPin(obj as UserVM);
            });
            ClientOrderSelectedCommand = new Command(obj =>
            {
                var clientVM = (obj as OrderViewModel).ClientVM;
                ShowClientPin(clientVM);
                IsShownAllOrders = !IsShownAllOrders;
            });
            DriversOnTheMapShowCheckedChangedCommand = new Command(() =>
            {
                if (IsDriversShownOnTheMap)
                {
                    foreach (var element in DispatcherMapController.UserPinDataCollection)
                    {
                        if (element.User.GetType() != typeof(DriverViewModel))
                            continue;
                        element.Pin.IsVisible = true;
                    }
                }
                else
                {
                    foreach (var element in DispatcherMapController.UserPinDataCollection)
                    {
                        if (element.User.GetType() != typeof(DriverViewModel))
                            continue;
                        element.Pin.IsVisible = false;
                    }
                }
            });
            AllClientOnTheMapCheckedChangedCommand = new Command(() =>
            {
                if(IsAllClientsShownOnTheMap)
                {
                    foreach(var element in DispatcherMapController.UserPinDataCollection)
                    {
                        if (element.User.GetType() != typeof(ClientViewModel))
                            continue;
                        element.Pin.IsVisible = true;
                    }
                }
                else
                {
                    foreach (var element in DispatcherMapController.UserPinDataCollection)
                    {
                        if (element.User.GetType() != typeof(ClientViewModel))
                            continue;
                        if (element.User == SelectedClientVM)
                        {
                            continue;
                        }
                        element.Pin.IsVisible = false;
                    }
                }
            });
        }
        private void ShowClientPin(UserVM userVM)
        {
            if (SelectedClientVM != null)
            {
                var previousClientPin = DispatcherMapController.UserPinDataCollection.Where(item => item.User == SelectedClientVM).FirstOrDefault();
                previousClientPin.Pin.IsVisible = false;
                previousClientPin.Pin.Color = DefaultClientColor;
            }
            SelectedClientVM = (ClientViewModel)userVM;
            var clientPin = DispatcherMapController.UserPinDataCollection.Where(item => item.User == SelectedClientVM).FirstOrDefault();
            clientPin.Pin.IsVisible = true;

        }
        
    }
}
