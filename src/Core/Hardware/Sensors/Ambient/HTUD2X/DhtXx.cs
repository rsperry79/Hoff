//using System.Device.I2c;
//using System.Diagnostics;
//using System.Threading;
//using Hoff.Core.Interfaces;
//using Iot.Device.DHTxx.Esp32;

//namespace Hoff.Hardware.Sensors.Ambient.HTUD2X
//{
//    public class DhtXx : AbstractSensor, IHTU21D, IAbstractSensor
//    {
//        #region Implementation
//        /// <summary>
//        /// The underlying I2C device
//        /// </summary>
//        private I2cDevice i2cDevice = null;

//        /// <summary>
//        /// How many decimal places to account in temperature and humidity measurements
//        /// </summary>
//        private readonly uint _scale = 2;
//        #endregion

//        #region Constructor

//        public DhtXx(int busSelector = 1, byte deviceAddr = 0x40, I2cBusSpeed speed = I2cBusSpeed.StandardMode,
//        uint scale = 2)

//        {
//            // Init DHT10 through I2C
//            Debug.WriteLine($"Using DHT10 on I2C.");
//            I2cConnectionSettings settings = new(1, Dht10.DefaultI2cAddress);
//            i2cDevice = I2cDevice.Create(settings);

//            Dht12 dht = new(i2cDevice);
//        }
//        #endregion

//        #region Properties
//        /// <summary>
//        /// Accessor/Mutator for relative humidity %
//        /// </summary>
//        public float RelativeHumidity { get; private set; }

//        /// <summary>
//        /// Accessor/Mutator for temperature in celcius
//        /// </summary>
//        public float TemperatureInCelcius { get; private set; }
//        /// <summary>
//        /// Sensor resolution
//        /// 0000 0000 = 12bit RH, 14bit Temperature
//        /// 0000 0001 = 8bit RH, 12bit Temperature
//        /// 1000 0000 = 10bit RH, 13bit Temperature
//        /// 1000 0001 = 11bit RH, 11bit Temperature
//        /// </summary>
//        public byte Resolution { get; set; }

//        /// <summary>
//        /// This sensor suports change tracking
//        /// </summary>
//        /// <returns>bool</returns>
//        public override bool CanTrackChanges()
//        {
//            return true;
//        }
//        #endregion

//        public override bool HasSensorValueChanged()
//        {
//            throw new System.NotImplementedException();
//        }

//        public override void Initialize()
//        {
//            throw new System.NotImplementedException();
//        }

//        public override void PrepareToRead()
//        {
//            throw new System.NotImplementedException();
//        }

//        public override bool Read()
//        {
//            throw new System.NotImplementedException();
//        }

//        public override void Reset()
//        {
//            throw new System.NotImplementedException();
//        }

//        protected override void DisposeSensor()
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
