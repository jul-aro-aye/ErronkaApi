namespace GastuakApi.DTOak
{
    //DTO-ak APIak itzuliko dituen datu motak dira
    public class FamiliaDto
    {
        public int Id { get; set; }
        public string Izena { get; set; }
        public List<ErabiltzaileaDto> Erabiltzaileak { get; set; } = new();
    }
}
