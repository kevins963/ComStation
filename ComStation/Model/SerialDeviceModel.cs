using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;

namespace ComStation.Model
{
    public class SerialDeviceModel : Template10.Mvvm.BindableBase
    {
        SerialDevice _serialDevice;

        private void OnSerialDeviceErrorReceived(SerialDevice sender, ErrorReceivedEventArgs args)
        {
            // TODO what should happen during error
            throw new NotImplementedException();
        }

        private void OnSerialDevicePinChanged(SerialDevice sender, PinChangedEventArgs args)
        {
            // TODO what should happen during pin changed
            throw new NotImplementedException();
        }

        public SerialDeviceModel(SerialDevice serialDevice)
        {
            _serialDevice = serialDevice;

            _serialDevice.ErrorReceived += OnSerialDeviceErrorReceived;
            _serialDevice.PinChanged += OnSerialDevicePinChanged;
        }

    }
}
