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

    public partial class Goals
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Goals()
        {
            this.Exercises = new HashSet<Exercise>();
        }
    
        public int Id { get; set; }
        public string GoalName { get; set; }
        [DataType(DataType.MultilineText)]
        public string GoalDescription { get; set; }
        public System.DateTime DueDate { get; set; }
        public System.DateTime CompletionDate { get; set; }
        [DataType(DataType.MultilineText)]
        public string MentorFeedback { get; set; }
        [DataType(DataType.MultilineText)]
        public string MenteeComments { get; set; }
        public Nullable<int> MenteeRating { get; set; }
        public int UsersId { get; set; }
        public int MentorId { get; set; }
    
        public virtual Users User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
