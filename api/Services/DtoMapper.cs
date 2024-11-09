public class DtoMapper
{
    public InmuebleDto ConvertirAInmuebleDto(Inmueble inmueble)
    {
        return new InmuebleDto
        {
            Id = inmueble.Id,
            Direccion = inmueble.Direccion,
            Uso = inmueble.Uso,
            Ciudad = inmueble.Ciudad,
            Precio = inmueble.Precio
        };
    }

    public ContratoDto ConvertirAContratoDto(Contrato contrato)
    {
        return new ContratoDto
        {
            Id = contrato.Id,
            FechaInicio = contrato.FechaInicio,
            FechaFin = contrato.FechaFin,
            Monto = contrato.Monto,
            Inmueble = ConvertirAInmuebleDto(contrato.Inmueble)
        };
    }
}
