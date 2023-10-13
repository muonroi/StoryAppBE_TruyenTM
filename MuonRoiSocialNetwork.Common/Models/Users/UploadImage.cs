using Microsoft.AspNetCore.Http;

namespace MuonRoiSocialNetwork.Common.Models.Users
{
    public class UploadImage
    {
        IFormFile ImageSrc { get; set; }
    }
}
