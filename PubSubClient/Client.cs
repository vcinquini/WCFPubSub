using PubSubClient.ServiceReference1;
using System.ServiceModel;
using System.Windows.Forms;

namespace PubSubClient
{
    public partial class Client : Form
    {
        PubSubServiceClient client;

        public delegate void MyEventCallbackHandler(string Name);
        public static event MyEventCallbackHandler MyEventCallbackEvent;

        delegate void SafeThreadCheck(string Name);

        [CallbackBehaviorAttribute(UseSynchronizationContext = false)]
        public class ServiceCallback : IPubSubServiceCallback
        {
            public void NameChange(string Name)
            {
                Client.MyEventCallbackEvent(Name);
            }
        }


        public Client()
        {
            InitializeComponent();

            InstanceContext context = new InstanceContext(new ServiceCallback());
            client = new PubSubServiceClient(context);

            MyEventCallbackHandler callbackHandler = new MyEventCallbackHandler(UpdateForm);
            MyEventCallbackEvent += callbackHandler;

            client.Subscribe();
        }

        ~Client()
        {
            client.Unsubscribe();
        }

        public void UpdateForm(string Name)
        {
            if (lblDisplay.InvokeRequired)
            {
                SafeThreadCheck sc = new SafeThreadCheck(UpdateForm);
                this.BeginInvoke(sc, new object[] { Name });
            }
            else
            {
                lblDisplay.Text += Name;
            }
        }
    }
}
