using Xamarin.Forms;

namespace AirMonitor.Views.Controls
{
    public partial class ContentWithHeader : StackLayout
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(ContentWithHeader));

        public static readonly BindableProperty ControlContentProperty = BindableProperty.Create(
            nameof(ControlContent),
            typeof(View),
            typeof(ContentWithHeader));

        public ContentWithHeader()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public View ControlContent
        {
            get => (View) GetValue(ControlContentProperty);
            set => SetValue(ControlContentProperty, value);
        }
    }
}