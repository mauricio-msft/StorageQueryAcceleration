# Filter data by using Azure Data Lake Storage query acceleration

## Packages used

```cli
dotnet add Azure.Storage.Blobs --version 12.9.0
dotnet add CsvHelper --version 27.1.0
```

In order to run this sample, you need to update the following lines in Program.cs:

```aspx-csharp
string connectionString = "xxxxxxxxxxxxxxxxxxx";
string containerName = "xxxxxxxxxxxxxxxxxxx";
string blobName = "xxxxxxxxxxxxxxxxxxx.csv";
```
