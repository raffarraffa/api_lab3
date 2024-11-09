namespace api.Dtos;
public class PagoDto
{
    public int Id { get; set; }
    public decimal Importe { get; set; }
    public DateTime FechaPago { get; set; }
    public string Estado { get; set; }
}
