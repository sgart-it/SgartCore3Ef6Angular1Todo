using System;

namespace SgartCore3Ef6Angular1Todo.Models
{
    public class MyTaskInput
    {
        public int? ID { get; set; }
        public DateTime? Date { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime? Completed { get; set; }
        public int? IDCategory { get; set; }

    }
}
