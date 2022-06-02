namespace BlazorApp1.Utilitys
{
    public class ImGuTIL
    {
        public async Task<string> ConvertToImage(Stream stream)
        {
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] byteArray = memoryStream.ToArray();
            string base64String = Convert.ToBase64String(byteArray, 0, byteArray.Length);
            return  base64String;
        }

        public void recorte(Stream stream)
        {
           
        }
        
    }
}
