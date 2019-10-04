using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SgartCore3Ef6Angular1Todo.Models
{
  public class MyTask
  {
    [Key]
    public int ID { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; }
    public string Note { get; set; }
    public DateTime? Completed { get; set; }
    [Required]
    public Category Category { get; set; }

    [Required]
    public DateTime Created { get; set; }
    [Required]
    public DateTime Modified { get; set; }

  }
}
