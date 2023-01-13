using Android.Views;
using Android.Widget;
using System;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using System.Collections.Generic;
using Blood_Donor.DataModels;

namespace Blood_Donor.Adapters
{
    internal class DonorsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<DonorsAdapterClickEventArgs> ItemClick;
        public event EventHandler<DonorsAdapterClickEventArgs> ItemLongClick;

        public event EventHandler<DonorsAdapterClickEventArgs> CallClick;
        public event EventHandler<DonorsAdapterClickEventArgs> EmailClick;
        public event EventHandler<DonorsAdapterClickEventArgs> DeleteClick;


        List<Donor> DonorsList;

        public DonorsAdapter(List<Donor> data)
        {
            DonorsList = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            //var id = Resource.Layout.__YOUR_ITEM_HERE;

            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.donor_row, parent, false);

            var vh = new DonorsAdapterViewHolder(itemView, OnClick, OnLongClick, onCallClick, onEmailClick, onDeleteClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var donor = DonorsList[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as DonorsAdapterViewHolder;
            holder.donornametextview.Text = donor.Fullname;
            holder.donorlocationtextview.Text = donor.City + ", " + donor.Country;

            //Assign appropriate images to Donors blood group
            if(donor.BloodGroup == "O+")
            {
                holder.bloodgroupimageview.SetImageResource(Resource.Drawable.o_positive);
            }

            else if(donor.BloodGroup == "A+")
            {
                holder.bloodgroupimageview.SetImageResource(Resource.Drawable.a_positive);
            }

            else if (donor.BloodGroup == "A-")
            {
                holder.bloodgroupimageview.SetImageResource(Resource.Drawable.a_negative);
            }

            else if (donor.BloodGroup == "AB+")
            {
                holder.bloodgroupimageview.SetImageResource(Resource.Drawable.ab_positive);
            }

            else if (donor.BloodGroup == "AB-")
            {
                holder.bloodgroupimageview.SetImageResource(Resource.Drawable.ab_negative);
            }

            else if (donor.BloodGroup == "B+")
            {
                holder.bloodgroupimageview.SetImageResource(Resource.Drawable.b_positive);
            }

            else if (donor.BloodGroup == "B-")
            {
                holder.bloodgroupimageview.SetImageResource(Resource.Drawable.b_negative);
            }

            else if (donor.BloodGroup == "O-")
            {
                holder.bloodgroupimageview.SetImageResource(Resource.Drawable.o_negative);
            }
        }

        public override int ItemCount => DonorsList.Count;

        void OnClick(DonorsAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(DonorsAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

        void onCallClick(DonorsAdapterClickEventArgs args) => CallClick?.Invoke(this, args);
        void onEmailClick(DonorsAdapterClickEventArgs args) => EmailClick?.Invoke(this, args);
        void onDeleteClick(DonorsAdapterClickEventArgs args) => DeleteClick?.Invoke(this, args);

    }

    public class DonorsAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }

        public TextView donornametextview;
        public TextView donorlocationtextview;
        public ImageView bloodgroupimageview;
        public RelativeLayout calllayout;
        public RelativeLayout emaillayout;
        public RelativeLayout deletelayout;


        public DonorsAdapterViewHolder(View itemView, Action<DonorsAdapterClickEventArgs> clickListener,
                            Action<DonorsAdapterClickEventArgs> longClickListener, Action<DonorsAdapterClickEventArgs> callClickListener,
                            Action<DonorsAdapterClickEventArgs> emailClickListener,
                            Action<DonorsAdapterClickEventArgs> deleteClickListener) : base(itemView)
        {
            //TextView = v;
            donornametextview = (TextView)itemView.FindViewById(Resource.Id.donornametextview);
            donorlocationtextview = (TextView)itemView.FindViewById(Resource.Id.donorlocationtextview);
            bloodgroupimageview = (ImageView)itemView.FindViewById(Resource.Id.bloodgroupimageview);
            emaillayout = (RelativeLayout)itemView.FindViewById(Resource.Id.emaillayout);
            deletelayout = (RelativeLayout)itemView.FindViewById(Resource.Id.deletelayout);
            calllayout = (RelativeLayout)itemView.FindViewById(Resource.Id.calllayout);

            itemView.Click += (sender, e) => clickListener(new DonorsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new DonorsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            calllayout.Click += (sender, e) => callClickListener(new DonorsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            emaillayout.Click += (sender, e) => emailClickListener(new DonorsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            deletelayout.Click += (sender, e) => deleteClickListener(new DonorsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class DonorsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}