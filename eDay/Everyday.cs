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
using System.Threading.Tasks;
using System.Threading;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;

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
    public class Events
    {
        public Events()
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
        public override string ToString()
        {
            string sBuffer = string.Empty;
            foreach(Item i in items)
            {
                sBuffer += "- " + i.caption + "\r\n";
            }
            return sBuffer;
        }

    }
    //public class Event : INotifyPropertyChanged
    //{
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
    public sealed class Everyday
    {
        public LoginData loginData = new LoginData();
        public ErrorStatus errStatus = new ErrorStatus();

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

        public async Task LoginEveryday()
        {
            LoginDialog loginDialog = new LoginDialog();
            login_now:
            // Show Dialog если залогинились -> 
            var result = await loginDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await Login(loginDialog.Login, loginDialog.Password);
                ErrorStatus res = JsonConvert.DeserializeObject<ErrorStatus>(response) as ErrorStatus;
                if (res.success == 0)
                {
                    MessageDialog msgbox = new MessageDialog(res.error_for_user, "Ошибка!");
                    await msgbox.ShowAsync();
                    goto login_now;
                }
                loginData = JsonConvert.DeserializeObject<LoginData>(response);
                Token = loginData.token;
                //Взять события на 5 дней
                await GetEvents(DateTime.Today.ToString("yyyy-MM-dd"), (DateTime.Today+TimeSpan.FromDays(5)).ToString("yyyy-MM-dd"));
                //await Task.Delay(5000);
            }
        }
        private async Task Login(string sLog, string sPass)
        {
            string qry;
            qry = (SERVER
                        + (("Login.php?&Devid="
                        + (deviceID + ("&Platform="
                        + (OSVersion + "&Query={\"login\":\""))))
                        + (sLog + ("\",\"pass\":\""
                        + (sPass + "\"}")))));
            HttpClient client = new HttpClient();
            response = await client.GetStringAsync(new Uri(qry));
        }
        public async Task GetEvents(string startDate, string endDate, bool save_data=true)
        {
            if (loginData == null) return;
            string postData = string.Format("Token={0}&Devid={1}&Platform={2}&Query={{\"date_start\":\"{3}\",\"date_end\":\"{4}\"}}",
                                        loginData.token, "bsm10", "Win 8.1", startDate, endDate);
            HttpClient client = new HttpClient();
            response = await client.GetStringAsync(new Uri(SERVER + "ios/rGetEvents.php?" + postData));
            if (save_data) await saveStringToLocalFile(response);
        }
        private async Task<string> HttpQuery(string qry)
        {
            HttpClient client = new HttpClient();
            return await client.GetStringAsync(new Uri(qry));
        }

        private async Task<byte[]> GetURLContentsAsync(string url)
        {
            // The downloaded resource ends up in the variable named content. 
            var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL. 
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            // Send the request to the Internet resource and wait for 
            // the response.                 
            using (WebResponse response = await webReq.GetResponseAsync())
            {
                // Get the data stream that is associated with the specified url. 
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Read the bytes in responseStream and copy them to content. 
                    await responseStream.CopyToAsync(content);
                }
            }
            // Return the result as a byte array. 
            return content.ToArray();
        }
        private async Task saveStringToLocalFile(string content)
        {

            Uri fileUri = new Uri("ms-appx:///DataModel/eDayData.json");
            // saves the string 'content' to a file 'filename' in the app's local storage folder
            byte[] fileBytes = Encoding.UTF8.GetBytes(content.ToCharArray());
            try
            {
                // create a file with the given filename in the local folder; replace any existing file with the same name
                //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(fileUri);
                await FileIO.WriteTextAsync(file, content);
                //using (var stream = await file.OpenStreamForWriteAsync())
                //{
                //    stream.Write(fileBytes, 0, fileBytes.Length);
                //}



            }
            catch (Exception ex)
            {
                string f = ex.ToString();
            }
        }
        private static async Task<string> readStringFromLocalFile(string filename)
        {
            // reads the contents of file 'filename' in the app's local storage folder and returns it as a string

            // access the local folder
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            // open the file 'filename' for reading
            Stream stream = await local.OpenStreamForReadAsync(filename);
            string text;

            // copy the file contents into the string 'text'
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }
    }
    public class DoubleToBool : IValueConverter
    {
        // This converts the DateTime object to the string to display.
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is double)
            {
                var val = (double)value; return (val == 0) ? false : true;
            }
            return null;
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is bool)
            {
                var val = (bool)value; return val ? 1 : 0;
            }
            return null;
            //throw new NotImplementedException();
        }


    }

}
