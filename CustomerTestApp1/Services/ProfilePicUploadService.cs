namespace CustomerTestApp1.Services;

public static class ProfilePicUploadService {
  private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];

  public static async Task<( string? data, string? error )> SaveProfileImage(IFormFile picture) {
    var extension = Path.GetExtension(picture.FileName).ToLower();

    Console.Write($"[-|-] : Picture extension {extension}");
    if (!AllowedExtensions.Contains(extension))
      return (null, "Only jpg, jpeg, and png files are allowed.");
    ;
    // return  "Only jpg, jpeg, and png files are allowed.";    

    if (picture.Length > 5 * 1024 * 1024)
      return (null, "picture must be 5MBs or less.");

    var fileName = $"CU-{Guid.NewGuid()}{extension}";

    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "ProfilePictures");

    if (!Directory.Exists(uploadFolder))
      Directory.CreateDirectory(uploadFolder);

    var filePath = Path.Combine(uploadFolder, fileName);
    await using (var steam = new FileStream(filePath, FileMode.Create)) {
      await picture.CopyToAsync(steam);
    }

    return ($"/Uploads/ProfilePictures/{fileName}", null);
  }

  public static void DeleteProfileImage(string? picUrl) {
    if (string.IsNullOrWhiteSpace(picUrl) || picUrl.Equals(null)) return;

    var fileName = Path.GetFileName(picUrl);
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "ProfilePictures", fileName);

    if (Path.Exists(filePath)) File.Delete(filePath);
  }
}