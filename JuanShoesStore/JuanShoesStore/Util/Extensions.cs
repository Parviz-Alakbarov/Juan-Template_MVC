using Microsoft.AspNetCore.Http;

namespace JuanShoesStore.Util
{
    public static class Extensions
    {
        public static bool IsImage(this IFormFile formFile)
        {
            return formFile.ContentType.Contains("image/");
        }

        public static bool IsValidSize(this IFormFile formFile, int LengthByBite)
        {
            return formFile.Length < LengthByBite;
        } 
    }
}
