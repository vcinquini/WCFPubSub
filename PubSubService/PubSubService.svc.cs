using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Timers;

namespace PubSubService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PubSubService : IPubSubService
    {
        
        public delegate void NameChangeEventHandler(object sender, ServiceEventArgs e);
        public static event NameChangeEventHandler NameChangeEvent;

        IPubSubContract ServiceCallback = null;
        NameChangeEventHandler NameHandler = null;
        Timer t = new Timer();

        public PubSubService()
        {
            t.Enabled = true;
            t.Interval = 3000;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
			string v = ConfigurationManager.AppSettings["chave"].ToString();
            PublishNameChange("from service => " + v + Environment.NewLine);
        }

        public void Subscribe()
        {
            ServiceCallback = OperationContext.Current.GetCallbackChannel<IPubSubContract>();
            NameHandler = new NameChangeEventHandler(PublishNameChangeHandler);
            NameChangeEvent += NameHandler;
        }

        public void Unsubscribe()
        {
            NameChangeEvent -= NameHandler;
        }

        public void PublishNameChange(string name)
        {
            ServiceEventArgs se = new ServiceEventArgs(name);
            NameChangeEvent(this, se);
        }

        public void PublishNameChangeHandler(object sender, ServiceEventArgs se)
        {
            ServiceCallback.NameChange(se.Name);

        }
    }

    public class ServiceEventArgs : EventArgs
    {
        public string Name { get; set; }

        public ServiceEventArgs(string name)
        {
            this.Name = name;
        }
    }
}
