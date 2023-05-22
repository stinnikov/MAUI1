using Mapsui.UI.Maui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Driver
{
    public class DriverMapController : MapController, INotifyPropertyChanged
    {
        public const int SECONDS_BEFORE_PIN_DISSAPEAR = 20;
        public System.Timers.Timer HideStartingPointPinCalloutTimer { get; set; }
        public System.Timers.Timer HideEndingPointPinCalloutTimer { get; set; }
        public int SecondsBeforeStartingPointPinCalloutDisappear { get; set; } = SECONDS_BEFORE_PIN_DISSAPEAR;
        public int SecondsBeforeEndingPointPinCalloutDisappear { get; set; } = SECONDS_BEFORE_PIN_DISSAPEAR;
        public string[] _startingPointLabelParts = new string[2]; // 1 - адрес челика, 2 - секунды до исчезновения рамки
        public string StartingPointPinLabelProperty
        {
            get => $"{_startingPointLabelParts[0]}, ({_startingPointLabelParts[1]})";
        }
        private Pin _startingPointPin;
        public Pin StartingPointPin
        {
            get => _startingPointPin;
            set
            {
                if (value != null)
                {
                    if(_startingPointPin != null)
                    {
                        mapView.Pins.Remove(_startingPointPin);
                    }
                    _startingPointPin = value;
                    _startingPointPin.BindingContext = this;
                    _startingPointLabelParts[0] = value.Label;
                    _startingPointLabelParts[1] = SecondsBeforeStartingPointPinCalloutDisappear.ToString();
                    _startingPointPin.SetBinding(Pin.LabelProperty, nameof(StartingPointPinLabelProperty));
                }
            }
        }
        public string[] _endingPointLabelParts = new string[2]; // 1 - адрес челика, 2 - секунды до исчезновения рамки
        public string EndingPointPinLabelProperty
        {
            get => $"{_endingPointLabelParts[0]}, ({_endingPointLabelParts[1]})";
        }
        private Pin _endingPointPin;
        public Pin EndingPointPin
        {
            get => _endingPointPin;
            set
            {
                if (value != null)
                {
                    if (_endingPointPin != null)
                    {
                        mapView.Pins.Remove(_endingPointPin);
                    }
                    _endingPointPin = value;
                    _endingPointPin.BindingContext = this;
                    _endingPointLabelParts[0] = value.Label;
                    _endingPointLabelParts[1] = SecondsBeforeEndingPointPinCalloutDisappear.ToString();
                    _endingPointPin.SetBinding(Pin.LabelProperty, nameof(EndingPointPinLabelProperty));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public DriverMapController(MapView mapView) : base(mapView)
        {
            IsMapClickable = false;
        }
        public void MapView_PinClicked()
        {

        }
    }
}
