//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resilience.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            this.DiaryEntries = new HashSet<DiaryEntries>();
            this.Goals = new HashSet<Goals>();
        }
    
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a first name")]               
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a last name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> MentorId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiaryEntries> DiaryEntries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Goals> Goals { get; set; }

        public int getOpenGoalsCount()
        {
            int open = 0;
            
            foreach(Goals g in this.Goals)
            {
                if(DateTime.Compare(g.CompletionDate.Date, new DateTime(1990, 2, 10).Date) == 0)
                {
                    open++;
                }
            }

            return open;
        }

        public int getDelayedTasksCount()
        {
            int dtasks = 0;

            foreach (Goals g in this.Goals)
            {
                if (DateTime.Compare(g.CompletionDate.Date, new DateTime(1990, 2, 10).Date) == 0)
                {
                    foreach(Exercise e in g.Exercises)
                    {
                        if(DateTime.Compare(e.CompletionDate.Date, new DateTime(1990, 2, 10).Date) == 0 && DateTime.Compare(DateTime.Now.Date, e.DueDate.Date) > 0)
                        {
                            dtasks++;
                        }
                    }
                }
            }

            return dtasks;
        }

        public string getLastDiaryDate()
        {
            string lastDiaryDate = "";

            foreach (DiaryEntries d in this.DiaryEntries)
            {
                lastDiaryDate = d.Date.ToShortDateString();
            }
            return lastDiaryDate;
        }

        public int getLastDiaryMood()
        {
            int lastDiaryMood = 0;

            foreach (DiaryEntries d in this.DiaryEntries)
            {
                lastDiaryMood = d.MenteeFeedback.Value;
            }

            return lastDiaryMood;
        }

        public bool getLastDiaryReviewed()
        {
            bool lastDiaryState = false;
            foreach (DiaryEntries d in this.DiaryEntries)
            {
                lastDiaryState = d.MentorFeedback != null;
            }
            return lastDiaryState;
        }

    }

    public class AddMentor
    {
        [Required]
        public string Email { get; set; }
    }

    public class AddMentee
    {
        [Required]
        public string Email { get; set; }
    }
}
