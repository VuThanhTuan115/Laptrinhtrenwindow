using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore
{
    public class Student
    {
        public int ID { get; set; }
        public required string FullName { get; set; }
        public double Grade { get; set; }

        public override string ToString()
        {
            return $"ID: {ID}, FullName: {FullName}, Grade: {Grade}";
        }
    }
}
