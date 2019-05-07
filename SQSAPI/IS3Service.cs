using System.Threading.Tasks;

namespace SQSAPI
{
    public interface IS3Service
    {
        Task CreateBucket(string bucketName);
        void UploadFile(string bucketName);
    }
}