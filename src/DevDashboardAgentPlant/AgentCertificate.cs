using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DevDashboardAgentPlant;

/// <summary>
/// The dashboard pins the SHA256 of the certificate, so it is generated once and reused; deleting the file regenerates
/// it and requires re-adding the machine in the dashboard.
/// </summary>
public static class AgentCertificate
{
  public static string DataFolder { get; } = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "TrayGarden",
    "DevDashboardAgent");

  public static string CredentialFile => Path.Combine(DataFolder, "agent-credentials.json");

  private static string CertificateFile => Path.Combine(DataFolder, "agent-certificate.pfx");

  public static X509Certificate2 LoadOrCreate(string displayName)
  {
    Directory.CreateDirectory(DataFolder);
    if (!File.Exists(CertificateFile))
    {
      File.WriteAllBytes(CertificateFile, Create(displayName));
    }

    return X509CertificateLoader.LoadPkcs12FromFile(CertificateFile, null, X509KeyStorageFlags.Exportable);
  }

  public static string Fingerprint(X509Certificate2 certificate)
  {
    return Convert.ToHexString(SHA256.HashData(certificate.RawData));
  }

  private static byte[] Create(string displayName)
  {
    string name = string.IsNullOrWhiteSpace(displayName) ? Environment.MachineName : displayName.Trim();
    using ECDsa key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
    var request = new CertificateRequest(new X500DistinguishedName("CN=" + name.Replace(",", " ")), key, HashAlgorithmName.SHA256);
    using X509Certificate2 certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(20));
    return certificate.Export(X509ContentType.Pfx);
  }
}
