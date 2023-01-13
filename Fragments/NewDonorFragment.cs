using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Blood_Donor.DataModels;
using Google.Android.Material.TextField;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_Donor.Fragments
{
    public class NewDonorFragment : AndroidX.Fragment.App.DialogFragment
    {
        TextInputLayout fullnametext, phonetext, emailtext, citytext, countrytext;
        Spinner materialspinner;
        Button savebutton;

        List<string> bloodGroupsList;
        ArrayAdapter<string> spinneradapter;
        string selectedBloodGroup;

        public event EventHandler<DonorDetailsEventArgs> onDonorRegistered;
        public class DonorDetailsEventArgs : EventArgs
        {
            public Donor Donor { get; set; }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.addnew, container, false);
            ConnectViews(view);
            SetupSpinner();
            return view;

        }

        void ConnectViews(View view)
        {
            fullnametext = (TextInputLayout)view.FindViewById<TextInputLayout>(Resource.Id.fullnametext);
            phonetext = (TextInputLayout)view.FindViewById<TextInputLayout>(Resource.Id.phonetext);
            emailtext = (TextInputLayout)view.FindViewById<TextInputLayout>(Resource.Id.emailtext);
            citytext = view.FindViewById<TextInputLayout>(Resource.Id.citytext);
            countrytext = (TextInputLayout)view.FindViewById<TextInputLayout>(Resource.Id.countrytext);
            savebutton = (Button)view.FindViewById<Button>(Resource.Id.savebutton);
            materialspinner = (Spinner)view.FindViewById(Resource.Id.materialspinner);

            savebutton.Click += Savebutton_Click;

        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            string fullname, email, phone, city, country;

            fullname = fullnametext.EditText.Text;
            phone = phonetext.EditText.Text;
            email = emailtext.EditText.Text;
            city = citytext.EditText.Text;
            country = countrytext.EditText.Text;

            if (fullname.Length < 5)
            {
                Toast.MakeText(Activity, "Please Provide a valid name", ToastLength.Short).Show();
                return;
            }

            else if (!email.Contains("@"))
            {
                Toast.MakeText(Activity, "Please Provide a valid email address", ToastLength.Short).Show();
                return;
            }

            else if (phone.Length < 10 || phone.Length > 15)
            {
                Toast.MakeText(Activity, "Please Provide a valid phone number", ToastLength.Short).Show();
                return;
            }

            else if (city.Length < 2)
            {
                Toast.MakeText(Activity, "Please Provide a valid city", ToastLength.Short).Show();
                return;
            }

            else if (country.Length < 2)
            {
                Toast.MakeText(Activity, "Please Provide a valid country", ToastLength.Short).Show();
                return;
            }

            else if (selectedBloodGroup.Length < 2)
            {
                Toast.MakeText(Activity, "Please Provide a valid blood group", ToastLength.Short).Show();
                return;
            }

            Donor donor = new Donor();
            donor.Fullname = fullname;
            donor.Phone = phone;
            donor.Email = email;
            donor.City = city;
            donor.Country = country;
            donor.BloodGroup = selectedBloodGroup;

            onDonorRegistered?.Invoke(this, new DonorDetailsEventArgs { Donor = donor });
        }

        void SetupSpinner()
        {
            bloodGroupsList = new List<string>();
            bloodGroupsList.Add("A+");
            bloodGroupsList.Add("A-");
            bloodGroupsList.Add("AB+");
            bloodGroupsList.Add("AB-");
            bloodGroupsList.Add("B+");
            bloodGroupsList.Add("B-");
            bloodGroupsList.Add("O+");
            bloodGroupsList.Add("O-");

            spinneradapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem, bloodGroupsList);
            spinneradapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            materialspinner.Adapter = spinneradapter;
            materialspinner.ItemSelected += Materialspinner_ItemSelected;
        }

        private void Materialspinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position != -1)
            {
                selectedBloodGroup = bloodGroupsList[e.Position];
                Console.WriteLine(selectedBloodGroup);
            }


        }


    }
}