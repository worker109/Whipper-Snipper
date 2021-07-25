# About Whipper Snipper

Whipper Snipper is a standalone Windows application that allows you to create actions for:

- Services (Start/Stop/Startup Type)
- Hardware (Enable/Disable)
- Processes (Start/Kill_
- Executables (Start/Kill)
- Sub Profiles

Actions are contained within a Profile, Profiles can be either run or reverted.

Shortcuts can be created for each Profile (run/revert).

# Warning

This application requires administrator privileges. You can break things with it.

# Download and Installation

Whipper Snipper is portable** and doesn't use an installer.

[Download Version 1.1 (Windows)](https://github.com/worker109/Whipper-Snipper/releases/download/1.1/WhipperSnipper.exe)

** *If you use shortcuts generated from Whipper Snipper, put the exe somewhere permanent*

# Examples

## 1. Application that uses an always running service

Some applications have a dedicated background service, if this service is only required when using the application you may not want it to run all the time.

Steps:

1. Create a new profile
2. Add a 'Service' action
   - Choose the service from the list
   - Choose 'Start' for the enable action
   - Choose 'Stop' for the revert action
   - Optionally choose 'Manual' for either the enable or revert action startup type. This will ensure the service won't start automatically (after the profile has been run or reverted at least once). You can also do this separately via services.msc in Windows
3. Add an 'Executable' action
   - Find the execitable path by using 'Browse'
   - Choose 'Start' for the enable action
   - Choose 'Kill' for the revert action
4. (Optional) Create shortcuts for each action

Use the 'Run Profile' option to start the application (this will also start the background service)

Use the 'Revert Profile' option to end the application (this will also kill the background service)

![01-ws_service_example](https://user-images.githubusercontent.com/77418705/126892419-27be543c-e204-4a6f-863c-3fe5758a2e6b.png)

## 2. Application that use specific hardware

You may have a hardware device that's only needed for some applications.

Steps:

1. Create a new profile
2. Add a 'Hardware' action
   - Choose the device from the list
   - Choose 'Enable' for the enable action
   - Choose 'Disable' for the revert action
3. Add an 'Executable' action
   - Find the execitable path by using 'Browse'
   - Choose 'Start' for the enable action
   - Choose 'Kill' for the revert action
4. (Optional) Create shortcuts for each action

Use the 'Run Profile' option to start the application (this will also enable the hardware device)

Use the 'Revert Profile' option to end the application (this will also disable the hardware device)

![02-ws_hardware_example](https://user-images.githubusercontent.com/77418705/126892456-b6407c13-1348-4756-b523-c10124284232.png)

## 3. Notoriously cumbersome software

E.g. Adobe products.

Basic Steps:

1. Create a new profile
2. Add a service action for each Adobe service (start/stop)
3. Add a process action for each Adobe process (nothing/kill)
4. Create a new profile for each Adobe application
   - Add the first profile as a 'Profile' action (e.g. a sub-profile)
   - Add the application as an 'Executable' action

Use the run/revert actions as needed for each application

![03-ws_adobe](https://user-images.githubusercontent.com/77418705/126892422-274e4e22-b6c0-4844-8330-6143b7ae9733.png)

## 4. Offline mode

Use a combination of services, hardware and process actions to create an offline/online profile.

E.g. disable NIC's and any services used for networking, then re-enable when back online.

![04-ws-offline-mode](https://user-images.githubusercontent.com/77418705/126892423-12b0feac-b306-4065-a88e-ec046936ff3a.png)

## 5. Gaming mode

Use a combination of services, hardware and process actions to create a custom gaming profile. If your game is offline you can also add the previous "Offline mode" profile example as a sub-profile.

This allows you to disable/stop anything that's unessecary for running a game, then revert back when you've ended the game. 

![05-ws-gaming mode](https://user-images.githubusercontent.com/77418705/126892426-dadfec09-6a1b-4b8f-92e0-35c59cbdf6dc.png)




