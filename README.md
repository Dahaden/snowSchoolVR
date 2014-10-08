SnowsSchoolVR
=============

#Setup

## Wii Balance Board

1. Connect to WiiBalance Board  
  a. Use the sync button under the battery cover while it is on  
  b. If your computer asks for a pin, leave it blank and press ok
2. Install Virtual JoyStick Driver (WiiBalanceWalker_v0.4/VJoy_1.2_Driver.exe)
3. Run WiiBalanceWalker (WiiBalanceWalker_v0.4/WiiBalanceWalker.exe)
4. Press "Connect to Wii balance board"
5. On the right panel, uncheck "Disable All Actions" and check "Enable Joystick"

### Pairing
1. Use Windows 8
2. Open panel on Wii Board and press the red button
3. Search for the Wii Board (Initially labeled as "Input Device" then changes to "Nintendo-XXX")
  a. Leave any pin field blank and press ok
4. Before the blue light on the power button stops flashing, press "Connect to Wii Balance Board"
5. If the blue light is not solid, repeat

If you ignore no. 1 and use Windows 7
1. Open "WiiBalanceWalker" and click the button above "Connect to Wii Balance Board"
2. Click Search.
3. Once finished, quickly close the window and click "Connect to Wii Balance Board"
If no. 3 is not done fast enough, you will need to retry.

## Kinect

1. Download and install the Kinect SDK from here (http://www.microsoft.com/en-us/download/details.aspx?id=40278) 
2. Plug in the Kinect Sensor
3. Run the game


## Oculus Rift

1. Signup for Oculus (https://developer.oculusvr.com)
2. Download Oculus Runtime For Windows through the download link
3. Install and run


## Turning OVR on
1. Click on "Gameplayer" in left bar
2. Activate all OVR scripts
3. Within "State Mech" script, check "OVRActive" variable
4. Expand hierarchy until you get within "Dana" object
5. Deactivate the non-OVR cameras and activate the OVR cameras

To turn OVR off, do the opposite of 2, 3 and 5

### Troubleshoot
If you are missing scripts for OVR in your assets, add scripts as follows:  
GamePlayer:
* OVRGamepad Controller
* OVRMain Menu

1stPersonOVRCameraController (and 3rd person)
* OVRDevice

CameraLeft
* OVRCamera

CameraRight
* OVRCamera (Check "RightEye" variable)
