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
    
    public partial class DiaryEntries
    {
        public int Id { get; set; }
        public string Entry { get; set; }
        public int UsersId { get; set; }
        public int MentorId { get; set; }
        public decimal SentimentScore { get; set; }
        public string MentorFeedback { get; set; }
        public System.DateTime Date { get; set; }
        public int MenteeFeedback { get; set; }
    
        public virtual Users User { get; set; }
    }
}
