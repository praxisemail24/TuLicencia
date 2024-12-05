using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLicense.Pdf.Models
{
    public class BillOfSalePdf : IPdfModel
    {
        public string Titulo { get; set; }
        public string SellerName { get; set; }
        public string SellerAddress { get; set; }
        public string SellerCity { get; set; }
        public string Description { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string OtherInfo { get; set; }
        public string BuyerName { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerCity { get; set; }
        public string PaymentAmount { get; set; }
        public string PaymentDate { get; set; }
        public string SellerSignature { get; set; }
        public string SellerSignatureAt { get; set; }
        public string BuyerSignature { get; set; }
        public string BuyerSignatureAt { get; set; }
        public string NroItem { get; set; }

        public BillOfSalePdf()
        {
            Titulo = "Contrato de compra y venta";
            SellerName = string.Empty;
            SellerAddress = string.Empty;
            SellerCity = string.Empty;
            Description = string.Empty;
            Make = string.Empty;
            Model = string.Empty;
            SerialNumber = string.Empty;
            OtherInfo = string.Empty;
            BuyerName = string.Empty;
            BuyerAddress = string.Empty;
            BuyerCity = string.Empty;
            PaymentAmount = string.Empty;
            PaymentDate = string.Empty;
            SellerSignature = string.Empty;
            SellerSignatureAt = string.Empty;
            BuyerSignature = string.Empty;
            BuyerSignatureAt = string.Empty;
            NroItem = "1";
        }
    }
}
