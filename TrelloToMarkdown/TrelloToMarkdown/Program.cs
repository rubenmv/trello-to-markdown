using System.Text.Json;
using TrelloToMarkdown;

const int MAX_FILENAME_LENGTH = 80;
const bool INCLUDE_ATTACHMENTS = true;

var filename = @"C:\SET_YOUR_PATH_HERE\trello-gaming-board.json";
var rootDir = @"C:\SET_YOUR_PATH_HERE\TrelloToMarkDown";

if (!Directory.Exists(rootDir))
{
    Directory.CreateDirectory(rootDir);
}

string jsonString = File.ReadAllText(filename);

var trello = JsonSerializer.Deserialize<TrelloBoard>(jsonString)
                ?? throw new Exception($"Couldn't read data from file {filename}");

if (string.IsNullOrWhiteSpace(trello?.Name)) throw new Exception("Wrong data, no board name found");

var boardDirectory = Path.Combine(rootDir, Helpers.MakeValidFileName(trello.Name));

if (!Directory.Exists(boardDirectory))
{
    Directory.CreateDirectory(boardDirectory);
}

if (trello?.Lists == null) throw new Exception("Wrong data, no list found");
if (trello?.Cards == null) throw new Exception("Wrong data, no cards found");
// remove archived lists
trello.Lists = trello.Lists.Where(l => l.Closed == false);
foreach (var list in trello.Lists)
{
    list.Name ??= "nonamelist";

    var listDirectory = Path.Combine(boardDirectory, Helpers.MakeValidFileName(list.Name));
    if (!Directory.Exists(listDirectory))
    {
        Directory.CreateDirectory(listDirectory);
    }
    var attachmentsPath = Path.Combine(rootDir, "_attachments");
    if (!Directory.Exists(attachmentsPath))
    {
        Directory.CreateDirectory(attachmentsPath);
    }

    // Search for all the cards related to the list
    var relatedCards = trello.Cards.Where(c => c.IdList == list.Id && !c.Closed);

    // Create card squeleton
    // Content = Images + Descripcion + Checklists
    foreach (var currentCard in relatedCards)
    {
        currentCard.Name ??= currentCard.Id ?? Guid.NewGuid().ToString();
        currentCard.Name = currentCard.Name.Length > MAX_FILENAME_LENGTH ? currentCard.Name.Substring(0, MAX_FILENAME_LENGTH) 
                                                                            : currentCard.Name;
        var cardFullFilename = Path.Combine(listDirectory, Helpers.MakeValidFileName($"{currentCard.DateOnlyString}-{currentCard.Name}.md"));
        
        string cardContent = string.Empty;

        if (currentCard.Attachments != null && INCLUDE_ATTACHMENTS)
        {
            var uniqueAttachments = currentCard.Attachments.DistinctBy(a => a.Url);
            foreach (var attachment in uniqueAttachments)
            {
                var extension = Path.GetExtension(attachment.Name);
                var shortFileName = Helpers.MakeValidFileName($"{attachment.Id}_{attachment.Name}");

                var shortFileNameWithoutExtension = Path.GetFileNameWithoutExtension(shortFileName);
                shortFileName = shortFileNameWithoutExtension.Length > MAX_FILENAME_LENGTH ? shortFileNameWithoutExtension.Substring(0, MAX_FILENAME_LENGTH) 
                                                                                            : shortFileName;

                var attachmentFullFileName = Path.Combine(attachmentsPath, shortFileName);

                if (attachment.Url != null && !File.Exists(attachmentFullFileName))
                {
                    var imageStream = await Helpers.DownloadFromUrl(attachment.Url);
                    if (imageStream != null)
                    {
                        // Write to _attachments folder
                        using var fileStream = new FileStream(attachmentFullFileName, FileMode.Create);
                        await imageStream.CopyToAsync(fileStream);
                    }
                }

                // Add it to md content
                cardContent += $"\r\n\r\n![[{shortFileName}]]\r\n";
            }
        }

        // Description
        cardContent += $"\r\n{currentCard.Desc}\r\n";

        // Checklists
        if (trello.Checklists != null)
        {
            var cardChecklists = trello.Checklists.Where(c => c.IdCard == currentCard.Id);
            foreach (var cl in cardChecklists)
            {
                cardContent += $"\r\n{cl.Name}\r\n";
                if (cl.CheckItems != null)
                {
                    foreach (var item in cl.CheckItems)
                    {
                        if (item.Checked)
                        {
                            cardContent += $"\r\n- [x] {item.Name}";
                        }
                        else
                        {
                            cardContent += $"\r\n- [ ] {item.Name}";
                        }
                    }
                }
            }
            cardContent += $"\r\n";
        }

        // Write md to file
        File.WriteAllText(cardFullFilename, cardContent);
    }
}


Console.WriteLine("FINISHED!!");
