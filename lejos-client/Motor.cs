using System;
using System.Collections.Generic;

namespace lejos_client
{
    public class Motor
    {
        private readonly EV3 _ev3;
        private readonly EV3Motor _ev3Motor;
        private readonly eMotorTypes _eMotorType;
        private readonly eMotorPorts _eMotorPort;

        public Motor(EV3 ev3, eMotorTypes motorType, eMotorPorts motorPort)
        {
            _ev3 = ev3;
            _ev3Motor = new EV3Motor(_ev3);
            _eMotorType = motorType;
            _eMotorPort = motorPort;
            _ev3Motor.Init(motorType, motorPort);
        }

        public void Close()
        {
            _ev3Motor.Close(_eMotorType, _eMotorPort);
        }

        /// <summary>
        /// Move the motor forward.
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="milliSec"></param>
        public void Forward(int speed = 300, int milliSec = 0)
        {
            _ev3Motor.Forward(_eMotorType, _eMotorPort, speed, milliSec);
        }

        public void Backward(int speed = 300, int milliSec = 0)
        {
            _ev3Motor.Backward(_eMotorType, _eMotorPort, speed, milliSec);
        }
        
        public void Stop()
        {
            _ev3Motor.Stop(_eMotorType, _eMotorPort);
        }

        public void Rotate(int speed, int degree, bool async)
        {
            _ev3Motor.Rotate(_eMotorType, _eMotorPort, speed, degree, async);
        }
    }
}
