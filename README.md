# Filter data by using Azure Data Lake Storage query acceleration

## Packages used

```cli
dotnet add Azure.Storage.Blobs --version 12.8.4
dotnet add CsvHelper --version 27.1.0
```

>If use package Azure.Storage.Blobs --version 12.9.0 then the entire sample breaks, this issue is already reported here:
[Issue 76856](https://github.com/MicrosoftDocs/azure-docs/issues/76856)

## Prepare solution

In order to run this sample, you need to update the following lines in Program.cs:

```aspx-csharp
string connectionString = "xxxxxxxxxxxxxxxxxxx";
string containerName = "xxxxxxxxxxxxxxxxxxx";
string blobName = "xxxxxxxxxxxxxxxxxxx.csv";
```
