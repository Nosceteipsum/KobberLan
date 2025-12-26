namespace KobberLan.DTO;

//-------------------------------------------------------------
public record DTO_Suggestion
//-------------------------------------------------------------
(
    string Key, //foldername used as key (both in torrentengine and kobberlan)
    SuggestionType Type,
    string Title,
    string Author,
    byte[] ImageCover,
    byte[] Torrent
);

//-------------------------------------------------------------
public enum SuggestionType
//-------------------------------------------------------------
{
    HDD,
    Internet
}
