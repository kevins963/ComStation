using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Streams;
using Windows.Foundation;
using System.Diagnostics;

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
        public SerialDevice SelectedSerialDevice
        {
            get { return _selectedSerialDevice; }
            set
            {
                SerialDevice device = value as SerialDevice;
                Set<SerialDevice>(ref _selectedSerialDevice, device);
                OpenPortCommand.RaiseCanExecuteChanged();
            }
        }

        private int _baudRate;
        public int BaudRate {
            get => _baudRate;
            set => Set(ref _baudRate, value);
        }

        private bool _isOpened;
        public bool IsOpened
        {
            get => _isOpened;
            private set
            {
                Set(ref _isOpened, value);
                OpenPortCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _openPortCommand;
        public DelegateCommand OpenPortCommand => _openPortCommand ?? (_openPortCommand = new DelegateCommand(OpenSerialPort, () => {
            if(SelectedSerialDevice == null || IsOpened)
            {
                return false;
            }

            return true;
        }));

        private String _outputData;
        public String OutputData
        {
            get => _outputData ?? (_outputData = String.Empty);
            set => Set(ref _outputData, value);
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

        async void OpenSerialPort()
        {
            IsOpened = true;
            try
            {
                SelectedSerialDevice.BaudRate = 115200;
                var dataReaderObject = new DataReader(SelectedSerialDevice.InputStream);
                dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

                while (true)
                {
                    var loadAsyncTask = dataReaderObject.LoadAsync(1024);

                    var bytesRead = await loadAsyncTask;

                    if (bytesRead > 0)
                    {
                        byte[] bytes = new byte[bytesRead];
                        dataReaderObject.ReadBytes(bytes);

                        String output = System.Text.Encoding.UTF8.GetString(bytes);
                        Debug.Write(output);
                        OutputData += output;
                    }
                }
            }
            catch
            {
                Debug.WriteLine("ERROR");
            }

            IsOpened = false;
        }


        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            await base.OnNavigatedToAsync(parameter, mode, state);
            await LoadDeviceData();
        }
    }
}