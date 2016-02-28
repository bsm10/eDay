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
    public class Result
    {
        public int success { get; set; }
    }
    public class Items
    {
        public string id { get; set; }
        public string name { get; set; }
        public override string ToString()
        {
            return name;
        }

    }
    public class Events : INotifyPropertyChanged
    {
        public Events(int event_id, string img, string time,string expert, string caption, bool confirmed, int items_count, int a_day_events_count, int not_confirmed_events_count)
        {
            Event_id = event_id;
            Img = img;
            Time = time;
            Expert = expert;
            Caption = caption;
            Items = new ObservableCollection<Items>();
            Confirmed = confirmed;
            Items_count = items_count;
            A_day_events_count=a_day_events_count;
            Not_confirmed_events_count = not_confirmed_events_count;
        }

        private string _caption;
        public int Event_id { get; set; }
        public string Img { get; set; }
        public string Time { get; set; }
        public string Expert { get; set; }
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                if (Caption != value)
                {
                    _caption = value;
                    OnPropertyChanged("caption");
                }
            }
        }
        //public int confirmed { get; set; }
        public bool Confirmed { get; set; }

        public int Items_count { get; set; }
        public ObservableCollection<Items> Items { get; set; }
        public int A_day_events_count { get; set; }
        public int Not_confirmed_events_count { get; set; }
        public float Working_time { get; set; }
        public override string ToString()
        {
            return Caption;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

    }

    public class GetEvents
    {
        public int success { get; set; }
        public string a_day_string { get; set; }
        public string a_day_date { get; set; }
        public ObservableCollection<Events> events { get; set; }
        public override string ToString()
        {
            return "Events count " + events.Count;
        }

    }
    public class AppSettings
    {
        public bool confirm_events { get; set; } // true,
        public bool enable_report_eating { get; set; } // true,
        public bool enable_report_preparats { get; set; } // true,
        public int cache_period { get; set; } // 7
    }
    public class GetUserInfo : INotifyPropertyChanged
    {
        private string _UserF;
        private string _UserI;
        private string _UserO;
        public int success { get; set; }
        
        public string UserId { get; set; }
        public string UserLogin { get; set; } // "elchukov",
        public string UserImg { get; set; } //"avatars/1.png",
        public string UserF
        {
            get { return _UserF; }
            set
            {
                _UserF = value;
                NotifyPropertyChanged("UserF");
            }
        } //"Ельчуков",
        public string UserI
        {
            get { return _UserI; }
            set
            {
                _UserI = value;
                NotifyPropertyChanged("UserI");
            }
        } // "Сергей",
        public string UserO
        {
            get { return _UserO; }
            set
            {
                _UserO = value;
                NotifyPropertyChanged("UserO");
            }
        } // "Викторович",
        public string UserDateReg { get; set; } //"2014-06-23 14:37:46",
        public AppSettings Settings { get; set; }
        public int not_confirmed_events_count { get; set; } // 12,          
        public int new_notifications_count { get; set; } // 5,
        public float working_time { get; set; } // 0.002

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Everyday
    {
        public LoginData loginData = new LoginData();
        public ErrorStatus errStatus = new ErrorStatus();
        public GetEvents getEvents = new GetEvents();
        public GetUserInfo getUserInfo = new GetUserInfo();
        //public Events Ev;


        public string OSVersion = "Windows Phone";//Environment.OSVersion.ToString();
        public string SERVER = "http://api.go.pl.ua/"; //"http://everyday.mk.ua/";
        public string deviceID;
        public string response;
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
        public GetEvents GetEventsByData(string date) //date format "2014-08-20"
        {
            string qry = SERVER + "GetEvents.php?Token=" + loginData.token
                         + "&Devid=" + deviceID
                         + "&Platform=" + OSVersion.ToString()
                         + "&Query={" + quote + "aday" + quote + ":" + quote + date + quote + "}";
            MakeQueryToServer(qry);
            return getEvents;
        }
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

        public async void MakeQueryToServer(string qry)
        {
            HttpClient client = new HttpClient();
            response = await client.GetStringAsync(new Uri(qry));
            SUCCESS = GetDataFromString("success", response) == "1" ? 1 : 0;
        }

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
