using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Phoneword
{
    public class MainPage : ContentPage
    {
        Entry entry;
        Button translateButton;
        Button callButton;
        string translatedNumber;

        public MainPage()
        {
            this.Padding = new Thickness(20, 20, 20, 20);

            Label label = new Label()
            {
                Text = "Enter a Phoneword:",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };

            entry = new Entry()
            {
                Text = "1-855-XAMARIN"
            };
            translateButton = new Button()
            {
                Text = "Translate"
            };
            translateButton.Clicked += OnTranslate;

            callButton = new Button()
            {
                Text = "Call",
                IsEnabled = false
            };
            callButton.Clicked += OnCall;
            Content = new StackLayout
            {
                Children = { 
                    label, 
                    entry,
                    translateButton ,
                    callButton

                },
                Spacing = 15,
            };

        }
        async void OnCall(object sender, EventArgs e)
        {
            if(await this.DisplayAlert(
                "Dial a Number",
                "Would you like to call " + translatedNumber,
                "Yes",
                "No"
                ))
            {
                try
                {
                    PhoneDialer.Open(translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing not supported.", "OK");
                }
                catch (Exception)
                {

                    // Other error has occurred.
                    await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
                }
            }
        }
        void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = entry.Text;

            translatedNumber = PhonewordTranslator.ToNumber(enteredNumber);

            if(!string.IsNullOrEmpty(translatedNumber))
            {
                callButton.Text = "Call " + translatedNumber;
                callButton.IsEnabled = true;
            }
            else
            {
                callButton.Text = "Call";
                callButton.IsEnabled = false;
            }

        }
    }
}