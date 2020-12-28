using System;

namespace lejos_client
{
    public class Sensor
    {
        private readonly EV3 _ev3;
        private readonly EV3Sensor _ev3Sensor;
        private readonly eSensorTypes _eSensorType;
        private readonly eSensorPorts _eSensorPort;
        private readonly eSensorModes _eSensorMode;

        public Sensor(EV3 ev3, eSensorTypes sensorType, eSensorPorts sensorPort, eSensorModes sensorMode)
        {
            _ev3 = ev3;
            _ev3Sensor = new EV3Sensor(_ev3);
            _eSensorType = sensorType;
            _eSensorPort = sensorPort;
            _eSensorMode = sensorMode;
            _ev3Sensor.Init(sensorType, sensorPort, sensorMode);
        }

        public void Close()
        {
            _ev3Sensor.Close(_eSensorPort);
        }

        public SensorValue GetValue()
        {
            return _ev3Sensor.GetValue(_eSensorPort);
        }
    }
}
