﻿using eDay.Common;
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
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public CalendarPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += this.NavigationHelper_SaveState;
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
        /// Заполняет страницу содержимым, передаваемым в процессе навигации. Также предоставляется (при наличии) сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="sender">
        /// Источник события; как правило, <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Данные события, предоставляющие параметр навигации, который передается в
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы, и
        /// словарь состояний, сохраненных этой страницей в ходе предыдущего
        /// сеанса.  Состояние будет равно значению NULL при первом посещении страницы.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Сохраняет состояние, связанное с данной страницей, в случае приостановки приложения или
        /// удаления страницы из кэша навигации.  Значения должны соответствовать требованиям сериализации
        /// <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">Источник события; как правило, <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Данные события, которые предоставляют пустой словарь для заполнения
        /// сериализуемым состоянием.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Сохраните здесь уникальное состояние страницы.
        }

        #region Регистрация NavigationHelper

        /// <summary>
        /// Методы, предоставленные в этом разделе, используются лишь для того, чтобы разрешить
        /// классу NavigationHelper отвечать на методы навигации страницы.
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
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);

        }
        #endregion
    }
}