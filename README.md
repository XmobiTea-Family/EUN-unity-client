# EUN-unity-client

Simple demo on [Demo EUN WebGL](https://xmobitea.com/demo-eun/)

### Support for
* `Android`
* `iOS`
* `WebGL`
* `Editor`
* `Windows`
* `Linux`
* `Mac`
* `Universal Windows Platform`
* `Dedicated Server`
* ...

# Require Unity
It supports for Unity 2020, Unity 2021 or newer

# Get started for EUN Unity
## How to connect EUN Server
To use EUN, please follow:
* Step 1:
download this release package at [Release EUN Unity Client](https://github.com/XmobiTea-Family/EUN-unity-client/releases)
* Step 2:
import this package to your Unity project
* Step 3:
open `EUN/EUN Settings` in unity menu bar and choose `Setup EUN` in the Inspector of EUN Server Settings (if this button display)

![image](https://user-images.githubusercontent.com/67969514/170856065-54954220-3d0c-4dd3-9d53-a382d60a3771.png)
![image](https://user-images.githubusercontent.com/67969514/170856140-fa86f381-630e-43a8-842e-eb0518363006.png)

_the default setup EUN Server has correct and you can connect to test this, but the you can re setup for some field: Socket Host, TCP port, UDP port, Zone Name, App Name, Send Rate, Send Rate Synchronize, use voice chat,..._
_if you want to connect your custom host, you can download [Release EUN Server](https://github.com/XmobiTea-Family/EUN-server/releases) and open this by IntelliJ and run `EUN-startup/src/main/java/org.youngmonkeys.xmobitea.eun.ApplicationStartup.main()`_
* Step 5:
create script `NetworkingManager.cs` and implement `EUNManagerBehaviour`, then implement some function
* Step 6:
on `NetworkingManager.OnCustomStart()` function, call `EUNNetwork.Connect("DeviceId or user id here")` and waiting for `NetworkingManager.OnEUNConnected()` callback

You can see more demo projects at [EUN-Example](https://github.com/XmobiTea-Family/EUN-Example)
