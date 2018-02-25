using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CSS490_Assignment4
{
    public class EmployeeEntity : TableEntity
    {
        public string fName { get; set; }
        public string lName { get; set; }
        public int id { get; set; }
        public long phone { get; set; }
        public string office { get; set; }
        //define entity here.

        public EmployeeEntity()
        {

        }
        public EmployeeEntity(string first, string last, int inId, long inPhone, string inOffice)
        {
            fName = first;
            lName = last;
            id = inId;
            phone = inPhone;
            office = inOffice;
        }
    }
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
            using (myclient)
            {
                Stream inData = myclient.OpenRead("https://css490.blob.core.windows.net/lab4/input.txt");
                StreamReader myreader = new StreamReader(inData);
                myBlob.UploadFromStream(inData);

                while(!myreader.EndOfStream)
                {
                    String stuff = myreader.ReadLine();
                    String firstName = "";
                    String lastName = "";
                    int Id = -100;
                    long phone = -999;
                    string Office = "";

                    List<String> parsed = stuff.Split(' ').ToList<String>();
                    firstName = parsed[0];
                    lastName = parsed[1];
                    Id = Convert.ToInt32(parsed[2].Split('=')[1]);
                    phone = Convert.ToInt64(parsed[3].Split('=')[1]);
                    Office = parsed[4].Split('=')[1];
                    EmployeeEntity thisone = new EmployeeEntity(firstName, lastName, Id, phone, Office);
                    var myOperation = TableOperation.InsertOrReplace(thisone);
                    myTable.Execute(myOperation);
                }
            }
        }

        protected void queryButton_Click(object sender, EventArgs e)
        {  //code to grab and display matching records.

        }
    }
}