using Android.App;
using Android.Widget;
using Android.OS;

namespace Lab11
{
    [Activity(Label = "Lab11", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Complex Data;
        int Counter = 0;
        string Result = "";

        protected override void OnCreate(Bundle bundle)
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnCreate");

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.StartActivity).Click += (sender, e) => 
            {
                var ActivityIntent = new Android.Content.Intent(this, typeof(SecondActivity));
                StartActivity(ActivityIntent);
            };

            var TextResult = FindViewById<TextView>(Resource.Id.ResultView);
            string EMail = "";
            string Password = "";
            string Device = Android.Provider.Settings.Secure.GetString(
                ContentResolver, Android.Provider.Settings.Secure.AndroidId);

            // Utilizar FragmentManager para recuperar el Fragmento
            Data = (Complex)this.FragmentManager.FindFragmentByTag("Data");
            if (Data == null)
            {
                // No ha sido almacenado, agregar el fragmento a la Activity
                Data = new Complex();
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(Data, "Data");
                FragmentTransaction.Commit();
            }
            // persistencia hecha con bundle
            if (bundle != null)
            {
                Counter = bundle.GetInt("CounterValue", 0);
                Result = bundle.GetString("ResultValue", "");
                Android.Util.Log.Debug("Lab11Log", "Activity A - Recovered Instance State");
            }
            // variable global Result declarada inicialmente con "" para preguntar por este valor
            // y así verificar que se ejecute sólo una vez
            if (Result == "")
            {
                Validate();
            }

            async void Validate()
            {
                var ServiceClient = new SALLab11.ServiceClient();
                var SvcResult = await ServiceClient.ValidateAsync(EMail, Password, Device);

                Result = $"{SvcResult.Status}\n{SvcResult.Fullname}\n{SvcResult.Token}";
                TextResult.Text = Result;
            }
            TextResult.Text = Result;

            var ClickCounter = FindViewById<Button>(Resource.Id.ClicksCounter);
            ClickCounter.Text = Resources.GetString(Resource.String.ClicksCounter_Text, Counter);
            ClickCounter.Text += $"\n{Data.ToString()}";
            ClickCounter.Click += (sender, e) =>
            {
                Counter++;
                ClickCounter.Text = Resources.GetString(Resource.String.ClicksCounter_Text, Counter);

                // Modificar con cualquier valor solo para verificar la persistencia
                Data.Real++;
                Data.Imaginary++;
                // Mostrar el valor de los miembros
                ClickCounter.Text += $"\n{Data.ToString()}";
            };
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("CounterValue", Counter);
            outState.PutString("ResultValue", Result); // Guarda Resultado de validación en Bundle
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnSaveInstanceState");
            base.OnSaveInstanceState(outState);
        }

        protected override void OnStart()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnStart");
            base.OnStart();
        }

        protected override void OnResume()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnResume");
            base.OnResume();
        }

        protected override void OnPause()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnPause");
            base.OnPause();
        }

        protected override void OnStop()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnStop");
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnDestroy");
            base.OnDestroy();
        }

        protected override void OnRestart()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnRestart");
            base.OnRestart();
        }
    }
}

