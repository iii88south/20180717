using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirstWeekWorkTest.Models.ViewModels
{
    public class CustomerDataVM:IValidatableObject
    {
        客戶資料Repository customerDataRepo;

        public CustomerDataVM()
        {
            customerDataRepo = RepositoryHelper.Get客戶資料Repository();
        }

        [Required]
        public string 帳號 { get; set; }

        [Required]
        public string 密碼 { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var result = customerDataRepo.ISMember(this.帳號, this.密碼);

            if (!result)
            {
                yield return new ValidationResult ("帳號密碼錯誤!");
            }            
        }
    }
}