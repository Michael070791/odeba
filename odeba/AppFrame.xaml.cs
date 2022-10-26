using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace odeba
{
    public partial class AppFrame : ContentView
    {

        public static readonly BindableProperty PageProperty =
           BindableProperty.Create(nameof(Page), typeof(View), typeof(AppFrame), propertyChanged: (bindable, old, newV) => {
               if (bindable is AppFrame frame)
                   frame.OnPropertyChanged(nameof(DisplayContent));
           });

        public View Page
        {
            get => (View)GetValue(PageProperty);
            set => SetValue(PageProperty, value);
        }

        public static readonly BindableProperty LeftButtonCommandProperty =
           BindableProperty.Create(nameof(LeftButtonCommand), typeof(ICommand), typeof(AppFrame));
        public ICommand LeftButtonCommand
        {
            get => (ICommand)GetValue(LeftButtonCommandProperty);
            set => SetValue(LeftButtonCommandProperty, value);
        }


        public static readonly BindableProperty RightButtonCommandProperty =
           BindableProperty.Create(nameof(RightButtonCommand), typeof(ICommand), typeof(AppFrame));
        public ICommand RightButtonCommand
        {
            get => (ICommand)GetValue(RightButtonCommandProperty);
            set => SetValue(RightButtonCommandProperty, value);
        }


        public static BindableProperty NavRightButtonIconProperty =
            BindableProperty.Create(nameof(NavRightButtonIcon), typeof(ImageSource), typeof(AppFrame));

        public ImageSource NavRightButtonIcon
        {
            get => (ImageSource)GetValue(NavRightButtonIconProperty);
            set => SetValue(NavRightButtonIconProperty, value);
        }
        public ImageSource NavLeftButtonIcon
        {
            get => (ImageSource)GetValue(NavLeftButtonIconProperty);
            set => SetValue(NavLeftButtonIconProperty, value);
        }

        public static BindableProperty NavLeftButtonIconProperty =
            BindableProperty.Create(nameof(NavLeftButtonIcon), typeof(ImageSource), typeof(AppFrame),
                ImageSource.FromFile("back.png"));



        public static BindableProperty PageTitleIconProperty =
          BindableProperty.Create(nameof(PageTitleIcon), typeof(ImageSource), typeof(AppFrame),
              ImageSource.FromFile("logo.png"));
        public ImageSource PageTitleIcon
        {
            get => (ImageSource)GetValue(PageTitleIconProperty);
            set => SetValue(PageTitleIconProperty, value);
        }

        public static BindableProperty PageTitleProperty =
          BindableProperty.Create(nameof(PageTitle), typeof(string), typeof(AppFrame), string.Empty);
        public string PageTitle
        {
            get => (string)GetValue(PageTitleProperty);
            set => SetValue(PageTitleProperty, value);
        }


        public static BindableProperty DisplayLogoProperty =
            BindableProperty.Create(nameof(DisplayLogo), typeof(bool), typeof(AppFrame), true);

        public bool DisplayLogo
        {
            get => (bool)GetValue(DisplayLogoProperty);
            set => SetValue(DisplayLogoProperty, value);
        }

        public static BindableProperty DisplayNavRightButtonProperty =
            BindableProperty.Create(nameof(DisplayNavRightButton), typeof(bool), typeof(AppFrame), false);

        public bool DisplayNavRightButton
        {
            get => (bool)GetValue(DisplayNavRightButtonProperty);
            set => SetValue(DisplayNavRightButtonProperty, value);
        }

        public static BindableProperty DisplayNavLeftButtonProperty =
            BindableProperty.Create(nameof(DisplayNavLeftButton), typeof(bool), typeof(AppFrame), false);

        public bool DisplayNavLeftButton
        {
            get => (bool)GetValue(DisplayNavLeftButtonProperty);
            set => SetValue(DisplayNavLeftButtonProperty, value);
        }

        public static BindableProperty FullScreenProperty =
            BindableProperty.Create(nameof(FullScreen), typeof(bool), typeof(AppFrame), true);

        public bool FullScreen
        {
            get => (bool)GetValue(FullScreenProperty);
            set => SetValue(FullScreenProperty, value);
        }


        public static BindableProperty SearchItemHeightProperty = BindableProperty.Create(
            nameof(SearchItemHeight),
            typeof(double),
            typeof(AppFrame),
            154d);

        public double SearchItemHeight
        {
            get => (double)GetValue(SearchItemHeightProperty);
            set => SetValue(SearchItemHeightProperty, value);
        }
        public static BindableProperty DisplaySearchProperty =
            BindableProperty.Create(nameof(DisplaySearch),
                typeof(bool),
                typeof(AppFrame),
                false,
                BindingMode.TwoWay, propertyChanged: (b, o, n) => {
                    if (b is AppFrame frame && n is bool value)
                    {
                        if (value && frame.DisplaySearchBar)
                            frame.searchEntry.Focus();
                        else
                        {
                            frame.searchSuggestions.SelectedItem = null;
                        }
                    }
                });

        public bool DisplaySearch { get => (bool)GetValue(DisplaySearchProperty); set => SetValue(DisplaySearchProperty, value); }

        public static BindableProperty DisplaySearchBarProperty = BindableProperty.Create(
            nameof(DisplaySearchBar),
            typeof(bool),
            typeof(AppFrame),
            true);

        public bool DisplaySearchBar { get => (bool)GetValue(DisplaySearchBarProperty); set => SetValue(DisplaySearchBarProperty, value); }

        public readonly static BindableProperty SearchItemsSourceProperty = BindableProperty.Create(
            nameof(SearchItemsSource),
            typeof(INotifyCollectionChanged),
            typeof(AppFrame),
            ItemsView.ItemsSourceProperty.DefaultValue,
            propertyChanged: (bindable, old, newValue) =>
            {
                if (bindable is AppFrame frame && newValue is INotifyCollectionChanged collection)
                {
                    void notify(object sender, NotifyCollectionChangedEventArgs args)
                    {
                        frame.searchSuggestions.HeightRequest = ((sender as ICollection).Count * frame.SearchItemHeight);
                    }
                    Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(
                        _ => collection.CollectionChanged += notify,
                        _ => collection.CollectionChanged -= notify
                        ).Subscribe();
                }
            });

        public INotifyCollectionChanged SearchItemsSource { get => (INotifyCollectionChanged)GetValue(SearchItemsSourceProperty); set => SetValue(SearchItemsSourceProperty, value); }


        public readonly static BindableProperty SearchItemTemplateProperty = BindableProperty.Create(
            nameof(SearchItemTemplate),
            typeof(DataTemplate),
            typeof(AppFrame),
            ItemsView.ItemTemplateProperty.DefaultValue
            );

        public DataTemplate SearchItemTemplate { get => (DataTemplate)GetValue(SearchItemTemplateProperty); set => SetValue(SearchItemTemplateProperty, value); }

        public static BindableProperty SearchSelectedItemProperty = BindableProperty.Create(
            nameof(SearchSelectedItem),
            typeof(object),
            typeof(AppFrame),
            SelectableItemsView.SelectedItemProperty.DefaultValue
            );

        public object SearchSelectedItem { get => GetValue(SearchSelectedItemProperty); set => SetValue(SearchSelectedItemProperty, value); }


        public static BindableProperty SearchTermProperty =
            BindableProperty.Create(nameof(SearchTerm),
                typeof(string),
                typeof(AppFrame),
                null,
                BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) => {
                    if (bindable is AppFrame frame && newValue is string value)
                    {
                        frame.searchSuggestions.IsVisible = (frame.SearchItemsSource as ICollection)?.Count > 0;
                        if (!frame.searchSuggestions.IsVisible)
                            frame.searchSuggestions.HeightRequest = 0;
                    }
                });

        public string SearchTerm { get => (string)GetValue(SearchTermProperty); set => SetValue(SearchTermProperty, value); }


        public static BindableProperty IsScrollableProperty = BindableProperty.Create(nameof(IsScrollable),
            typeof(bool), typeof(AppFrame), true);

        public bool IsScrollable { get => (bool)GetValue(IsScrollableProperty); set => SetValue(IsScrollableProperty, value); }

        readonly Lazy<ScrollView> ScrollViewContainer = new Lazy<ScrollView>(() => new ScrollView
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
            VerticalScrollBarVisibility = ScrollBarVisibility.Never,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand
        });

        public View DisplayContent
        {
            get
            {
                if (FullScreen) return Page;
                if (!IsScrollable) return Page;
                var scrollview = ScrollViewContainer.Value;
                scrollview.Content = Page;
                return scrollview;
            }
        }


        public AppFrame()
        {
            InitializeComponent();
            LazyInitializer.EnsureInitialized(ref ScrollViewContainer);
        }


        void ClearSearch_Tapped(Object sender, Xamarin.CommunityToolkit.Effects.TouchCompletedEventArgs e)
        {
            SearchTerm = string.Empty;
            searchEntry.Focus();
        }

        void HideSearch_Tapped(Object sender, Xamarin.CommunityToolkit.Effects.TouchCompletedEventArgs e)
        {
            searchEntry.Unfocus();
            DisplaySearch = false;
            SearchTerm = string.Empty;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            //if (NavRightButtonIcon == null && BindingContext is ViewModelBase vm)
            //{
            //    SetBinding(NavRightButtonIconProperty, new Binding
            //    {
            //        Path = nameof(ViewModelBase.NotificationIcon)
            //    });
            //}
        }
    }
}

