Service description:

This service will get notification of media to be processed through a private MSMQ. 
The message from the queue contains the path to the media, which is then moved to a
local processing directory. Expression Media Encoder is then instantiated from the command
line with configured presets. Finally, the resulting Silverlight media player application,
whether it be audio or video, is packaged into a Zip file that meets the Silverlight Streaming
requirements and uploaded to a configured SS account using their API.


Service prerequisites:

1) MSMQ installed. Security must be configured so that the account used to run the
   media processing service has the ability to read messages from the configured queue.
2) Visual J# Redistributable:
   http://msdn2.microsoft.com/en-us/vjsharp/bb188598.aspx
   Note: used for Zip capability.
3) Expression Media Encoder and EME Update installed.
   Note: right now we are using templates provided with the default install, but in the future 
   we *may* install our own templates as part of the installation process if custom branding
   is needed.
  
  
Service deployment & configuration:
  
1) Copy the .EXE, .CONFIG, *.xml, and error.zip from build output and copy them to the location 
   you want the service to reside at.
2) Copy Scripts\*.bat from build output directory and copy to same location as you did for
   the previous step. These will help install, start, stop, and uninstall the service.
3) Check the .CONFIG file. Configurations need to be made before you start the service. See
   comments in the .CONFIG file for more detailed configuration information.
   Note: if changes need to be made, stop the service first, make the changes, and then restart.
   

Service operation notes:

* By default, events are sent to the Application event log with a source of 
  "WLQuickApps Media Processor Service".