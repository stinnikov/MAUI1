using GeolocatorPlugin;
using GeolocatorPlugin.Abstractions;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;
using Mapsui.UI.Maui;
using Mapsui.Utilities;
using Mapsui.Widgets;
using Mapsui.Widgets.ButtonWidget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User
{
    internal class MapController
    {
        internal MapView mapView { get; set; }
        public ICommand ButtonCommand { get; set; }
        private Location _location = new();
        public Location Location { get { return _location; } set { if (value != null) { _location = value; } } }
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;
        private ButtonWidget _zoomInButtonWidget;
        private ButtonWidget _zoomOutButtonWidget;
        private ButtonWidget _myLocationButtonWidget;
        private ButtonWidget _northingButtonWidget;
        public MapController(MapView mapView)
        {
            this.mapView = mapView;
            MapViewInit();
            ButtonCommand = new Command(async obj =>
            {
                char[] separators = { ',', '.', ' ' };
                object[] objects = obj as object[];
                string target_string = objects[0].ToString().ToLower();
                string[] target_string_splitted = target_string.Split(separators).Where(item => item != "").ToArray();
                string destination_string = objects[1].ToString().ToLower();
                string[] destination_string_splitted = destination_string.Split(separators);
                target_string = "";
                destination_string = "";
                foreach (string str in target_string_splitted)
                {
                    if (str == "ул")
                    {
                        target_string += "улица ";
                        continue;
                    }
                    if (str == "д")
                    {
                        target_string += "дом ";
                        continue;
                    }
                    if (str == "кв")
                    {
                        target_string += "квартира ";
                        continue;
                    }
                    target_string += str + " ";
                }
                foreach (string str in destination_string_splitted)
                {
                    if (str == "ул")
                    {
                        destination_string += "улица ";
                        continue;
                    }
                    else if (str == "д")
                    {
                        destination_string += "дом ";
                        continue;
                    }
                    else if (str == "кв" || str == "кварт")
                    {
                        destination_string += "квартира ";
                        continue;
                    }
                    destination_string += str + " ";
                }
                IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(target_string);
                Location location = locations?.FirstOrDefault();
                var (x, y) = SphericalMercator.FromLonLat(location.Longitude, location.Latitude);
                var pin1 = new Pin(mapView)
                {
                    Position = new Mapsui.UI.Maui.Position(location.Latitude, location.Longitude),
                    Type = PinType.Pin,
                    Label = $"DDDDDDDDDDDDDDDDDDDD",
                    Address = target_string,
                    Scale = 0.7F,
                    Color = Colors.Red,
                };
                locations = await Geocoding.Default.GetLocationsAsync(destination_string);
                location = locations?.FirstOrDefault();
                (x, y) = SphericalMercator.FromLonLat(location.Longitude, location.Latitude);
                var pin2 = new Pin(mapView)
                {
                    Position = new Mapsui.UI.Maui.Position(location.Latitude, location.Longitude),
                    Type = PinType.Pin,
                    Label = $"DDDDDDDDDDDDDDDDDDDD",
                    Address = destination_string,
                    Scale = 0.7F,
                    Color = Colors.Red,
                };
                mapView.Pins.Add(pin1);
                mapView.Pins.Add(pin2);
                mapView.Refresh();
            });
        }
        public async void MapViewInit()
        {
            mapView.IsMyLocationButtonVisible = false;
            mapView.IsNorthingButtonVisible = false;
            mapView.IsZoomButtonVisible = false;
            mapView.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            mapView.Map.Navigator.Limiter = new Mapsui.Limiting.ViewportLimiterKeepWithinExtent();
            PermissionStatus status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
            {
                ToggleGPS(true);
            }
            CreateButtons();
            mapView.MapClicked += Mapview_MapClicked;
            mapView.SizeChanged += UpdateButtons;
            mapView.Refresh();
        }
        #region КАРТА
        #region КНОПКИ
        #region СОЗДАНИЕ КНОПОК
        private void CreateButtons()
        {
            double buttonMarginRight = 20;
            double buttonSize = 40;
            double buttonSpacing = 8;
            void CreateZoomInButton()
            {
                var pict = EmbeddedResourceLoader.Load("Images.ZoomIn.svg", typeof(MapView)).LoadSvgPicture();
                _zoomInButtonWidget = new ButtonWidget();
                _zoomInButtonWidget.Picture = pict;
                var x = mapView.Width - buttonMarginRight - buttonSize;
                var y = 20; //buttonMarginTop
                _zoomInButtonWidget.Envelope = new MRect(x, y + buttonSpacing, x + buttonSize, y + buttonSize + buttonSpacing);
                _zoomInButtonWidget.Rotation = 0f;
                _zoomInButtonWidget.Enabled = true;
                _zoomInButtonWidget.WidgetTouched += delegate (object s, WidgetTouchedEventArgs e)
                {
                    ZoomInButtonAction(s, e);
                    e.Handled = true;
                };
                _zoomInButtonWidget.PropertyChanged += delegate
                {
                    mapView.RefreshGraphics();
                };
                mapView.Map.Widgets.Add(_zoomInButtonWidget);
            }
            void CreateZoomOutButton()
            {
                var pict = Mapsui.Utilities.EmbeddedResourceLoader.Load("Images.ZoomOut.svg", typeof(MapView)).LoadSvgPicture();
                _zoomOutButtonWidget = new ButtonWidget();
                _zoomOutButtonWidget.Picture = pict;
                var x = mapView.Width - buttonMarginRight - buttonSize;
                var y = buttonSize + buttonSpacing + buttonSpacing + buttonSpacing / 2;
                _zoomOutButtonWidget.Envelope = new MRect(x, y + buttonSpacing, x + buttonSize, y + buttonSize + buttonSpacing);
                _zoomOutButtonWidget.Rotation = 0f;
                _zoomOutButtonWidget.Enabled = true;
                _zoomOutButtonWidget.WidgetTouched += delegate (object s, WidgetTouchedEventArgs e)
                {
                    ZoomOutButtonAction(s, e);
                    e.Handled = true;
                };
                _zoomOutButtonWidget.PropertyChanged += delegate
                {
                    mapView.RefreshGraphics();
                };
                mapView.Map.Widgets.Add(_zoomOutButtonWidget);
            }
            void CreateMyLocButton()
            {
                var pict = Mapsui.Utilities.EmbeddedResourceLoader.Load("Images.LocationCenter.svg", typeof(MapView)).LoadSvgPicture();
                _myLocationButtonWidget = new ButtonWidget();
                _myLocationButtonWidget.Picture = pict;
                var x = mapView.Width - buttonMarginRight - buttonSize;
                var y = buttonSize + buttonSize + buttonSpacing / 2 + buttonSpacing + buttonSpacing;
                _myLocationButtonWidget.Envelope = new MRect(x, y + buttonSpacing, x + buttonSize, y + buttonSize + buttonSpacing);
                _myLocationButtonWidget.Rotation = 0f;
                _myLocationButtonWidget.Enabled = true;
                _myLocationButtonWidget.WidgetTouched += delegate (object s, WidgetTouchedEventArgs e)
                {
                    MyLocButtonAction(s, e);
                    e.Handled = true;
                };
                _myLocationButtonWidget.PropertyChanged += delegate
                {
                    mapView.RefreshGraphics();
                };
                mapView.Map.Widgets.Add(_myLocationButtonWidget);
            }
            void CreateNorthingButton()
            {
                var pict = Mapsui.Utilities.EmbeddedResourceLoader.Load("Images.RotationZero.svg", typeof(MapView)).LoadSvgPicture();
                _northingButtonWidget = new ButtonWidget();
                _northingButtonWidget.Picture = pict;
                var x = mapView.Width - buttonMarginRight - buttonSize;
                var y = buttonSize + buttonSize + buttonSize + buttonSpacing / 2 + buttonSpacing + buttonSpacing;
                _northingButtonWidget.Envelope = new MRect(x, y + buttonSpacing, x + buttonSize, y + buttonSize + buttonSpacing);
                _northingButtonWidget.Rotation = 0f;
                _northingButtonWidget.Enabled = true;
                _northingButtonWidget.WidgetTouched += delegate (object s, WidgetTouchedEventArgs e)
                {
                    NorthingButtonAction(s, e);
                    e.Handled = true;
                };
                _northingButtonWidget.PropertyChanged += delegate
                {
                    mapView.RefreshGraphics();
                };
                mapView.Map.Widgets.Add(_northingButtonWidget);
            }
            CreateZoomInButton();
            CreateZoomOutButton();
            CreateMyLocButton();
            CreateNorthingButton();
        }
        #endregion
        #region ОБНОВЛЕНИЕ КНОПОК
        private void UpdateButtons(object sender, EventArgs e)
        {
            double buttonMarginRight = 20;
            double buttonSize = 40;
            double buttonSpacing = 8;
            void UpdateZoomInButton()
            {
                var x = mapView.Width - buttonMarginRight - buttonSize;
                var y = 20; //buttonMarginTop
                _zoomInButtonWidget.Envelope = new MRect(x, y + buttonSpacing, x + buttonSize, y + buttonSize + buttonSpacing);
            }
            void UpdateZoomOutButton()
            {
                var x = mapView.Width - buttonMarginRight - buttonSize;
                var y = buttonSize + buttonSpacing + buttonSpacing + buttonSpacing / 2;
                _zoomOutButtonWidget.Envelope = new MRect(x, y + buttonSpacing, x + buttonSize, y + buttonSize + buttonSpacing);
            }
            void UpdateMyLocButton()
            {
                var x = mapView.Width - buttonMarginRight - buttonSize;
                var y = buttonSize + buttonSize + buttonSpacing / 2 + buttonSpacing + buttonSpacing;
                _myLocationButtonWidget.Envelope = new MRect(x, y + buttonSpacing, x + buttonSize, y + buttonSize + buttonSpacing);
            }
            void UpdateNorthingButton()
            {
                var x = mapView.Width - buttonMarginRight - buttonSize;
                var y = buttonSize + buttonSize + buttonSize + buttonSpacing / 2 + buttonSpacing + buttonSpacing;
                _northingButtonWidget.Envelope = new MRect(x, y + buttonSpacing, x + buttonSize, y + buttonSize + buttonSpacing);
            }
            UpdateZoomInButton();
            UpdateZoomOutButton();
            UpdateMyLocButton();
            UpdateNorthingButton();
        }
        #endregion
        #region ДЕЙСТВИЯ ПРИ НАЖАТИИ
        private void ZoomInButtonAction(object sender, WidgetTouchedEventArgs e)
        {
            mapView.Map.Navigator.ZoomIn(25, Mapsui.Animations.Easing.Linear);
        }
        private void ZoomOutButtonAction(object sender, WidgetTouchedEventArgs e)
        {
            mapView.Map.Navigator.ZoomOut(25, Mapsui.Animations.Easing.Linear);
        }
        private void MyLocButtonAction(object sender, WidgetTouchedEventArgs e)
        {
            mapView.Map.Navigator.CenterOn(mapView.MyLocationLayer.MyLocation.ToMapsui(), 200, Mapsui.Animations.Easing.Linear);
        }
        private void NorthingButtonAction(object sender, WidgetTouchedEventArgs e)
        {
            mapView.Map.Navigator.RotateTo(0.0, 100, Mapsui.Animations.Easing.Linear);
        }
        #endregion
        #endregion
        private async void Mapview_MapClicked(object sender, MapClickedEventArgs e)
        {
            if (mapView.Pins.Count >= 0)
            {
                var p = e.Point;
                try
                {
                    IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(e.Point.Latitude, e.Point.Longitude);
                    Placemark placemark = placemarks?.FirstOrDefault();
                    var pin = new Pin(mapView)
                    {
                        Position = new Mapsui.UI.Maui.Position(e.Point),
                        Type = PinType.Pin,
                        Label = $"{e.Point.Latitude},{e.Point.Longitude}",
                        //Label = $"{placemark?.AdminArea}, {placemark?.SubAdminArea}, {placemark?.Locality}, {placemark?.Thoroughfare}, {placemark?.SubThoroughfare}",
                        Address = placemark?.Locality + " " + placemark?.SubLocality,
                        Scale = 0.7F,
                        Color = Colors.Red,
                    };
                    mapView.Pins.Add(pin);
                    System.Timers.Timer timer = new System.Timers.Timer();
                    PinTimerInitialization(timer, pin);
                    pin.ShowCallout();
                    return;
                }
                catch
                {
                    return;
                }
            }
            //mapView.Pins.ToList().ForEach(item => item.HideCallout());
            //mapView.Pins.Clear();
        }
        private void PinTimerInitialization(System.Timers.Timer timer, Mapsui.UI.Maui.Pin pin)
        {
            timer.Enabled = true;
            timer.Interval = 5 * 60 * 60;
            timer.Elapsed += (sender, e_args) => { Pin_HideCallout(pin, timer); };
            timer.Start();
        }
        private void Pin_HideCallout(Mapsui.UI.Maui.Pin pin, System.Timers.Timer timer)
        {
            pin.HideCallout();
            timer.Stop();
        }
        private void Mapview_PinClicked(object sender, PinClickedEventArgs e)
        {

        }
        #endregion
        #region ЛОКАЦИЯ
        public async void ToggleGPS(bool toggleOn)
        {
            if (toggleOn)
            {
                if (await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 5, true, new ListenerSettings
                {
                    ActivityType = ActivityType.AutomotiveNavigation,
                    AllowBackgroundUpdates = true,
                    DeferLocationUpdates = false,
                    ListenForSignificantChanges = false,
                    PauseLocationUpdatesAutomatically = false,
                }))
                {
                    CrossGeolocator.Current.PositionChanged += GetLocation;
                    CrossGeolocator.Current.PositionError += NotifyPosError;
                }
            }
            else
            {
                if (await CrossGeolocator.Current.StopListeningAsync())
                {
                    CrossGeolocator.Current.PositionChanged -= GetLocation;
                    CrossGeolocator.Current.PositionError -= NotifyPosError;
                }
            }
        }
        public async void GetLocation(object sender, PositionEventArgs e)
        {
            try
            {
                _isCheckingLocation = true;
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                _cancelTokenSource = new CancellationTokenSource();
                var loc = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
                if (_cancelTokenSource.Token.IsCancellationRequested)
                {
                    //TODO:обработка отмены получения локации
                    return;
                }
                if (Location != null)
                {
                    Location = loc;
                    mapView.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Maui.Position(Location.Latitude, Location.Longitude));
                }

            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
            }
            finally
            {
                _isCheckingLocation = false;
            }
            //mapView.RefreshGraphics();
        }
        private void NotifyPosError(object sender, PositionErrorEventArgs e)
        {
            //TODO:обработать действие при ошибке получения геолокации
            throw new NotImplementedException();
        }
        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
        #endregion
    }
}
