using System.IO;

namespace AchieveIt.BusinessLogic.Services
{
    public class BlobFileInfo
    {
        public Stream Content { get; }
        
        public string ContentType { get; }

        public BlobFileInfo(Stream content, string contentType)
        {
            Content = content;
            ContentType = contentType;
        }
    }
}