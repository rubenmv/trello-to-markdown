namespace TrelloToMarkdown
{
    public static class Helpers
    {
        public static string MakeValidFileName(string name)
        {
            // Define invalid characters
            char[] invalidChars = Path.GetInvalidFileNameChars();

            // Replace invalid characters with an underscore
            var validString = new string(name.Select(ch => invalidChars.Contains(ch) ? '_' : ch).ToArray());

            return MakeHyphenedName(validString);
        }

        private static string MakeHyphenedName(string name)
        {
            return name.Replace(" ", "-").ToLowerInvariant();
        }

        public static async Task<Stream?> DownloadFromUrl(string url)
        {
            ArgumentNullException.ThrowIfNull(url);

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStreamAsync();
                    //using var fileStream = new FileStream(filename, FileMode.Create);
                    //await stream.CopyToAsync(fileStream);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to download image: " + ex.Message);
                }
            }

            return null;
        }
    }
}
