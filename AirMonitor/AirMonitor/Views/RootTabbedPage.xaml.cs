using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirMonitor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootTabbedPage : TabbedPage
    {
        public RootTabbedPage()
        {
            InitializeComponent();
        }
    }
}