using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileManager.Views;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace FileManager
{
    public class Server
    {
        public SftpClient sftpClient;

        public Server(string host, int port, string username, string password) {
            try { 
                sftpClient = new SftpClient(host, port, username, password); 
            } catch (Exception e) 
            { 
                Debug.WriteLine(e.Message); 
            };
            
        }
        
        public void connectClient() {
            try { 
                sftpClient.Connect(); 
            } catch (Exception e) { 
                Debug.WriteLine(e.Message);
            };
        }
        private void disConnectClient()
        {
            try { 
                this.sftpClient.Disconnect(); 
            } catch (Exception e) {
                Debug.WriteLine(e.Message); 
            };
            
        }

        private void downloadFiles(IReadOnlyList<StorageFile> files,string destiantion) {
            if (files.Count == 1)
            {
                Task uploadTaskFirst = Task.Factory.StartNew(() => downloadAsync(files));
            }
            else
            {
                if (files.Count <= 100)
                {
                    int partialSize = (int)(files.Count / 2);
                    Task uploadTaskFirst = Task.Factory.StartNew(() => downloadAsync(files.Take(partialSize).ToList().AsReadOnly()));
                    Task uploadTaskSecond = Task.Factory.StartNew(() => downloadAsync(files.Skip(partialSize).ToList().AsReadOnly()));
                }
                else
                {
                    int partialSize = (int)files.Count / 3;
                    Task uploadTaskFirst = Task.Factory.StartNew(() => downloadAsync(files.Take(partialSize).ToList().AsReadOnly()));
                    Task uploadTaskSecond = Task.Factory.StartNew(() => downloadAsync(files.Skip(partialSize).ToList().AsReadOnly()));
                    Task uploadTaskThird = Task.Factory.StartNew(() => downloadAsync(files.Skip(partialSize * 2).ToList().AsReadOnly()));
                }
            }

        }

        public void UploadTask(string destiantion, IReadOnlyList<StorageFile> files) {
            if (files.Count == 1) 
            {
                Task uploadTaskFirst = Task.Factory.StartNew(() => UploadAsync(destiantion, files));
            } else {
                if (files.Count <= 100)
                {
                    int partialSize = (int)(files.Count / 2);
                    Task uploadTaskFirst = Task.Factory.StartNew(() => UploadAsync(destiantion, files.Take(partialSize).ToList().AsReadOnly()));
                    Task uploadTaskSecond = Task.Factory.StartNew(() => UploadAsync(destiantion, files.Skip(partialSize).ToList().AsReadOnly()));
                }
                else
                {
                    int partialSize = (int)files.Count / 3;
                    Task uploadTaskFirst = Task.Factory.StartNew(() => UploadAsync(destiantion, files.Take(partialSize).ToList().AsReadOnly()));
                    Task uploadTaskSecond = Task.Factory.StartNew(() => UploadAsync(destiantion, files.Skip(partialSize).ToList().AsReadOnly()));
                    Task uploadTaskThird = Task.Factory.StartNew(() => UploadAsync(destiantion, files.Skip(partialSize * 2).ToList().AsReadOnly()));
                }
            }
        }

        public async void UploadWithoutTask(string destinationDir, IReadOnlyList<StorageFile> files) {
            if (files.Count == 1)
            {
                try
                {
                    var stream = await files[0].OpenStreamForReadAsync();
                    addFileNameList(files[0].Name);
                    maxSize(stream.Length);
                    sftpClient.UploadFile(stream, destinationDir + "" + files[0].Name, progress);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                foreach (StorageFile file in files)
                {
                    try
                    {
                        var stream = await file.OpenStreamForReadAsync();
                        addFileNameList(file.Name);
                        sftpClient.UploadFile(stream, destinationDir + "" + file.Name, null);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }
            }
        }
        public async void UploadAsync(string destinationDir, IReadOnlyList<StorageFile> files)
        {
            if (files.Count == 1)
            {
                try
                {
                    var stream = await files[0].OpenStreamForReadAsync();
                    addFileNameList(files[0].Name);
                    maxSize(stream.Length);
                    sftpClient.UploadFile(stream, destinationDir + "" + files[0].Name, progress);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else {
                foreach (StorageFile file in files)
                {
                    try
                    {
                        var stream = await file.OpenStreamForReadAsync();
                        addFileNameList(file.Name);
                        sftpClient.UploadFile(stream, destinationDir + "" + file.Name, null);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }
            }
        }

        public void addFileNameList(string name) {
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { 
                SFTP.Current.fileNameLV.Items.Add(name); 
            });
        }
        public void maxSize(long length)
        {
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                SFTP.Current.fullSize.Text = length.ToString();
            });
        }

        public void progress(ulong progress) {
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                SFTP.Current.sizeProgress.Text = progress.ToString(); 
            });
        }

        public async void downloadAsync(IReadOnlyList<StorageFile> files)
        {
            foreach (StorageFile file in files)
            {
                try
                {
                    StorageFile newFile = await DownloadsFolder.CreateFileAsync(file.Name);
                    var stream = await newFile.OpenStreamForWriteAsync();
                    sftpClient.DownloadFile("/public/"+file.Name,stream);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                }

            }
        }
    }
}
