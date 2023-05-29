using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mapsui.UI.Maui;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Order;
using System.ComponentModel;
using System.Net.Sockets;

namespace MAUI1.User.Driver
{
    public class DriverViewModel : UserVM
    {
        public DriverMapController DriverMapController { get; set; }
        public OrderViewModel Order { get; set; }
        public ImageSource AvatarSource
        {
            get
            {
                var avatarPath = $"{App.projectPersonalFolderPath}\\avatar.png";
                if (File.Exists(avatarPath))
                {
                    return ImageSource.FromFile(avatarPath);
                }
                else
                {
                    var defaultDriverImagePath = $"{App.projectPersonalFolderPath}\\DefaultDriverImage.png";
                    return ImageSource.FromFile(defaultDriverImagePath);
                }
            }
        }
        public ICommand camanda { get; set; }
        public ICommand TapOrderStartingPointLabelTappedCommand { get; set; }
        public ICommand TapOrderEndingPointLabelTappedCommand { get; set; }

        public DriverViewModel(MapView mapview, UserModel driver)
        {
            this.DriverMapController = new DriverMapController(mapview);
            User = driver;
            DriverCommandsInit();
        }
        public DriverViewModel(UserModel driver)
        {
            User = driver;
            DriverCommandsInit();
        }

        public DriverViewModel()
        {
            DriverCommandsInit();
        }
        public void DriverCommandsInit()
        {
            TapOrderStartingPointLabelTappedCommand = new Command
            (
                async () =>
                {
                    this.DriverMapController.SecondsBeforeStartingPointPinCalloutDisappear = DriverMapController.SECONDS_BEFORE_PIN_DISSAPEAR;
                    if (this.DriverMapController.HideStartingPointPinCalloutTimer == null)
                    {
                        var lonLat = await MapController.GetLonLatFromAddress(Order.StartingPoint);
                        this.DriverMapController.StartingPointPin = new Pin(DriverMapController.mapView)
                        {
                            Position = new Position(lonLat.Item2, lonLat.Item1),
                            Address = Order.StartingPoint,
                            Color = Colors.Red,
                            Scale = 0.67f,
                            Type = PinType.Pin,
                            Label = "Точка отправки",

                        };
                        this.DriverMapController.mapView.Pins.Add(this.DriverMapController.StartingPointPin);
                        this.DriverMapController.mapView.Refresh();
                        this.DriverMapController.StartingPointPin.ShowCallout();
                        this.DriverMapController.HideStartingPointPinCalloutTimer = new System.Timers.Timer(1000);
                        this.DriverMapController.HideStartingPointPinCalloutTimer.Elapsed += (o, e) =>
                        {
                            if (!this.DriverMapController.StartingPointPin.Callout.IsVisible)
                            {
                                this.DriverMapController.HideStartingPointPinCalloutTimer.Stop();
                                this.DriverMapController.HideStartingPointPinCalloutTimer.Dispose();
                                this.DriverMapController.HideStartingPointPinCalloutTimer = null;
                                this.DriverMapController.SecondsBeforeStartingPointPinCalloutDisappear = DriverMapController.SECONDS_BEFORE_PIN_DISSAPEAR;
                                return;
                            }
                            if (this.DriverMapController.SecondsBeforeStartingPointPinCalloutDisappear == 0)
                            {
                                this.DriverMapController.StartingPointPin.HideCallout();
                                this.DriverMapController.HideStartingPointPinCalloutTimer.Stop();
                                this.DriverMapController.HideStartingPointPinCalloutTimer.Dispose();
                                this.DriverMapController.HideStartingPointPinCalloutTimer = null;
                                this.DriverMapController.SecondsBeforeStartingPointPinCalloutDisappear = DriverMapController.SECONDS_BEFORE_PIN_DISSAPEAR;
                                return;
                            }
                            this.DriverMapController.SecondsBeforeStartingPointPinCalloutDisappear -= 1;
                            this.DriverMapController._startingPointLabelParts[1] = this.DriverMapController.SecondsBeforeStartingPointPinCalloutDisappear.ToString();
                            this.DriverMapController.OnPropertyChanged(nameof(this.DriverMapController.StartingPointPinLabelProperty));
                        };
                        this.DriverMapController.HideStartingPointPinCalloutTimer.Start();
                    }
                }
            );
            TapOrderEndingPointLabelTappedCommand = new Command
            (
                async () =>
                {
                    this.DriverMapController.SecondsBeforeEndingPointPinCalloutDisappear = DriverMapController.SECONDS_BEFORE_PIN_DISSAPEAR;
                    if (this.DriverMapController.HideEndingPointPinCalloutTimer == null)
                    {
                        var lonLat = await MapController.GetLonLatFromAddress(Order.EndingPoint);
                        this.DriverMapController.EndingPointPin = new Pin(DriverMapController.mapView)
                        {
                            Position = new Position(lonLat.Item2, lonLat.Item1),
                            Address = Order.EndingPoint,
                            Color = Colors.Red,
                            Scale = 0.67f,
                            Type = PinType.Pin,
                            Label = "Точка прибытия",

                        };
                        this.DriverMapController.mapView.Pins.Add(this.DriverMapController.EndingPointPin);
                        this.DriverMapController.mapView.Refresh();
                        this.DriverMapController.EndingPointPin.ShowCallout();
                        this.DriverMapController.HideEndingPointPinCalloutTimer = new System.Timers.Timer(1000);
                        this.DriverMapController.HideEndingPointPinCalloutTimer.Elapsed += (o, e) =>
                        {
                            if (!this.DriverMapController.EndingPointPin.Callout.IsVisible)
                            {
                                this.DriverMapController.HideEndingPointPinCalloutTimer.Stop();
                                this.DriverMapController.HideEndingPointPinCalloutTimer.Dispose();
                                this.DriverMapController.HideEndingPointPinCalloutTimer = null;
                                this.DriverMapController.SecondsBeforeEndingPointPinCalloutDisappear = DriverMapController.SECONDS_BEFORE_PIN_DISSAPEAR;
                                return;
                            }
                            if (this.DriverMapController.SecondsBeforeEndingPointPinCalloutDisappear == 0)
                            {
                                this.DriverMapController.EndingPointPin.HideCallout();
                                this.DriverMapController.HideEndingPointPinCalloutTimer.Stop();
                                this.DriverMapController.HideEndingPointPinCalloutTimer.Dispose();
                                this.DriverMapController.HideEndingPointPinCalloutTimer = null;
                                this.DriverMapController.SecondsBeforeEndingPointPinCalloutDisappear = DriverMapController.SECONDS_BEFORE_PIN_DISSAPEAR;
                                return;
                            }
                            this.DriverMapController.SecondsBeforeEndingPointPinCalloutDisappear -= 1;
                            this.DriverMapController._endingPointLabelParts[1] = this.DriverMapController.SecondsBeforeEndingPointPinCalloutDisappear.ToString();
                            this.DriverMapController.OnPropertyChanged(nameof(this.DriverMapController.EndingPointPinLabelProperty));
                        };
                        this.DriverMapController.HideEndingPointPinCalloutTimer.Start();
                    }
                }
            );
        }
    }
}

