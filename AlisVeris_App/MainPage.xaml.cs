using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
 
using Microsoft.Phone.Data.Linq;
using AlisVeris_App.Model;

namespace AlisVeris_App
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string ConnectionString = @"isostore:/AlisVerisDB.sdf";

        public MainPage()
        {
            InitializeComponent();

        }

        public void AlisVerisYazdir()
        {
            using (AlisVerisDataContext context = new AlisVerisDataContext(ConnectionString))
            {
                this.AlisVerisYazdir(context);
            }
        }

        public void AlisVerisYazdir(AlisVerisDataContext context)
        {
            AlisVeris av = new AlisVeris()
            {
                Alinacak = string.Format("{0} Adet {1}  ", textBox2.Text, textBox1.Text),

            };
            context.AlisVeris.InsertOnSubmit(av);
            context.SubmitChanges();
        }

        public IEnumerable<AlisVeris> AlisVerisOku()
        {
            IEnumerable<AlisVeris> av = null;
            using (AlisVerisDataContext context = new AlisVerisDataContext(ConnectionString))
            {
                av = context.AlisVeris.ToList();
            }
            return av;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            using (AlisVerisDataContext context = new AlisVerisDataContext(ConnectionString))
            {
                if (!context.DatabaseExists())
                {
                    context.CreateDatabase();
                    this.AlisVerisYazdir(context);
                }
                else
                {
                    DatabaseSchemaUpdater schemaUpdater = context.CreateDatabaseSchemaUpdater();
                    int version = schemaUpdater.DatabaseSchemaVersion;
                    if (version == 0)
                    {
                        this.AlisVerisYazdir(context);
                        //schemaUpdater.AddColumn<AlisVeris>("Address");
                        //schemaUpdater.AddColumn<AlisVeris>("Email");
                        //schemaUpdater.DatabaseSchemaVersion = 1;
                        schemaUpdater.Execute();
                    }
                }
            }

            //listBox1.Items.Clear();
           this.listBox1.ItemsSource = this.AlisVerisOku();
           textBox1.Text = "";
           textBox2.Text = "";
        
        }

        //private void button3_Click(object sender, RoutedEventArgs e)   //veritabanını silme butonu
        //{
        //    using (AlisVerisDataContext context = new AlisVerisDataContext(ConnectionString))
        //    {
        //        if (context.DatabaseExists())
        //        {
        //            context.DeleteDatabase();
        //        }
        //    }
        //}

        private void ContentPanel_Loaded(object sender, RoutedEventArgs e)
        {
            using (AlisVerisDataContext context = new AlisVerisDataContext(ConnectionString))
            {
                if (context.DatabaseExists())
                {
                    this.listBox1.ItemsSource = this.AlisVerisOku();

                }
                else
                { this.listBox1.Items.Clear(); }
            }


        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (AlisVerisDataContext context = new AlisVerisDataContext(ConnectionString))
                {

                    IQueryable<AlisVeris> alverSorgu = from av in context.AlisVeris
                                                       where av.Alinacak == this.listBox1.SelectedItem.ToString()
                                                       select av;

                    AlisVeris alverRemove = alverSorgu.FirstOrDefault();
                    context.AlisVeris.DeleteOnSubmit(alverRemove);
                    context.SubmitChanges();

                    this.listBox1.ItemsSource = this.AlisVerisOku();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lütfen sileceğiniz maddeyi seçiniz...");
            
            }
             
        }
    }
}