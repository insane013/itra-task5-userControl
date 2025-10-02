using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Task5.Services;

public class BaseService
{
    protected readonly IMapper mapper;
    protected readonly ILogger<BaseService> _logger;
    protected BaseService(IMapper mapper, ILogger<BaseService> logger)
    {
        this.mapper = mapper;
        this._logger = logger;
    }
}