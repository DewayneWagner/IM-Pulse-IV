using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IM_Pulse_IV.Models.Main
{
    public class ReadData
    {
        public ReadData() { }
        public int ID { get; set; }
        public char Data { get; set; }
        public bool IsCommand { get; set; }
        public int Index { get; set; }
        public int SegmentID { get; set; }
    }
    public class ReadDataDBConfig : IEntityTypeConfiguration<ReadData>
    {
        public void Configure(EntityTypeBuilder<ReadData> builder)
        {
            builder.HasKey(r => r.ID);

            builder.Property(r => r.Data)                
                .IsRequired();

            builder.Property(r => r.Index)
                .IsRequired();

            builder.Property(r => r.SegmentID)
                .IsRequired();
        }
    }
}
