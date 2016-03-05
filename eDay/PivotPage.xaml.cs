using eDay.Common;
using eDay.Data;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// Документацию по шаблону "Приложение с Pivot" см. по адресу http://go.microsoft.com/fwlink/?LinkID=391641

namespace eDay
{
    public sealed partial class PivotPage : Page
    {
        public Everyday EVERYDAY { get; set; }
        string response;
        LoginData loginData;
        private const string quote = "\"";

        private const string FirstGroupName = "FirstGroup";
        private const string SecondGroupName = "SecondGroup";

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public PivotPage()
        {
            InitializeComponent();
            
            NavigationCacheMode = NavigationCacheMode.Required;
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
        }

        /// <summary>
        /// Получает объект <see cref="NavigationHelper"/>, связанный с данным объектом <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        /// <summary>
        /// Получает модель представлений для данного объекта <see cref="Page"/>.
        /// Эту настройку можно изменить на модель строго типизированных представлений.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return defaultViewModel; }
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации. Также предоставляется (при наличии) сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="sender">
        /// Источник события; как правило, <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Данные события, предоставляющие параметр навигации, который передается
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы и
        /// словарь состояний, сохраненных этой страницей в ходе предыдущего
        /// сеанса. Состояние будет равно значению NULL при первом посещении страницы.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Создание соответствующей модели данных для области проблемы, чтобы заменить пример данных
            // var eDayDataGroup = await eDayDataSource.GetEventsByDateAsync("2016-02-20"); // ("");
            
            var eDayDataGroup = await eDayDataSource.GetGroupEventsAsync();
            DefaultViewModel[FirstGroupName] = eDayDataGroup;
            await LoginEveryday();
            //await GetEvents(DateTime.Today.ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd"));
        }

        private async Task LoginEveryday()
        {
            EVERYDAY = new Everyday();
            LoginDialog loginDialog = new LoginDialog();
            loginDialog.EVERYDAY = EVERYDAY;
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
                
            }
        }
        private async Task Login(string sLog, string sPass)
        {
            string qry;
            qry = (EVERYDAY.SERVER
                        + (("Login.php?&Devid="
                        + (EVERYDAY.deviceID + ("&Platform="
                        + (EVERYDAY.OSVersion + "&Query={\"login\":\""))))
                        + (sLog + ("\",\"pass\":\""
                        + (sPass + "\"}")))));
            HttpClient client = new HttpClient();
            response = await client.GetStringAsync(new Uri(qry));
        }
        private async Task GetEvents(string startDate, string endDate)
        {
            if (loginData == null) return;
            string postData = string.Format("Token={0}&Devid={1}&Platform={2}&Query={{\"date_start\":\"{3}\",\"date_end\":\"{4}\"}}",
                                        loginData.token, "bsm10", "Win 8.1", startDate, endDate);
            HttpClient client = new HttpClient();
            response = await client.GetStringAsync(new Uri(EVERYDAY.SERVER + "ios/rGetEvents.php?"+ postData));
            await saveStringToLocalFile(response);
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

            Uri fileUri = new Uri ("ms-appx:///DataModel/eDayData.json");
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
            catch(Exception ex)
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

        /// <summary>
        /// Сохраняет состояние, связанное с данной страницей, в случае приостановки приложения или
        /// удаления страницы из кэша навигации. Значения должны соответствовать требованиям сериализации
        /// <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">Источник события; как правило, <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Данные события, которые предоставляют пустой словарь для заполнения
        /// сериализуемым состоянием.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Сохраните здесь уникальное состояние страницы.
        }

        /// <summary>
        /// Добавляет элемент в список при нажатии кнопки на панели приложения.
        /// </summary>
        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //string groupName = pivot.SelectedIndex == 0 ? FirstGroupName : SecondGroupName;
            //var group = DefaultViewModel[groupName] as EverydayGroupEvents;
            //var nextItemId = group.Items.Count + 1;
            //var newItem = new EverydayEvents(
            //    string.Format(CultureInfo.InvariantCulture, "Group-{0}-Item-{1}", pivot.SelectedIndex + 1, nextItemId),
            //    string.Format(CultureInfo.CurrentCulture, resourceLoader.GetString("NewItemTitle"), nextItemId),
            //    string.Empty,
            //    string.Empty,
            //    resourceLoader.GetString("NewItemDescription"),
            //    string.Empty);

            //group.Items.Add(newItem);

            //// Прокручиваем, чтобы новый элемент оказался видимым.
            //var container = pivot.ContainerFromIndex(this.pivot.SelectedIndex) as ContentControl;
            //var listView = container.ContentTemplateRoot as ListView;
            //listView.ScrollIntoView(newItem, ScrollIntoViewAlignment.Leading);
        }

        /// <summary>
        /// Вызывается при нажатии элемента внутри раздела.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Переход к соответствующей странице назначения и настройка новой страницы
            // путем передачи необходимой информации в виде параметра навигации
            var itemId = ((Event)e.ClickedItem).event_name;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Загружает содержимое для второго элемента Pivot, когда он становится видимым в результате прокрутки.
        /// </summary>
        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var sampleDataGroup = await eDayDataSource.GetEventsByDateAsync("Group-2");
            DefaultViewModel[SecondGroupName] = sampleDataGroup;
        }

        #region Регистрация NavigationHelper

        /// <summary>
        /// Методы, предоставленные в этом разделе, используются исключительно для того, чтобы
        /// NavigationHelper для отклика на методы навигации страницы.
        /// <para>
        /// Логика страницы должна быть размещена в обработчиках событий для 
        /// <see cref="NavigationHelper.LoadState"/>
        /// и <see cref="NavigationHelper.SaveState"/>.
        /// Параметр навигации доступен в методе LoadState 
        /// в дополнение к состоянию страницы, сохраненному в ходе предыдущего сеанса.
        /// </para>
        /// </summary>
        /// <param name="e">Предоставляет данные для методов навигации и обработчики
        /// событий, которые не могут отменить запрос навигации.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }
        #endregion

        private async void SecondaryButton1_Click(object sender, RoutedEventArgs e)
        {
            await GetEvents(DateTime.Today.ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd"));
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
