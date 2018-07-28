using System.ComponentModel.DataAnnotations;

namespace FirstWeekWorkTest.Models
{
    public class CustomerContactVM
    {

        [Required]
        public int Id { get; set; }
        [StringLength(250, ErrorMessage = "欄位長度不得大於 250 個字元")]
        [Required]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})$", ErrorMessage = "請輸入正確的電子郵件位址.")]
        public string Email { get; set; }
        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 手機 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
    }
}