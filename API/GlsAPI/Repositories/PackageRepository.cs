using Azure;
using GlsAPI.Data;
using GlsAPI.Interfaces;
using GlsAPI.Models;
using GlsAPI.Models.Responses;
using GlsAPI.Models.Responses.Items;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Metadata;
using ZXing;
using ZXing.Common;
using Document = iTextSharp.text.Document;

namespace GlsAPI.Repository
{
    public class PackageRepository : IPackageRepository
    {
        private readonly DBContext _dBContext;

        public PackageRepository(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public PackagesResponse GetPackagesIDs(Guid sessionId, int idStart)
        {
            PackagesResponse response = new();
            var session = GetSession(sessionId);
            if (session == null)
            {
                response.Error = GetError("err_sess_not_found");
                return response;
            }
            if (!session.IsActive)
            {
                response.Error = GetError("err_sess_expired");
                return response;
            }
            if (idStart < 0)
            {
                response.Error = GetError("err_id_start_invalid");
                return response;
            }
            var packages = _dBContext.Packages.Where(p => p.Id >= idStart).OrderBy(p => p.Id).Take(100).ToList();
            foreach (var item in packages)
            {
                response.package_id.Add(item.Id.ToString("X"));
            }
            return response;
        }

        public PackageResponse GetPackage(Guid sessionId, int packageId)
        {
            PackageResponse response = new();
            var session = GetSession(sessionId);
            if (session == null)
            {
                response.Error = GetError("err_sess_not_found");
                return response;
            }
            if (!session.IsActive)
            {
                response.Error = GetError("err_sess_expired");
                return response;
            }
            var package = _dBContext.Packages
                .Include(p => p.Sender)
                .Include(p => p.Recipient)
                .Include(p => p.Status)
                .FirstOrDefault(p => p.Id == packageId);
            if (packageId < 0 || package == null)
            {
                response.Error = GetError("err_cons_not_found");
                return response;
            }
            response.Package = new PackageItem
            {
                Package_Id = packageId.ToString("X"),
                Recipient = new RecipientInPackageItem
                {
                    RecipientName1 = package.Recipient.Name1,
                    RecipientName2 = package.Recipient.Name2,
                    RecipientName3 = package.Recipient.Name3,
                    RecipientCountry = package.Recipient.Country,
                    RecipientZipCode = package.Recipient.ZipCode,
                    RecipientCity = package.Recipient.City,
                    RecipientStreet = package.Recipient.Street,
                    RecipientPhone = package.Recipient.Phone,
                    RecipientContact = package.Recipient.Contact,
                }
            };
            return response;
        }

        public LabelResponse GetConsignLabels(Guid sessionId, List<int> packageIds, string mode)
        {
            LabelResponse response = new LabelResponse();
            var session = GetSession(sessionId);
            if (session == null)
            {
                response.Error = GetError("err_sess_not_found");
                return response;
            }
            if (!session.IsActive)
            {
                response.Error = GetError("err_sess_expired");
                return response;
            }
            var packages = _dBContext.Packages.Where(p => packageIds.Contains(p.Id)).Include(p=> p.Recipient).OrderBy(p => p.Id).ToList();
            if (packages.Count < 1 || packages == null)
            {
                response.Error = GetError("err_cons_not_found");
                return response;
            }
            foreach (var package in packages)
            {
                response.Labels.Add(GeneratePdfLabel(package));
            }
            return response;
        }

        public Session GetSession(Guid sessionId)
        {
            var session = _dBContext.Sessions.FirstOrDefault(s => s.Id == sessionId);
            if (session != null)
            {
                return session;
            }
            return null;
        }

        public Error GetError(string name)
        {
            return _dBContext.Errors.FirstOrDefault(e => e.Name.Equals(name));
        }

        public string GeneratePdfLabel(Package package)
        {
            Document document = new Document();
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(document, stream))
                    {
                        document.Open();
                        document.Add(new Paragraph($"Etykieta {package.Id}"));
                        AddInfoToDocument(document, "Adres", package.Recipient);
                        document.Close();
                    }
                    byte[] pdfBytes = stream.ToArray();
                    string base64String = Convert.ToBase64String(pdfBytes);

                    return base64String;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private void AddInfoToDocument(Document document, string title, Customer customer)
        {
            var paragraphs = new List<Paragraph>
            {
                new Paragraph($"{title}:\n"),
                new Paragraph($"{customer.Name1} {customer.Name2} {customer.Name3}\n"),
                new Paragraph($"{customer.Street}\n{customer.ZipCode} {customer.City}\n"),
                new Paragraph($"{customer.Country}\n"),
                new Paragraph($"Telefon: {customer.Phone}\n"),
                new Paragraph($"Kontakt: {customer.Contact}\n\n")
            };
            foreach (var paragraph in paragraphs)
            {
                document.Add(paragraph);
            }
        }
    }
}
