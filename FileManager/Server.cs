using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace FileManager
{
    internal class Server
    {
        string host;
        int port;
        string username;
        string password;
        public SftpClient sftpClient { get { return sftpClient; } set { sftpClient = value; } }

        public Server(string host, int port, string username, string password) {
            sftpClient = new SftpClient(host, port, username, password);
        }

        private void connectClient() { 
            this.sftpClient.Connect();
        }
        private void disConnectClient()
        {
            this.sftpClient.Disconnect();
        }

        private void downloadFiles(ArrayList files,string destiantion) { }

        private void uploadTask(ArrayList files, string destiantion) {
            using (sftpClient) {
                connectClient();
                if (files.Count <= 100)
                {
                    int partialSize = (int)(files.Count / 2);
                    Task uploadTaskFirst = Task.Factory.StartNew(() => Upload(destiantion, files.GetRange(0, partialSize)));
                    Task uploadTaskSecond = Task.Factory.StartNew(() => Upload(destiantion, files.GetRange(partialSize, files.Count)));
                }
                else
                {
                    int partialSize = (int)files.Count / 3;
                    Task uploadTaskFirst = Task.Factory.StartNew(() => Upload(destiantion, files.GetRange(0, partialSize)));
                    Task uploadTaskSecond = Task.Factory.StartNew(() => Upload(destiantion, files.GetRange(partialSize, 2*partialSize)));
                    Task uploadTaskThird = Task.Factory.StartNew(() => Upload(destiantion, files.GetRange(2*partialSize, files.Count)));
                }
                disConnectClient();
            }
        }
        public void Upload(string destinationDir, ArrayList files)
        {
            foreach (string file in files)
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Open))
                {
                    sftpClient.UploadFile(fileStream, destinationDir, null);
                }
            }
        }
    }
}
