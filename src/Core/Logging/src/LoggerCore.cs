using Hoff.Core.Interfaces;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;
using nanoFramework.Logging.Debug;
using nanoFramework.Logging.Serial;
using nanoFramework.Logging.Stream;

using System.IO;

#if BUIID_FOR_ESP32
using nanoFramework.Hardware.Esp32;
#endif


namespace Hoff.Core.Logging
{
   

    public class LoggerCore : ILoggerCore
    {
        private static DebugLogger _logger { get; set; }
        public LoggerCore()
        {

        }

        public DebugLogger GetDebugLogger(string loggerName, LogLevel logLevel)
        {
            _logger = new DebugLogger(loggerName)
            {
                MinLogLevel = logLevel
            };

            LogDispatcher.LoggerFactory = new DebugLoggerFactory();

            return _logger;
        }

        public void GetSerialLogger()
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
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                // COM6 in STM32F769IDiscovery board (Tx, Rx pins exposed in Arduino header CN13: TX->D1, RX->D0)
                // open COM6
                LogDispatcher.LoggerFactory = new SerialLoggerFactory("COM6");
#endif

            }
            finally
            {
                LogDispatcher.LoggerFactory = null;
            }
        }

        public void GetMemoryStreamLogger()
        {
            MemoryStream memoryStream = new MemoryStream();
            LogDispatcher.LoggerFactory = new StreamLoggerFactory(memoryStream);
        }
    }
}
