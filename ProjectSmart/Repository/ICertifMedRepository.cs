using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface ICertifMedRepository
    {
        Task<ResponseEntity<CertifMed>> GetAllCertifMed();
        Task<ResponseEntity<CertifMed>> GetCertifMedById(int id);
        Task<ResponseEntity<CertifMed>> AddCertifMed(CertifMed certifMed);
        Task<ResponseEntity<CertifMed>> UpdateCertifMed(CertifMed id);
        Task<ResponseEntity<CertifMed>> DeleteCertifMed(int id);
        Task<ResponseEntity<EstadoCertificado>> CheckCertifiedStatus(int frmId, int trId);
    }
}
