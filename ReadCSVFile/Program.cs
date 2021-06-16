using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //reference:https://docs.microsoft.com/en-us/dotnet/api/overview/azure/Storage.Blobs-readme?view=azure-dotnet



            // Get a connection string to our Azure Storage account. You can
            // obtain your connection string from the Azure Portal (click
            // Access Keys under Settings in the Portal Storage account blade)
            // or using the Azure CLI with:
            //
            // az storage account show-connection-string --name <account_name> --resource-group <resource_group>
            //
            // And you can provide the connection string to your application
            // using an environment variable.



            string connectionString = "DefaultEndpointsProtocol=https;AccountName=sa2105190040002809;AccountKey=K3sKG7LwqELpdeTEEdifQygPvP+q9vGIfefeIqHV80GqKTspviWBsilGIbIFlM/aNy4yR92+XQBd7fcaEhUQyA==;EndpointSuffix=core.windows.net";
            string containerName = "containersa2105190040002809";
            string blobName = "sa2105190040002809.csv";



            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            //container.Create();



            // Get a reference to a blob named "sample-file" in a container named "sample-container"
            //BlobClient blob0 = container.GetBlobClient(blobName);
            // This transfers data in the file to the blob on the service.

            BlockBlobClient blob = container.GetBlockBlobClient(blobName);

            Task t = Task.Run(() => readcsv(blob));

            //QueryHemingway(blob);
            Console.WriteLine("downloading blob, please wait..");
            while (t.Status != TaskStatus.RanToCompletion)
            {
                //Console.Write(".");
                Thread.Sleep(1000);
            }

            Console.WriteLine(System.Environment.NewLine + "done...");
            Console.Read();
        }

        static async Task readcsv(BlockBlobClient blob)
        {
            string query = @"SELECT col1 FROM BlobStorage";
            await DumpQueryCsv(blob, query, true);
        }


        private static async Task DumpQueryCsv(BlockBlobClient blob, string query, bool headers)
        {
            try
            {
                var options = new BlobQueryOptions()
                {
                    InputTextConfiguration = new BlobQueryCsvTextOptions() { HasHeaders = headers, ColumnSeparator = ",", QuotationCharacter = '\\', RecordSeparator = "|", EscapeCharacter = '"' },
                    //OutputTextConfiguration = new BlobQueryCsvTextOptions() { HasHeaders = headers, ColumnSeparator = ",", QuotationCharacter = '\\', RecordSeparator = "|", EscapeCharacter = '"' },
                    ProgressHandler = new Progress<long>((finishedBytes) => Console.Error.WriteLine($"Data read: {finishedBytes}"))
                };
                options.ErrorHandler += (BlobQueryError err) => {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"Error: {err.Position}:{err.Name}:{err.Description}");
                    Console.ResetColor();
                };
                // BlobDownloadInfo exposes a Stream that will make results available when received rather than blocking for the entire response.
                using (var reader = new StreamReader((await blob.QueryAsync(
                        query,
                        options)).Value.Content))
                {
                    using (var parser = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture) { HasHeaderRecord = true }))
                    {
                        while (await parser.ReadAsync())
                        {
                            Console.Out.WriteLine(String.Join(" ", parser.Parser.Record));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Exception: " + ex.ToString());
            }
        }
    }
}