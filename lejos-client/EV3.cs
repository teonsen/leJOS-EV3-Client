using System;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;

namespace lejos_client
{
    // This is a sample client library for the "crunchiness / lejos-server".
    //https://github.com/crunchiness/lejos-server

    #region EV3 'lejos-server' defined commands.
    // Commands are defined in Command.java
    public enum eDeviceTypes
    {
        BRICK, MOTOR, SENSOR, CAMERA, WHEELS
    }

    public enum eCommandTypes
    {
        INIT, BEEP, BUZZ, EXIT, PLAYWAV, LED,
        FORWARD, BACKWARD, STOP, CLOSE, GETTACHO, RESETTACHO, GETSPEED, SETSPEED, ROTATE, ISMOVING,
        GETVALUE, SETMODE, GETMODE, TAKEPIC,
        GO_FORWARD, GO_BACKWARD, TURN_LEFT, TURN_RIGHT
    }

    public enum eSensorTypes
    {
        COLOR, IR, TOUCH
    }

    public enum eMotorTypes
    {
        MEDIUM, LARGE
    }

    public enum eSensorModes
    {
        RGB, RED, AMBIENT, DISTANCE, TOUCH, COLORID
    }

    public enum eMotorPorts
    {
        A, B, C, D
    }
    public enum eSensorPorts
    {
        S1, S2, S3, S4
    }

    public enum eColorID
    {
        None,
        Black,
        Blue,
        Green,
        Yellow,
        Red,
        White,
        Brawn
    }

    public enum eLEDPatterns
    {
        OFF,
        Green,
        Red,
        Orange,
        GreenBrink1,
        RedBrink1,
        OrangeBrink1,
        GreenBrink2,
        RedBrink2,
        OrangeBrink2
    }
    #endregion

    public class EV3
    {
        private readonly string _EV3_IP;
        private readonly int _EV3TCPPort;
        public readonly EV3Brick Brick;
        public readonly EV3Motor Motor;
        public readonly EV3Sensor Sensor;

        /// <summary>
        /// Wheels motor must be connected to B and C.
        /// </summary>
        public readonly EV3Wheels Wheels;

        public EV3(string ipAddr, int portNo)
        {
            _EV3_IP = ipAddr;
            _EV3TCPPort = portNo;
            Brick = new EV3Brick(this);
            Motor = new EV3Motor(this);
            Sensor = new EV3Sensor(this);
            Wheels = new EV3Wheels(this);
        }

        public void Exit()
        {
            var ev3Cmd = new EV3CommandRoot
            {
                command = new Command
                {
                    dev = eDeviceTypes.BRICK.ToString().ToLower(),
                    cmd = eCommandTypes.EXIT.ToString().ToLower()
                }
            };
            var jsonCmd = JsonConvert.SerializeObject(ev3Cmd);
            SendCommand(jsonCmd);
        }

        internal string SendCommand(string jsonCmd, bool replyExpected = false)
        {
            //https://docs.microsoft.com/ja-jp/dotnet/api/system.net.sockets.tcpclient?view=netcore-3.1
            // String to store the response ASCII representation.
            String responseData = String.Empty;
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                using (var client = new TcpClient(_EV3_IP, _EV3TCPPort))
                {
                    // Translate the passed message into ASCII and store it as a Byte array.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(jsonCmd + "\n");
                    using (var ns = client.GetStream())
                    {
                        // Send the message to the connected TcpServer.
                        ns.Write(data, 0, data.Length);
                        if (replyExpected)
                        {
                            // Receive the TcpServer.response.
                            // Buffer to store the response bytes.
                            data = new Byte[256];
                            // Read the first batch of the TcpServer response bytes.
                            Int32 bytes = ns.Read(data, 0, data.Length);
                            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes).Trim();
                            Console.WriteLine("Received: {0}", responseData);
                        }
                        ns.Close();
                        client.Close();
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (IOException)
            { 
            }
            return responseData;
        }

        class Command
        {
            public string cmd { get; set; }
            public string dev { get; set; }
        }

        class EV3CommandRoot
        {
            public Command command { get; set; }
        }

    }
}
