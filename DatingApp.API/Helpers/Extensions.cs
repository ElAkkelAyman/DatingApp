using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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

        public static void AddPagination(this HttpResponse response,int currentPage,int itemsPerPage,int totalItems ,int totalPgaes)
        {
            var PaginationHeader = new PaginaionHeader(currentPage,itemsPerPage,totalItems,totalPgaes);
            response.Headers.Add("Pagination",JsonConvert.SerializeObject(PaginationHeader));
            response.Headers.Add("Acess-Control-Expose-Headers","Pagination"); // expose the header so we won t have a CORS error

        }


    }
}