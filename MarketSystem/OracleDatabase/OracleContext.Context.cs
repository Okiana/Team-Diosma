﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MarketSystem.OracleDatabase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OracleContext : DbContext
    {
        public OracleContext()
            : base("name=OracleContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<MEASURE> MEASURES { get; set; }
        public virtual DbSet<PRODUCT> PRODUCTS { get; set; }
        public virtual DbSet<PRODUCTSTYPE> PRODUCTSTYPES { get; set; }
        public virtual DbSet<VENDOR> VENDORS { get; set; }
        public virtual DbSet<SUPERMARKET> SUPERMARKETS { get; set; }
        public virtual DbSet<TOWN> TOWNS { get; set; }
    }
}
