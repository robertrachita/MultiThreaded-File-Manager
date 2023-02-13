# MultiThreaded-File-Manager
*A project implementing different methods of multi threading techniques, part of the "Threading in C#" Course at NHL Stenden Y3*


### Application description 

The application that the group wants to create therefore is a File & Folder Manager. The application will include typical features expected in a file explorer, such as file search or data transfer and multiple file manipulation commands (please refer to the following chapter for more information on the features). At the same time, these operations will make use of threading improve the performance and respect the course requirements. Additionally, a parallel FTP client will be included as well.

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