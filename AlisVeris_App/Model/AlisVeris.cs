using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq;


namespace AlisVeris_App.Model
{
    [Table(Name = "AlisVeris")]
    public class AlisVeris
    { 
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID
        {
            get;
            set;
        }

        [Column(CanBeNull = false)]
        public string Alinacak
        {
            get;
            set;
        }

         
        public override string ToString()
        { 
            return string.Format("{0}",
                this.Alinacak); 
        }

    }
}
