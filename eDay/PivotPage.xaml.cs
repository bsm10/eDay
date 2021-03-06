﻿using eDay.Common;
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
using Windows.UI.Xaml.Media;
using System.Collections.Generic;

// Документацию по шаблону "Приложение с Pivot" см. по адресу http://go.microsoft.com/fwlink/?LinkID=391641

namespace eDay
{
    public sealed partial class PivotPage : Page
    {
        private string token;
        private const string quote = "\"";
        private const string FirstGroupName = "FirstGroup";
        private const string SecondGroupName = "SecondGroup";
        IEnumerable<Event> eDayDataGroup;

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public PivotPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            NavigationCacheMode = NavigationCacheMode.Required;
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
        }
        async void OnLoaded(object sender, RoutedEventArgs arg)
        {

            if (Everyday.Token == null)
            {
                //NotifyUser("Обновляю данные...", NotifyType.StatusMessage);
                await Everyday.LoginEveryday();
                eDayDataGroup = await eDayDataSource.GetEventsByDateAsync(DateTime.Today.ToString("yyyy-MM-dd"));
                DefaultViewModel[FirstGroupName] = eDayDataGroup;
                pivot.ItemsSource = eDayDataGroup;
                //foreach (Event e in eDayDataGroup)
                //{
                //    ScheduleToast(e.event_name, DateTime.Parse(e.date + " " + e.time));
                //}
                //NotifyUser("", NotifyType.StatusMessage);
            }
            //listView.ItemsSource = eDayDataGroup;
            //listView.Visibility = Visibility.Visible;
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
            var eDayDataGroup = await eDayDataSource.GetEventsByDateAsync(DateTime.Today.ToString("yyyy-MM-dd"));
            //var eDayDataGroup = await eDayDataSource.GetGroupEventsAsync();
            //DefaultViewModel[FirstGroupName] = eDayDataGroup;
            //if (Everyday.Token==null) await Everyday.LoginEveryday();
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
            var itemId = ((Event)e.ClickedItem).id;
            //// Переход к соответствующей странице назначения и настройка новой страницы
            //// путем передачи необходимой информации в виде параметра навигации
            //var itemId = ((Event)e.ClickedItem).event_name;
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
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
            await Everyday.LoginEveryday();
        }
        private void listView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            
        }
        private void listView_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            //lst.Visibility = lst.Visibility== Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton b = sender as AppBarButton;
            switch (b.Name)
            {
                case "LoginButton":
                    await Everyday.LoginEveryday();
                    token = Everyday.Token;
                    break;
                case "SettingsButton":
                    break;
                case "AddEventButton":

                    break;
                case "UpdateButton":
                    break;

            }
        }
    }
}
