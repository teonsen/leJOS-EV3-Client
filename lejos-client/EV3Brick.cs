using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace lejos_client
{
    public class EV3Brick
    {
        private readonly EV3 _ev3;

        public EV3Brick(EV3 ev3)
        {
            _ev3 = ev3;
        }

        class Command
        {
            public string cmd { get; set; }
            public string dev { get; set; }
            public int int_param1 { get; set; }
            public string str_param1 { get; set; }
        }

        class EV3CommandRoot
        {
            public Command command { get; set; }
        }

        public void Beep()
        {
            BrickCommand(eCommandTypes.BEEP);
        }

        public void Buzz()
        {
            BrickCommand(eCommandTypes.BUZZ);
        }

        public void Exit()
        {
            BrickCommand(eCommandTypes.EXIT);
        }

        public void LED(int pattern)
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.BRICK.ToString().ToLower(),
                    cmd = eCommandTypes.LED.ToString().ToLower(),
                    int_param1 = pattern
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            _ev3.SendCommand(jsonCmd);
        }

        public void PlayWAV(string wavFileName, int volume)
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.BRICK.ToString().ToLower(),
                    cmd = eCommandTypes.PLAYWAV.ToString().ToLower(),
                    str_param1 = wavFileName,
                    int_param1 = volume
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            _ev3.SendCommand(jsonCmd);
        }

        internal void BrickCommand(eCommandTypes cmdType)
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.BRICK.ToString().ToLower(),
                    cmd = cmdType.ToString().ToLower(),
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            _ev3.SendCommand(jsonCmd);
        }
    }
}
