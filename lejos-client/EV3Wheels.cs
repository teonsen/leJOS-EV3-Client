using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace lejos_client
{
    /// <summary>
    /// Wheels motor must be connected to B and C.
    /// </summary>
    public class EV3Wheels
    {
        private readonly EV3 _ev3;

        public EV3Wheels(EV3 ev3)
        {
            _ev3 = ev3;
        }

        class Command
        {
            public string cmd { get; set; }
            public string dev { get; set; }
            public int speed { get; set; }
            public int rotate_deg { get; set; }
            public string port { get; set; }
        }

        class EV3CommandRoot
        {
            public Command command { get; set; }
        }

        public void GoForward(int speed, int rotate_Degree)
        {
            WheelsCommand(eCommandTypes.GO_FORWARD, speed, rotate_Degree);
        }

        public void GoBackward(int speed, int rotate_Degree)
        {
            WheelsCommand(eCommandTypes.GO_BACKWARD, speed, rotate_Degree);
        }

        public void TurnLeft(int speed, int rotate_Degree)
        {
            WheelsCommand(eCommandTypes.TURN_LEFT, speed, rotate_Degree);
        }

        public void TurnRight(int speed, int rotate_Degree)
        {
            WheelsCommand(eCommandTypes.TURN_RIGHT, speed, rotate_Degree);
        }

        public void Stop()
        {
            WheelsCommand(eCommandTypes.STOP, 1);
        }


        private void WheelsCommand(eCommandTypes eCmd, int speedToSet, int rotateDeg = 360)
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.WHEELS.ToString().ToLower(),
                    cmd = eCmd.ToString().ToLower(),
                    speed = speedToSet,
                    rotate_deg = rotateDeg
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            _ev3.SendCommand(jsonCmd);
        }

        public bool IsMoving()
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.WHEELS.ToString().ToLower(),
                    cmd = eCommandTypes.ISMOVING.ToString().ToLower(),
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            string jsonRet = _ev3.SendCommand(jsonCmd, true);
            if (jsonRet.Contains("[null]"))
            {
                return false;
            }
            else
            {
                var jv = JsonConvert.DeserializeObject<lejosIsMoving>(jsonRet);
                return jv.ismoving;
            }
        }

        public class lejosIsMoving
        {
            public string dev { get; set; }
            public bool ismoving { get; set; }
        }

    }
}
