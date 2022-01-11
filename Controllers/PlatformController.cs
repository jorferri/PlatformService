using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformController : ControllerBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;

    public PlatformController(IPlatformRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("--> Getting Platforms...");

        var allPlatforms = _repository.GetAllPlatforms();

        return Ok(_mapper.Map<IEnumerable<Platform>>(allPlatforms));
    }
    
    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        Console.WriteLine("--> Getting Platforms by ID...");

        var platformItem = _repository.GetPlatformById(id);

        if (platformItem != null)
        {
            return Ok(_mapper.Map<Platform>(platformItem));    
        }

        return NotFound();
    }
    
    [HttpPost]
    public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        Console.WriteLine("--> Creating a platform...");

        var platform = _mapper.Map<Platform>(platformCreateDto);
        _repository.CreatePlatform(platform);
        _repository.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

        // return Ok(platformReadDto);
        
        // in order to get location in result headers
        return CreatedAtRoute(nameof(GetPlatformById), new {platformReadDto.Id}, platformReadDto);
    }
}