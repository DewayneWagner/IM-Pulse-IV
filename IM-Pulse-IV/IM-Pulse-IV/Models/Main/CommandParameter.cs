using IM_Pulse_IV.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.ObjectModel;
using System.Linq;

namespace IM_Pulse_IV.Models.Main
{
    public class CommandParameter
    {
        public string CommandName { get; set; }
    }
    public class CommandParameterList : ObservableCollection<CommandParameter>
    {
        public CommandParameterList()
        {
            InitListFromDB();
            this.OrderByDescending(c => c.CommandName);
        }
        private void InitListFromDB()
        {
            try
            {
                using (DBContext db = new DBContext())
                {
                    var list = db.CommandParameters.ToList();

                    if(list.Count == 0) { setDefaultCommandParameters(); }
                    else
                    {
                        foreach (CommandParameter cp in list)
                        {
                            this.Add(cp);
                        }
                    }
                }
            }
            catch 
            {
                using (DBContext db = new DBContext())
                {
                    db.DeleteDatabase();
                    db.SaveChanges();
                }
                setDefaultCommandParameters();                
            }
            void setDefaultCommandParameters()
            {
                this.Add(new CommandParameter() { CommandName = "A" });
                this.Add(new CommandParameter() { CommandName = "B" });
                this.Add(new CommandParameter() { CommandName = "C" });
                this.Add(new CommandParameter() { CommandName = "D" });

                using (DBContext db = new DBContext())
                {
                    foreach (CommandParameter commandParameter in this)
                    {
                        db.CommandParameters.Add(commandParameter);
                    }
                    db.SaveChanges();
                }
            }
        }
    }
    public class CommandParameterDBConfig : IEntityTypeConfiguration<CommandParameter>
    {
        public void Configure(EntityTypeBuilder<CommandParameter> builder)
        {
            builder.HasKey(p => p.CommandName);
        }
    }
}
