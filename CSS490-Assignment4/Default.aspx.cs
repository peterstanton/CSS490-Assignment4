using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;


namespace CSS490_Assignment4
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void loadButton_Click(object sender, EventArgs e)
        {
            outputBox.Text = "";
            CloudStorageAccount hiAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("css490myblob_AzureStorageConnectionString"));
            CloudBlobClient blobClient = hiAccount.CreateCloudBlobClient();
            CloudBlobContainer myCont = blobClient.GetContainerReference("petercss490blob");
            CloudBlockBlob myBlob = myCont.GetBlockBlobReference("arrrrghhhhh.txt");
            BlobContainerPermissions perm = myCont.GetPermissions();
            perm.PublicAccess = BlobContainerPublicAccessType.Blob;
            myCont.SetPermissions(perm);

            CloudStorageAccount tableAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("petercss490table_AzureStorageConnectionString"));
            CloudTableClient littleBobbyTables = tableAccount.CreateCloudTableClient();
            CloudTable myTable = littleBobbyTables.GetTableReference("csspeteremployees");
            myTable.DeleteIfExists();
            try
            {
                myTable.CreateIfNotExists();
            }
            catch(Exception)
            {
                outputBox.Text = "There was an error in processing, please hold for 25 seconds.";
                myTable.DeleteIfExists();
                System.Threading.Thread.Sleep(25000);
                try
                {
                    myTable.CreateIfNotExists();
                }
                catch(Exception)
                {
                    outputBox.Text = "The Storage service is being slow, please try again in a few minutes...";
                    myTable.DeleteIfExists();
                    return;
                }
            }

            WebClient myclient = new WebClient();
            int rowCounter = 1;

            using (myclient)
            {
                Stream inData = myclient.OpenRead("https://css490.blob.core.windows.net/lab4/input.txt");
                Stream inParse = myclient.OpenRead("https://css490.blob.core.windows.net/lab4/input.txt");
                StreamReader myParse = new StreamReader(inParse);
                string nowparse = myParse.ReadToEnd();
                myBlob.UploadFromStream(inData);
                List<string> hello = new List<string>(Regex.Split(nowparse, Environment.NewLine));
                hello.RemoveAll(String.IsNullOrWhiteSpace);


                foreach (var entry in hello)
                {
                    DynamicTableEntity thisOne = new DynamicTableEntity();
                    Dictionary<string, EntityProperty> data1 = new Dictionary<string, EntityProperty>();
                    String stuff = entry;
                    List<String> parsed = entry.Split(' ').ToList<String>();
                    parsed.RemoveAll(String.IsNullOrWhiteSpace);
                    string firstName = parsed[1].ToLower();
                    string lastName = parsed[0].ToLower();
                    data1.Add("firstname", new EntityProperty(firstName));
                    data1.Add("lastname", new EntityProperty(lastName));
                    foreach (var datum in parsed.Skip(2))
                    {                    
                        string[] thisVar = datum.Split('=');
                        data1.Add(thisVar[0].ToLower(), new EntityProperty(thisVar[1].ToLower()));
                    }
                    thisOne.Properties = data1;
                    thisOne.PartitionKey = "partition1";
                    thisOne.RowKey = Convert.ToString(rowCounter);
                    var updater = TableOperation.InsertOrReplace(thisOne);
                    myTable.Execute(updater);
                    rowCounter++;
                }
            }
            outputBox.Text = "Data transferred to storage!";
        }

        protected void queryButton_Click(object sender, EventArgs e)
        {
            string first = null;
            string last = null;
            outputBox.Text = "";
            if (!String.IsNullOrWhiteSpace(firstNameBox.Text))
                first = firstNameBox.Text.ToLower();
            if (!String.IsNullOrWhiteSpace(lastNameBox.Text))
                last = lastNameBox.Text.ToLower();
            if (first == null && last == null)
            {
                outputBox.Text = "You need to enter a first and/or last name to find anything.";
                return;
            }

            CloudStorageAccount tableAccount = CloudStorageAccount.Parse(
    CloudConfigurationManager.GetSetting("petercss490table_AzureStorageConnectionString"));
            CloudTableClient littleBobbyTables = tableAccount.CreateCloudTableClient();
            CloudTable myTable = littleBobbyTables.GetTableReference("csspeteremployees");
            myTable.CreateIfNotExists();

            TableQuery<DynamicTableEntity> getter = new TableQuery<DynamicTableEntity>();
            if(first != null && last == null)
            {
              getter.Where(TableQuery.GenerateFilterCondition("firstname",
                                                              QueryComparisons.Equal,
                                                              first));
            }
            else if(first == null && last != null)
            {
                getter.Where(TableQuery.GenerateFilterCondition("lastname",
                                                                QueryComparisons.Equal,
                                                                last));
            }
            else
            {
                string filterA = TableQuery.GenerateFilterCondition(
                   "firstname", QueryComparisons.Equal,
                   first);
                string filterB = TableQuery.GenerateFilterCondition(
                                   "lastname", QueryComparisons.Equal,
                                   last);

                getter.Where(TableQuery.CombineFilters(filterA, TableOperators.And, filterB));
            }
            StringBuilder hello = new StringBuilder();
            foreach (DynamicTableEntity result in myTable.ExecuteQuery(getter))
            {                
                for(int hi = 0; hi < result.Properties.Count; hi++)
                {
                    hello.Append(result.Properties.ElementAt(hi).Key);
                    hello.Append('\t');
                }
                hello.Append(Environment.NewLine);
                hello.Append("-----------------------------------------------------------------\n");
                for(int bye = 0; bye < result.Properties.Count; bye++)
                {
                    hello.Append(result.Properties.ElementAt(bye).Value.StringValue);
                    hello.Append('\t');
                }
                hello.Append(Environment.NewLine);
                hello.Append(Environment.NewLine);
            }
            if (String.IsNullOrEmpty(hello.ToString()))
                outputBox.Text = "No results found.";
            else
                outputBox.Text = hello.ToString();
        }
    }
}