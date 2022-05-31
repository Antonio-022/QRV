using IronBarCode;
using System.Drawing;


namespace API_SAADS_CORE_61.Helpers
{
    public static class QR
    {
        public static byte[] GenerarQR(string pTexto)
        {
            var qrCodeImageBmp = QRCodeWriter.CreateQrCodeWithLogo(pTexto, "logo5.jpg", 500);
            qrCodeImageBmp.ChangeBarCodeColor(System.Drawing.Color.Black);
            var QR = qrCodeImageBmp.Image;
            ImageConverter converter = new();
            return (byte[])converter.ConvertTo(QR, typeof(byte[]));
        }
        public  static byte[] GenerarQRWithUploadImage(string pTexto,string Archivo,string rutaCompleta)
        {
            var qrCodeImageBmp = QRCodeWriter.CreateQrCodeWithLogo(pTexto,"Archivos/"+ Archivo, 500);
            qrCodeImageBmp.ChangeBarCodeColor(System.Drawing.Color.Black);
            var QR = qrCodeImageBmp.Image;
            ImageConverter converter = new();
            return (byte[])converter.ConvertTo(QR, typeof(byte[]));
        }
    }

}
