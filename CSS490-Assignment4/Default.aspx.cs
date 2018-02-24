using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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

            WebClient myclient = new WebClient();
            using (myclient)
            {
                Stream inData = myclient.OpenRead("https://css490.blob.core.windows.net/lab4/input.txt");
                StreamReader myreader = new StreamReader(inData);
                myBlob.UploadFromStream(inData);
            }
        }
    }
}