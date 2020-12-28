using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace lejos_client
{
    public class EV3Sensor
    {
        private readonly EV3 _ev3;

        public EV3Sensor(EV3 ev3)
        {
            _ev3 = ev3;
        }

        class Command
        {
            public string cmd { get; set; }
            public string dev { get; set; }
            public string port { get; set; }
            public string sensor_type { get; set; }
            public string mode { get; set; }
        }

        class EV3CommandRoot
        {
            public Command command { get; set; }
        }

        public void Init(eSensorTypes sensorType, eSensorPorts port, eSensorModes sensorMode)
        {
            InitSensor(sensorType, port);
            SetMode(sensorMode, port);
        }

        public void Close(eSensorPorts port)
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.SENSOR.ToString().ToLower(),
                    cmd = eCommandTypes.CLOSE.ToString().ToLower(),
                    port = port.ToString()
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            _ev3.SendCommand(jsonCmd);
        }

        private void InitSensor(eSensorTypes sensorType, eSensorPorts ev3port)
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.SENSOR.ToString().ToLower(),
                    cmd = eCommandTypes.INIT.ToString().ToLower(),
                    sensor_type = sensorType.ToString().ToLower(),
                    port = ev3port.ToString()
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            _ev3.SendCommand(jsonCmd);
        }

        private void SetMode(eSensorModes sensorMode, eSensorPorts ev3port)
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.SENSOR.ToString().ToLower(),
                    cmd = eCommandTypes.SETMODE.ToString().ToLower(),
                    mode = sensorMode.ToString().ToLower(),
                    port = ev3port.ToString()
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            _ev3.SendCommand(jsonCmd);
        }

        public SensorValue GetValue(eSensorPorts ev3port)
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.SENSOR.ToString().ToLower(),
                    cmd = eCommandTypes.GETVALUE.ToString().ToLower(),
                    port = ev3port.ToString()
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            string jsonRet = _ev3.SendCommand(jsonCmd, true);
            if (jsonRet.Contains("[null]"))
            {
                return new SensorValue(null);
            }
            else
            {
                var jv = JsonConvert.DeserializeObject<lejosGetValue>(jsonRet);
                return new SensorValue(jv);
            }
        }

        public void WaitColor(int colorIDtoWait, int interval, int retry)
        {

        }

    }


    public class lejosGetValue
    {
        public string port { get; set; }
        public string dev { get; set; }
        public List<double> value { get; set; }
        public string mode { get; set; }
    }

    public class SensorValue
    {
        public SensorValue(lejosGetValue v)
        {
            if (v == null) return;
            
            Port = v.port;
            Dev = v.dev;
            Value = v.value;
            Mode = v.mode;
            for (int i = 0; i < v.value.Count; i++)
            {
                if (i < v.value.Count - 1)
                {
                    Values += v.value[i] + ", ";
                }
                else
                {
                    Values += v.value[i];
                }
            }

            switch (v.mode)
            {
                case "rgb":
                    R = v.value[0];
                    G = v.value[1];
                    B = v.value[2];
                    break;
                case "colorid":
                    ColorID = (int)v.value[0];
                    switch (ColorID)
                    {
                        case 0:
                            Color = "0:None";
                            break;
                        case 1:
                            Color = "1:Black";
                            break;
                        case 2:
                            Color = "2:Blue";
                            break;
                        case 3:
                            Color = "3:Green";
                            break;
                        case 4:
                            Color = "4:Yellow";
                            break;
                        case 5:
                            Color = "5:Red";
                            break;
                        case 6:
                            Color = "6:White";
                            break;
                        case 7:
                            Color = "7:Brawn";
                            break;
                        default:
                            Color = "-1:Not detected";
                            break;
                    }
                    Values = Color;
                    break;
            }
        }

        public string Port { get; private set; } = "";
        public string Dev { get; private set; } = "";
        public List<double> Value { get; private set; }
        public string Mode { get; private set; } = "";

        public double R { get; private set; } = 0;
        public double G { get; private set; } = 0;
        public double B { get; private set; } = 0;
        public string Values { get; private set; } = "";
        public string Color { get; private set; } = "";
        public int ColorID { get; private set; } = -1;
    }

}
