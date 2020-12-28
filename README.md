# lejos-client

lejos-client is a TCP client when 'lejos-server' is running on EV3.
At the moment, the functionality is limited, but you can control EV3 with C# on both Win and Mac as shown below.

```csharp
using lejos_client;

namespace lejos_client_mac
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connect to EV3 ('lejos-server' must be running on EV3)
            var ev3 = new EV3("10.0.1.1", 6789);
            ev3.Brick.LED((int)eLEDPatterns.GreenBrink2);
            // speed:100, rotation:360
            ev3.Wheels.GoForward(100, 360);
        }
    }
}
```

## Installation
you can install it from NuGet.
https://www.nuget.org/packages/lejos-client/

## Acknowledgements
Thanks to @crunchiness https://github.com/crunchiness/lejos-server
