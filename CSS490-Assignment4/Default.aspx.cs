using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


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
            CloudStorageAccount hiAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("css490myblob_AzureStorageConnectionString"));
            CloudBlobClient blobClient = hiAccount.CreateCloudBlobClient();
            CloudBlobContainer myCont = blobClient.GetContainerReference("petercss490blob");
            CloudBlockBlob myBlob = myCont.GetBlockBlobReference("arrrrghhhhh.txt");
            BlobContainerPermissions perm = myCont.GetPermissions();
            perm.PublicAccess = BlobContainerPublicAccessType.Blob;
            myCont.SetPermissions(perm);


            CloudTableClient littleBobbyTables = hiAccount.CreateCloudTableClient();
            CloudTable myTable = littleBobbyTables.GetTableReference("Employees");
            WebClient myclient = new WebClient();
            int rowCounter = 1;
            using (myclient)
            {
                Stream inData = myclient.OpenRead("https://css490.blob.core.windows.net/lab4/input.txt");
                Stream inParse = myclient.OpenRead("https://css490.blob.core.windows.net/lab4/input.txt");
               // StreamReader myreader = new StreamReader(inData);
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
                    if(entry == "\r") {
                        break;
                    }
                    List<String> parsed = entry.Split(' ').ToList<String>();
                    parsed.RemoveAll(String.IsNullOrWhiteSpace);
                    string firstName = parsed[1];
                    string lastName = parsed[0];
                    data1.Add("First Name", new EntityProperty(firstName));
                    data1.Add("Last Name", new EntityProperty(lastName));
                    foreach (var datum in parsed.Skip(2))
                    {                    
                        string[] thisVar = datum.Split('=');
                        data1.Add(thisVar[0], new EntityProperty(thisVar[1]));
                    }
                    thisOne.Properties = data1;
                    thisOne.PartitionKey = "Partition1";
                    thisOne.RowKey = Convert.ToString(rowCounter);
                    var updater = TableOperation.InsertOrReplace(thisOne);
                    myTable.Execute(updater); 


                    /*
                    Id = Convert.ToInt32(parsed[2].Split('=')[1]);
                    phone = Convert.ToInt64(parsed[3].Split('=')[1]);
                    Office = parsed[4].Split('=')[1];
                    DynamicTableEntity thisOne = new DynamicTableEntity();
                    Dictionary<string, EntityProperty> data1 = new Dictionary<string, EntityProperty>();
                    data1.Add("First Name", new EntityProperty(firstName));
                    data1.Add("Last Name", new EntityProperty(lastName));
                    data1.Add("ID", new EntityProperty(Id));
                    data1.Add("Phone #", new EntityProperty(phone));
                    data1.Add("Office location", new EntityProperty(Office));
                    thisOne.Properties = data1;
                    thisOne.PartitionKey = "Partition1";
                    thisOne.RowKey = Convert.ToString(rowCounter);
                    var updater = TableOperation.InsertOrReplace(thisOne);
                    myTable.Execute(updater);*/
                }
            }
        }

        protected void queryButton_Click(object sender, EventArgs e)
        {  //code to grab and display matching records.

        }
    }
}