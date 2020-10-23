using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sodexo_JTH.PopUpControl
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupTitleView : ContentView
    {
        public event EventHandler<EventArgs> Add;
        public event EventHandler<EventArgs> Close;

        #region IsVisible
        public static readonly BindableProperty VisibilityProperty = BindableProperty.Create(
       nameof(Visibility),
       typeof(bool),
       typeof(PopupTitleView),
       false,
       BindingMode.TwoWay);

        public bool Visibility
        {
            get => (bool)GetValue(VisibilityProperty);
            set { SetValue(VisibilityProperty, value); }
        }

        #endregion

        #region Text
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
       nameof(Text),
       typeof(string),
       typeof(PopupTitleView),
       null,
       BindingMode.TwoWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set { SetValue(TextProperty, value.ToUpper()); }
        }

        #endregion

        #region AddText
        public static readonly BindableProperty AddTextProperty = BindableProperty.Create(
       nameof(AddText),
       typeof(string),
       typeof(PopupTitleView),
       null,
       BindingMode.TwoWay,
       propertyChanged: AddTextPropertyChanged);

        private static void AddTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!((bindable as PopupTitleView).AddLbl).IsVisible)
                return;

            ((bindable as PopupTitleView).AddLbl).IsVisible = string.IsNullOrEmpty(newValue.ToString()) ? false : true;
        }

        public string AddText
        {
            get => (string)GetValue(AddTextProperty);
            set => SetValue(AddTextProperty, value);
        }

        #endregion

        TapGestureRecognizer AddTap;
        TapGestureRecognizer CloseTap;

        public PopupTitleView()
        {
            InitializeComponent();
            AddTap = new TapGestureRecognizer();
            AddTap.Tapped += (s, e) => { Add?.Invoke(s, e); };
            AddLbl.GestureRecognizers.Add(AddTap);

            //CloseTap = new TapGestureRecognizer();
            //CloseTap.Tapped += (s, e) => { Close?.Invoke(s, e); };
            //CloseLbl.GestureRecognizers.Add(CloseTap);
            Title.SetBinding(Label.TextProperty, new Binding(nameof(this.Text), BindingMode.TwoWay, source: this));
            AddLbl.SetBinding(Label.TextProperty, new Binding(nameof(this.AddText), BindingMode.TwoWay, source: this));
            AddLbl.SetBinding(Label.IsVisibleProperty, new Binding(nameof(this.Visibility), BindingMode.TwoWay, source: this));
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Close?.Invoke(sender, e);
        }
    }
}