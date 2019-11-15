using System.ComponentModel.DataAnnotations;


namespace Exam.Models
{
    public class RSVP
    {
        [Key]
        public int RSVPId {get; set;}
        public int UserId {get; set;}
        public int JobId {get; set;}
        public User Worker {get; set;}
        public Job Attending {get; set;}
    }
}