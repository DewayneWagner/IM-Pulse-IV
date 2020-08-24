using IM_Pulse_IV.Models.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IM_Pulse_IV.Models.RandomDataGenerator
{
    public class RandomDataStats
    {        
        public RandomDataStats() { }
        public int ID { get; set; }
        public string CommandParameter { get; set; }
        public int Index { get; set; }
        public bool IsReadVsInsert { get; set; }
        public int SegmentID { get; set; }
    }
    public class RandomDataStatsDBConfig : IEntityTypeConfiguration<RandomDataStats>
    {
        public void Configure(EntityTypeBuilder<RandomDataStats> builder)
        {
            builder.HasKey(r => r.ID);
        }
    }
}
