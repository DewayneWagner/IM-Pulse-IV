using IM_Pulse_IV.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IM_Pulse_IV.Models.Main
{
    public class UniqueTextParameter
    {
        public UniqueTextParameter() { }
        public string Text { get; set; }
        public string Command { get; set; }
        public static void InitDB()
        {
            int totalNumberOfUniqueTextParameters = 100;

            using (DBContext db = new DBContext())
            {                
                var currentValues = db.UniqueTextParameters.ToList();
                db.RemoveRange(currentValues);

                db.AddRange(getListOfValues());
                db.SaveChanges();
            }

            List<UniqueTextParameter> getListOfValues()
            {
                List<UniqueTextParameter> values = new List<UniqueTextParameter>();
                int index = 33;
                char charValue;
                string stringValue;

                do
                {
                    charValue = Convert.ToChar(index);
                    stringValue = charValue.ToString();

                    if(stringValue.Length == 1 && !values.Any(v => v.Text == stringValue)) 
                    { 
                        values.Add(new UniqueTextParameter() { Text = stringValue }); 
                    }

                    index = getNextIndex(index);

                } while (values.Count < totalNumberOfUniqueTextParameters);

                return values;
            }
            int getNextIndex(int currentIndex)
            {
                currentIndex++;
                if(currentIndex >= 127 && currentIndex <= 160) { return 161; }
                else { return currentIndex; }
            }
        }
    }
    public class UniqueTextParameterDBConfig : IEntityTypeConfiguration<UniqueTextParameter>
    {
        public void Configure(EntityTypeBuilder<UniqueTextParameter> builder)
        {
            builder.HasKey(u => u.Text);
        }
    }
}
