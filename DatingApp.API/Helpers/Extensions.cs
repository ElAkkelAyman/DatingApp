using System;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static int CalculateAge(this DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if(birthDate.AddYears(age) > DateTime.Today)
            age--;
            return age;
        }
    }
}