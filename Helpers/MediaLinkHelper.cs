namespace MYChamp.Helpers
{
    public class MediaLinkHelper
    {
        public static string GetIconClass(string url)
        {
            if (url.Contains("linkedin.com"))
            {
                return "fab fa-linkedin";
            }
            if (url.Contains("facebook.com"))
            {
                return "fab fa-facebook";
            }
            if (url.Contains("twitter.com"))
            {
                return "fab fa-twitter";
            }
            if (url.Contains("instagram.com"))
            {
                return "fab fa-instagram";
            }
            //return "fas fa-link";
            return "fab fa-google";
        }
    }
}
