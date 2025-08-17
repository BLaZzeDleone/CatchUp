using System.ComponentModel.DataAnnotations;

public class ProfilDTO
{
    public int id { get; set; }

    public string vorname { get; set; }

    public string nachname { get; set; }

    public DateOnly geburtsdatum { get; set; }

    public string geschlecht { get; set; }

    public string sexualität { get; set; }
    
    public List<string> bilder_base_64 { get; set; }
}