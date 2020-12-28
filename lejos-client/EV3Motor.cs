using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace lejos_client
{
    public class EV3Motor
    {
        private readonly EV3 _ev3;

        public EV3Motor(EV3 ev3)
        {
            _ev3 = ev3;
        }

        class Command
        {
            public string cmd { get; set; }
            public string dev { get; set; }
            public string motor_type { get; set; }
            public string port { get; set; }
            public int is_async { get; set; }
            public int speed { get; set; }
            public int rotate_deg { get; set; }
        }

        class EV3CommandRoot
        {
            public Command command { get; set; }
        }

        public void Init(eMotorTypes motorType, eMotorPorts ev3port)
        {
            MotorCommand(eCommandTypes.INIT, motorType, ev3port);
        }

        public void Close(eMotorTypes motorType, eMotorPorts ev3port)
        {
            MotorCommand(eCommandTypes.CLOSE, motorType, ev3port);
        }

        public void Forward(eMotorTypes motorType, eMotorPorts ev3port, int speed = 300, int milliSec = 0)
        {
            MotorCommandSequence(motorType, ev3port, eCommandTypes.FORWARD, speed, milliSec);
        }

        public void Backward(eMotorTypes motorType, eMotorPorts ev3port, int speed = 300, int milliSec = 0)
        {
            MotorCommandSequence(motorType, ev3port, eCommandTypes.BACKWARD, speed, milliSec);
        }

        private void MotorCommandSequence(eMotorTypes motorType, eMotorPorts ev3port, eCommandTypes cmdType, int speed, int milliSec)
        {
            MotorCommand(eCommandTypes.SETSPEED, motorType, ev3port, speed);
            MotorCommand(cmdType, motorType, ev3port);
            if (milliSec > 0)
            {
                Thread.Sleep(milliSec);
                MotorCommand(eCommandTypes.STOP, motorType, ev3port);
            }
        }

        public void Stop(eMotorTypes motorType, eMotorPorts ev3port) => MotorCommand(eCommandTypes.STOP, motorType, ev3port);

        public void SetSpeed(eMotorTypes motorType, eMotorPorts ev3port, int speed)
        {
            MotorCommand(eCommandTypes.SETSPEED, motorType, ev3port, speed);
        }

        public void Rotate(eMotorTypes motorType, eMotorPorts ev3port, int speed, int angle, bool async)
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.MOTOR.ToString().ToLower(),
                    cmd = eCommandTypes.ROTATE.ToString().ToLower(),
                    motor_type = motorType.ToString().ToLower(),
                    port = ev3port.ToString(),
                    speed = speed,
                    is_async = async ? 1 : 0,
                    rotate_deg = angle
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            _ev3.SendCommand(jsonCmd);
        }

        internal void MotorCommand(eCommandTypes cmdType, eMotorTypes motorType, eMotorPorts ev3port, int speedToSet = 0)
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.MOTOR.ToString().ToLower(),
                    cmd = cmdType.ToString().ToLower(),
                    motor_type = motorType.ToString().ToLower(),
                    port = ev3port.ToString(),
                    speed = speedToSet
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            _ev3.SendCommand(jsonCmd);
        }
    }
}
