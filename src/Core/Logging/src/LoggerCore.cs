using System.IO;

using Hoff.Core.Interfaces;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;
using nanoFramework.Logging.Debug;
using nanoFramework.Logging.Serial;
using nanoFramework.Logging.Stream;

#if BUIID_FOR_ESP32
using nanoFramework.Hardware.Esp32;
#endif

namespace Hoff.Core.Services.Logging
{
    public class LoggerCore : ILoggerCore
    {
        private static DebugLogger Logger { get; set; }

        public DebugLogger GetDebugLogger(string loggerName, LogLevel logLevel = LogLevel.Trace)
        {
            Logger = new DebugLogger(loggerName)
            {
                MinLogLevel = logLevel
            };

            LogDispatcher.LoggerFactory = new DebugLoggerFactory();

            return Logger;
        }

        public void GetSerialLogger(string port = null)
        {
            try
            {
#if BUIID_FOR_ESP32
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                // COM2 in ESP32-WROVER-KIT mapped to free GPIO pins
                // mind to NOT USE pins shared with other devices, like serial flash and PSRAM
                // also it's MANDATORY to set pin funcion to the appropriate COM before instanciating it

                // set GPIO functions for COM2 (this is UART1 on ESP32)
                Configuration.SetPinFunction(Gpio.IO04, DeviceFunction.COM2_TX);
                Configuration.SetPinFunction(Gpio.IO05, DeviceFunction.COM2_RX);

                // open COM2
                LogDispatcher.LoggerFactory = new SerialLoggerFactory("COM2");
#else

                // COM6 in STM32F769IDiscovery board (Tx, Rx pins exposed in Arduino header CN13: TX->D1, RX->D0)
                port ??= "COM6";
                LogDispatcher.LoggerFactory = new SerialLoggerFactory(port);
#endif

            }
            finally
            {
                LogDispatcher.LoggerFactory = null;
            }
        }

        public void GetMemoryStreamLogger()
        {
            MemoryStream memoryStream = new();
            LogDispatcher.LoggerFactory = new StreamLoggerFactory(memoryStream);
        }
    }
}
