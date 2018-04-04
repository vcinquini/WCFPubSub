using System;
using System.ServiceModel;
using System.Windows.Forms;
using Publisher;
using Publisher.ServiceReference;

namespace Publisher
{
    public partial class Publish : Form
    {
        InstanceContext context = null;
        PubSubServiceClient client = null;

        public class ServiceCallback : IPubSubServiceCallback
        {
            public void NameChange(string Name)
            {
                MessageBox.Show(Name);
            }
        }

        public Publish()
        {
            InitializeComponent();
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {

            context = new InstanceContext(new ServiceCallback());
            client = new PubSubServiceClient(context);
            client.PublishNameChange(txtMessage.Text + Environment.NewLine);
            client.Close();
        }
    }
}
