* Performing tutorial on Windows Services

* Create the service
The service contains the following components:

1. Logger Event - which will record all the actions the service performs.
2. Installer - which will install the ImageService in the Services list
System.
3.ServiceStatus - which will log Logger in the Basic Messaging Service
Start, Stop, Start.

* Create Modal Logging and Modal Image
The Modal will manage the Logger service
Once when the Logger receives a message.
We will make a complete separation between the display and the modal.)
The logger will add to the Logger Event all the messages it receives.

* Modal Image which will perform the basic operations of the system.), Etc. Move File, Create Folder

* Create a server
We will create a server (which currently does not listen to the client), which is intended to manage all the folders to be listened to.
The server's tasks are:
1. For each path to the Path that we want to listen to, we will create a Handler
Which will handle events occurring in the folder.
2. When the service is about to close, it must ensure that the created handlers are finished.
3. Server sends Handlers objects that specify the
What to do with the file system (such as adding a new image)

* Create a controller
The Controller will receive Commands (containing an ID number, according to which you can decrypt what
The desired command type as well as a list of Args), decrypts the command and starts the command
The desired operation using the Modal Image.
In "Controller", we will use a dictionary that corresponds to the ID Command
The object that is responsible for the command will be returned to the command.

* Create Handler
You can use the FileSystemWatcher class to listen to folders
https://msdn.microsoft.com/enus/library/system.io.filesystemwatcher(v=vs.110).aspx
The images we will listen to are:
* .jpg, * .png, * .gif, * .bmp
We first note that the OutputDir folder is a hidden folder that contains folders of years
And Thumbnails folder.
Handler Functions:
When a new image is inserted into the folder, the Handler must retrieve its date
Image created.
Now that we have the year and the month the photo was taken, we go to the folder
OutputDir, we will create a folder for the year of the image (if it does not exist yet) and within
Folder of the year we will create a folder with the month (integer).
And finally we'll move the picture to the new path.
Now, in the Thumbnails folder which is located in the OutputDir folder we will generate it
Structure the folders (by year and month) and create a Thumbnail image file. Set its size
The image by ThumbnailSize.
Finally, Logger should be notified of the failure or success of the operation.
2. When the service is about to close, make sure the FileSystemWatcher is not listening
And notify Logger of a state of failure or success.
For each action the Handler receives, logger must be registered.

* AppConfig
In order for us to change the service without having to recompile, we will store several entries
.App.config in the file
Below is a list of the entries to be stored there (all entries are String type):

1.Handler - contains a list of paths to listening folders, separated by ;

2. InputDir - The path of the folder where all images are stored by year
And months as well as the Thumbnails folder.

3. SourceName - The source name to be given to the Logger Event,
Service

4.LogName - The log name to be given to the Logger Event that we defined in the Service
Its value will be: ImageServiceLog

5 .ThumbnailSize - The size of the Thumbnail image
