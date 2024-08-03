using CollegeApp.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeApp.Models
{
    public class StudentDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Khong duoc de trong du lieu, vui long nhap lai")]
        [StringLength(50, ErrorMessage = "Do dai du lieu khong duoc qua 50 ki tu")]
        public string StudentName { get; set; }
        [EmailAddress(ErrorMessage = "Email khong dung dinh dang, vui long nhap lai")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Khong duoc de trong du lieu, vui long nhap lai")]
        [StringLength(50, ErrorMessage = "Do dai du lieu khong duoc qua 50 ki tu")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Khong duoc de trong du lieu, vui long nhap lai")]
        public DateTime DOB { get; set; }

    }
}
