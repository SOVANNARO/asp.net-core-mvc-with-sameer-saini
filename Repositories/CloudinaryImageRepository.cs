using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace asp_net_core_mvc.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly IConfiguration configuration;
        private readonly Cloudinary cloudinary;

        public CloudinaryImageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            var account = new Account(
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]
            );
            cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream)
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
        }
    }
}