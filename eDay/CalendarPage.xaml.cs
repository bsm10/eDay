using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace eDay
{
    //public class DateTimeToString : IValueConverter
    //{
    //    // This converts the DateTime object to the string to display.
    //    public object Convert(object value, Type targetType, object parameter, string language)
    //    {
    //        if (value != null && value is DateTime)
    //        {
    //            DateTime val = (DateTime)value;

    //            return val.Day.ToString();
    //        }
    //        return null;
    //    }

    //    // No need to implement converting back on a one-way binding 
    //    public object ConvertBack(object value, Type targetType, object parameter, string language)
    //    {
    //        if (value != null && value is DateTime)
    //        {
    //            string val = (string)value;
    //            //тут неправильно возвращает дату, но это пока как заглушка
    //            return DateTime.Now.Day;
    //        }
    //        return null;
    //        //throw new NotImplementedException();
    //    }
    //}
    public class MonthCalendar
    {
        public ObservableCollection<MonthDay> days { get; set; }
        public int year { get; set; }
        public int month { get; set; }

        public MonthCalendar()
        {
            days = CalendarFo.GetDaysOfMonth(DateTime.Now.Year, DateTime.Now.Month);
        }
        public string monthMMMMyyyy
        {
            get
            {
                if (year == 0) year = DateTime.Now.Year;
                if (month == 0) month = DateTime.Now.Month;
                return new DateTime(year, month, 1).ToString("MMMM yyyy");
            }
        }
    }
    public class MonthDay
    {
        public DateTime datetime { get; set; }
        //public Color color { get; set; }
        public SolidColorBrush colorDay { get; set; }
        public MonthDay(DateTime date)
        {
            datetime = date;
        }
        public override string ToString()
        {
            return datetime.Day.ToString();
        }
    }

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CalendarPage : Page
    {
        public CalendarPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;

        }

        void OnLoaded(object sender, RoutedEventArgs arg)
        {
            MonthCalendar mcal = new MonthCalendar();
            //mcal.days = CalendarFo.GetDaysOfMonth(DateTime.Now.Year, DateTime.Now.Month);
            mcal.month = DateTime.Now.Month;
            mcal.year = DateTime.Now.Year;
            calendar.DataContext = mcal;
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
