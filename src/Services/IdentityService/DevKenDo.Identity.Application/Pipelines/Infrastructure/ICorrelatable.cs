namespace DevKenDo.Identity.Application.Pipelines.Infrastructure;

public interface ICorrelatable
{
    string CorrelationId { get; set; }
}