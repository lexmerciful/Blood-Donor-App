using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Blood_Donor.Adapters;
using Blood_Donor.DataModels;
using Blood_Donor.Fragments;
using Google.Android.Material.FloatingActionButton;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Blood_Donor
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        RecyclerView donorsrecyclerview;
        DonorsAdapter donorsAdapter;
        List<Donor> listofDonors = new List<Donor>();
        FloatingActionButton fab;
        TextView nodonortextview;

        NewDonorFragment newDonorFragment;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("donors", FileCreationMode.Private);
        ISharedPreferencesEditor editor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            donorsrecyclerview = FindViewById<RecyclerView>(Resource.Id.donorsrecyclerview);
            nodonortextview = FindViewById<TextView>(Resource.Id.nodonortextview);

            SupportActionBar.Title = ("Blood Donors");
            fab.Click += Fab_Click;
            //CreateData();
            RetrieveData();
            if(listofDonors.Count > 0)
            {
                SetupRecyclerView();
            }
            else
            {
                nodonortextview.Visibility = Android.Views.ViewStates.Visible;
            }
            editor = pref.Edit();
            
        }

        private void Fab_Click(object sender, System.EventArgs e)
        {
            newDonorFragment = new NewDonorFragment();
            var trans = SupportFragmentManager.BeginTransaction();
            newDonorFragment.Show(trans, "new donor");
            newDonorFragment.onDonorRegistered += NewDonorFragment_onDonorRegistered;
        }

        private void NewDonorFragment_onDonorRegistered(object sender, NewDonorFragment.DonorDetailsEventArgs e)
        {
            if (newDonorFragment != null)
            {
                newDonorFragment.Dismiss();
                newDonorFragment = null;
            }

            if (listofDonors.Count > 0)
            {
                listofDonors.Insert(0, e.Donor);
                donorsAdapter.NotifyItemInserted(0);

                string jsonString = JsonConvert.SerializeObject(listofDonors);
                editor.PutString("donors", jsonString);
                editor.Apply();
            }
            else
            {
                listofDonors.Add(e.Donor);
                string jsonString = JsonConvert.SerializeObject(listofDonors);
                editor.PutString("donors", jsonString);
                editor.Apply();

                SetupRecyclerView();
            }
        }

        void CreateData()
        {
            listofDonors = new List<Donor>();

            listofDonors.Add(new Donor { BloodGroup = "AB+", City = "Delaware", Country = "USA", Email = "lexmerciful@gmail.com", Fullname = "Lexx Merciful", Phone = "08106899810"});
            listofDonors.Add(new Donor { BloodGroup = "0+", City = "Kwara", Country = "Nigeria", Email = "marveyyg@gmail.com", Fullname = "Afolabi Marvellous", Phone = "07038646496" });
            listofDonors.Add(new Donor { BloodGroup = "O-", City = "Lagos", Country = "Nigeria", Email = "Gloryjosh@gmail.com", Fullname = "Afolabi Glory", Phone = "08141848814" });
            listofDonors.Add(new Donor { BloodGroup = "A+", City = "Ibadan", Country = "Nigeria", Email = "dawilfred@gmail.com", Fullname = "Olapade Damilare", Phone = "09040359840" });
            listofDonors.Add(new Donor { BloodGroup = "AB-", City = "Ekiti", Country = "Nigeria", Email = "boluwajiogundola@gmail.com", Fullname = "Ogundola Adeshewa", Phone = "09030067818" });
            listofDonors.Add(new Donor { BloodGroup = "A-", City = "Kwara", Country = "USA", Email = "blessingjesutomi@gmail.com", Fullname = "Afolabi Blessing", Phone = "07068744430" });
        }

        void RetrieveData()
        {
            string json = pref.GetString("donors", "");
            if (!string.IsNullOrEmpty(json))
            {
                listofDonors = JsonConvert.DeserializeObject<List<Donor>>(json);
            }
        }

        void SetupRecyclerView()
        {
            donorsrecyclerview.SetLayoutManager(new LinearLayoutManager(donorsrecyclerview.Context));
            donorsAdapter = new DonorsAdapter(listofDonors);
            donorsAdapter.ItemClick += DonorsAdapter_ItemClick;
            donorsAdapter.CallClick += DonorsAdapter_CallClick;
            donorsAdapter.EmailClick += DonorsAdapter_EmailClick;
            donorsAdapter.DeleteClick += DonorsAdapter_DeleteClick;

            donorsrecyclerview.SetAdapter(donorsAdapter);

            nodonortextview.Visibility = Android.Views.ViewStates.Invisible;
        }

        private void DonorsAdapter_DeleteClick(object sender, DonorsAdapterClickEventArgs e)
        {
            var donor = listofDonors[e.Position];
            AndroidX.AppCompat.App.AlertDialog.Builder DeleteAlert = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
            DeleteAlert.SetMessage("Are you sure ");
            DeleteAlert.SetTitle("Delete Donor");

            DeleteAlert.SetPositiveButton("Delete", (alert, args) =>
            {
                listofDonors.RemoveAt(e.Position);
                donorsAdapter.NotifyItemRemoved(e.Position);

                string jsonString = JsonConvert.SerializeObject(listofDonors);
                editor.PutString("donors", jsonString);
                editor.Apply();
            });

            DeleteAlert.SetNegativeButton("Cancel", (alert, args) =>
            {
                DeleteAlert.Dispose();
            });
            DeleteAlert.Show();
        }

        private void DonorsAdapter_EmailClick(object sender, DonorsAdapterClickEventArgs e)
        {
            var donor = listofDonors[e.Position];
            AndroidX.AppCompat.App.AlertDialog.Builder EmailAlert = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
            EmailAlert.SetMessage("Send mail to " + donor.Fullname);

            EmailAlert.SetPositiveButton("Send", (alert, args) =>
            {
                //Send Email
                Intent intent = new Intent();
                intent.SetType("plain/text");
                intent.SetAction(Intent.ActionSend);
                intent.PutExtra(Intent.ExtraEmail, new string[] { donor.Email });
                intent.PutExtra(Intent.ExtraSubject, "Enquiry on your availability for blood doation");
                StartActivity(intent);
            });

            EmailAlert.SetNegativeButton("Cancel", (alert, args) =>
            {
                EmailAlert.Dispose();
            });
            EmailAlert.Show();
        }

        private void DonorsAdapter_CallClick(object sender, DonorsAdapterClickEventArgs e)
        {
            var donor = listofDonors[e.Position];
            AndroidX.AppCompat.App.AlertDialog.Builder CallAlert = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
            CallAlert.SetMessage("Call " + donor.Fullname);

            CallAlert.SetPositiveButton("Call", (alert, args) =>
            {
                var uri = Android.Net.Uri.Parse("tel:" + donor.Phone);
                var intent = new Intent(Intent.ActionDial, uri);
                StartActivity(intent);
            });

            CallAlert.SetNegativeButton("Cancel", (alert, args) =>
            {
                CallAlert.Dispose();
            });
            CallAlert.Show();
        }

        private void DonorsAdapter_ItemClick(object sender, DonorsAdapterClickEventArgs e)
        {
            Toast.MakeText(this, "Row was clicked", ToastLength.Short).Show();
        }
    }
}