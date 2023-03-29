# MultiThreaded-File-Manager
*A project implementing different methods of multi threading techniques, part of the "Threading in C#" Course at NHL Stenden Y3*


### Application description 

The application is a File & Folder Manager. The application will include typical features expected in a file explorer, such as file search or data transfer and multiple file manipulation commands (please refer to the following chapters for more information on the features). At the same time, these operations will make use of threading improve the performance and respect the course requirements. Additionally, a parallel SFTP client will be included as well.

### Installation and Running the App
Run the provided executable. On the initial launch, the application will search if the correct permisions have been granted, and will launch the Windows Settings application, enabling the user to do so. The user should search for the application ("File Manager"), click on the 3 dots menu on the right, click on "Advanced Settings" and then check all the permissions that the app requires (File System and Pictures). These steps are also detailed in the app at runtime. If the user grants the required permissions, the message should not appear anymore in-app.

Once the app has been launched, the user can select the desired functionality from the left hand Menu. Once a feature has been selected, this menu will shrink. Tabs can be changed at will, and the menu expanded again with the burger button. Each in-app feature has it's own description on how to be used. 

- File Searcher: Input the name of the file(s) to be searched for in the first input text box, and either use the following button to open a Windows Explorer dialog box, or manually input the path in the next input text box. Note that both boxes need to be filled in for the Search button to unlock
- CRUD operations:Move, Rename, Copy and Delete one or multiple files at the same time, lighnint fast with the power of threads. Select the desired operations using the dropdown menu, then select the file and finally the desired output path.
- File Comparison: Select 2 files using the on-screen buttons (successfull file upload will trigger a popup), then compare these files. The app supports both single threaded and multi threaded comparisons and timers.
- Tree Generator: Scans the provided path in the input text box and generates a tree view. User can select between seeing the tree at once, or having a nice typewriter-like animation
-SFTP Server Control: Allows the user to transfer multiple files between a server and the computer at once. See the next section for more information on how to use this feature. 


### SFTP Server

The application's SFTP function requires an SFTP server and its user credentials. The team recommend using Rebex Tiny SFTP Server as it was the same used during development. The link where you can download the server is as follows: https://www.rebex.net/tiny-sftp-server/#download . Unzip the .zip file, start the executable file in the folder, and allow access to the internet if it is needed. After it loaded, you can find the IP, port, username, password that you need to type in the app code in the 'SFTP.xaml.cs' file. The credentials needs to be placed when creating the server object in the SFTP's constructor. For example (IP, port, username, password): 'server = new Server("192.168.178.207", 2222, "tester", "password");' .
  
### Features
* 	Multi-threaded file search feature that searches for the given file in multiple directories concurrently
*	Multi use file operations (copy, move, rename, delete)
*	File comparisons – used to compare the contents of two files to determine if they are identical (byte level)
*	Multi-threaded scan of a directory which produces a visual overview.
*	Multi-threaded, segmented and/or concurrent FTP. 


### Technology
*	Threads
*	Task Parallel Library/Async
*	Semaphore
*	Asynchronous I/O
*	UWP
*	Visual Studio 2022/C#