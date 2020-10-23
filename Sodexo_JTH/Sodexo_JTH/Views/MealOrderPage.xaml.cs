using Rg.Plugins.Popup.Extensions;
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Models;
using Sodexo_JTH.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sodexo_JTH.Views
{
    public partial class MealOrderPage : ContentPage
    {
        MealOrderPageViewModel _viewModel;
        public MealOrderPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as MealOrderPageViewModel;
            if (Device.RuntimePlatform == Device.iOS)
            {
                NavigationPage.SetHasBackButton(this, true);
            }
            else if(Device.RuntimePlatform == Device.Android)
                NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, true);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            segment.Children = new System.Collections.Generic.List<string>(_viewModel.MstrMeals.Select(x => x.meal_name));
        }

        private async void Addremovebtn_Clicked(object sender, System.EventArgs e)
        {
            await _viewModel.AddOrRemoveMenuItem(((Button)sender).BindingContext as MenuItemClass);
        }
        
        private void MealTime_Tapped(object sender, System.EventArgs e)
        {
            var MealTime =((StackLayout)sender).BindingContext  as mstr_meal_time;
            if (MealTime.IsTapped)
            {
               
            }
            else
            {
                var checkedMealTime = _viewModel.MstrMeals.Where(x => x.IsTapped && x.meal_name != MealTime.meal_name);
                if (checkedMealTime.Any())
                {
                    checkedMealTime.First().IsTapped = false;
                    MealTime.IsTapped = true;
                }
                else
                    MealTime.IsTapped = true;

                _viewModel.SetCutOffTime(MealTime);
            }
                
        }

      
      

        private void TapGestureRecognizer_Tapped_1(object sender, System.EventArgs e)
        {
            if (_viewModel.IsMenuEnable)
            {
                SelectElement((Frame)sender);
            }
            
        }
        
        private async void SelectElement(Frame frame)
        {
            
            var stack = (frame.Children.Where(p => p is StackLayout).First() as StackLayout);

            if (_viewModel._lastElementSelectedFrame != null)
            {
                VisualStateManager.GoToState(_viewModel._lastElementSelectedFrame, "UnSelected");
                VisualStateManager.GoToState(_viewModel._lastElementSelectedImage, "UnSelected");
                VisualStateManager.GoToState(_viewModel._lastElementSelectedLabel, "UnSelected");
            }

            VisualStateManager.GoToState(frame, "Selected");
            VisualStateManager.GoToState(stack.Children.First(x => x is Label) as Label, "Selected");
            VisualStateManager.GoToState(stack.Children.First(x => x is Image) as Image, "Selected");

            _viewModel._lastElementSelectedFrame = frame;

            _viewModel._lastElementSelectedImage = stack.Children.First(x => x is Image) as Image;
            _viewModel._lastElementSelectedLabel = stack.Children.First(x => x is Label) as Label;


          
            
            _viewModel._lastElementSelectedFrame = frame;
            _viewModel.SelectedMenuCategory = frame.BindingContext as mstr_menu_item_category;

           await Task.Run(async () =>
            {
               await _viewModel.SetMenuCategories(_viewModel.SelectedMenuCategory);
            });
        }

        private async void NxtBtn_Clicked(object sender, EventArgs e)
        {
            string param = ((Button)sender).CommandParameter.ToString();
            if (param.ToLower().Contains("NEXT".ToLower()))
            {
                if (_viewModel.others.ID == 1 || _viewModel.others.ID == 9 || _viewModel.others.ID == 10)
                {
                    await _viewModel.NavigateToMealSummary();
                }
                else
                {
                    var enabledView = menuCategories.Children.Where(p => p is Frame).Where(x => x.IsEnabled).First();
                    SelectElement(enabledView as Frame);
                }
            }
            else
                _viewModel.PlaceOrder();
        }

        private void MstrMeals_Tapped(object sender, EventArgs e)
        {
            SelectUnSelectMeal((StackLayout)sender);
        }

        private void SelectUnSelectMeal(StackLayout stacklayout)
        {
            var stack = (stacklayout.Children.Where(p => p is StackLayout).First() as StackLayout);
            Label currentLabel = stack.Children.First(x => x is Label) as Label;
            BoxView currentBoxView = stack.Children.First(x => x is BoxView) as BoxView;

            if (_viewModel._lastElementSelectedMealLabel != null)
            {
                VisualStateManager.GoToState(_viewModel._lastElementSelectedMealLabel, "UnSelected");
                VisualStateManager.GoToState(_viewModel._lastElementSelectedBoxView, "UnSelected");
            }

            VisualStateManager.GoToState(stacklayout, "Selected");
            VisualStateManager.GoToState(currentLabel, "Selected");
            VisualStateManager.GoToState(currentBoxView, "Selected");

            _viewModel._lastElementSelectedMealLabel = currentLabel;
            _viewModel._lastElementSelectedBoxView = currentBoxView;

            mealTimeSelectionChanged(stack.BindingContext as mstr_meal_time);
        }

        private void mealTimeSelectionChanged(mstr_meal_time meal_Time)
        {
            //_viewModel.FilterItemsOnMealTime(meal_Time);

            _viewModel.ReloadMenuCategory();
        }
        private void mealtime_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            _viewModel.FilterItemsOnMealTime(e.SelectedItem.ToString());

            _viewModel.ReloadMenuCategory();

        }

       

        private void LangPicker_SelectionChanged(object sender, Events.EventArgs<string> e)
        {
            Library.KEY_USER_LANGUAGE = e.Value;
        }

        private async void OnCuisineTapped(object sender, EventArgs e)
        {

            var view = (Frame)sender;
            view.Opacity = 0;
            await view.FadeTo(1, 250);
            var cuisinePopUp = new Sodexo_JTH.PopUpControl.CuisinePopUp
            {
                BindingContext = _viewModel
            };

            cuisinePopUp.cancel_Clicked += (s, e2) =>
            {
                _viewModel.ResetCuisineSelection();
            };

            cuisinePopUp.submit_Clicked += (s, e1) =>
            {
                _viewModel.OnCuisineSelected();
            };

            await Navigation.PushPopupAsync(cuisinePopUp, true);
        }
    }
}
