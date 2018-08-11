using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.UI.Xaml.Navigation;

namespace ComStation.ViewModel
{
    public class SerialViewModel : ViewModelBase
    {
        private ObservableCollection<SerialDevice> _devices;

        public ObservableCollection<SerialDevice> Devices
        {
            get => _devices ?? (_devices = new ObservableCollection<SerialDevice>());
            private set => Set(ref _devices, value);
        }

        private SerialDevice _selectedSerialDevice;
        public object SelectedSerialDevice
        {
            get { return _selectedSerialDevice; }
            set
            {
                SerialDevice device = value as SerialDevice;
                Set<SerialDevice>(ref _selectedSerialDevice, device);
            }
        }
        public SerialViewModel()
        {
        }

        public async Task LoadDeviceData()
        {
            var devices = await DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector());

            foreach (var device in devices)
            {
                var sd = await SerialDevice.FromIdAsync(device.Id);
                Devices.Add(sd);
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            await base.OnNavigatedToAsync(parameter, mode, state);
            await LoadDeviceData();
        }
    }
}