using eDay.Common;
using eDay.Data;
using NotificationsExtensions.TileContent;
using NotificationsExtensions.ToastContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Основная страница" см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace eDay
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private string token;
        //private const string quote = "\"";
        Everyday eday;
        private const string FirstGroupName = "FirstGroup";
        private const string SecondGroupName = "SecondGroup";
        IEnumerable<Event> eDayDataGroup;

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public MainPage()
        {
            InitializeComponent();
            listView.Visibility = Visibility.Collapsed;
            App thisApp = Application.Current as App;
            eday = thisApp.eday;
            Loaded += OnLoaded; 
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        async void OnLoaded(object sender, RoutedEventArgs arg)
        {
            AppHeader.Text = "Обновляю данные...";
            if (eday.Token == null) await eday.LoginEveryday();
            eDayDataGroup = await eDayDataSource.GetEventsByDateAsync(DateTime.Today.ToString("yyyy-MM-dd"));
            DefaultViewModel[FirstGroupName] = eDayDataGroup;
            listView.ItemsSource = eDayDataGroup;
            AppHeader.Text = "Everyday";
            listView.Visibility = Visibility.Visible;
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
        }

        /// <summary>
        /// Получает объект <see cref="NavigationHelper"/>, связанный с данным объектом <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Получает модель представлений для данного объекта <see cref="Page"/>.
        /// Эту настройку можно изменить на модель строго типизированных представлений.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации.  Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="sender">
        /// Источник события; как правило, <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Данные события, предоставляющие параметр навигации, который передается
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы и
        /// словарь состояний, сохраненных этой страницей в ходе предыдущего
        /// сеанса.  Это состояние будет равно NULL при первом посещении страницы.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            DefaultViewModel[FirstGroupName] = eDayDataGroup;
        }

        /// <summary>
        /// Сохраняет состояние, связанное с данной страницей, в случае приостановки приложения или
        /// удаления страницы из кэша навигации.  Значения должны соответствовать требованиям сериализации
        /// <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">Источник события; как правило, <see cref="NavigationHelper"/></param>
        /// <param name="e">Данные события, которые предоставляют пустой словарь для заполнения
        /// сериализуемым состоянием.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((Event)e.ClickedItem).id;
            //// Переход к соответствующей странице назначения и настройка новой страницы
            //// путем передачи необходимой информации в виде параметра навигации
            //var itemId = ((Event)e.ClickedItem).event_name;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }

        }


        private async void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {

            eDayDataGroup = await eDayDataSource.GetEventsByDateAsync(e.NewDate.ToString("yyyy-MM-dd"));
            listView.ItemsSource = eDayDataGroup;
        }

        private async void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton b = sender as AppBarButton;
            switch (b.Name)
            {
                case "LoginButton":
                    await eday.LoginEveryday();
                    break;
                case "SettingsButton":
                    break;
                case "AddEventButton":
                    break;
                case "UpdateButton":
                    break;

            }
        }

        public void NotifyUser(string strMessage, NotifyType type)
        {
            if (StatusBlock != null)
            {
                switch (type)
                {
                    case NotifyType.StatusMessage:
                        StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                        break;
                    case NotifyType.ErrorMessage:
                        StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                        break;
                }
                StatusBlock.Text = strMessage;
                // Collapse the StatusBlock if it has no text to conserve real estate.
                if (StatusBlock.Text != string.Empty)
                {
                    StatusBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    StatusBorder.Visibility = Visibility.Collapsed;
                }
            }
        }
        void ScheduleToast(string updateString, DateTime dueTime, int idNumber)
        {
            // Scheduled toasts use the same toast templates as all other kinds of toasts.
            IToastText02 toastContent = ToastContentFactory.CreateToastText02();
            toastContent.TextHeading.Text = updateString;
            toastContent.TextBodyWrap.Text = "Received: " + dueTime.ToLocalTime();

            ScheduledToastNotification toast;
            //if (RepeatBox.IsChecked != null && (bool)RepeatBox.IsChecked)
            //{
            //    toast = new ScheduledToastNotification(toastContent.GetXml(), dueTime, TimeSpan.FromSeconds(60), 5);

            //    // You can specify an ID so that you can manage toasts later.
            //    // Make sure the ID is 15 characters or less.
            //    toast.Id = "Repeat" + idNumber;
            //}
            //else
            //{
                toast = new ScheduledToastNotification(toastContent.GetXml(), dueTime);
                toast.Id = "Toast" + idNumber;
            //}

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
            NotifyUser("Scheduled a toast with ID: " + toast.Id, NotifyType.StatusMessage);
        }
        void ScheduleTile(String updateString, DateTime dueTime, int idNumber)
        {
            // Set up the wide tile text
            ITileWide310x150Text09 tileContent = TileContentFactory.CreateTileWide310x150Text09();
            tileContent.TextHeading.Text = updateString;
            tileContent.TextBodyWrap.Text = "Received: " + dueTime.ToLocalTime();

            // Set up square tile text
            ITileSquare150x150Text04 squareContent = TileContentFactory.CreateTileSquare150x150Text04();
            squareContent.TextBodyWrap.Text = updateString;

            tileContent.Square150x150Content = squareContent;

            // Create the notification object
            ScheduledTileNotification futureTile = new ScheduledTileNotification(tileContent.GetXml(), dueTime);
            futureTile.Id = "Tile" + idNumber;

            // Add to schedule
            // You can update a secondary tile in the same manner using CreateTileUpdaterForSecondaryTile(tileId)
            // See "Tiles" sample for more details
            TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(futureTile);
            NotifyUser("Scheduled a tile with ID: " + futureTile.Id, NotifyType.StatusMessage);
        }
        void ScheduleToastWithStringManipulation(String updateString, DateTime dueTime, int idNumber)
        {
            // Scheduled toasts use the same toast templates as all other kinds of toasts.
            string toastXmlString = "<toast>"
            + "<visual version='2'>"
            + "<binding template='ToastText02'>"
            + "<text id='2'>" + updateString + "</text>"
            + "<text id='1'>" + "Received: " + dueTime.ToLocalTime() + "</text>"
            + "</binding>"
            + "</visual>"
            + "</toast>";

            Windows.Data.Xml.Dom.XmlDocument toastDOM = new Windows.Data.Xml.Dom.XmlDocument();
            try
            {
                toastDOM.LoadXml(toastXmlString);

                ScheduledToastNotification toast;
                //if (RepeatBox.IsChecked != null && (bool)RepeatBox.IsChecked)
                //{
                //    toast = new ScheduledToastNotification(toastDOM, dueTime, TimeSpan.FromSeconds(60), 5);

                //    // You can specify an ID so that you can manage toasts later.
                //    // Make sure the ID is 15 characters or less.
                //    toast.Id = "Repeat" + idNumber;
                //}
                //else
                //{
                    toast = new ScheduledToastNotification(toastDOM, dueTime);
                    toast.Id = "Toast" + idNumber;
                //}

                ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
                NotifyUser("Scheduled a toast with ID: " + toast.Id, NotifyType.StatusMessage);
            }
            catch (Exception)
            {
                NotifyUser("Error loading the xml, check for invalid characters in the input", NotifyType.ErrorMessage);
            }
        }
        void ScheduleTileWithStringManipulation(String updateString, DateTime dueTime, int idNumber)
        {
            string tileXmlString = "<tile>"
                         + "<visual version='2'>"
                         + "<binding template='TileWide310x150Text09' fallback='TileWideText09'>"
                         + "<text id='1'>" + updateString + "</text>"
                         + "<text id='2'>" + "Received: " + dueTime.ToLocalTime() + "</text>"
                         + "</binding>"
                         + "<binding template='TileSquare150x150Text04' fallback='TileSquareText04'>"
                         + "<text id='1'>" + updateString + "</text>"
                         + "</binding>"
                         + "</visual>"
                         + "</tile>";

            Windows.Data.Xml.Dom.XmlDocument tileDOM = new Windows.Data.Xml.Dom.XmlDocument();
            try
            {
                tileDOM.LoadXml(tileXmlString);

                // Create the notification object
                ScheduledTileNotification futureTile = new ScheduledTileNotification(tileDOM, dueTime);
                futureTile.Id = "Tile" + idNumber;

                // Add to schedule
                // You can update a secondary tile in the same manner using CreateTileUpdaterForSecondaryTile(tileId)
                // See "Tiles" sample for more details
                TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(futureTile);
                NotifyUser("Scheduled a tile with ID: " + futureTile.Id, NotifyType.StatusMessage);
            }
            catch (Exception)
            {
                NotifyUser("Error loading the xml, check for invalid characters in the input", NotifyType.ErrorMessage);
            }
        }
        public enum NotifyType
        {
            StatusMessage,
            ErrorMessage
        };

    }

}
