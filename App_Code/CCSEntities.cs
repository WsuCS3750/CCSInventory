﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public partial class Address
{
    public Address()
    {
        this.Agencies = new HashSet<Agency>();
        this.FoodSources = new HashSet<FoodSource>();
    }

    public short AddressID { get; set; }
    public string StreetAddress1 { get; set; }
    public string StreetAddress2 { get; set; }
    public Nullable<short> CityID { get; set; }
    public Nullable<short> StateID { get; set; }
    public Nullable<short> ZipID { get; set; }

    public virtual City City { get; set; }
    public virtual State State { get; set; }
    public virtual Zipcode Zipcode { get; set; }
    public virtual ICollection<Agency> Agencies { get; set; }
    public virtual ICollection<FoodSource> FoodSources { get; set; }
}

public partial class Adjustment
{
    public int AdjustmentID { get; set; }
    public Nullable<decimal> Weight { get; set; }
    public string FoodCategory { get; set; }
    public string Location { get; set; }
    public Nullable<bool> isUSDA { get; set; }
    public string USDANumber { get; set; }
    public Nullable<short> Cases { get; set; }
    public short AuditID { get; set; }

    public virtual Audit Audit { get; set; }
}

public partial class Agency
{
    public Agency()
    {
        this.FoodOuts = new HashSet<FoodOut>();
    }

    public short AgencyID { get; set; }
    public string AgencyName { get; set; }
    public Nullable<short> AddressID { get; set; }

    public virtual Address Address { get; set; }
    public virtual ICollection<FoodOut> FoodOuts { get; set; }
}

public partial class Audit
{
    public Audit()
    {
        this.Adjustments = new HashSet<Adjustment>();
    }

    public short AuditID { get; set; }
    public System.DateTime Date { get; set; }
    public short UserID { get; set; }

    public virtual ICollection<Adjustment> Adjustments { get; set; }
    public virtual User User { get; set; }
}

public partial class City
{
    public City()
    {
        this.Addresses = new HashSet<Address>();
    }

    public short CityID { get; set; }
    public string CityName { get; set; }

    public virtual ICollection<Address> Addresses { get; set; }
}

public partial class Container
{
    public short ContainerID { get; set; }
    public short BinNumber { get; set; }
    public decimal Weight { get; set; }
    public Nullable<bool> isUSDA { get; set; }
    public Nullable<short> USDAID { get; set; }
    public Nullable<short> FoodCategoryID { get; set; }
    public short LocationID { get; set; }
    public Nullable<short> Cases { get; set; }
    public Nullable<short> FoodSourcesTypeID { get; set; }
    public System.DateTime DateCreated { get; set; }

    public virtual FoodCategory FoodCategory { get; set; }
    public virtual FoodSourceType FoodSourceType { get; set; }
    public virtual Location Location { get; set; }
    public virtual USDACategory USDACategory { get; set; }
}

public partial class DistributionType
{
    public DistributionType()
    {
        this.FoodOuts = new HashSet<FoodOut>();
    }

    public short DistributionTypeID { get; set; }
    public string DistributionType1 { get; set; }
    public bool ToClients { get; set; }

    public virtual ICollection<FoodOut> FoodOuts { get; set; }
}

public partial class ErrorLog
{
    public int ErrorLogID { get; set; }
    public string FileName { get; set; }
    public System.DateTime TimeStamp { get; set; }
    public string FunctionName { get; set; }
    public string LineNumber { get; set; }
    public string ErrorText { get; set; }
}

public partial class FoodCategory
{
    public FoodCategory()
    {
        this.Containers = new HashSet<Container>();
        this.FoodIns = new HashSet<FoodIn>();
        this.FoodOuts = new HashSet<FoodOut>();
    }

    public short FoodCategoryID { get; set; }
    public string CategoryType { get; set; }
    public bool Perishable { get; set; }
    public bool NonFood { get; set; }
    public Nullable<decimal> CaseWeight { get; set; }

    public virtual ICollection<Container> Containers { get; set; }
    public virtual ICollection<FoodIn> FoodIns { get; set; }
    public virtual ICollection<FoodOut> FoodOuts { get; set; }
}

public partial class FoodIn
{
    public short FoodInID { get; set; }
    public System.DateTime TimeStamp { get; set; }
    public decimal Weight { get; set; }
    public Nullable<short> Count { get; set; }
    public short FoodSourceID { get; set; }
    public Nullable<short> FoodCategoryID { get; set; }
    public Nullable<short> USDAID { get; set; }

    public virtual FoodCategory FoodCategory { get; set; }
    public virtual FoodSource FoodSource { get; set; }
    public virtual USDACategory USDACategory { get; set; }
}

public partial class FoodOut
{
    public short DistributionID { get; set; }
    public double Weight { get; set; }
    public short FoodSourceTypeID { get; set; }
    public short DistributionTypeID { get; set; }
    public Nullable<short> Count { get; set; }
    public Nullable<short> USDAID { get; set; }
    public Nullable<short> FoodCategoryID { get; set; }
    public Nullable<short> AgencyID { get; set; }
    public short BinNumber { get; set; }
    public System.DateTime DateCreated { get; set; }
    public System.DateTime TimeStamp { get; set; }

    public virtual Agency Agency { get; set; }
    public virtual DistributionType DistributionType { get; set; }
    public virtual FoodCategory FoodCategory { get; set; }
    public virtual FoodSourceType FoodSourceType { get; set; }
    public virtual USDACategory USDACategory { get; set; }
}

public partial class FoodSource
{
    public FoodSource()
    {
        this.FoodIns = new HashSet<FoodIn>();
    }

    public short FoodSourceID { get; set; }
    public string Source { get; set; }
    public string StoreID { get; set; }
    public short FoodSourceTypeID { get; set; }
    public Nullable<short> AddressID { get; set; }

    public virtual Address Address { get; set; }
    public virtual ICollection<FoodIn> FoodIns { get; set; }
    public virtual FoodSourceType FoodSourceType { get; set; }
}

public partial class FoodSourceType
{
    public FoodSourceType()
    {
        this.Containers = new HashSet<Container>();
        this.FoodSources = new HashSet<FoodSource>();
        this.FoodOuts = new HashSet<FoodOut>();
    }

    public short FoodSourceTypeID { get; set; }
    public string FoodSourceType1 { get; set; }

    public virtual ICollection<Container> Containers { get; set; }
    public virtual ICollection<FoodSource> FoodSources { get; set; }
    public virtual ICollection<FoodOut> FoodOuts { get; set; }
}

public partial class Location
{
    public Location()
    {
        this.Containers = new HashSet<Container>();
    }

    public short LocationID { get; set; }
    public string RoomName { get; set; }

    public virtual ICollection<Container> Containers { get; set; }
}

public partial class Log
{
    public int LogID { get; set; }
    public string Description { get; set; }
    public System.DateTime Date { get; set; }
    public short UserID { get; set; }

    public virtual User User { get; set; }
}

public partial class State
{
    public State()
    {
        this.Addresses = new HashSet<Address>();
    }

    public short StateID { get; set; }
    public string StateFullName { get; set; }
    public string StateShortName { get; set; }

    public virtual ICollection<Address> Addresses { get; set; }
}

public partial class Template
{
    public int TemplateID { get; set; }
    public string TemplateName { get; set; }
    public string Data { get; set; }
    public short TemplateType { get; set; }
    public System.DateTime LastUpdated { get; set; }
    public string LastUpdatedBy { get; set; }
}

public partial class USDACategory
{
    public USDACategory()
    {
        this.Containers = new HashSet<Container>();
        this.FoodIns = new HashSet<FoodIn>();
        this.FoodOuts = new HashSet<FoodOut>();
    }

    public short USDAID { get; set; }
    public string Description { get; set; }
    public string USDANumber { get; set; }
    public Nullable<decimal> CaseWeight { get; set; }

    public virtual ICollection<Container> Containers { get; set; }
    public virtual ICollection<FoodIn> FoodIns { get; set; }
    public virtual ICollection<FoodOut> FoodOuts { get; set; }
}

public partial class User
{
    public User()
    {
        this.Logs = new HashSet<Log>();
        this.Audits = new HashSet<Audit>();
    }

    public short UserID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public bool Admin { get; set; }
    public string Password { get; set; }

    public virtual ICollection<Log> Logs { get; set; }
    public virtual ICollection<Audit> Audits { get; set; }
}

public partial class Zipcode
{
    public Zipcode()
    {
        this.Addresses = new HashSet<Address>();
    }

    public short ZipID { get; set; }
    public string ZipCode1 { get; set; }

    public virtual ICollection<Address> Addresses { get; set; }
}
