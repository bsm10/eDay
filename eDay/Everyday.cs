using System;
using System.Text;
using System.Net;
using System.IO;
using Windows.Web.Http;
using System.ComponentModel;
using System.Collections.ObjectModel;
//using Microsoft.Phone.Info;
using Windows.System.Profile;
using Windows.Storage.Streams;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;

namespace eDay
{
    public class ErrorStatus
    {
        public int success { get; set; }
        public string error_code { get; set; }
        public string error_description { get; set; }
        public string error_for_user { get; set; }
        public string working_time { get; set; }
        public override string ToString()
        {
            return error_description;
        }

    }
    public class LoginData
    {
        public int success { get; set; }
        public string token { get; set; }
        public string client_id { get; set; }
        public int new_notifications_count { get; set; }
        public int not_confirmed_events_count { get; set; }
        public float working_time { get; set; }
        public override string ToString()
        {
            return token;
        }


    }
    //public class Result
    //{
    //    public int success { get; set; }
    //}
    //public class Items
    //{
    //    public string id { get; set; }
    //    public string name { get; set; }
    //    public override string ToString()
    //    {
    //        return name;
    //    }

    //}

    /// <summary>
    /// Универсальная модель данных элементов.
    /// </summary>
    //public class Event : INotifyPropertyChanged
    //{
    //    public Event(double event_id, string img, string time, string expert, string caption, bool unscheduled, string items_count, double confirmed)
    //    {
    //        Event_id = event_id;
    //        Img = img;
    //        Time = time;
    //        Expert = expert;
    //        Caption = caption;
    //        Items = new ObservableCollection<Items>();
    //        Confirmed = confirmed;
    //        Items_count = items_count;
    //        Unscheduled = unscheduled;
    //    }
    //    private string _caption;
    //    private double _confirmed;
    //    public double Event_id { get; set; }
    //    public string Img { get; set; }
    //    public string Time { get; set; }
    //    public string Expert { get; set; }
    //    public string Caption
    //    {
    //        get
    //        {
    //            return _caption;
    //        }
    //        set
    //        {
    //            if (Caption != value)
    //            {
    //                _caption = value;
    //                OnPropertyChanged("caption");
    //            }
    //        }
    //    }
    //    //public int confirmed { get; set; }
    //    public double Confirmed
    //    {
    //        get
    //        {
    //            return _confirmed;
    //        }
    //        set
    //        {
    //            if (_confirmed != value)
    //            {
    //                _confirmed = value;
    //                OnPropertyChanged("_confirmed");
    //            }
    //        }
    //    }
    //    public string Items_count { get; set; }
    //    public ObservableCollection<Items> Items { get; set; }
    //    public bool Unscheduled { get; set; }
    //    //public double _Class { get; set; }
    //    public override string ToString()
    //    {
    //        return Caption;
    //    }
    //    public event PropertyChangedEventHandler PropertyChanged;
    //    private void OnPropertyChanged(string info)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;
    //        if (handler != null)
    //        {
    //            handler(this, new PropertyChangedEventArgs(info));
    //        }
    //    }

    //}

    /// <summary>
    /// Универсальная модель данных групп.
    /// </summary>
    public class GroupEvents
    {
        public GroupEvents()
        {
            events = new ObservableCollection<Event>();
        }
        public int success { get; set; }
        public string last_events_update { get; set; }
        public ObservableCollection<Event> events { get; set; }
        public bool DevMode { get; set; }
        public Result result { get; set; }
        public Debug debug { get; set; }
        public override string ToString()
        {
            return "Events count " + events.Count;
        }
    }
    public class Events
    {
        public ObservableCollection<Event> events { get; set; }
    }
    public class Item
    {
        public string id { get; set; }
        public string event_id { get; set; }
        public string item_type_id { get; set; }
        public string reference_item_id { get; set; }
        public string dish_count { get; set; }
        public object measure { get; set; }
        public string proteins { get; set; }
        public string fats { get; set; }
        public string carbs { get; set; }
        public string cellulose { get; set; }
        public string kkal { get; set; }
        public object repeats { get; set; }
        public object sets { get; set; }
        public object rest { get; set; }
        public object exercises_count { get; set; }
        public string caption { get; set; }
        public override string ToString()
        {
            return caption;
        }

    }

    public class Event
    {
        public int event_class { get; set; }
        public int id { get; set; }
        public Img img { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string event_name { get; set; }
        public string expert_name { get; set; }
        public int confirmed { get; set; }
        public string comment { get; set; }
        public Details details { get; set; }
        public string data_md5 { get; set; }
        public override string ToString()
        {
            return string.Format("{0}, {1} - {2}", date, time, event_name);
        }

    }

    public class Result
    {
        public ObservableCollection<object> errors { get; set; }
        public ObservableCollection<object> warnings { get; set; }
        public ObservableCollection<object> notifies { get; set; }
    }

    public class Messages
    {
        public ObservableCollection<object> errors { get; set; }
        public ObservableCollection<object> warnings { get; set; }
        public ObservableCollection<object> notifies { get; set; }
    }
    public class Img
    {
        public object path { get; set; }
        public object md5 { get; set; }
    }
    public class Debug
    {
        public string client_id { get; set; }
        public double runtime { get; set; }
        public string script { get; set; }
        public int queries { get; set; }
        public Messages messages { get; set; }
        public int responsesize { get; set; }
    }
    public class Performer
    {
        public string id { get; set; }
        public string expert_name { get; set; }
        public string expert_login { get; set; }
        public string avatar { get; set; }
        public string expert_sex { get; set; }
        public string task_id { get; set; }
        public string expert_id { get; set; }
        public string mail { get; set; }
    }

    public class Details
    {
        public Details()
        {
            items = new ObservableCollection<Item>();
            performers=new ObservableCollection<Performer>();
        }

        public string proteins { get; set; }
        public string fats { get; set; }
        public string carbs { get; set; }
        public string cellulose { get; set; }
        public string kkal { get; set; }
        public ObservableCollection<Item> items { get; set; }
        public string descr { get; set; }
        public object assoc_client { get; set; }
        public ObservableCollection<Performer> performers { get; set; }
    }
    //public class Event : INotifyPropertyChanged
    //{
    //    public Event(int event_id, string img, string time,string expert, string caption, bool confirmed)
    //    {
    //        Event_id = event_id;
    //        Img = img;
    //        Time = time;
    //        Expert = expert;
    //        Caption = caption;
    //        Items = new ObservableCollection<Items>();
    //        Confirmed = confirmed;
    //    }

    //    private string _caption;
    //    public int Event_id { get; set; }
    //    public string Img { get; set; }
    //    public string Time { get; set; }
    //    public string Expert { get; set; }
    //    public string Caption
    //    {
    //        get
    //        {
    //            return _caption;
    //        }
    //        set
    //        {
    //            if (Caption != value)
    //            {
    //                _caption = value;
    //                OnPropertyChanged("caption");
    //            }
    //        }
    //    }
    //    public bool Confirmed { get; set; }
    //    public ObservableCollection<Items> Items { get; set; }
    //    public override string ToString()
    //    {
    //        return Caption;
    //    }
    //    public event PropertyChangedEventHandler PropertyChanged;
    //    private void OnPropertyChanged(string info)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;
    //        if (handler != null)
    //        {
    //            handler(this, new PropertyChangedEventArgs(info));
    //        }
    //    }

    //}

    //public class Events
    //{
    //    public string a_day_date { get; set; }
    //    public ObservableCollection<Event> events { get; set; }
    //    public override string ToString()
    //    {
    //        return "Events count " + events.Count;
    //    }
    //}
    //public class AppSettings
    //{
    //    public bool confirm_events { get; set; } // true,
    //    public bool enable_report_eating { get; set; } // true,
    //    public bool enable_report_preparats { get; set; } // true,
    //    public int cache_period { get; set; } // 7
    //}
    //public class GetUserInfo : INotifyPropertyChanged
    //{
    //    private string _UserF;
    //    private string _UserI;
    //    private string _UserO;
    //    public int success { get; set; }

    //    public string UserId { get; set; }
    //    public string UserLogin { get; set; } // "elchukov",
    //    public string UserImg { get; set; } //"avatars/1.png",
    //    public string UserF
    //    {
    //        get { return _UserF; }
    //        set
    //        {
    //            _UserF = value;
    //            NotifyPropertyChanged("UserF");
    //        }
    //    } //"Ельчуков",
    //    public string UserI
    //    {
    //        get { return _UserI; }
    //        set
    //        {
    //            _UserI = value;
    //            NotifyPropertyChanged("UserI");
    //        }
    //    } // "Сергей",
    //    public string UserO
    //    {
    //        get { return _UserO; }
    //        set
    //        {
    //            _UserO = value;
    //            NotifyPropertyChanged("UserO");
    //        }
    //    } // "Викторович",
    //    public string UserDateReg { get; set; } //"2014-06-23 14:37:46",
    //    public AppSettings Settings { get; set; }
    //    public int not_confirmed_events_count { get; set; } // 12,          
    //    public int new_notifications_count { get; set; } // 5,
    //    public float working_time { get; set; } // 0.002

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    private void NotifyPropertyChanged(string propertyName)
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }
    //}

    public sealed class Everyday
    {
        public LoginData loginData = new LoginData();
        public ErrorStatus errStatus = new ErrorStatus();
        //public Events getEvents = new Events();
       // public GetUserInfo getUserInfo = new GetUserInfo();
        //public Events Ev;


        public string OSVersion = "Windows Phone";//Environment.OSVersion.ToString();
        public string SERVER = "http://api.go.pl.ua/"; //"http://everyday.mk.ua/";
        public string deviceID { get; set; }
        public string response { get; set; }
        public string qry;

        //public Bitmap UserImg;

        private const string quote = "\"";

        public int SUCCESS
        {
            get; set;
        }

        public string Token
        {
            get; set;
        }

        public Everyday()
        {
            deviceID = GetDeviceId();
        }
        //==================================================================
        //public Events GetEventsByData(string date) //date format "2014-08-20"
        //{
        //    string qry = SERVER + "GetEvents.php?Token=" + loginData.token
        //                 + "&Devid=" + deviceID
        //                 + "&Platform=" + OSVersion.ToString()
        //                 + "&Query={" + quote + "aday" + quote + ":" + quote + date + quote + "}";
        //    MakeQueryToServer(qry);
        //    return getEvents;
        //}
        public string GetDataFromString(string sParametr, string StringResponse)
        {
            int i;
            int j;
            int k;
            i = (StringResponse.IndexOf(sParametr) + 1);
            if ((i > 0))
            {
                j = (StringResponse.IndexOf(":", (i - 1), StringComparison.Ordinal) + 1);
                k = (StringResponse.IndexOf(",", (j - 1), StringComparison.Ordinal) + 1);
                return StringResponse.Substring(j, (k - (j - 1))).Trim('\"', ',');
            }
            return "NoData";
        }

        //public async void MakeQueryToServer(string qry)
        //{
        //    HttpClient client = new HttpClient();
        //    response = await client.GetStringAsync(new Uri(qry));
        //    SUCCESS = GetDataFromString("success", response) == "1" ? 1 : 0;
        //}

        //public class GroupEvents
        //{
        //    public int success { get; set; }
        //    public string a_day_string { get; set; }
        //    public int a_day_date { get; set; }
        //    public ObservableCollection<Event> events { get; set; }
        //    public int a_day_events_count { get; set; }
        //    public bool DevMode { get; set; }
        //    public Result result { get; set; }
        //    public Debug debug { get; set; }
        //    public override string ToString()
        //    {
        //        return "Events count " + events.Count;
        //    }
        //}

        private string GetDeviceID2()
        {
            HardwareToken token = HardwareIdentification.GetPackageSpecificToken(null);
            IBuffer hardwareId = token.Id;

            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer hashed = hasher.HashData(hardwareId);

            string hashedString = CryptographicBuffer.EncodeToHexString(hashed);
            return hashedString;
        }
        private string GetDeviceId()
        {
            var token = HardwareIdentification.GetPackageSpecificToken(null);
            var hardwareId = token.Id;
            var dataReader = DataReader.FromBuffer(hardwareId);

            byte[] bytes = new byte[hardwareId.Length];
            dataReader.ReadBytes(bytes);

            return BitConverter.ToString(bytes).Replace("-", "");
        }//Note: This function may throw an exception. 
    }
}
