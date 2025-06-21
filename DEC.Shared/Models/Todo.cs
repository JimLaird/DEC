using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DEC.Shared.Models
{
    public class Todo
    {
        public string Id { get; set; }

        public bool IsCompleted { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Driver { get; set; }

        public string Uid { get; set; }
    }

}
