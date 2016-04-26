using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsadWebsite.App_Code
{
    public enum Data : byte
    {
        Measurements, Operators, Organisations, Patients
    }

    // Swapped for Data enum
    //public enum Groups : byte
    //{
    //    Patients, Operators, Organisations
    //}
    
    public enum Gender : byte
    {
        Male, Female
    }

    public enum Status : byte
    {
        Active, Inactive, Pending
    }

    
}