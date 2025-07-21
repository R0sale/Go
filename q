[1mdiff --git a/Facilities/Application/Contracts/IFacilityRepository.cs b/Facilities/Application/Contracts/IFacilityRepository.cs[m
[1mindex 1c8b097..717096f 100644[m
[1m--- a/Facilities/Application/Contracts/IFacilityRepository.cs[m
[1m+++ b/Facilities/Application/Contracts/IFacilityRepository.cs[m
[36m@@ -9,10 +9,10 @@[m [mnamespace Application.Contracts[m
 {[m
     public interface IFacilityRepository[m
     {[m
[31m-        Task<IList<Facility>> GetFacilities();[m
[31m-        Task<Facility> GetFacility(string id);[m
[31m-        Task CreateFacility(Facility newFacility);[m
[31m-        Task UpdateFacility(string id, Facility updatedFacility);[m
[31m-        Task RemoveFacility(string id);[m
[32m+[m[32m        Task<IList<Facility>> GetFacilitiesAsync();[m
[32m+[m[32m        Task<Facility> GetFacilityAsync(string id);[m
[32m+[m[32m        Task CreateFacilityAsync(Facility newFacility);[m
[32m+[m[32m        Task UpdateFacilityAsync(string id, Facility updatedFacility);[m
[32m+[m[32m        Task RemoveFacilityAsync(string id);[m
     }[m
 }[m
[1mdiff --git a/Facilities/Application/Contracts/IFacilityService.cs b/Facilities/Application/Contracts/IFacilityService.cs[m
[1mindex 7457ea5..65feb92 100644[m
[1m--- a/Facilities/Application/Contracts/IFacilityService.cs[m
[1m+++ b/Facilities/Application/Contracts/IFacilityService.cs[m
[36m@@ -3,10 +3,17 @@[m [musing System.Collections.Generic;[m
 using System.Linq;[m
 using System.Text;[m
 using System.Threading.Tasks;[m
[32m+[m[32musing Application.Dtos;[m
[32m+[m[32musing Entities.Models;[m
 [m
 namespace Application.Contracts[m
 {[m
[31m-    class IFacilityService[m
[32m+[m[32m    public interface IFacilityService[m
     {[m
[32m+[m[32m        Task<IList<FacilityDto>> GetFacilitiesAsync();[m
[32m+[m[32m        Task<FacilityDto> GetFacilityAsync(string id);[m
[32m+[m[32m        Task CreateFacilityAsync(FacilityDto newFacility);[m
[32m+[m[32m        Task UpdateFacilityAsync(string id, FacilityDto updatedFacility);[m
[32m+[m[32m        Task RemoveFacilityAsync(string id);[m
     }[m
 }[m
[1mdiff --git a/Facilities/Application/FacilitiesService.cs b/Facilities/Application/FacilitiesService.cs[m
[1mindex 1388c84..f3c8fc8 100644[m
[1m--- a/Facilities/Application/FacilitiesService.cs[m
[1m+++ b/Facilities/Application/FacilitiesService.cs[m
[36m@@ -1,4 +1,8 @@[m
[31m-﻿using System;[m
[32m+[m[32m﻿using Application.Contracts;[m
[32m+[m[32musing Application.Dtos;[m
[32m+[m[32musing AutoMapper;[m
[32m+[m[32musing Entities.Models;[m
[32m+[m[32musing System;[m
 using System.Collections.Generic;[m
 using System.Linq;[m
 using System.Text;[m
[36m@@ -6,7 +10,51 @@[m [musing System.Threading.Tasks;[m
 [m
 namespace Application[m
 {[m
[31m-    class FacilitiesService[m
[32m+[m[32m    public class FacilitiesService[m
     {[m
[32m+[m[32m        private readonly IFacilityRepository _facilityRepository;[m
[32m+[m[32m        private readonly IMapper _mapper;[m
[32m+[m
[32m+[m[32m        public FacilitiesService(IFacilityRepository facilityRepository, IMapper mapper)[m
[32m+[m[32m        {[m
[32m+[m[32m            _facilityRepository = facilityRepository;[m
[32m+[m[32m            _mapper = mapper;[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        public async Task<IList<FacilityDto>> GetFacilitiesAsync()[m
[32m+[m[32m        {[m
[32m+[m[32m            var facilities = await _facilityRepository.GetFacilitiesAsync();[m
[32m+[m
[32m+[m[32m            var facilitiesDto = _mapper.Map<IList<FacilityDto>>(facilities);[m
[32m+[m
[32m+[m[32m            return facilitiesDto;[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        public async Task<FacilityDto> GetFacilityAsync(string id)[m
[32m+[m[32m        {[m
[32m+[m[32m            var facility = await _facilityRepository.GetFacilityAsync(id);[m
[32m+[m
[32m+[m[32m            var facilityDto = _mapper.Map<FacilityDto>(facility);[m
[32m+[m[32m            return facilityDto;[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        public async Task CreateFacilityAsync(FacilityDto newFacility)[m
[32m+[m[32m        {[m
[32m+[m[32m            var facility = _mapper.Map<Facility>(newFacility);[m
[32m+[m
[32m+[m[32m            await _facilityRepository.CreateFacilityAsync(facility);[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        public async Task UpdateFacilityAsync(string id, FacilityDto updatedFacility)[m
[32m+[m[32m        {[m
[32m+[m[32m            var facility = _mapper.Map<Facility>(updatedFacility);[m
[32m+[m
[32m+[m[32m            await _facilityRepository.UpdateFacilityAsync(id, facility);[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        public async Task RemoveFacilityAsync(string id)[m
[32m+[m[32m        {[m
[32m+[m[32m            await _facilityRepository.RemoveFacilityAsync(id);[m
[32m+[m[32m        }[m
     }[m
 }[m
[1mdiff --git a/Facilities/Entities/Models/Facility.cs b/Facilities/Entities/Models/Facility.cs[m
[1mindex 2cd8b38..c5f62e3 100644[m
[1m--- a/Facilities/Entities/Models/Facility.cs[m
[1m+++ b/Facilities/Entities/Models/Facility.cs[m
[36m@@ -18,5 +18,6 @@[m [mnamespace Entities.Models[m
         public string? Email { get; set; }[m
         public string? Description { get; set; }[m
         public string? OpeningHours { get; set; }[m
[32m+[m[32m        public string? WebsiteURL { get; set; }[m
     }[m
 }[m
[1mdiff --git a/Facilities/Entities/Models/Restaurant.cs b/Facilities/Entities/Models/Restaurant.cs[m
[1mindex 4dbe857..53380b7 100644[m
[1m--- a/Facilities/Entities/Models/Restaurant.cs[m
[1m+++ b/Facilities/Entities/Models/Restaurant.cs[m
[36m@@ -7,9 +7,23 @@[m [musing System.Threading.Tasks;[m
 [m
 namespace Entities.Models[m
 {[m
[31m-    class Restaurant : Facility[m
[32m+[m[32m    public enum CuisineType[m
     {[m
[31m-        public string? CuisineType { get; set; } [m
[32m+[m[32m        Italian,[m
[32m+[m[32m        Chinese,[m
[32m+[m[32m        Indian,[m
[32m+[m[32m        Mexican,[m
[32m+[m[32m        American,[m
[32m+[m[32m        French,[m
[32m+[m[32m        Japanese,[m
[32m+[m[32m        Mediterranean,[m
[32m+[m[32m        Thai,[m
[32m+[m[32m        Other[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    public class Restaurant : Facility[m
[32m+[m[32m    {[m
[32m+[m[32m        public List<CuisineType>? CuisineType { get; set; }[m[41m [m
         public int SeatingCapacity { get; set; } [m
         public bool HasDeliveryService { get; set; } [m
         public bool HasTakeoutService { get; set; } [m
[1mdiff --git a/Facilities/Facilities.sln b/Facilities/Facilities.sln[m
[1mindex 56658c3..46fba95 100644[m
[1m--- a/Facilities/Facilities.sln[m
[1m+++ b/Facilities/Facilities.sln[m
[36m@@ -3,7 +3,7 @@[m [mMicrosoft Visual Studio Solution File, Format Version 12.00[m
 # Visual Studio Version 17[m
 VisualStudioVersion = 17.13.35825.156[m
 MinimumVisualStudioVersion = 10.0.40219.1[m
[31m-Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Facilities", "Facilities\Facilities.csproj", "{1CD1248C-0A59-4907-9CC6-A2D3C4D87AF3}"[m
[32m+[m[32mProject("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Web", "Facilities\Web.csproj", "{1CD1248C-0A59-4907-9CC6-A2D3C4D87AF3}"[m
 EndProject[m
 Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Entities", "Entities\Entities.csproj", "{3FEC7D20-F4B9-41D4-AE12-DE9320525903}"[m
 EndProject[m
[1mdiff --git a/Facilities/Facilities/Controllers/FacilityController.cs b/Facilities/Facilities/Controllers/FacilityController.cs[m
[1mdeleted file mode 100644[m
[1mindex 025768a..0000000[m
[1m--- a/Facilities/Facilities/Controllers/FacilityController.cs[m
[1m+++ /dev/null[m
[36m@@ -1,52 +0,0 @@[m
[31m-﻿using Microsoft.AspNetCore.Mvc;[m
[31m-using Application.Contracts;[m
[31m-using Application.Dtos;[m
[31m-using Entities.Models;[m
[31m-using AutoMapper;[m
[31m-[m
[31m-namespace Facilities.Controllers[m
[31m-{[m
[31m-    [ApiController][m
[31m-    [Route("api/facility")][m
[31m-    public class FacilityController : ControllerBase[m
[31m-    {[m
[31m-        private readonly IFacilityRepository _facilityRepository;[m
[31m-        private readonly IMapper _mapper;[m
[31m-[m
[31m-        public FacilityController(IFacilityRepository facilityRepository, IMapper mapper)[m
[31m-        {[m
[31m-            _facilityRepository = facilityRepository;[m
[31m-            _mapper = mapper;[m
[31m-        }[m
[31m-[m
[31m-        [HttpGet][m
[31m-        public async Task<IActionResult> GetFacilities()[m
[31m-        {[m
[31m-            return Ok(await _facilityRepository.GetFacilities());[m
[31m-        }[m
[31m-[m
[31m-        [HttpGet("{id}")][m
[31m-        public async Task<IActionResult> GetFacility(string id)[m
[31m-        {[m
[31m-            return Ok(await _facilityRepository.GetFacility(id));[m
[31m-        }[m
[31m-[m
[31m-        [HttpPost][m
[31m-        public async Task<IActionResult> CreateFacility([FromBody] FacilityDto facilityDto)[m
[31m-        {[m
[31m-            var facility = _mapper.Map<Facility>(facilityDto);[m
[31m-[m
[31m-            await _facilityRepository.CreateFacility(facility);[m
[31m-[m
[31m-            return Created();[m
[31m-        }[m
[31m-[m
[31m-        [HttpDelete("{id}")][m
[31m-        public async Task<IActionResult> DeleteFacility(string id)[m
[31m-        {[m
[31m-            await _facilityRepository.RemoveFacility(id);[m
[31m-[m
[31m-            return NoContent();[m
[31m-        }[m
[31m-    }[m
[31m-}[m
[1mdiff --git a/Facilities/Facilities/Extensions/ExtensionService.cs b/Facilities/Facilities/Extensions/ExtensionService.cs[m
[1mdeleted file mode 100644[m
[1mindex 4907b6b..0000000[m
[1m--- a/Facilities/Facilities/Extensions/ExtensionService.cs[m
[1m+++ /dev/null[m
[36m@@ -1,12 +0,0 @@[m
[31m-﻿using Entities.Models;[m
[31m-[m
[31m-namespace Facilities.Extensions[m
[31m-{[m
[31m-    public static class ExtensionService[m
[31m-    {[m
[31m-        public static void ConfigureDB(this IServiceCollection service, IConfiguration config)[m
[31m-        {[m
[31m-            service.Configure<FacilitiesDatabaseSettings>(config.GetSection("FacilitiesDatabase"));[m
[31m-        }[m
[31m-    }[m
[31m-}[m
[1mdiff --git a/Facilities/Facilities/Facilities.csproj b/Facilities/Facilities/Facilities.csproj[m
[1mdeleted file mode 100644[m
[1mindex f985d0d..0000000[m
[1m--- a/Facilities/Facilities/Facilities.csproj[m
[1m+++ /dev/null[m
[36m@@ -1,13 +0,0 @@[m
[31m-﻿<Project Sdk="Microsoft.NET.Sdk.Web">[m
[31m-[m
[31m-  <PropertyGroup>[m
[31m-    <TargetFramework>net9.0</TargetFramework>[m
[31m-    <Nullable>enable</Nullable>[m
[31m-    <ImplicitUsings>enable</ImplicitUsings>[m
[31m-  </PropertyGroup>[m
[31m-[m
[31m-  <ItemGroup>[m
[31m-    <ProjectReference Include="..\Infrastructure\Infrastructure.csproj" />[m
[31m-  </ItemGroup>[m
[31m-[m
[31m-</Project>[m
[1mdiff --git a/Facilities/Facilities/Program.cs b/Facilities/Facilities/Program.cs[m
[1mdeleted file mode 100644[m
[1mindex ba99ff5..0000000[m
[1m--- a/Facilities/Facilities/Program.cs[m
[1m+++ /dev/null[m
[36m@@ -1,18 +0,0 @@[m
[31m-using Application.Contracts;[m
[31m-using Facilities.Extensions;[m
[31m-using Infrastructure;[m
[31m-[m
[31m-var builder = WebApplication.CreateBuilder(args);[m
[31m-[m
[31m-[m
[31m-[m
[31m-builder.Services.ConfigureDB(builder.Configuration);[m
[31m-builder.Services.AddAutoMapper(typeof(Application.AssemblyReference).Assembly);[m
[31m-builder.Services.AddControllers();[m
[31m-builder.Services.AddScoped<IFacilityRepository, FacilitiesRepository>();[m
[31m-[m
[31m-var app = builder.Build();[m
[31m-[m
[31m-app.MapControllers();[m
[31m-[m
[31m-app.Run();[m
[1mdiff --git a/Facilities/Facilities/Properties/launchSettings.json b/Facilities/Facilities/Properties/launchSettings.json[m
[1mdeleted file mode 100644[m
[1mindex 728bdd3..0000000[m
[1m--- a/Facilities/Facilities/Properties/launchSettings.json[m
[1m+++ /dev/null[m
[36m@@ -1,38 +0,0 @@[m
[31m-﻿{[m
[31m-  "$schema": "http://json.schemastore.org/launchsettings.json",[m
[31m-  "iisSettings": {[m
[31m-    "windowsAuthentication": false,[m
[31m-    "anonymousAuthentication": true,[m
[31m-    "iisExpress": {[m
[31m-      "applicationUrl": "http://localhost:42486",[m
[31m-      "sslPort": 44301[m
[31m-    }[m
[31m-  },[m
[31m-  "profiles": {[m
[31m-    "http": {[m
[31m-      "commandName": "Project",[m
[31m-      "dotnetRunMessages": true,[m
[31m-      "launchBrowser": true,[m
[31m-      "applicationUrl": "http://localhost:5221",[m
[31m-      "environmentVariables": {[m
[31m-        "ASPNETCORE_ENVIRONMENT": "Development"[m
[31m-      }[m
[31m-    },[m
[31m-    "https": {[m
[31m-      "commandName": "Project",[m
[31m-      "dotnetRunMessages": true,[m
[31m-      "launchBrowser": true,[m
[31m-      "applicationUrl": "https://localhost:7236;http://localhost:5221",[m
[31m-      "environmentVariables": {[m
[31m-        "ASPNETCORE_ENVIRONMENT": "Development"[m
[31m-      }[m
[31m-    },[m
[31m-    "IIS Express": {[m
[31m-      "commandName": "IISExpress",[m
[31m-      "launchBrowser": true,[m
[31m-      "environmentVariables": {[m
[31m-        "ASPNETCORE_ENVIRONMENT": "Development"[m
[31m-      }[m
[31m-    }[m
[31m-  }[m
[31m-}[m
[1mdiff --git a/Facilities/Facilities/appsettings.Development.json b/Facilities/Facilities/appsettings.Development.json[m
[1mdeleted file mode 100644[m
[1mindex 0c208ae..0000000[m
[1m--- a/Facilities/Facilities/appsettings.Development.json[m
[1m+++ /dev/null[m
[36m@@ -1,8 +0,0 @@[m
[31m-{[m
[31m-  "Logging": {[m
[31m-    "LogLevel": {[m
[31m-      "Default": "Information",[m
[31m-      "Microsoft.AspNetCore": "Warning"[m
[31m-    }[m
[31m-  }[m
[31m-}[m
[1mdiff --git a/Facilities/Facilities/appsettings.json b/Facilities/Facilities/appsettings.json[m
[1mdeleted file mode 100644[m
[1mindex 570563d..0000000[m
[1m--- a/Facilities/Facilities/appsettings.json[m
[1m+++ /dev/null[m
[36m@@ -1,14 +0,0 @@[m
[31m-{[m
[31m-  "FacilitiesDatabase": {[m
[31m-    "ConnectionString": "mongodb://localhost:27017",[m
[31m-    "DatabaseName": "FacilitiesDB",[m
[31m-    "FacilitiesCollectionName": "Facilities"[m
[31m-  },[m
[31m-  "Logging": {[m
[31m-    "LogLevel": {[m
[31m-      "Default": "Information",[m
[31m-      "Microsoft.AspNetCore": "Warning"[m
[31m-    }[m
[31m-  },[m
[31m-  "AllowedHosts": "*"[m
[31m-}[m
[1mdiff --git a/Facilities/Infrastructure/FacilitiesRepository.cs b/Facilities/Infrastructure/FacilitiesRepository.cs[m
[1mindex 53a1115..6659b1a 100644[m
[1m--- a/Facilities/Infrastructure/FacilitiesRepository.cs[m
[1m+++ b/Facilities/Infrastructure/FacilitiesRepository.cs[m
[36m@@ -23,14 +23,14 @@[m [mnamespace Infrastructure[m
             _facilities = mongoDatabase.GetCollection<Facility>(facilitiesDatabaseSettings.Value.FacilitiesCollectionName); [m
         }[m
 [m
[31m-        public async Task<IList<Facility>> GetFacilities() => await _facilities.Find(_ => true).ToListAsync();[m
[32m+[m[32m        public async Task<IList<Facility>> GetFacilitiesAsync() => await _facilities.Find(_ => true).ToListAsync();[m
 [m
[31m-        public async Task<Facility> GetFacility(string id) => await _facilities.Find(facility => facility.Id == id).FirstOrDefaultAsync();[m
[32m+[m[32m        public async Task<Facility> GetFacilityAsync(string id) => await _facilities.Find(facility => facility.Id == id).FirstOrDefaultAsync();[m
 [m
[31m-        public async Task CreateFacility(Facility newFacility) => await _facilities.InsertOneAsync(newFacility);[m
[32m+[m[32m        public async Task CreateFacilityAsync(Facility newFacility) => await _facilities.InsertOneAsync(newFacility);[m
 [m
[31m-        public async Task UpdateFacility(string id, Facility updatedFacility) => await _facilities.ReplaceOneAsync(facility => facility.Id == id, updatedFacility);[m
[32m+[m[32m        public async Task UpdateFacilityAsync(string id, Facility updatedFacility) => await _facilities.ReplaceOneAsync(facility => facility.Id == id, updatedFacility);[m
 [m
[31m-        public async Task RemoveFacility(string id) => await _facilities.DeleteOneAsync(facility => facility.Id == id);[m
[32m+[m[32m        public async Task RemoveFacilityAsync(string id) => await _facilities.DeleteOneAsync(facility => facility.Id == id);[m
     }[m
 }[m
[1mdiff --git a/Facilities/Infrastructure/Infrastructure.csproj b/Facilities/Infrastructure/Infrastructure.csproj[m
[1mindex dc0a69f..d9edd58 100644[m
[1m--- a/Facilities/Infrastructure/Infrastructure.csproj[m
[1m+++ b/Facilities/Infrastructure/Infrastructure.csproj[m
[36m@@ -8,6 +8,7 @@[m
 [m
   <ItemGroup>[m
     <PackageReference Include="Microsoft.Extensions.Options" Version="9.0.7" />[m
[32m+[m[32m    <PackageReference Include="MongoDB.Driver" Version="3.4.1" />[m
   </ItemGroup>[m
 [m
   <ItemGroup>[m
